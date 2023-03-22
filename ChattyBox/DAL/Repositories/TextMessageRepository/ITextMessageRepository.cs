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
        TextMessage CreateTextMessage(string userEmail,string content, int chatId);
        void DeleteTextMessage(DateTime date, int senderId, int chatId);

        TextMessage GetTextMessage(int id);
    }
}
