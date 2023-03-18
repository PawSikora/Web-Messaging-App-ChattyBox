using DAL.Database;
using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.TextMessageRepository
{
    public class TextMessageRepository : ITextMessageRepository
    {
        private readonly DbChattyBox _context;
        public TextMessageRepository(DbChattyBox context) 
        {
            _context = context;
        }

        public TextMessage CreateTextMessage(string userEmail,string content)
        {
            User user = _context.Users.SingleOrDefault(u => u.Email == userEmail)?? throw new Exception("Nie znaleziono uzytkownika");
            
            TextMessage message = new TextMessage
            {
                Content = content,
                Sender = user,
                TimeStamp = DateTime.Now
            };
            return message;
        }


        public void DeleteTextMessage(DateTime date, User sender)
        {
            TextMessage textMessage= _context.Texts.Where(d=>d.TimeStamp==date).Where(u=>u.Sender==sender).FirstOrDefault() ?? throw new Exception("blad w usuwaniu wiadomosci");
            _context.Texts.Remove(textMessage);
        }
    }
}
