using DAL.Database.Entities;
using DAL.Repositories.TextMessageRepository;

namespace UnitTests.DAL.DummyRepositories
{
    public class DummyTextMessageRepository : ITextMessageRepository
    {
        public void CreateTextMessage(TextMessage message)
        {
            throw new NotImplementedException();
        }

        public void DeleteTextMessage(TextMessage message)
        {
            throw new NotImplementedException();
        }

        public TextMessage? GetLastTextMessage(int chatid)
        {
            throw new NotImplementedException();
        }

        public TextMessage? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
