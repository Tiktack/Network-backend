using Microsoft.EntityFrameworkCore;
using Tiktack.Messaging.DataAccessLayer.Entities;

namespace Tiktack.Messaging.DataAccessLayer.Infrastructure
{
    public class MessagingDBContext : DbContext
    {
        public MessagingDBContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<UserInfoDBLayer> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
