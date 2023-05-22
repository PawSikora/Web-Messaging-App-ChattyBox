using BLL.DataTransferObjects.MessageDtos;

namespace BLL.Services.FileMessageService
{
    public interface IFileMessageService
    {
        void CreateFileMessage(CreateFileMessageDTO dto);
        void DeleteFileMessage(int id);
        FileMessageDTO GetFileMessage(int id);
        GetNewestMessageDTO GetLastFileMessage(int chatId);
    }
}
