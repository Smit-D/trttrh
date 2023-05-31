
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FirstTaskWeb.AuthHelper
{
     public class JwtAuthenticationAttribute : ActionFilterAttribute
     {
        public string Roles { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
         {
             var token = filterContext.HttpContext.Request.Cookies["JWTToken"]?.ToString();// extract the token from the request, e.g., from a cookie or request header
             var getToken = filterContext.HttpContext.Request.Headers.Authorization;// extract the token from the request, e.g., from a cookie or request header
             var getToken1 = filterContext.HttpContext.Response.Headers.Authorization;
             if (!string.IsNullOrEmpty(token))
             {

                 var principles = JwtTokenHelper.ValidateToken(token);

                 if (principles == null)
                 {
                     // Redirect the user to the login page
                     filterContext.Result = new RedirectResult("~/User/Login");
                     // filterContext.Result = new UnauthorizedResult();

                 }
                 else
                 {
                     /*if (principles.Identity.IsAuthenticated && filterContext.HttpContext.Request.Path.Value.Contains("/Home/Login"))
                     {
                         filterContext.Result = new RedirectResult("~/Home/Index");
                     }
                     filterContext.HttpContext.User = principles; */   
                     base.OnActionExecuting(filterContext);
                 }
             }

             base.OnActionExecuting(filterContext);
         }

     }
/*    public class JwtAuthenticationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContext httpContext)
        {
            if (!_authorize)
                return true;

            return base.AuthorizeCore(httpContext);
        }
    }*/


}
