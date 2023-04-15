using BLL.DataTransferObjects.UserDtos;

namespace WebApi.ViewModels
{
    public class ChatAndUsers
    {
        public IEnumerable<UserDTO> Users { get; set; }
        public int Count { get; set; }
        public int UsersPerPage { get; set; }
        public int ChatId { get; set; }
        public int PageNumber { get; set; }
        public string UserRole { get; set; }
        public int UserId { get; set; }

    }
}
