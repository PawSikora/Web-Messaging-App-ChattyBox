using BLL.DataTransferObjects.MessageDtos;
using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
