using DAL.Database.Entities;

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
