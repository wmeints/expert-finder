﻿using MediatR;

namespace ExpertFinder.Shared;

public interface ICommandHandler<TCommand, TResponse>: IRequestHandler<TCommand, TResponse> where TCommand: ICommand<TResponse>
{
    
}

public interface ICommandHandler<TCommand>: IRequestHandler<TCommand> where TCommand: ICommand
{
    
}