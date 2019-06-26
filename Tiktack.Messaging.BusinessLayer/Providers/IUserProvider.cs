using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tiktack.Messaging.DataAccessLayer.Entities;

namespace Tiktack.Messaging.BusinessLayer.Providers
{
    public interface IUserProvider
    {
        Task<UserInfoDBLayer> AddOrUpdate(UserInfoDBLayer user);
        IEnumerable<UserInfoDBLayer> GetAllUsers();
        Task<string> GetUserIdentifierById(int id);
        Task<int?> GetUserIdByIdentifier(string identifier);
        Task<UserInfoDBLayer> Create(UserInfoDBLayer user);
        Task<Tuple<UserInfoDBLayer, string>> Authenticate(string username, string password);
        Task<Tuple<UserInfoDBLayer, string>> GenerateToken(string userIdentifier, string accessToken);
        Task<UserInfoDBLayer> GetUserByIdentifier(string identifier);
    }
}