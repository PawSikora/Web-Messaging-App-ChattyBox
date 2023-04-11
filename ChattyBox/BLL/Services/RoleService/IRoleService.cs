using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.RoleService
{
    public interface IRoleService
    {
        void CreateRole(string name);
        void DeleteRole(int id);
        string GetRole(int id);
    }
}
