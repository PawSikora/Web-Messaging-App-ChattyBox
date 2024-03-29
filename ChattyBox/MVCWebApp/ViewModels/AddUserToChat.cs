﻿using BLL.DataTransferObjects.UserDtos;

namespace MVCWebApp.ViewModels
{
    public class AddUserToChat
    {
        public int AdminId { get; set; }
        public int ChatId { get; set; }
        public string Email { get; set; }
        public UserDTO? userDto { get; set; }
        public IEnumerable<UserDTO>? UsersInChat { get; set; }
    }
}
