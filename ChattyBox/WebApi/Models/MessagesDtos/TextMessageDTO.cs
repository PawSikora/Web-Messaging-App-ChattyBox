using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.MessagesDtos
{
    public class TextMessageDTO : MessageDTO
    {
        public string Content { get; set; }

        public override string MessageType => "text";
    }
}
