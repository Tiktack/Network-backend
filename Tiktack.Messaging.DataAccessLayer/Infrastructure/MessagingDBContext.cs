using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tiktack.Messaging.DataAccessLayer.Entities;

namespace Tiktack.Messaging.DataAccessLayer.Infrastructure
{
    public class MessagingDBContext : IdentityDbContext<ApplicationUser>
    {
        public MessagingDBContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Message> Messages { get; set; }
    }
}
