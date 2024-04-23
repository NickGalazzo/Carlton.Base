using Carlton.Core.Flux.Debug.Extensions;
namespace Carlton.Core.Flux.Debug.State;

internal sealed record FluxDebugState
{
	private readonly IFluxStateWrapper _wrapper;

	//Logs
	public IReadOnlyList<FluxDebugLogMessage> LogMessages { get; init; } = new List<FluxDebugLogMessage>();
	public IReadOnlyList<TraceLogMessageGroup> TraceLogMessageGroups => LogMessages.MapLogMessagesToTraceLogMessage().ToList();

	//Selected Log Messages
	public int? SelectedLogMessageIndex { get; init; }
	public int? SelectedTraceLogMessageIndex { get; init; }

	public FluxDebugLogMessage? SelectedLogMessage => LogMessages.ElementAtOrDefault(SelectedLogMessageIndex ?? -1);
	public FluxDebugLogMessage? SelectedTraceLogMessage => LogMessages.ElementAtOrDefault(SelectedTraceLogMessageIndex ?? -1);

	//Main Application State
	public object State => _wrapper.State;

	public IEnumerable<Type> ViewModelTypes { get; private init; } = new List<Type>();

	public FluxDebugState(FluxTypes viewModelTypes, IFluxStateWrapper wrapper)
		=> (ViewModelTypes, _wrapper) = (viewModelTypes.ViewModelTypes, wrapper);
}

