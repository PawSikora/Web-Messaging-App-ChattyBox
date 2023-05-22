using AutoMapper;
using BLL.Exceptions;
using DAL.Database.Entities;
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
            if(_unitOfWork.Roles.RoleExists(name))
                throw new NotUniqueElementException("Rola o takiej nazwie już istnieje");

            var role = new Role
            {
                Name = name
            };

            _unitOfWork.Roles.CreateRole(role);
            _unitOfWork.Save();
        }

        public void DeleteRole(int id)
        {
            var role = _unitOfWork.Roles.GetById(id);

            if (role is null)
                throw new NotFoundException("Nie znaleziono roli");

	        _unitOfWork.Roles.DeleteRole(role);
	        _unitOfWork.Save();
        }
        public string GetRole(int id)
        {
	        var role = _unitOfWork.Roles.GetById(id);
	        return role.Name;
		}
    }
}
