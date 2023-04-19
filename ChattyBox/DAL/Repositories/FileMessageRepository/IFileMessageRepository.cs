using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
