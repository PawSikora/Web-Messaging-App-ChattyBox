using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Database.Entities;
using DAL.Repositories.FileMessageRepository;

namespace UnitTests.BLL.FakeRepositories
{
    public class FakeFileMessageRepository : IFileMessageRepository
    {
        private List<FileMessage> _fileMessages = new List<FileMessage>();
        public void CreateFileMessage(FileMessage fileMessage)
        {
            this._fileMessages.Add(fileMessage);
        }

        public void DeleteFileMessage(FileMessage fileMessage)
        {
            var index = this._fileMessages.FindIndex(x => x.Id == fileMessage.Id);
            this._fileMessages.RemoveAt(index);
        }

        public FileMessage? GetLastFileMessage(int chatid)
        {
            FileMessage lastMessage = null;
            foreach (var message in _fileMessages)
            {
                if (message.TimeStamp > lastMessage.TimeStamp)
                    lastMessage = message;
            }
            return lastMessage;
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
            //notImplemented
        }

        public void Dispose()
        {
            //notImplemented
        }
    }
}
