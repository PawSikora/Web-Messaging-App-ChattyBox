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

        public TextMessage CreateTextMessage(string userEmail,string content, int chatId)
        {
            User user = _context.Users.SingleOrDefault(u => u.Email == userEmail)?? throw new Exception("Nie znaleziono uzytkownika");
            var chat = _context.Chats.SingleOrDefault(c => c.Id == chatId) ?? throw new Exception("Nie znaleziono czatu");
            TextMessage message = new TextMessage
            {
                Content = content,
                Sender = user,
                ChatId = chatId,
                TimeStamp = DateTime.Now
            };

            _context.TextMessages.Add(message);
            return message;
        }


        public void DeleteTextMessage(DateTime date, int senderId, int chatId)
        {
            var textMessage = _context.TextMessages.FirstOrDefault(c => c.ChatId == chatId && c.SenderId == senderId && c.TimeStamp == date) 
                ?? throw new Exception("Nie znaleziono wiadomosci");
          
            _context.TextMessages.Remove(textMessage);
        }

        public TextMessage GetTextMessage(int id)
        {
            var textMessage = _context.TextMessages.SingleOrDefault(t => t.Id == id) ?? throw new Exception("Nie znaleziono wiadomosci");
            return textMessage;
        }
    }
}
