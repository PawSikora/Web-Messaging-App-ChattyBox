﻿using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.MessageDtos;

namespace WebApi.ViewModels
{
    public class MessagesAndCount
    {
        public GetChatDTO Chat { get; set; }
        public int Count { get; set; }
        public int MessagesPerPage { get; set; }
        public int PageNumber { get; set; }
        public int UserId { get; set; }
        public string UserRole { get; set; }
        public CreateTextMessageDTO? CreateTextMessage { get; set; }
    }
}
