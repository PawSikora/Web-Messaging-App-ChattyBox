using DAL.Database.Entities;
using DAL.Repositories.FileMessageRepository;

namespace UnitTests.DAL.DummyRepositories
{
    public class DummyFileMessageRepository : IFileMessageRepository
    {
        public void CreateFileMessage(FileMessage fileMessage)
        {
            throw new NotImplementedException();
        }

        public void DeleteFileMessage(FileMessage fileMessage)
        {
            throw new NotImplementedException();
        }

        public FileMessage? GetLastFileMessage(int chatid)
        {
            throw new NotImplementedException();
        }

        public FileMessage? GetById(int chatid)
        {
            throw new NotImplementedException();
        }

        public bool IsFileNameTaken(string fileName)
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
