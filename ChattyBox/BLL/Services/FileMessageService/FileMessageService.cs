using AutoMapper;
using BLL.DataTransferObjects.MessageDtos;
using BLL.Exceptions;
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
            var user = _unitOfWork.Users.GetById(dto.SenderId);

            if (user is null)
                throw new NotFoundException("Nie znaleziono użytkownika");

            var chat = _unitOfWork.Chats.GetById(dto.ChatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono czatu");

            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var relativePath = Path.Combine("files", chat.Name, dto.File.FileName);
            var fullPath = Path.Combine(wwwrootPath, relativePath);

            if (File.Exists(fullPath)) throw new NotUniqueElementException("Plik o takiej nazwie już istnieje");

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                dto.File.CopyTo(fileStream);
            }

            FileInfo file = new FileInfo(fullPath);
            double fileSizeOnMB = (double)file.Length / (1024 * 1024);

            FileMessage message = new FileMessage
            {
                Path = relativePath,
                Name = file.Name,
                Sender = user,
                Chat = chat,
                Size = fileSizeOnMB,
                TimeStamp = DateTime.Now
            };

            _unitOfWork.FileMessages.CreateFileMessage(message);
            chat.Updated = DateTime.Now;
            _unitOfWork.Save();
        }

        public void DeleteFileMessage(int id)
        {
            var file = _unitOfWork.FileMessages.GetById(id);

            if (file is null)
                throw new NotFoundException("Nie znaleziono pliku");

            var chat = _unitOfWork.Chats.GetById(file.ChatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono czatu");

            string path = file.Path;

            _unitOfWork.FileMessages.DeleteFileMessage(file);
            chat.Updated = DateTime.Now;
            _unitOfWork.Save();

            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fullPath = Path.Combine(wwwrootPath, path);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public FileMessageDTO GetFileMessage(int id)
        {
            var file = _unitOfWork.FileMessages.GetById(id);

            if (file is null)
                throw new NotFoundException("Nie znaleziono pliku");

            return _mapper.Map<FileMessageDTO>(file);
        }

        public GetNewestMessageDTO GetLastFileMessage(int chatId)
        {
            var chat = _unitOfWork.Chats.GetById(chatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono czatu");

            var file = _unitOfWork.FileMessages.GetLastFileMessage(chatId);

            if (file is null)
                throw new NotFoundException("Nie znaleziono pliku");

            file.Path = $"/{file.Path.Replace('\\', '/')}";

            return _mapper.Map<GetNewestMessageDTO>(file);
        }
    }
}
