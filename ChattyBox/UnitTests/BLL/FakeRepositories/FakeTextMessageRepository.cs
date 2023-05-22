using DAL.Database.Entities;
using DAL.Repositories.TextMessageRepository;

namespace UnitTests.BLL.FakeRepositories
{
    public class FakeTextMessageRepository : ITextMessageRepository
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
            //notImplemented
        }

        public void Dispose()
        {
            //notImplemented
        }
    }
}
