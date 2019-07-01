using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Tiktack.Messaging.BusinessLayer.Services;

namespace Tiktack.Messaging.WebApi.Hubs
{
    [Authorize(AuthenticationSchemes = "identity")]
    public class MessagingHub : Hub
    {
        private readonly IMessageService _messageService;

        public MessagingHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendDirect(string targetId, string text)
        {
            var message = await _messageService.AddMessage(Context.UserIdentifier, targetId, text);

            await Clients.Caller.SendAsync("MessageSent", message);
            await Clients.User(targetId).SendAsync("UpdateDialog", message);
        }

        public async Task GetDialogMessages(string targetId)
        {
            await Clients.Caller.SendAsync("GetDialogMessages", await _messageService.GetMessages(Context.UserIdentifier, targetId));
        }

        //public async Task GetDialogMessagesWithPage(int targetId, int page)
        //{
        //    await Clients.Caller.SendAsync("GetDialogMessagesWithPage", _messageService.GetDialogMessagesByIdWithPages(int.Parse(Context.UserIdentifier), targetId, page));
        //}

    }
}
