using DAL.UnitOfWork;
using UnitTests.DAL.DummyRepositories;

namespace UnitTests.DAL
{
    public class UnitOfWorkTest
    {
        [Fact]
        public void UnitOfWorkInit_ReturnsTrue_WhenRepositoriesAreSame()
        {
            // Arrange
            var userRepository = new DummyUserRepository();
            var chatRepository = new DummyChatRepository();
            var textMessageRepository = new DummyTextMessageRepository();
            var fileMessageRepository = new DummyFileMessageRepository();
            var roleRepository = new DummyRoleRepository();
            var unitOfWork = new UnitOfWork(chatRepository, fileMessageRepository, textMessageRepository, userRepository, roleRepository);
            
            // Act & Assert
            Assert.Same(userRepository, unitOfWork.Users);
            Assert.Same(chatRepository, unitOfWork.Chats);
            Assert.Same(textMessageRepository, unitOfWork.TextMessages);
            Assert.Same(fileMessageRepository, unitOfWork.FileMessages);
            Assert.Same(roleRepository, unitOfWork.Roles);
        }
    }
}
