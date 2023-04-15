using BLL.DataTransferObjects.UserDtos;
using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
