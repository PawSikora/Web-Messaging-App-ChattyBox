using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BLL.Services.ChatService;
using DAL.Database.Entities;

namespace MVCWebApp
{
    public class RolesAuthorization : ActionFilterAttribute
    {
        public string AllowedRole { get; set; }
        private readonly IChatService chatService;
        public RolesAuthorization(IChatService chatService, string role)
        {
            this.chatService = chatService;
            AllowedRole = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var senderIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            var senderId = int.Parse(senderIdClaim.Value);

            var chatId = Convert.ToInt32(context.HttpContext.Request.Query["chatId"]);
            var userRole = chatService.GetUserRole(senderId, chatId);
            if (userRole != "Admin")
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "User",
                    action = "Unauthorized"
                }));
            }
            base.OnActionExecuting(context);
        }
    }

}

