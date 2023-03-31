using AutoMapper;
using DAL.Database.Entities;
using WebApiWithoutBLL.Models.ChatDtos;
using WebApiWithoutBLL.Models.MessagesDtos;
using WebApiWithoutBLL.Models.UserDtos;

namespace WebApiWithoutBLL.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<User, UserDTO>();

            CreateMap<TextMessage, TextMessageDTO>();
            CreateMap<TextMessage, GetNewestMessageDTO>();
            CreateMap<FileMessage, GetNewestMessageDTO>()
                .ForMember(dto => dto.Content, opt => opt.MapFrom(fm => fm.Name));

            CreateMap<Chat, GetUserChatDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<FileMessage, FileMessageDTO>();

            CreateMap<Chat, GetChatDTO>()
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
                           Path = m.Path,
                           Name = m.Name,
                           SenderId = m.SenderId,
                           TimeStamp = m.TimeStamp
                       })
                       .Cast<MessageDTO>();

                   dto.AllMessages = textMessagesDto.Concat(fileMessagesDto).OrderByDescending(t => t.TimeStamp).ToList();
               });
        }
    }
}



