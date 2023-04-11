using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Exceptions;
using DAL.UnitOfWork;

namespace BLL.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void CreateRole(string name)
        {
            _unitOfWork.Roles.CreateRole(name);
            _unitOfWork.Save();
        }

        public void DeleteRole(int id)
        {
	        _unitOfWork.Roles.DeleteRole(id);
	        _unitOfWork.Save();
        }
        public string GetRole(int id)
        {
	        var role = _unitOfWork.Roles.GetRole(id);
	        return role.Name;
		}
    }
}
