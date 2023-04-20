using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.UnitOfWork;
using UnitTests.DAL.DummyRepositories;

namespace UnitTests.DAL
{
    public class UnitOfWorkTest
    {
        [Fact]

        public void UnitOfWorkInit_ReturnsTrue_WhenRepositoriesAreSame()
        {
            var userRepository = new DummyUserRepository();
            var chatRepository = new DummyChatRepository();
            var textMessageRepository = new DummyTextMessageRepository();
            var fileMessageRepository = new DummyFileMessageRepository();
            var roleRepository = new DummyRoleRepository();
            var unitOfWork = new UnitOfWork(chatRepository, fileMessageRepository, textMessageRepository, userRepository, roleRepository);
            
            Assert.Same(userRepository, unitOfWork.Users);
            Assert.Same(chatRepository, unitOfWork.Chats);
            Assert.Same(textMessageRepository, unitOfWork.TextMessages);
            Assert.Same(fileMessageRepository, unitOfWork.FileMessages);
            Assert.Same(roleRepository, unitOfWork.Roles);
        }
    }
}
