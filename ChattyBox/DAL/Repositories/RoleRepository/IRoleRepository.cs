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
        void CreateRole(string name);
        void DeleteRole(int id);
        Role GetRole(int id);
    }
}
