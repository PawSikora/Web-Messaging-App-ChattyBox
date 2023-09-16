using BLL.DataTransferObjects;
using BLL.DataTransferObjects.UserDtos;
using DAL.Database.Entities;

namespace BLL.Services.UserService
{
    public interface IUserService
    {
        TokenToReturn LoginUser(LoginUserDTO dto);
        void RegisterUser(CreateUserDTO dto);
        UserDTO GetUser(int id);
        User GetUser(string email);
        IEnumerable<GetUserChatDTO> GetChats(int id, int pageNumber,int chatsPerPage);
        int GetUserChatsCount(int id);
        string GetRole(int userId, int chatId);
        string GenerateNewToken(User user);
    }
}
