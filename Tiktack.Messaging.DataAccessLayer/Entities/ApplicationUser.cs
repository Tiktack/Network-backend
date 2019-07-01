using Microsoft.AspNetCore.Identity;
using Tiktack.Messaging.DataAccessLayer.Entities.UserProperties;

namespace Tiktack.Messaging.DataAccessLayer.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Avatar Avatar { get; set; }
    }
}
