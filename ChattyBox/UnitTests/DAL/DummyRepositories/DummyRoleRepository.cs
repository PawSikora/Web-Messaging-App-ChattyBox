using DAL.Database.Entities;
using DAL.Repositories.RoleRepository;

namespace UnitTests.DAL.DummyRepositories
{
    public class DummyRoleRepository : IRoleRepository
    {
        public void CreateRole(Role role)
        {
            throw new NotImplementedException();
        }

        public void DeleteRole(Role role)
        {
            throw new NotImplementedException();
        }

        public Role? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Role? GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool RoleExists(string name)
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
