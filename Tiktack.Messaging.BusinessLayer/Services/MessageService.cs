using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tiktack.Messaging.DataAccessLayer.Entities;
using Tiktack.Messaging.DataAccessLayer.Infrastructure;

namespace Tiktack.Messaging.BusinessLayer.Services
{
    public class MessageService : IMessageService
    {
        private readonly UnitOfWork _unitOfWork;

        public MessageService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Message> AddMessage(string callerId, string receiverId, string text)
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

        public async Task<IEnumerable<Message>> GetMessages(string callerId, string receiverId)
        {
            return _unitOfWork.Messages.GetAll().Where(message =>
                message.SenderId == callerId && message.ReceiverId == receiverId ||
                message.SenderId == receiverId && message.ReceiverId == callerId);
        }
    }
}
