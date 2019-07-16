using System;
using Tiktack.Common.DataAccessLayer.Repositories;
using Tiktack.Messaging.DataAccessLayer.Entities;

namespace Tiktack.Messaging.DataAccessLayer.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Message> Messages { get; }
    }
}