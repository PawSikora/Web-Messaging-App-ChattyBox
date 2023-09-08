using AutoMapper;
using BLL.DataTransferObjects;
using BLL.DataTransferObjects.UserDtos;
using BLL.Exceptions;
using DAL.Database.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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

        public TokenToReturn LoginUser(LoginUserDTO dto)
        {
           var user = _unitOfWork.Users.GetUserByEmail(dto.Email);

            if (user is null)
                throw new NotFoundException("Nie znaleziono uzytkownika");

            if (!VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
                throw new LoginFailedException("Niepoprawny login lub hasło");
  
            string generatedToken = GenerateNewToken(user);

            return new TokenToReturn(generatedToken);
        }

        public string GenerateNewToken(User user)
        {
            var refreshToken = new RefreshToken();
            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.Created;
            user.TokenExpires = refreshToken.Expires;
            user.LastLog = DateTime.Now;

            _unitOfWork.Save();

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires
            });

            return CreateToken(user);
        }

        private string CreateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenSettings:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenParams = new JwtSecurityToken
                (
                    issuer: _configuration["TokenSettings:Issuer"], audience: _configuration["TokenSettings:Audience"],
                    claims: new List<Claim>
                    {
                            new (type:"userId",user.Id.ToString()),
                    },
                    expires: DateTime.Now.AddMinutes(1),
                    signingCredentials: creds
               );

            return new JwtSecurityTokenHandler().WriteToken(tokenParams);
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
            var chatsCount = _unitOfWork.Users.GetUserChatsCount(id);

            if (chatsCount == 0)
                throw new NotFoundException("Nie znaleziono czatów");

            return _unitOfWork.Users.GetUserChatsCount(id);
        }

        public string GetRole(int userId, int chatId)
        {
            var role = _unitOfWork.Chats.GetUserRole(userId, chatId);

            if (role is null)
                throw new NotFoundException("Nie znaleziono roli");

            return role.Name;
        }

        public User GetUser(string email)
        {
            var user= _unitOfWork.Users.GetUserByEmail(email);

            if(user is null)
                throw new NotFoundException("Nie znaleziono uzytkownika");

            return user;
        }

    }
}
