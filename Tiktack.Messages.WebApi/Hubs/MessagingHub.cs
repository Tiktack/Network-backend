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
    [Authorize]
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
            var userIdentifier = await _userProvider.GetUserIdentifierById(targetId);
            var callerId = await _userProvider.GetUserIdByIdentifier(Context.UserIdentifier);

            var message = await _messageProvider.AddMessage((int)callerId, targetId, text);

            await Clients.Caller.SendAsync("UpdateDialog", message);
            await Clients.User(userIdentifier).SendAsync("UpdateDialog", message);
        }

        public async Task GetDialogMessages(int targetId)
        {
            await Clients.Caller.SendAsync("GetDialogMessages", await _messageProvider.GetDialogMessagesByUserIdentifier(Context.UserIdentifier, targetId));
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _userProvider.GetUserIdByIdentifier(Context.UserIdentifier);
            if (user == null)
            {
                var apiClient = new AuthenticationApiClient(new Uri("https://dev-vvcaaesb.eu.auth0.com/userinfo"));

                var userInfo = await apiClient.GetUserInfoAsync(_requestProvider.Token);

                var mappedUser = _mapper.Map<UserInfoDBLayer>(userInfo);

                await _userProvider.Create(mappedUser);
            }

            await base.OnConnectedAsync();
        }
    }
}
