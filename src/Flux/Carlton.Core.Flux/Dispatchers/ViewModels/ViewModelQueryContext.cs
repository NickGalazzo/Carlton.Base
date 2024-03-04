﻿namespace Carlton.Core.Flux.Dispatchers.ViewModels;

public class ViewModelQueryContext<TViewModel> : BaseRequestContext
{
    public TViewModel ResultViewModel { get; private set; }
    public string ViewModelTypeName { get => typeof(TViewModel).GetDisplayName(); }
    public bool StateModifiedByHttpRefresh { get; private set; }

    internal void MarkAsStateModifiedByHttpRefresh()
        => StateModifiedByHttpRefresh = true;

    internal void MarkAsSucceeded(TViewModel resultViewModel)
    {
        ResultViewModel = resultViewModel;
        MarkAsSucceeded();
    }
}