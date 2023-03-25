using AutoMapper;
using DAL.Database.Entities;
using Web.Models.UserDtos;
using WebApi.Models.ChatDtos;
using WebApi.Models.MessagesDtos;

namespace WebApi.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
           
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
                       })
                       .Cast<MessageDTO>();

                   dto.AllMessages = textMessagesDto.Concat(fileMessagesDto).ToList();
               });


            CreateMap<User, UserDTO>();

            CreateMap<TextMessage, TextMessageDTO>();
            CreateMap<TextMessage, GetNewestMessageDTO>();
            CreateMap<FileMessage, GetNewestMessageDTO>()
                .ForMember(dto => dto.Content, opt => opt.MapFrom(fm => fm.Name));
            
            CreateMap<Chat, GetUserChatDTO>().ReverseMap();

            CreateMap<User, UserDTO>();

            CreateMap<FileMessage, FileMessageDTO>();

        }
    }
}
