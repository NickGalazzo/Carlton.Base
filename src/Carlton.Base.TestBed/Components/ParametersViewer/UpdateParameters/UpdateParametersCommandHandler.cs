﻿namespace Carlton.Base.TestBed;

public sealed class UpdateParametersCommandHandler : TestBedCommandRequestHandler<UpdateParametersCommand>
{
    public UpdateParametersCommandHandler(ICommandProcessor processor) : base(processor)
    {
    }
}
