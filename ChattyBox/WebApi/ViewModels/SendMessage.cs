using BLL.DataTransferObjects.MessageDtos;

namespace WebApi.ViewModels
{
    public class SendMessage
    {
        public CreateTextMessageDTO TextMessage { get; set; }
        public CreateFileMessageDTO FileMessage { get; set; }
    }
}
