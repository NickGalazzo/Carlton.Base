﻿using Microsoft.AspNetCore.Components;

namespace Carlton.Core.Components.Flux.Contracts;

public interface IConnectedComponent<TViewModel>
{
    TViewModel ViewModel { get; set; }
    Func<Task<TViewModel>> GetViewModel { get; init; }
    EventCallback<MutationCommand> OnComponentEvent { get; init; }
}
