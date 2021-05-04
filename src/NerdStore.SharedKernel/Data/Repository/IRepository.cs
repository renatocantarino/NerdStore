using NerdStore.SharedKernel.DomainObjects;
using System;

namespace NerdStore.SharedKernel.Data.Repository
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}