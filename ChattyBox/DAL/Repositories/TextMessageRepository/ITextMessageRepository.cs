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
        TextMessage CreateTextMessage(string userEmail,string content);
        void DeleteTextMessage(DateTime date, User sender);
    }
}
