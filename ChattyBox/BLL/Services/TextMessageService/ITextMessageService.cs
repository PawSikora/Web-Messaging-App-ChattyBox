using BLL.DataTransferObjects.MessageDtos;

namespace BLL.Services.TextMessageService
{
    public interface ITextMessageService
    {
        void CreateTextMessage(CreateTextMessageDTO dto);
        void DeleteTextMessage(int id);
        TextMessageDTO GetTextMessage(int id);
        GetNewestMessageDTO GetLastTextMessage(int chatId);
    }
}
