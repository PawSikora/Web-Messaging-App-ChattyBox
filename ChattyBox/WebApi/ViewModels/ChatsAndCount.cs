using System.Security.Cryptography.Pkcs;
using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.MessageDtos;
using BLL.DataTransferObjects.UserDtos;

namespace WebApi.ViewModels
{
    public class ChatsAndCount
    {
        public ICollection<GetUserChatDTO> Chats { get; set; }
        public int Count { get; set; }
        public int ChatsPerPage { get; set; }
        public int UserId { get; set; }

    }
}
