﻿using Carlton.Core.Flux.Contracts;
using Carlton.Core.Flux.Dispatchers.Mutations;
using Carlton.Core.Flux.Dispatchers.ViewModels;
using Carlton.Core.Utilities.Results;
using NSubstitute.ExceptionExtensions;
namespace Carlton.Core.Flux.Tests.Common.Extensions;

public static class DispatcherExtensions
{
    public static void SetupQueryDispatcher(this IViewModelQueryDispatcher<TestState> dispatcher, TestViewModel vm)
    {
        dispatcher.Dispatch(
         Arg.Any<object>(),
         Arg.Any<ViewModelQueryContext<TestViewModel>>(),
         Arg.Any<CancellationToken>())
        .Returns(Task.FromResult((Result<TestViewModel, FluxError>) vm));
    }

    public static void SetupMutationDispatcher(this IMutationCommandDispatcher<TestState> dispatcher, TestCommand1 command)
    {
        dispatcher.Dispatch(
         Arg.Any<object>(),
         Arg.Is<MutationCommandContext<object>>(context => context.MutationCommand.Equals(command)),
         Arg.Any<CancellationToken>())
        .Returns(Task.FromResult((Result<MutationCommandResult, FluxError>)new MutationCommandResult()));
    }

    public static void VerifyQueryDispatcher(this IViewModelQueryDispatcher<TestState> dispatcher, int receivedNumCalls)
    {
        dispatcher.Received(receivedNumCalls).Dispatch(
            Arg.Any<object>(),
            Arg.Any<ViewModelQueryContext<TestViewModel>>(),
            Arg.Any<CancellationToken>());
    }

    public static void VerifyCommandDispatcher(this IMutationCommandDispatcher<TestState> dispatcher, int receivedNumCalls, object command)
    {
        dispatcher.Received(receivedNumCalls).Dispatch(
            Arg.Any<object>(),
            Arg.Is<MutationCommandContext<object>>(_ => _.MutationCommand == command),
            Arg.Any<CancellationToken>());
    }

    public static void SetupQueryDispatcherException(this IViewModelQueryDispatcher<TestState> dispatcher, Exception ex)
    {
        dispatcher.Dispatch(
         Arg.Any<object>(),
         Arg.Any<ViewModelQueryContext<TestViewModel>>(),
         Arg.Any<CancellationToken>())
        .Throws(ex);
    }

    public static void SetupCommandDispatcherError(this IMutationCommandDispatcher<TestState> dispatcher, FluxError error)
    {
        dispatcher.Dispatch(
         Arg.Any<object>(),
         Arg.Any<MutationCommandContext<object>>(),
         Arg.Any<CancellationToken>())
        .Returns(error);
    }
}


