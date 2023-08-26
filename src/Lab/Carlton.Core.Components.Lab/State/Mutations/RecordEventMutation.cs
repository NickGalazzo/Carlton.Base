﻿using Carlton.Core.Components.Flux.State;

namespace Carlton.Core.Components.Lab.State.Mutations;


public class RecordEventMutation : IFluxStateMutation<LabState, RecordEventCommand>
{
    public string StateEvent => LabStateEvents.EventRecorded.ToString();

    public LabState Mutate(LabState currentState, RecordEventCommand command)
    {
        var newEvents = currentState.ComponentEvents.Append(new ComponentRecordedEvent(command.RecordedEventName, command.EventArgs));
        return currentState with { ComponentEvents = newEvents };
    }
}