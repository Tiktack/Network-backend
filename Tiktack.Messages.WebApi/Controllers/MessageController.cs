using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Tiktack.Messaging.BusinessLayer.Providers;
using Tiktack.Messaging.DataAccessLayer.Entities;

namespace Tiktack.Messaging.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = "self")]
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IUserProvider _userProvider;
        private readonly MessageProvider _messageProvider;

        public MessageController(IUserProvider userProvider, MessageProvider messageProvider)
        {
            _userProvider = userProvider;
            _messageProvider = messageProvider;
        }

        [HttpGet("dialogMessages")]
        public async Task<IEnumerable<Message>> GetDialogMessages(int targetId)
        {
            return await _messageProvider.GetDialogMessagesByUserIdentifier(ControllerContext.HttpContext.User.Identity.Name,
                 targetId);
        }

        [HttpGet("getTest")]
        public async Task<string> GetTest()
        {
            await Task.Delay(12);
            return "cool";
        }
    }
}