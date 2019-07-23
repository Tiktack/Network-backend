using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Tiktack.Messaging.BusinessLayer.Services;
using Tiktack.Messaging.WebApi.DTOs;

namespace Tiktack.Messaging.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<string> Login(LoginDTO model) =>
            await _accountService.LoginWithCredentials(model.Email, model.Password);

        [HttpPost]
        [Authorize(AuthenticationSchemes = "auth0")]
        public async Task<string> LoginWithAuth() =>
            await _accountService.LoginWithExternalIdentifier(ControllerContext.HttpContext.User.Identity.Name);


        [HttpPost]
        public async Task<string> Register(RegisterDTO model) =>
            await _accountService.Register(model.Email, model.Password);
    }
}