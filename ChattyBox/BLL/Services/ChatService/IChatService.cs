using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.UserDtos;

namespace BLL.Services.ChatService
{
    public interface IChatService
    {
        IEnumerable<UserDTO> GetUsersInChat(int chatId, int pageNumber, int usersPerPage);
        public void AddUserById(int userId, int chatId);
        public void DeleteUserById(int userId, int chatId);
        void CreateChat(CreateChatDTO dto);
        void DeleteChat(int chatId);
        GetChatDTO GetChat(int id, int pageNumber,int messagesPerPage);
        int GetChatMessagesCount(int id);
        int GetChatUsersCount(int id);
        void AssignRole(int userId, int chatId, int roleId);
        void RevokeRole(int userId, int chatId);
        string GetUserRole(int userId, int chatId);
        UserDTO GetUserByEmail(string email);
    }
}
