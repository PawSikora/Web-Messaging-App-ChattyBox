using DAL.Database.Entities;

namespace DAL.Repositories.FileMessageRepository
{
    public interface IFileMessageRepository
    {
        void CreateFileMessage(FileMessage fileMessage);
        void DeleteFileMessage(FileMessage fileMessage);
        FileMessage? GetLastFileMessage(int chatid);
        FileMessage? GetById(int chatid);
        bool IsFileNameTaken(string fileName);
        void Save();
        void Dispose();
    }
}
