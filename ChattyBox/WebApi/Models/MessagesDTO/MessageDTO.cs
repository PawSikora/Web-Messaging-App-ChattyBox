
using System.Text.Json.Serialization;

namespace WebApi.Models.MessagesDTO
{
    [JsonConverter(typeof(MessageConverter))]
    public abstract class MessageDTO
    {
        public int Id { get; set; }

        public int ChatId { get; set; }

        public int SenderId { get; set; }

        public DateTime TimeStamp { get; set; }

        public abstract string MessageType { get; }
    }
}
