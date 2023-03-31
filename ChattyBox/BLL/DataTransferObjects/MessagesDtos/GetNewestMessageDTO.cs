namespace BLL.DataTransferObjects.MessageDtos
{
    public class GetNewestMessageDTO
    {

        public int SenderId { get; set; }
        public string Content { get; set; }
        public string SenderName { get; set; }

    }
}
