using AutoMapper;
using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.UserDtos;
using DAL.Database.Entities;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ChatService
{
    public  class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChatService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void AddUserById(int userId, int chatId)
        {
            _unitOfWork.Chats.AddUserById(userId, chatId);
            _unitOfWork.Save();
        }
        
        public void CreateChat(CreateChatDTO dto)
        {
            _unitOfWork.Chats.CreateChat(dto.Name, dto.UserId);
            _unitOfWork.Save();
        }
        
        public void DeleteChat(int chatId)
        {
            _unitOfWork.Chats.DeleteChat(chatId);
            _unitOfWork.Save();
        }

        public void DeleteUserById(int userId, int chatId)
        {
            _unitOfWork.Chats.DeleteUserById(userId, chatId);
            _unitOfWork.Save();
        }

        public GetChatDTO GetChat(int id, int pageNumber)
        {
            var chat = _unitOfWork.Chats.GetChat(id, pageNumber);

            if (chat == null) 
                throw new Exception("Nie znaleziono chatu");

            return _mapper.Map<GetChatDTO>(chat);
        }

        public IEnumerable<UserDTO> GetUsersInChat(int chatId)
        {
            var users = _unitOfWork.Chats.GetUsersInChat(chatId);

            if (users == null)
                throw new Exception("Nie znaleziono użytkowników");

            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }
    }
}
