﻿using Microsoft.Extensions.Logging;
namespace Carlton.Core.Flux.Debug.Models.Commands;

internal record ChangeEventLogLevelFiltersCommand
{
    [Required]
    public required LogLevel LogLevel { get; init; }
    [Required]
    public required bool IsIncluded { get; init; }
}
