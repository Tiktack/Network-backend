using System.Threading.Tasks;
using Tiktack.Messaging.DataAccessLayer.Entities;

namespace Tiktack.Messaging.BusinessLayer.Services
{
    public interface IAccountService
    {
        string GenerateJwtToken(ApplicationUser user);
        Task<string> LoginWithCredentials(string login, string password);
        Task<string> Register(string email, string password);
    }
}