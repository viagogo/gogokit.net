using GogoKit.Models.Response;
using GogoKit.Services;
using ManagingSales.Services;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ManagingSales.Attributes
{
    public class CookieAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var tokenCookie = HttpContext.Current.Request.Cookies[CookieOAuth2TokenStore.CookieName];
            OAuth2Token token = null;
            if (tokenCookie != null)
            {
                token = JsonConvert.DeserializeObject<OAuth2Token>(tokenCookie.Value);
            }

            return token != null && token.IssueDate.AddSeconds(token.ExpiresIn) > DateTimeOffset.UtcNow;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult("Login", new RouteValueDictionary());
        }
    }
}