using DAL.Database;
using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.TextMessageRepository
{
    
    public interface ITextMessageRepository
    {
        void CreateTextMessage(int userId, string content, int chatId);
        void DeleteTextMessage(int id);

        TextMessage GetTextMessage(int id);
    }
}
