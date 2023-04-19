using AutoMapper;
using BLL.DataTransferObjects.UserDtos;
using BLL.Exceptions;
using DAL.Database.Entities;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        public IEnumerable<GetUserChatDTO> GetChats(int id, int pageNumber, int chatsPerPage)
        {
            if (pageNumber < 1)
                throw new IllegalOperationException("Numer strony nie może być mniejszy od 1");
            
            var chatCount = _unitOfWork.Users.GetUserChatsCount(id);

            if (chatCount == 0)
                throw new NotFoundException("Nie znaleziono czatów");

            int maxPageNumber = (int)Math.Ceiling((double)chatCount / chatsPerPage);

            pageNumber = pageNumber > maxPageNumber ? maxPageNumber : pageNumber;

            var chatList = _unitOfWork.Chats.GetChatsForUser(id, pageNumber, chatsPerPage);

            if (chatList is null)
                throw new NotFoundException("Nie znaleziono czatu");

            return _mapper.Map<IEnumerable<GetUserChatDTO>>(chatList);
        }

        public UserDTO GetUser(int id)
        {
            var user = _unitOfWork.Users.GetById(id);

            if (user is null)
                throw new NotFoundException("Nie znaleziono uzytkownika");

            var userDTO = _mapper.Map<UserDTO>(user);
            userDTO.ChatsCount = _unitOfWork.Users.GetUserChatsCount(user.Id);

            return userDTO;
        }

        public UserDTO LoginUser(LoginUserDTO dto)
        {
           var user = _unitOfWork.Users.GetUserByEmail(dto.Email);

            if (user is null)
                throw new NotFoundException("Nie znaleziono uzytkownika");

            if (!VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
                throw new LoginFailedException("Niepoprawny login lub hasło");

            if (user is null)
                throw new NotFoundException("Nie znaleziono uzytkownika");
            
            user.LastLog = DateTime.Now;
            _unitOfWork.Save();

            var userDTO = _mapper.Map<UserDTO>(user);
            userDTO.ChatsCount = _unitOfWork.Users.GetUserChatsCount(user.Id);

            return userDTO;
        }

        public void RegisterUser(CreateUserDTO dto)
        {
            if(_unitOfWork.Users.IsEmailTaken(dto.Email))
                throw new EmailAlreadyUsedException("Email jest już zajęty");

            createPasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Email = dto.Email,
                Username = dto.Name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Created = DateTime.Now
            };

            _unitOfWork.Users.CreateUser(user);
            _unitOfWork.Save();
        }

        public int GetUserChatsCount(int id)
        {
            return _unitOfWork.Users.GetUserChatsCount(id);
        }

        public string GetRole(int userId, int chatId)
        {
            var role = _unitOfWork.Chats.GetUserRole(userId, chatId);

            if (role is null)
                throw new NotFoundException("Nie znaleziono roli");

            return role.Name;
        }

    }
}
