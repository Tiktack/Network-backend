using System.Collections.Generic;
using System.Threading.Tasks;
using Tiktack.Messaging.DataAccessLayer.Entities;

namespace Tiktack.Messaging.BusinessLayer.Services
{
    public interface IMessageService
    {
        Task<Message> AddMessage(string callerId, string receiverId, string text);
        Task<IEnumerable<Message>> GetMessages(string callerId, string receiverId);
    }
}