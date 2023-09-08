using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.UserDtos;
using BLL.Services.ChatService;

namespace UnitTests.BLL.MockServices
{
    public class ChatServiceMock : IChatService
    {
        List<UserDTO> _users = new List<UserDTO>()
        {
            new UserDTO { Id = 1, Username = "Mock1",Email ="testUser@mail1.com" },
            new UserDTO { Id = 2, Username = "Mock2",Email ="testUser@mail2.com" },
            new UserDTO { Id = 3, Username = "Mock3",Email ="testUser@mail3.com" },
            new UserDTO { Id = 4, Username = "Mock4",Email ="testUser@mail4.com" },

        };
        public IEnumerable<UserDTO> GetUsersInChat(int chatId, int pageNumber, int usersPerPage)
        {
            throw new NotImplementedException();
        }

        public void AddUserById(int userId, int chatId)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserById(int userId, int chatId)
        {
            throw new NotImplementedException();
        }

        public void CreateChat(CreateChatDTO dto)
        {
            throw new NotImplementedException();
        }

        public void DeleteChat(int chatId)
        {
            throw new NotImplementedException();
        }

        public GetChatDTO GetChat(int id, int pageNumber, int messagesPerPage)
        {
            throw new NotImplementedException();
        }

        public int GetChatMessagesCount(int id)
        {
            throw new NotImplementedException();
        }
        public int GetChatUsersCount(int id)
        {
            throw new NotImplementedException();
        }

        public void AssignRole(int userId, int chatId, int roleId)
        {
            throw new NotImplementedException();
        }

        public void RevokeRole(int userId, int chatId)
        {
            throw new NotImplementedException();
        }

        public string GetUserRole(int userId, int chatId)
        {
            throw new NotImplementedException();
        }

        public UserDTO GetUserByEmail(string email)
        {
            var user = _users.FirstOrDefault(x => x.Email == email);
            return user;
        }
    }
}
