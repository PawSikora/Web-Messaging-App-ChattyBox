using AutoMapper;
using DAL.Database.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiWithoutBLL.Exceptions;
using WebApiWithoutBLL.Models.MessagesDtos;

namespace WebApiWithoutBLL.Controllers
{
    public class FileMessageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FileMessageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public ActionResult<FileMessageDTO> Get([FromRoute] int id)
        {
            var file = _unitOfWork.FileMessages.GetById(id);

            if (file is null)
                throw new NotFoundException("Nie znaleziono pliku");

            return _mapper.Map<FileMessageDTO>(file);
        }

        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateFileMessageDTO dto)
        {
            if (_unitOfWork.FileMessages.IsFileNameTaken(dto.Name))
                throw new NotUniqueElementException("Plik o takiej nazwie juz istnieje");

            var user = _unitOfWork.Users.GetById(dto.SenderId);

            if (user is null)
                throw new NotFoundException("Nie znaleziono użytkownika");

            var chat = _unitOfWork.Chats.GetById(dto.ChatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono czatu");

            var path = Path.Combine(Path.GetFullPath("wwwroot"), dto.File.FileName);

            if (System.IO.File.Exists(path))
                throw new NotUniqueElementException("Plik o takiej nazwie już istnieje");

            FileInfo file = new FileInfo(path);
            double fileSizeOnMB = (double)file.Length / (1024 * 1024);

            FileMessage message = new FileMessage
            {
                Path = path,
                Name = file.Name,
                Sender = user,
                Chat = chat,
                Size = fileSizeOnMB,
                TimeStamp = DateTime.Now
            };

            _unitOfWork.FileMessages.CreateFileMessage(message);
            _unitOfWork.Save();

            return View();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            var file = _unitOfWork.FileMessages.GetById(id);

            if (file is null)
                throw new NotFoundException("Nie znaleziono pliku");

            _unitOfWork.FileMessages.DeleteFileMessage(file);
            _unitOfWork.Save();

            return View();
        }

        [HttpGet("GetNewestFileMessage/{idChat}")]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage([FromRoute] int chatId)
        {
            var chat = _unitOfWork.Chats.GetById(chatId);

            if (chat is null)
                throw new NotFoundException("Nie znaleziono czatu");

            var file = _unitOfWork.FileMessages.GetLastFileMessage(chatId);

            if (file is null)
                throw new NotFoundException("Nie znaleziono pliku");

            return View(_mapper.Map<GetNewestMessageDTO>(file));
        }
    }
}
