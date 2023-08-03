using BLL.DataTransferObjects;
using BLL.DataTransferObjects.UserDtos;
using BLL.Services.UserService;
using DAL.Database.Entities;

namespace UnitTests.BLL.MockServices
{
    public class UserServiceBllMock : IUserService
    {
        List<UserDTO> _users=new List<UserDTO>()
        {
            new UserDTO { Id = 1, Username = "Mock1" },
            new UserDTO { Id = 2, Username = "Mock2" },
            new UserDTO { Id = 3, Username = "Mock3" },
        };
        public TokenToReturn LoginUser(LoginUserDTO dto)
        {
            throw new NotImplementedException();
        }

        public void RegisterUser(CreateUserDTO dto)
        {
            throw new NotImplementedException();
        }

        public UserDTO GetUser(int id)
        {
            var user = _users.FirstOrDefault(x => x.Id == id);
            return user;
        }

        public IEnumerable<GetUserChatDTO> GetChats(int id, int pageNumber, int chatsPerPage)
        {
            throw new NotImplementedException();
        }

        public int GetUserChatsCount(int id)
        {
            throw new NotImplementedException();
        }

        public string GetRole(int userId, int chatId)
        {
            throw new NotImplementedException();
        }

        public string GenerateNewToken(User user)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string email)
        {
            throw new NotImplementedException();
        }
    }
}
