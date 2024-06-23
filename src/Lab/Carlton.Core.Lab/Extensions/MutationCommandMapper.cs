﻿using Carlton.Core.Components.Accordion.AccordionSelectGroup;
using Carlton.Core.Components.DynamicComponents;
using Carlton.Core.Lab.Components.ParametersViewer;
using Riok.Mapperly.Abstractions;
namespace Carlton.Core.Lab.Extensions;

[Mapper]
internal partial class MutationCommandMapper
{
	internal static partial TCommand Map<TCommand>(object args);

	[MapProperty(nameof(AccordionSelectGroupItemChangedEventArgs<ComponentState>.GroupIndexId), nameof(SelectMenuItemCommand.ComponentIndex))]
	[MapProperty(nameof(AccordionSelectGroupItemChangedEventArgs<ComponentState>.ItemIndexId), nameof(SelectMenuItemCommand.ComponentStateIndex))]
	internal static partial SelectMenuItemCommand ToCommand(AccordionSelectGroupItemChangedEventArgs<ComponentState> args);

	[MapProperty(nameof(CapturedComponentEventArgs.EventName), nameof(RecordEventCommand.RecordedEventName))]
	[MapProperty(nameof(CapturedComponentEventArgs.EventArgs), nameof(RecordEventCommand.EventArgs))]
	internal static partial RecordEventCommand ToCommand(CapturedComponentEventArgs args);

	[MapProperty(nameof(OnParametersChangedArgs.UpdatedParameters), nameof(UpdateParametersCommand.Parameters))]
	internal static partial UpdateParametersCommand ToUpdateParametersCommand(OnParametersChangedArgs args);

	internal static ClearEventsCommand ToClearEventsCommand(object args) => new();
}


