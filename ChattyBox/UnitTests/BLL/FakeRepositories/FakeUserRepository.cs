using DAL.Database.Entities;
using DAL.Repositories.UserRepository;

namespace UnitTests.BLL.FakeRepositories
{
    public class FakeUserRepository : IUserRepository
    {
        private List<User> _users = new List<User>();
        //private readonly List<UserChat> _userChats = new List<UserChat>();

        public void CreateUser(User user)
        {
            _users.Add(user);
        }

        public int GetUserChatsCount(int id)
        {
            return _users.SingleOrDefault(u => u.Id == id).UserChats.Count;
            //return _userChats.Count(x => x.UserId == id);
        }

        public User GetById(int id)
        {
            return _users.SingleOrDefault(u => u.Id == id);
        }

        public User GetUserByEmail(string email)
        {
            return _users.SingleOrDefault(x => x.Email == email);
        }

        public bool IsEmailTaken(string email)
        {
            return _users.Any(x => x.Email == email);
        }

        public void Save()
        {
            // Do nothing.
        }

        public void Dispose()
        {
            // Do nothing.
        }
    }
}
