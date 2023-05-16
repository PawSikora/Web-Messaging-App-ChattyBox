using BLL.DataTransferObjects.UserDtos;

namespace WebApi.ViewModels
{
    public class AddUserToChatTest
    {
        public int AdminId { get; set; }
        public int ChatId { get; set; }
        public string Email { get; set; }
        public UserDTO? userDto { get; set; }
    }
}
