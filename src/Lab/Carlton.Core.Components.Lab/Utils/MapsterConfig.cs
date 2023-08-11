﻿namespace Carlton.Core.Components.Lab;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration()
    {
        TypeAdapterConfig<LabState, NavMenuViewModel>
            .NewConfig()
            .Map(dest => dest.SelectedItem, src => src.SelectedComponentState)
            .Map(dest => dest.MenuItems, src => src.ComponentStates);

        TypeAdapterConfig<LabState, ComponentViewerViewModel>
            .NewConfig()
            .Map(dest => dest.ComponentType, src => src.SelectedComponentType)
            .Map(dest => dest.ComponentParameters, src => src.SelectedComponentParameters);

        TypeAdapterConfig<LabState, EventConsoleViewModel>
            .NewConfig()
            .Map(dest => dest.RecordedEvents, src => src.ComponentEvents);

        TypeAdapterConfig<LabState, ParametersViewerViewModel>
            .NewConfig()
            .Map(dest => dest.ComponentParameters, src => src.SelectedComponentParameters);

        TypeAdapterConfig<LabState, BreadCrumbsViewModel>
            .NewConfig()
            .Map(dest => dest.SelectedComponentState, src => src.SelectedComponentState);

        TypeAdapterConfig<LabState, TestResultsViewModel>
            .NewConfig()
            .ConstructUsing(_ => new TestResultsViewModel(_.SelectedComponentTestReport));

        TypeAdapterConfig<LabState, SourceViewerViewModel>
            .NewConfig()
            .TwoWays()
            .Map(dest => dest.ComponentSource, src => src.SelectedComponentMarkup);

        TypeAdapterConfig<LabState, LabState>
         .NewConfig()
          .ConstructUsing(_ => new LabState(_.ComponentStates, _.ComponentTestResults));
    }

}

