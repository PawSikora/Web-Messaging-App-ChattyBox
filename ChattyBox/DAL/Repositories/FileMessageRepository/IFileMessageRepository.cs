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
        FileMessage CreateFileMessage(int userId, string path, int chatId);
        void DeleteFileMessage(int id);
        FileMessage GetFileMessage(int id);
        FileMessage GetLastFileMessage(int chatid);
    }
}
