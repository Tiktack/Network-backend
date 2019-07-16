using System;
using Moq;
using System.Threading.Tasks;
using Tiktack.Common.DataAccessLayer.Repositories;
using Tiktack.Messaging.BusinessLayer.Services;
using Tiktack.Messaging.DataAccessLayer.Entities;
using Tiktack.Messaging.DataAccessLayer.Infrastructure;
using Xunit;

namespace Tiktack.Messaging.BusinessLayer.UnitTests
{
    public class MessageService
    {
        private readonly IMessageService _messageService;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        public MessageService()
        {
            var repository = new Mock<IRepository<Message>>();
            repository.Setup(x => x.Add(It.IsAny<Message>())).Returns<Message>(message =>
           {
               Console.WriteLine(message);
               return Task.FromResult(message);
           });
            _unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.SetupGet(x => x.Messages).Returns(repository.Object);

            _messageService = new Services.MessageService(_unitOfWork.Object);
        }

        [Fact]
        public async Task AddMessageShouldShouldReturnMessage()
        {
            //Arrange 
            var message = new Message
            {
                SenderId = "1",
                ReceiverId = "2",
                Text = "test message"
            };
            //Act
            var result = await _messageService.AddMessage("1", "2", "test message");

            //Assert
            _unitOfWork.VerifyAll();
            Assert.NotNull(result);
            Assert.Equal(message.SenderId, result.SenderId);
            Assert.Equal(message.ReceiverId, result.ReceiverId);
            Assert.Equal(message.Text, result.Text);
        }
    }
}
