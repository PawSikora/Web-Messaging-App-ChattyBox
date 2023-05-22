using DAL.Database.Entities;
using DAL.Repositories.RoleRepository;

namespace UnitTests.BLL.FakeRepositories
{
    public class FakeRoleRepository : IRoleRepository
    {
        private List<Role> _roles = new List<Role>();
        public void CreateRole(Role role)
        {
            _roles.Add(role);
        }

        public void DeleteRole(Role role)
        {
            throw new NotImplementedException();
        }

        public Role? GetById(int id)
        {
            return _roles.FirstOrDefault(x => x.Id == id);
        }

        public Role? GetByName(string name)
        {
            return _roles.FirstOrDefault(x => x.Name == name);
        }

        public bool RoleExists(string name)
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
