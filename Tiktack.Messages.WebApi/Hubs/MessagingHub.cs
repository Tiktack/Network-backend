using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Tiktack.Messaging.BusinessLayer.Providers;

namespace Tiktack.Messaging.WebApi.Hubs
{
    [Authorize(AuthenticationSchemes = "self")]
    public class MessagingHub : Hub
    {
        private readonly IUserProvider _userProvider;
        private readonly IMessageProvider _messageProvider;
        private readonly RequestProvider _requestProvider;
        private readonly IMapper _mapper;

        public MessagingHub(IUserProvider userProvider, IMessageProvider messageProvider, RequestProvider requestProvider, IMapper mapper)
        {
            _userProvider = userProvider;
            _messageProvider = messageProvider;
            _requestProvider = requestProvider;
            _mapper = mapper;
        }

        public async Task SendDirect(int targetId, string text)
        {
            var message = await _messageProvider.AddMessage(int.Parse(Context.UserIdentifier), targetId, text);

            await Clients.Caller.SendAsync("MessageSent", message);
            await Clients.User(targetId.ToString()).SendAsync("UpdateDialog", message);
        }

        public async Task GetDialogMessages(int targetId)
        {
            await Clients.Caller.SendAsync("GetDialogMessages", _messageProvider.GetDialogMessagesById(int.Parse(Context.UserIdentifier), targetId));
        }

        public async Task GetDialogMessagesWithPage(int targetId, int page)
        {
            await Clients.Caller.SendAsync("GetDialogMessagesWithPage", _messageProvider.GetDialogMessagesByIdWithPages(int.Parse(Context.UserIdentifier), targetId, page));
        }

    }
}
