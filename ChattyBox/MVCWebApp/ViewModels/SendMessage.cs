using BLL.DataTransferObjects.MessageDtos;

namespace MVCWebApp.ViewModels
{
    public class SendMessage
    {
        public CreateTextMessageDTO TextMessage { get; set; }
        public CreateFileMessageDTO FileMessage { get; set; }
    }
}
