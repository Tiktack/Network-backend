using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tiktack.Messaging.BusinessLayer.Providers;
using Tiktack.Messaging.WebApi.DTOs;
using Tiktack.Messaging.WebApi.Hubs;

namespace Tiktack.Messaging.WebApi.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserProvider _userProvider;
        private readonly IMapper _mapper;
        private readonly RequestProvider _requestProvider;


        public AuthenticationController(IUserProvider userProvider, IMapper mapper, RequestProvider requestProvider)
        {
            _userProvider = userProvider;
            _mapper = mapper;
            _requestProvider = requestProvider;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]UserFormInputDTO userDto)
        {
            var (user, token) = await _userProvider.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });


            var mappedUser = _mapper.Map<UserDTO>(user);
            mappedUser.Token = token;

            return Ok(mappedUser);
        }

        [Authorize(AuthenticationSchemes = "auth0")]
        [HttpGet("authenticateWithExternals")]
        public async Task<UserDTO> AuthenticateWithExternals()
        {
            var userIdentifier = ControllerContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var accessToken = _requestProvider.Token;

            var (user, token) = await _userProvider.GenerateToken(userIdentifier, accessToken);
            var mappedUser = _mapper.Map<UserDTO>(user);
            mappedUser.Token = token;

            return mappedUser;
        }

        [Authorize(AuthenticationSchemes = "self")]
        [HttpGet("authenticateWithToken")]
        public async Task<UserDTO> AuthenticateWithToken()
        {
            var userIdentifier = ControllerContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            var user = await _userProvider.GetUserByIdentifier(userIdentifier);
            var mappedUser = _mapper.Map<UserDTO>(user);
            mappedUser.Token = _requestProvider.Token;

            return mappedUser;
        }
    }
}