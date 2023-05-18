using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FirstTaskWeb.AuthHelper
{
    public class JwtAuthenticationAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var token = filterContext.HttpContext.Request.Cookies["JWTToken"]?.ToString();// extract the token from the request, e.g., from a cookie or request header
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
                    filterContext.HttpContext.User = principles;    
                    base.OnActionExecuting(filterContext);
                }
            }

            base.OnActionExecuting(filterContext);
        }

    }
}
