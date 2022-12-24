﻿namespace Carlton.Base.TestBedFramework;

public record ComponentViewerViewModel(Type ComponentType, object ComponentViewModel, bool IsCarltonComponent);

public class ComponentViewerViewModelRequest : IRequest<ComponentViewerViewModel>
{
}

public class ComponentViewerViewModelRequestHandler : TestBedRequestHandlerViewModelBase<ComponentViewerViewModelRequest, ComponentViewerViewModel>
{

    public ComponentViewerViewModelRequestHandler(TestBedState state)
        : base(state)
    {
    }

    public override Task<ComponentViewerViewModel> Handle(ComponentViewerViewModelRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ComponentViewerViewModel
        (
            State.TestComponentType,
            State.TestComponentViewModel,
            State.IsTestComponentCarltonComponent
        ));
    }
}
