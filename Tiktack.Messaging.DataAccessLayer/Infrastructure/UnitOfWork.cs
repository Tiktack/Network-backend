using Microsoft.EntityFrameworkCore;
using System;
using Tiktack.Common.DataAccessLayer.Repositories;
using Tiktack.Messaging.DataAccessLayer.Entities;

namespace Tiktack.Messaging.DataAccessLayer.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MessagingDBContext _context;

        private bool _disposed;

        public UnitOfWork(DbContext context, IRepository<Message> messages)
        {
            _context = (MessagingDBContext)context;
            Messages = messages;
        }

        public IRepository<Message> Messages { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing) _context.Dispose();

            _disposed = true;
        }
    }
}