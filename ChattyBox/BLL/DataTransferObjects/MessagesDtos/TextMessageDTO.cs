using System.ComponentModel.DataAnnotations;

namespace BLL.DataTransferObjects.MessageDtos
{
    public class TextMessageDTO : MessageDTO
    {
        public string Content { get; set; }

        public override string MessageType => "text";
    }
}
