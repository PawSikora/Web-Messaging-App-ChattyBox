using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetFile([FromQuery] string chatName, [FromQuery] string fileName)
        {
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"); 
            var path = $"{rootPath}\\files\\{chatName}\\{fileName}";

            if (!System.IO.File.Exists(path))
                return NotFound();

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(path, out var contentType);

            var fileContentes = System.IO.File.ReadAllBytes(path);

            return File(fileContentes, contentType);
        }
    }
}
