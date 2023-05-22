using AutoMapper;
using BLL.DataTransferObjects.MessageDtos;
using BLL.Exceptions;
using DAL.Database.Entities;
using DAL.UnitOfWork;

namespace BLL.Services.TextMessageService
{
    public class TextMessageService : ITextMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TextMessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void CreateTextMessage(CreateTextMessageDTO dto)
        {
            var sender = _unitOfWork.Users.GetById(dto.SenderId);

            if (sender is null)
                throw new NotFoundException("Nie znaleziono uzytkownika");

            var chat = _unitOfWork.Chats.GetById(dto.ChatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono czatu");

            var message = _mapper.Map<TextMessage>(dto);

            _unitOfWork.TextMessages.CreateTextMessage(message);
            chat.Updated = DateTime.Now;
            _unitOfWork.Save();
        }

        public void DeleteTextMessage(int id)
        {
            var message = _unitOfWork.TextMessages.GetById(id);

            if (message is null)
                throw new NotFoundException("Nie znaleziono wiadomosci");

            var chat = _unitOfWork.Chats.GetById(message.ChatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono czatu");

            _unitOfWork.TextMessages.DeleteTextMessage(message);
            chat.Updated = DateTime.Now;
            _unitOfWork.Save();
        }

        public GetNewestMessageDTO GetLastTextMessage(int chatId)
        {
            var chat = _unitOfWork.Chats.GetById(chatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono czatu");

            var message = _unitOfWork.TextMessages.GetLastTextMessage(chatId);

            if (message is null)
                throw new NotFoundException("Nie znaleziono wiadomosci");

            return _mapper.Map<GetNewestMessageDTO>(message);
        }

        public TextMessageDTO GetTextMessage(int id)
        {
            var message = _unitOfWork.TextMessages.GetById(id);

            if (message is null)
                throw new NotFoundException("Nie znaleziono wiadomosci");

            return _mapper.Map<TextMessageDTO>(message);
        }
    }
}
