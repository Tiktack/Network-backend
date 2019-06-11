using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;
using Tiktack.Messaging.DataAccessLayer.Entities;
using Tiktack.Messaging.DataAccessLayer.Infrastructure;

namespace Tiktack.Messaging.BusinessLayer.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly UnitOfWork _unitOfWork;

        public UserProvider(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserInfoDBLayer> AddOrUpdate(UserInfoDBLayer user)
        {
            var userInfoDBLayer =
                await _unitOfWork.Users.GetAll().FirstOrDefaultAsync(x => x.UserIdentifier == user.UserIdentifier);

            if (userInfoDBLayer == null)
                return await _unitOfWork.Users.Add(user);

            if (user == userInfoDBLayer)
                return user;

            user.Id = userInfoDBLayer.Id;
            return await _unitOfWork.Users.Update(user);
        }

        public async Task<UserInfoDBLayer> Create(UserInfoDBLayer user)
        {
            return await _unitOfWork.Users.Add(user);
        }

        public IEnumerable<UserInfoDBLayer> GetAllUsers()
        {
            return _unitOfWork.Users.GetAll();
        }

        public async Task<string> GetUserIdentifierById(int id)
        {
            var user = await _unitOfWork.Users.GetById(id);
            return user.UserIdentifier;
        }

        public async Task<int?> GetUserIdByIdentifier(string identifier)
        {
            var user = await _unitOfWork.Users.GetAll().FirstOrDefaultAsync(x => x.UserIdentifier == identifier);
            return user?.Id;
        }
    }
}
