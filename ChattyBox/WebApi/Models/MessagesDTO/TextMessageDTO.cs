using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.MessagesDTO
{
    public class TextMessageDTO : MessageDTO
    {
        public string Content { get; set; }

        public override string MessageType => "text";
    }
}
