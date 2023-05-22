using AutoMapper;
using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.UserDtos;
using BLL.Exceptions;
using DAL.Database.Entities;
using DAL.UnitOfWork;

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
            var user = _unitOfWork.Users.GetById(userId);

            if (user is null)
                throw new NotFoundException("Nie znaleziono użytkownika");

            var chat = _unitOfWork.Chats.GetById(chatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono chatu");

            var role = _unitOfWork.Roles.GetByName("User");

            if (role is null)
                throw new NotFoundException("Nie znaleziono roli");

            if(_unitOfWork.Chats.IsUserInChat(userId,chatId))
                throw new IllegalOperationException("Użytkownik jest już w czacie");

            var userChat = new UserChat { UserId = userId, ChatId = chatId, RoleId = role.Id};

            _unitOfWork.Chats.AddUserToChat(userChat);
            chat.Updated = DateTime.Now;
            _unitOfWork.Save();
        }
        
        public void CreateChat(CreateChatDTO dto)
        {
            if(_unitOfWork.Chats.IsChatNameTaken(dto.Name))
                throw new NotUniqueElementException("Nazwa czatu jest już zajęta");

            var user = _unitOfWork.Users.GetById(dto.UserId);

            if (user is null)
                throw new NotFoundException("Nie znaleziono użytkownika");

            var role = _unitOfWork.Roles.GetByName("Admin");

            if (role is null)
                throw new NotFoundException("Nie znaleziono roli");

            var chat = _mapper.Map<Chat>(dto);

            UserChat userChat = new UserChat
            {
                Chat = chat,
                UserId = dto.UserId,
                RoleId = role.Id
            };

            _unitOfWork.Chats.CreateChat(chat,userChat);
            _unitOfWork.Save();

            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var relativePath = Path.Combine("files", chat.Name);
            var fullPath = Path.Combine(wwwrootPath, relativePath);
            Directory.CreateDirectory(fullPath);
        }
        
        public void DeleteChat(int chatId)
        {
            var chat = _unitOfWork.Chats.GetById(chatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono chatu");

            var chatUsers = _unitOfWork.Chats.GetChatUsersById(chatId);

            if(chatUsers is not null)
                _unitOfWork.Chats.RemoveUsersFromChat(chatUsers);

            _unitOfWork.Chats.DeleteChat(chat);
            _unitOfWork.Save();

            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var relativePath = Path.Combine("files", chat.Name);
            var fullPath = Path.Combine(wwwrootPath, relativePath);
            Directory.Delete(fullPath, true);
        }

        public void DeleteUserById(int userId, int chatId)
        {
            var user = _unitOfWork.Users.GetById(userId);

            if (user is null)
                throw new NotFoundException("Nie znaleziono użytkownika");

            var chat = _unitOfWork.Chats.GetById(chatId);
            if (chat is null)
                throw new NotFoundException("Nie znaleziono chatu");

            var userChat = _unitOfWork.Chats.GetUserChatById(userId, chatId);

            if (userChat is null)
                throw new NotFoundException("Nie znaleziono użytkownika w czacie");

            _unitOfWork.Chats.RemoveUserFromChat(userChat);
            chat.Updated = DateTime.Now;
            _unitOfWork.Save();
        }

        public GetChatDTO GetChat(int id, int pageNumber, int messagesPerPage)
        {
            if (pageNumber < 1)
                throw new IllegalOperationException("Numer strony nie może być mniejszy od 1");

            var messageCount = _unitOfWork.Chats.GetChatMessagesCount(id);

            int maxPageNumber = (int)Math.Ceiling((double)messageCount / messagesPerPage);

            pageNumber = pageNumber > maxPageNumber ? maxPageNumber : pageNumber;

            var chat = _unitOfWork.Chats.GetChat(id, pageNumber,messagesPerPage);

            if (chat is null) 
                throw new NotFoundException("Nie znaleziono chatu");

            return _mapper.Map<GetChatDTO>(chat);
        }

        public int GetChatMessagesCount(int id)
        {
            return _unitOfWork.Chats.GetChatMessagesCount(id);
        }

        public IEnumerable<UserDTO> GetUsersInChat(int chatId)
        {
            var chat = _unitOfWork.Chats.GetById(chatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono chatu");

            var users = _unitOfWork.Chats.GetUsersInChat(chatId);

            if (users is null)
                throw new NotFoundException("Nie znaleziono użytkowników");

            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public void AssignRole(int userId, int chatId, int roleId)
        {
            var user = _unitOfWork.Users.GetById(userId);

            if (user is null)
                throw new NotFoundException("Nie znaleziono użytkownika");

            var chat = _unitOfWork.Chats.GetById(chatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono chatu");

            var role = _unitOfWork.Roles.GetById(roleId);

            if (role is null)
                throw new NotFoundException("Nie znaleziono roli");

            if(_unitOfWork.Chats.IsUserRole(userId,chatId,roleId))
                throw new IllegalOperationException("Użytkownik ma już tą rolę");

            var userChat = _unitOfWork.Chats.GetUserChatById(userId, chatId);

            if (userChat is null)
                throw new NotFoundException("Nie znaleziono użytkownika w czacie");

            userChat.RoleId = roleId;
            _unitOfWork.Save();
        }

        public void RevokeRole(int userId, int chatId)
        {
            var user = _unitOfWork.Users.GetById(userId);

            if (user is null)
                throw new NotFoundException("Nie znaleziono użytkownika");

            var chat = _unitOfWork.Chats.GetById(chatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono chatu");

            var role = _unitOfWork.Roles.GetByName("User");

            if (role is null)
                throw new NotFoundException("Nie znaleziono roli");

            if(_unitOfWork.Chats.IsUserRole(userId,chatId,role.Id))
                throw new IllegalOperationException("Użytkownik ma już tą rolę");

            var userChat = _unitOfWork.Chats.GetUserChatById(userId, chatId);

            if (userChat is null)
                throw new NotFoundException("Nie znaleziono użytkownika w czacie");

            userChat.RoleId = role.Id;
            _unitOfWork.Save();
        }

        public string GetUserRole(int userId, int chatId)
        {
            var user = _unitOfWork.Users.GetById(userId);

            if (user is null)
                throw new NotFoundException("Nie znaleziono użytkownika");

            var chat = _unitOfWork.Chats.GetById(chatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono chatu");

            var userChat = _unitOfWork.Chats.GetUserChatById(userId, chatId);

            if (userChat is null)
                throw new NotFoundException("Nie znaleziono użytkownika w czacie");

            var role = _unitOfWork.Chats.GetUserRole(userId, chatId);

            if (role is null)
                throw new NotFoundException("Nie znaleziono roli");

            return role.Name;
		}

        public UserDTO GetUserByEmail(string email)
        {
            var user = _unitOfWork.Users.GetUserByEmail(email);

            if (user is null)
                throw new NotFoundException("Nie znaleziono użytkownika");

            return _mapper.Map<UserDTO>(user);
        }
    }
}
