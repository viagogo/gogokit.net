using GogoKit.Services;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ManagingSales.Attributes
{
    public class CookieAuthorizeAttribute : AuthorizeAttribute
    {
        public IOAuth2TokenStore OAuthTokenStore { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var token = OAuthTokenStore.GetTokenAsync().Result;

            return token != null && token.IssueDate.AddSeconds(token.ExpiresIn) < DateTimeOffset.UtcNow;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult("Login", new RouteValueDictionary());
        }
    }
}