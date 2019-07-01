using Tiktack.Messaging.DataAccessLayer.Entities;

namespace Tiktack.Messaging.BusinessLayer.Services
{
    public interface IAccountService
    {
        string GenerateJwtToken(ApplicationUser user);
    }
}