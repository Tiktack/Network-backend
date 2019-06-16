using Auth0.AuthenticationApi;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Tiktack.Messaging.BusinessLayer.Providers;
using Tiktack.Messaging.DataAccessLayer.Entities;

namespace Tiktack.Messaging.WebApi.Hubs
{
    [Authorize(AuthenticationSchemes = "self")]
    public class MessagingHub : Hub
    {
        private readonly IUserProvider _userProvider;
        private readonly MessageProvider _messageProvider;
        private readonly RequestProvider _requestProvider;
        private readonly IMapper _mapper;

        public MessagingHub(IUserProvider userProvider, MessageProvider messageProvider, RequestProvider requestProvider, IMapper mapper)
        {
            _userProvider = userProvider;
            _messageProvider = messageProvider;
            _requestProvider = requestProvider;
            _mapper = mapper;
        }

        public async Task SendDirect(int targetId, string text)
        {
            //var userIdentifier = await _userProvider.GetUserIdentifierById(targetId);
            //var callerId = await _userProvider.GetUserIdByIdentifier(Context.UserIdentifier);

            var message = await _messageProvider.AddMessage(int.Parse(Context.UserIdentifier), targetId, text);

            await Clients.Caller.SendAsync("UpdateDialog", message);
            await Clients.User(targetId.ToString()).SendAsync("UpdateDialog", message);
        }

        public async Task GetDialogMessages(int targetId)
        {
            await Clients.Caller.SendAsync("GetDialogMessages", await _messageProvider.GetDialogMessagesByUserIdentifier(Context.UserIdentifier, targetId));
        }
    }
}
