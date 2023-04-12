using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.UserDtos;
using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ChatService
{
    public interface IChatService
    {
        IEnumerable<UserDTO> GetUsersInChat(int chatId);
        public void AddUserById(int userId, int chatId);
        public void DeleteUserById(int userId, int chatId);
        void CreateChat(CreateChatDTO dto);
        void DeleteChat(int chatId);
        GetChatDTO GetChat(int id, int pageNumber,int messagePerPage);
        int GetChatMessagesCount(int id);
        void AssignRole(int userId, int chatId, int roleId);
        void RevokeRole(int userId, int chatId);
        string GetUserRole(int userId, int chatId);
        UserDTO GetUserByEmail(string email);
    }
}
