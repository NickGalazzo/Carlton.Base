﻿namespace Carlton.Core.Flux.Dispatchers.ViewModels.Decorators;

public class ViewModelExceptionDecorator<TState>(
    IViewModelQueryDispatcher<TState> _decorated,
    ILogger<ViewModelExceptionDecorator<TState>> _logger)
    : IViewModelQueryDispatcher<TState>
{
    public async Task<Result<TViewModel, FluxError>> Dispatch<TViewModel>(object sender, ViewModelQueryContext<TViewModel> context, CancellationToken cancellationToken)
    {
        return await ResultExtensions.SafeExecuteAsync
        (
            async () => await _decorated.Dispatch(sender, context, cancellationToken), //Progress through the pipeline wrapped in a try/catch
            success => HandleSuccess(success, context), //Success Handler
            err => HandleError(err, context), //Error Handler
            ex => HandleException(ex, context) //Exception handler
        );
    }

    private TViewModel HandleSuccess<TViewModel>(TViewModel vm, ViewModelQueryContext<TViewModel> context)
    {
        _logger.ViewModelQueryCompleted(context.FluxOperationTypeName);
        return vm;
    }

    private FluxError HandleError<TViewModel>(FluxError error, ViewModelQueryContext<TViewModel> context)
    {
        context.MarkAsErrored(error);
        using (_logger.BeginRequestErrorLoggingScopes(FluxLogs.ViewModel_Unhandled_Error))
            _logger.ViewModelQueryErrored(context.FluxOperationTypeName, error);

        return error;
    }

    private UnhandledFluxError HandleException<TViewModel>(Exception ex, ViewModelQueryContext<TViewModel> context)
    {
        context.MarkAsErrored(ex);
        using (_logger.BeginRequestErrorLoggingScopes(FluxLogs.ViewModel_Unhandled_Error))
            _logger.ViewModelQueryErrored(context.FluxOperationTypeName, ex);

        return new UnhandledFluxError(ex, context);
    }
}
