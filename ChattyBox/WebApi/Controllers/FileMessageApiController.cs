﻿using BLL.DataTransferObjects.MessageDtos;
using BLL.Services.FileMessageService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileMessageApiController : ControllerBase
    {
        private readonly IFileMessageService _fileMessageService;

        public FileMessageApiController(IFileMessageService fileMessageService)
        {
            _fileMessageService = fileMessageService;
        }

        [HttpGet("{id}")]
        public ActionResult<FileMessageDTO> Get([FromRoute] int id)
        {
            return Ok(_fileMessageService.GetFileMessage(id));
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateFileMessageDTO createFile)
        {
            if (!ModelState.IsValid)
                return BadRequest("Błąd dodawania pliku!");

            createFile.Name = createFile.File.FileName;
            _fileMessageService.CreateFileMessage(createFile);
            return Ok();
        }

        [HttpDelete("{messageId}")]
        public ActionResult Delete([FromRoute] int messageId)
        {
            _fileMessageService.DeleteFileMessage(messageId);
           return Ok();
        }

        [HttpGet("GetNewest/{idChat}")]
        public ActionResult<GetNewestMessageDTO> GetNewestMessage([FromRoute] int idChat)
        {
            return Ok(_fileMessageService.GetLastFileMessage(idChat));
        }
    }
}