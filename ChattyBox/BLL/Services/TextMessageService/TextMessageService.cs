using AutoMapper;
using BLL.DataTransferObjects.MessageDtos;
using DAL.Database.Entities;
using DAL.Exceptions;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _unitOfWork.TextMessages.CreateTextMessage(dto.SenderId, dto.Content, dto.ChatId);
            _unitOfWork.Save();
        }

        public void DeleteTextMessage(int id)
        {
            _unitOfWork.TextMessages.DeleteTextMessage(id);
            _unitOfWork.Save();
        }

        public GetNewestMessageDTO GetLastTextMessage(int chatId)
        {
            var message = _unitOfWork.TextMessages.GetLastTextMessage(chatId);

            if (message == null)
                throw new NotFoundException("Nie znaleziono wiadomosci");

            return _mapper.Map<GetNewestMessageDTO>(message);
        }

        public TextMessageDTO GetTextMessage(int id)
        {
            var message = _unitOfWork.TextMessages.GetTextMessage(id);

            if (message == null)
                throw new NotFoundException("Nie znaleziono wiadomosci");

            return _mapper.Map<TextMessageDTO>(message);
        }
    }
}
