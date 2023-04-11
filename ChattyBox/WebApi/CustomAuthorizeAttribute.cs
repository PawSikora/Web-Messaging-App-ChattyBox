using System.Security.Principal;
using System.Web.Http.Controllers;
using System.Web;
using System.Web.Mvc;
using BLL.Services.UserService;
using DAL.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace WebApi
{
    public class CustomAuthorizationAttribute : AuthorizeAttribute
    {
        /*private readonly IUserService service;

        public IUserService Service { get; set; }

        public string Role { get; set; }

        public CustomAuthorizationAttribute()
        {
            // Inicjalizacja serwisu z warstwy logiki biznesowej (BLL)
        }

        protected override bool AuthorizeCore(HttpContext httpContext)
        {
            // Pobranie danych autoryzacyjnych z kontekstu HTTP
            var userService = httpContext.RequestServices.GetRequiredService<UserService>();

            // Pobierz informacje o zalogowanym użytkowniku z HttpContext
            var userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var chatId = Convert.ToInt32(httpContext.Request.RouteValues["chatId"]);
            return Role == userService.GetRole(userId, chatId);
        }



        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string customRole = filterContext.HttpContext.Session.GetString("CustomRole"); // Odczytywanie argumentu z sesji
            // Logika autoryzacji na podstawie customRole...
            base.OnAuthorization(filterContext);
        }

    }*/

        /*public class CustomAuthorizeAttribute : AuthorizeAttribute
        {
    
            /*private readonly IUserService _userService;
            private string role;
            private string[] authorizedRoles = new string[] { "Admin", "User" };
    
            /*public CustomAuthorizeAttribute(string role, IUserService userService)
            {
                this._userService = userService;
                this.role = role;
            }#2#
    
            /*public CustomAuthorizeAttribute(IUserService userService)
            {
                this._userService = userService;
            }#2#
    
    
            public void setRole(string role)
            {
                this.role = role;
            }
    
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                //var userRole = _userService.GetRole(userId, chatId);
                //return userRole.Name == role;
                return false;
            }
    
            public override void OnAuthorization(AuthorizationContext filterContext)
            {
    
            }
    
    
            /*protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                // Pobranie danych autoryzacyjnych z kontekstu HTTP
                var chatId = Convert.ToInt32(httpContext.Request.QueryString["chatId"]);
                var userId = Convert.ToInt32(httpContext.Request.QueryString["userId"]);
                var roleId = Convert.ToInt32(httpContext.Request.QueryString["roleId"]);
    
                // Sprawdzenie autoryzacji z użyciem UserChatService
                var isAuthorized = _userChatService.CheckAuthorization(chatId, userId, roleId);
    
                return isAuthorized;
            }
    
            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                // Przekierowanie na stronę logowania lub wyświetlenie komunikatu o błędzie
                filterContext.Result = new RedirectResult("AuthorizedFailed");
            }#2#
    
                /*public override void OnAuthorization(AuthorizationContext filterContext)
                {
                    base.OnAuthorization(filterContext);
                    if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                    {
                        filterContext.Controller.TempData["ErrorDetails"] = "You must be logged in to access this page";
                        filterContext.Result = new RedirectResult("~/User/Login");
                        return;
                    }
                    if (filterContext.Result is HttpUnauthorizedResult)
                    {
                        filterContext.Controller.TempData["ErrorDetails"] = "You don't have access rights to this page";
                        filterContext.Result = new RedirectResult("~/User/Login");
                        return;
                    }
                }#2##1#
    
            }*/
    }
}
