using DAL.Database;
using DAL.Database.Entities;
using DAL.Exceptions;
using Microsoft.EntityFrameworkCore;
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

        public void CreateTextMessage(int userId,string content, int chatId)
        {
            User user = _context.Users.SingleOrDefault(u => u.Id == userId)?? throw new NotFoundException("Nie znaleziono uzytkownika");
            var chat = _context.Chats.SingleOrDefault(c => c.Id == chatId) ?? throw new NotFoundException("Nie znaleziono czatu");
            TextMessage message = new TextMessage
            {
                Content = content,
                Sender = user,
                ChatId = chatId,
                TimeStamp = DateTime.Now
            };

            _context.TextMessages.Add(message);
        }

        public void DeleteTextMessage(int id)
        {
            var textMessage = _context.TextMessages.FirstOrDefault(c => c.Id == id) 
                ?? throw new NotFoundException("Nie znaleziono wiadomosci");
          
            _context.TextMessages.Remove(textMessage);
        }

        public TextMessage GetTextMessage(int id)
        {
            var textMessage = _context.TextMessages.SingleOrDefault(t => t.Id == id) ?? throw new NotFoundException("Nie znaleziono wiadomosci");
            return textMessage;
        }

        public TextMessage GetLastTextMessage(int chatid)
        {
            var message = _context.TextMessages
                .Include(m => m.Sender)
                .Where(m => m.ChatId == chatid)
                .OrderByDescending(m => m.TimeStamp)
                .FirstOrDefault() ?? throw new NotFoundException("Nie znaleziono czatu");

            return message;
        }

    }
}
