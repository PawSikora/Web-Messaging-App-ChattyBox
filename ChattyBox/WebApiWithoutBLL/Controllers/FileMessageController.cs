using AutoMapper;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var file = _unitOfWork.FileMessages.GetFileMessage(id);

            if (file == null)
            {
                return View("Error");
            }
            
            return View(_mapper.Map<FileMessageDTO>(file));
        }

        [HttpPost("create")]
        public ActionResult Create([FromBody] CreateFileMessageDTO createFile)
        {
            _unitOfWork.FileMessages.CreateFileMessage(createFile.SenderId, createFile.Path, createFile.ChatId);
            _unitOfWork.Save();
            return View();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _unitOfWork.FileMessages.DeleteFileMessage(id);
            _unitOfWork.Save();
            return View();
        }

        [HttpGet("GetNewestFileMessage/{idChat}")]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage([FromRoute] int idChat)
        {
            var message = _unitOfWork.FileMessages.GetLastFileMessage(idChat);

            if (message == null)
            {
                return View("Error");
            }

            return View(_mapper.Map<GetNewestMessageDTO>(message));
        }
    }
}
