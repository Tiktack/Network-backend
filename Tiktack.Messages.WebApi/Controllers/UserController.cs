using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tiktack.Messaging.BusinessLayer.Providers;
using Tiktack.Messaging.DataAccessLayer.Entities;
using Tiktack.Messaging.WebApi.Hubs;

namespace Tiktack.Messaging.WebApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserProvider _userProvider;
        private readonly IMapper _mapper;
        private readonly RequestProvider _requestProvider;


        public UserController(IUserProvider userProvider, IMapper mapper, RequestProvider requestProvider)
        {
            _userProvider = userProvider;
            _mapper = mapper;
            _requestProvider = requestProvider;
        }

        [HttpGet("getall")]
        public IEnumerable<UserInfoDBLayer> GetAll()
        {
            return _userProvider.GetAllUsers();
        }

        //[HttpPut]
        //public async Task UpdateUser([FromBody] user)
        //{

        //}
    }
}