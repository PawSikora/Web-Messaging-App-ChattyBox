using System.ComponentModel.DataAnnotations;

namespace WebApiWithoutBLL.Models.MessagesDtos
{
    public class TextMessageDTO : MessageDTO
    {
        public string Content { get; set; }

        public override string MessageType => "text";
    }
}
