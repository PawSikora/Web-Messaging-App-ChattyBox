using AutoMapper;
using BLL.DataTransferObjects.ChatDtos;
using BLL.DataTransferObjects.MessageDtos;
using BLL.DataTransferObjects.UserDtos;
using DAL.Database.Entities;

namespace BLL.DataTransferObjects
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<User, UserDTO>();

            CreateMap<TextMessage, TextMessageDTO>();

            CreateMap<CreateTextMessageDTO, TextMessage>()
                .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<TextMessage, GetNewestMessageDTO>();

            CreateMap<FileMessage, GetNewestMessageDTO>()
                .ForMember(dto => dto.Content, opt => opt.MapFrom(fm => fm.Name));
                

            CreateMap<Chat, GetUserChatDTO>().ReverseMap();

            CreateMap<CreateChatDTO, Chat>()
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<FileMessage, FileMessageDTO>()
                .ForMember(dto => dto.Path, opt => opt.MapFrom(fm => $"/{fm.Path.Replace('\\', '/')}"))
                .ForMember(dto => dto.Type, opt => opt.MapFrom(fm => GetFileType(fm.Path)));

            CreateMap<Chat, GetChatDTO>()
                .ForMember(dto=>dto.ChatId,opt=>opt.MapFrom(chat=>chat.Id))
               .ForMember(dto => dto.Users, opt => opt.MapFrom(chat => chat.UserChats.Select(uc => uc.User)))
               .ForMember(dto => dto.AllMessages, opt => opt.Ignore())
               .AfterMap((chat, dto) =>
               {
                   var textMessagesDto = chat.Messages.OfType<TextMessage>()
                       .Select(m => new TextMessageDTO
                       {
                           Id = m.Id,
                           ChatId = m.ChatId,
                           Content = m.Content,
                           SenderId = m.SenderId,
                           TimeStamp = m.TimeStamp
                       })
                       .Cast<MessageDTO>();

                   var fileMessagesDto = chat.Messages.OfType<FileMessage>()
                       .Select(m => new FileMessageDTO
                       {
                           Id = m.Id,
                           ChatId = m.ChatId,
                           Path = $"/{m.Path.Replace('\\', '/')}",
                           Name = m.Name,
                           Type = GetFileType(m.Path),
                           SenderId = m.SenderId,
                           TimeStamp = m.TimeStamp
                       })
                       .Cast<MessageDTO>();

                   dto.AllMessages = textMessagesDto.Concat(fileMessagesDto).OrderByDescending(t => t.TimeStamp).ToList();
               });
        }

        string GetFileType(string filePath)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            string[] videoExtensions = { ".mp4", ".avi", ".mov", ".wmv", ".mkv" };
            string[] audioExtensions = { ".mp3", ".wav", ".ogg", ".wma", ".aac" };

            string fileExtension = Path.GetExtension(filePath)?.ToLower();

            if (imageExtensions.Contains(fileExtension))
            {
                return "image";
            }
            else if (videoExtensions.Contains(fileExtension))
            {
                return "video";
            }
            else if (audioExtensions.Contains(fileExtension))
            {
                return "audio";
            }
            else
            {
                return "unknown";
            }
        }
    }
}



