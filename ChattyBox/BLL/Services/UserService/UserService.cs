using AutoMapper;
using BLL.DataTransferObjects.UserDtos;
using DAL.Exceptions;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ICollection<GetUserChatDTO> GetChats(int id, int pageNumber, int chatsPerPage)
        {
            var chat = _unitOfWork.Users.GetChats(id, pageNumber,chatsPerPage).ToList();
            if (chat == null)
                throw new NotFoundException("Nie znaleziono czatu");

            return _mapper.Map<ICollection<GetUserChatDTO>>(chat);
        }

        public UserDTO GetUser(int id)
        {
            var user = _unitOfWork.Users.GetUser(id);

            if (user == null)
                throw new NotFoundException("Nie znaleziono uzytkownika");

            return _mapper.Map<UserDTO>(user);
        }

        public UserDTO LoginUser(LoginUserDTO dto)
        {
            var user = _unitOfWork.Users.LoginUser(dto.Email, dto.Password);

            if (user == null)
                throw new NotFoundException("Nie znaleziono uzytkownika");

            _unitOfWork.Save();

            return _mapper.Map<UserDTO>(user);
        }

        public void RegisterUser(CreateUserDTO dto)
        {
            _unitOfWork.Users.RegisterUser(dto.Email, dto.Name, dto.Password);
            _unitOfWork.Save();
        }

        public int GetUserChatsCount(int id)
        {
            return _unitOfWork.Users.GetUserChatsCount(id);
        }
    }
}
