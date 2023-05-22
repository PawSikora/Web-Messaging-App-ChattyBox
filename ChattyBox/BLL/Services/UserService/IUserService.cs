using BLL.DataTransferObjects.UserDtos;

namespace BLL.Services.UserService
{
    public interface IUserService
    {
        UserDTO LoginUser(LoginUserDTO dto);
        void RegisterUser(CreateUserDTO dto);
        UserDTO GetUser(int id);
        IEnumerable<GetUserChatDTO> GetChats(int id, int pageNumber,int chatsPerPage);
        int GetUserChatsCount(int id);
        string GetRole(int userId, int chatId);
        
    }
}
