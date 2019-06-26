using System.Collections.Generic;
using System.Threading.Tasks;
using Tiktack.Messaging.DataAccessLayer.Entities;

namespace Tiktack.Messaging.BusinessLayer.Providers
{
    public interface IMessageProvider
    {
        Task<Message> AddMessage(int callerId, int receiverId, string text);
        Task<IEnumerable<Message>> GetDialogMessagesByUserIdentifier(string userIdentifier, int targetId);
        IEnumerable<Message> GetDialogMessagesById(int id, int targetId);
        IEnumerable<Message> GetDialogMessagesByIdWithPages(int id, int targetId, int page = 0);
    }
}