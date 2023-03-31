using BLL.DataTransferObjects.MessageDtos;
using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
