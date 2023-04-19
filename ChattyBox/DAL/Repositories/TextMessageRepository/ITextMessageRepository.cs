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
        void CreateTextMessage(TextMessage message);
        void DeleteTextMessage(TextMessage message);
        TextMessage? GetLastTextMessage(int chatid);
        TextMessage? GetById(int id);
        void Save();
        void Dispose();
    }
}
