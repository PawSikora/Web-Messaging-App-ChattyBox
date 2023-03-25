using Microsoft.VisualBasic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi.Models.MessagesDtos
{
    public class MessageConverter : JsonConverter<MessageDTO>
    {
        public override MessageDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsonObject = JsonSerializer.Deserialize<JsonElement>(ref reader);

            if (!jsonObject.TryGetProperty("messageType", out JsonElement messageTypeElement))
            {
                throw new JsonException("Brak pola \"messageType\".");
            }

            var messageType = messageTypeElement.GetString();

            switch (messageType)
            {
                case "text":
                    return JsonSerializer.Deserialize<TextMessageDTO>(jsonObject.GetRawText(), options);
                case "file":
                    return JsonSerializer.Deserialize<FileMessageDTO>(jsonObject.GetRawText(), options);
                case "createText":
                    return JsonSerializer.Deserialize<CreateTextMessageDTO>(jsonObject.GetRawText(), options);
                case "createFile":
                    return JsonSerializer.Deserialize<CreateFileMessageDTO>(jsonObject.GetRawText(), options);
                default:
                    throw new JsonException($"Nieznany typ wiadomości: {messageType}.");
            }
        }

        public override void Write(Utf8JsonWriter writer, MessageDTO value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
