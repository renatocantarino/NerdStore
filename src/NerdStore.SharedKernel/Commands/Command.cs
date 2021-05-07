using FluentValidation.Results;
using MediatR;
using NerdStore.SharedKernel.Messages;
using System;

namespace NerdStore.SharedKernel.Commands
{
    public abstract class Command : Message, IRequest<ResponseBase>
    {
        public ValidationResult ValidationResult { get; set; }

        public virtual bool IsValid() => throw new NotImplementedException();
    }
}