using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Tiktack.Messaging.BusinessLayer.Providers;
using Tiktack.Messaging.DataAccessLayer.Entities;

namespace Tiktack.Messaging.WebApi.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserProvider _userProvider;

        public UserController(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        [HttpGet("addorupdate")]
        public async Task AddOrUpdate()
        {
            var a = await _userProvider.AddOrUpdate(new UserInfoDBLayer
            {
                UserIdentifier = "2",
                Email = "asdsa",
                Name = "asdas",
                PictureUrl = "wefw"
            });
        }
        [HttpGet("getall")]
        public IEnumerable<UserInfoDBLayer> GetAll()
        {
            return _userProvider.GetAllUsers();
        }

        [HttpGet("details")]
        public async Task<UserInfoDBLayer> GetDetails()
        {
            var context = ControllerContext.HttpContext.User;
            return new UserInfoDBLayer();
        }
    }
}