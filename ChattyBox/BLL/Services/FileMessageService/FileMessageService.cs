using AutoMapper;
using BLL.DataTransferObjects.MessageDtos;
using DAL.Database.Entities;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.FileMessageService
{
    public class FileMessageService : IFileMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FileMessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public void CreateFileMessage(CreateFileMessageDTO dto)
        {
            _unitOfWork.FileMessages.CreateFileMessage(dto.SenderId, dto.Path, dto.ChatId);
            _unitOfWork.Save();
        }

        public void DeleteFileMessage(int id)
        {
            _unitOfWork.FileMessages.DeleteFileMessage(id);
            _unitOfWork.Save();
        }

        public FileMessageDTO GetFileMessage(int id)
        {
            var file = _unitOfWork.FileMessages.GetFileMessage(id);

            if (file == null)
                throw new Exception("Nie znaleziono pliku");

            return _mapper.Map<FileMessageDTO>(file);
        }

        public GetNewestMessageDTO GetLastFileMessage(int chatId)
        {
            var file = _unitOfWork.FileMessages.GetLastFileMessage(chatId);

            if (file == null)
                throw new Exception("Nie znaleziono pliku");

            return _mapper.Map<GetNewestMessageDTO>(file);
        }
    }
}
