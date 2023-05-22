using BLL.DataTransferObjects.UserDtos;

namespace MVCWebApp.ViewModels
{
    public class ChatsAndCount
    {
        public IEnumerable<GetUserChatDTO> Chats { get; set; }
        public int Count { get; set; }
        public int ChatsPerPage { get; set; }
        public int UserId { get; set; }

    }
}
