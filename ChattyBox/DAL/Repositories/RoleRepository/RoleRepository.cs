using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Database;
using DAL.Database.Entities;
using DAL.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DbChattyBox _context;
        public RoleRepository(DbChattyBox context)
        {
            _context = context;
        }
        public void CreateRole(string name)
        {
            if(_context.Roles.Any(x => x.Name == name))
                throw new NotUniqueElementException("Rola juz istnieje");

            var role = new Role
            {
                Name = name
            };

            _context.Roles.Add(role);
        }

        public void DeleteRole(int id)
        {
            var role = _context.Roles.FirstOrDefault(x => x.Id == id) ?? throw new NotFoundException("Nie znaleziono roli");

            _context.Roles.Remove(role);
        }

        public Role GetRole(int id)
        {
            Role role = _context.Roles.SingleOrDefault(r => r.Id == id) ?? throw new NotFoundException("Nie znaleziono roli");
            return role;
        }
    }
}
