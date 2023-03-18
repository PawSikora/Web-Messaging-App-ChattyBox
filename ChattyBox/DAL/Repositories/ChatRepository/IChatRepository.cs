using DAL.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.ChatRepository
{
    public interface IChatRepository
    {
        public void AddUserByEmail(string email, string chatName);
        public void AddMessage(Message message, string chatName);
        public IEnumerable<User> SortByName(string chatName);
        public void RemoveUser(string email, string chatName);
        public void RemoveMessage(Message message, string chatName);
        public Chat CreateChat(string name,User user);
        public void DeleteChat(string name);



    }
}
