using DAL.Database.Entities;
using DAL.Repositories.ChatRepository;

namespace UnitTests.DAL.DummyRepositories
{
    public class DummyChatRepository : IChatRepository
    {
        public Chat? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsUserInChat(int userId, int chatId)
        {
            throw new NotImplementedException();
        }

        public bool IsChatNameTaken(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserChat>? GetChatUsersById(int chatId)
        {
            throw new NotImplementedException();
        }

        public void RemoveUsersFromChat(IEnumerable<UserChat> chatUsers)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User>? GetUsersInChat(int chatId, int pageNumber, int usersPerPage)
        {
            throw new NotImplementedException();
        }

        public UserChat? GetUserChatById(int userId, int chatId)
        {
            throw new NotImplementedException();
        }

        public void AddUserToChat(UserChat userChat)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromChat(UserChat userChat)
        {
            throw new NotImplementedException();
        }

        public void CreateChat(Chat chat, UserChat userChat)
        {
            throw new NotImplementedException();
        }

        public void DeleteChat(Chat chat)
        {
            throw new NotImplementedException();
        }

        public Chat? GetChat(int id, int pageNumber, int messagesPerPage)
        {
            throw new NotImplementedException();
        }

        public int GetChatMessagesCount(int chatId)
        {
            throw new NotImplementedException();
        }

        public int GetChatUsersCount(int chatId)
        {
            throw new NotImplementedException();
        }

        public bool IsUserRole(int userId, int chatId, int roleId)
        {
            throw new NotImplementedException();
        }

        public Role? GetUserRole(int userId, int chatId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Chat> GetChatsForUser(int userId, int pageNumber, int chatsPerPage)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User>? GetAllUsers(int chatId)
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
