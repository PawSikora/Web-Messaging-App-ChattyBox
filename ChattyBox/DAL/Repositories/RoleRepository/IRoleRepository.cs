using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Database.Entities;

namespace DAL.Repositories.RoleRepository
{
    public interface IRoleRepository
    {
        void CreateRole(Role role);
        void DeleteRole(Role role);
        Role? GetById(int id);
        Role? GetByName(string name);

        bool RoleExists(string name);
        void Save();
        void Dispose();
    }
}
