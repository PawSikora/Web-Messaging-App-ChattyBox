namespace MVCWebAppWithoutBLL.Models.MessagesDtos
{
    public class TextMessageDTO : MessageDTO
    {
        public string Content { get; set; }

        public override string MessageType => "text";
    }
}
