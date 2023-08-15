﻿using Carlton.Core.Components.Flux.Decorators.Commands;
using Carlton.Core.Components.Flux.Decorators.Queries;
using Carlton.Core.Components.Flux.Dispatchers;
using Carlton.Core.Components.Flux.Handlers;

namespace Carlton.Core.Components.Flux;

public static class ContainerExtensions
{
    public static void AddCarltonFlux<TState>(this IServiceCollection services)
    {
        /*Connected Components*/
        RegisterFluxConnectedComponents(services);

        /*Dispatchers*/
        RegisterFluxDispatchers<TState>(services);

        /*Handlers*/
        RegisterFluxHandlers<TState>(services);

        /*State Mutations*/
        RegisterFluxStateMutations(services);

        /*Validators*/
        RegisterValidators(services);
    }

    private static void RegisterFluxDispatchers<TState>(IServiceCollection services)
    {
        /*ViewModel Dispatchers*/
        services.AddSingleton<IViewModelQueryDispatcher<TState>, ViewModelQueryDispatcher<TState>>();
        services.Decorate<IViewModelQueryDispatcher<TState>, ViewModelValidationDecorator<TState>>();
        services.Decorate<IViewModelQueryDispatcher<TState>, ViewModelExceptionDecorator<TState>>();
        //services.Decorate<IViewModelDispatcher, ViewModelHttpDecorator<TState>>();
        //services.Decorate<IViewModelDispatcher, ViewModelJsDecorator<TState>>();

        /*Mutation Dispatchers*/
        services.AddSingleton<IMutationCommandDispatcher<TState>, MutationCommandDispatcher<TState>>();
        services.Decorate<IMutationCommandDispatcher<TState>, MutationValidationDecorator<TState>>();
        services.Decorate<IMutationCommandDispatcher<TState>, MutationExceptionDecorator<TState>>();
    }

    private static void RegisterFluxConnectedComponents(IServiceCollection services)
    {
        services.Scan(scan => scan
                    .FromApplicationDependencies()
                    .AddClasses(classes => classes.AssignableTo(typeof(IConnectedComponent<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
    }

    private static void RegisterFluxHandlers<TState>(IServiceCollection services)
    {
        bool interfacePredicate(Type _) => _.IsGenericType && _.GetGenericTypeDefinition() == typeof(IConnectedComponent<>);
        bool baseConnComPredicate(Type _) => _.IsGenericType && _.GetGenericTypeDefinition() == typeof(BaseConnectedComponent<>);
        bool isCommandPredicate(Type _) => _.IsAssignableTo(typeof(MutationCommand));
        bool isConnComPredicate(Type _) => _.GetInterfaces().Any(interfacePredicate) && !baseConnComPredicate(_);

        AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(_ => _.DefinedTypes)
                        .Where(_ => _.IsClass && !_.IsAbstract)
                        .Where(_ => isCommandPredicate(_) || isConnComPredicate(_))
                        .ToList().ForEach(_ =>
                        {
                            var isMutationCommand = isCommandPredicate(_);
                            var isConnectedComponent = isConnComPredicate(_);

                            if (isConnectedComponent)
                            {
                                //Register ViewModel Queries
                                var vmType = _.GetInterfaces().First(interfacePredicate).GetGenericArguments()[0];
                                var interfaceType = typeof(IViewModelQueryHandler<,>).MakeGenericType(typeof(TState), vmType);
                                var implementationType = typeof(ViewModelQueryHandler<,>).MakeGenericType(typeof(TState), vmType);
                                services.AddTransient(interfaceType, implementationType);
                            }
                            else if (isMutationCommand)
                            {
                                //Register Mutation Commands
                                var interfaceType = typeof(IMutationCommandHandler<,>).MakeGenericType(typeof(TState), _);
                                var implementationType = typeof(MutationCommandHandler<,>).MakeGenericType(typeof(TState), _);
                                services.AddTransient(interfaceType, implementationType);
                            }
                        });
    }

    private static void RegisterFluxStateMutations(IServiceCollection services)
    {
        services.Scan(_ => _
            .FromApplicationDependencies()
            .AddClasses(classes => classes.AssignableTo(typeof(IFluxStateMutation<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }

    private static void RegisterValidators(IServiceCollection services)
    {
        services.Scan(_ =>
            _.FromApplicationDependencies()
            .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
}


