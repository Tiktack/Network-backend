using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tiktack.Messaging.DataAccessLayer.Entities;
using Tiktack.Messaging.DataAccessLayer.Infrastructure;

namespace Tiktack.Messaging.BusinessLayer.Providers
{
    public class MessageProvider : IMessageProvider
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IUserProvider _userProvider;

        public MessageProvider(UnitOfWork unitOfWork, IUserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _userProvider = userProvider;
        }

        public async Task<Message> AddMessage(int callerId, int receiverId, string text)
        {
            var message = new Message
            {
                ReceiverId = receiverId,
                SenderId = callerId,
                Text = text,
                Timestamp = DateTime.Now
            };

            return await _unitOfWork.Messages.Add(message);
        }

        public async Task<IEnumerable<Message>> GetDialogMessagesByUserIdentifier(string userIdentifier, int targetId)
        {
            var id = await _userProvider.GetUserIdByIdentifier(userIdentifier);

            if (id != null)
                return _unitOfWork.Messages.GetAll(message =>
                    message.ReceiverId == id && message.SenderId == targetId ||
                    message.SenderId == id && message.ReceiverId == targetId);
            throw new ArgumentException("User Identifier is wrong");
        }

        public IEnumerable<Message> GetDialogMessagesById(int id, int targetId) =>
            _unitOfWork.Messages.GetAll(message =>
                message.ReceiverId == id && message.SenderId == targetId ||
                message.SenderId == id && message.ReceiverId == targetId);

        public IEnumerable<Message> GetDialogMessagesByIdWithPages(int id, int targetId, int page = 0)
        {
            var result = _unitOfWork.Messages.GetAll(message =>
               message.ReceiverId == id && message.SenderId == targetId ||
               message.SenderId == id && message.ReceiverId == targetId).ToList();
            var res = result.OrderByDescending(x => x.Id).Skip(page * 10).Take(10);
            return res;
        }
    }
}
