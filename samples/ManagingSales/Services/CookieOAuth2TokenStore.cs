using GogoKit.Services;
using System;
using GogoKit.Models.Response;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace ManagingSales.Services
{
    public class CookieOAuth2TokenStore : IOAuth2TokenStore
    {
        public const string CookieName = "__oauthtoken";

        public Task DeleteTokenAsync()
        {
            HttpContext.Current.Request.Cookies.Remove(CookieName);
            HttpContext.Current.Response.Cookies.Remove(CookieName);
            return Task.FromResult(-1);
        }

        public Task<OAuth2Token> GetTokenAsync()
        {
            var tokenCookie = HttpContext.Current.Request.Cookies[CookieName];
            if (tokenCookie == null)
            {
                return Task.FromResult<OAuth2Token>(null);
            }

            var token = JsonConvert.DeserializeObject<OAuth2Token>(tokenCookie.Value);
            return Task.FromResult(token);
        }

        public Task SetTokenAsync(OAuth2Token token)
        {
            var tokenCookie = HttpContext.Current.Request.Cookies[CookieName];
            if (tokenCookie == null)
            {
                tokenCookie = new HttpCookie(CookieName);
                HttpContext.Current.Request.Cookies.Add(tokenCookie);
            }

            tokenCookie.Value = JsonConvert.SerializeObject(token);
            HttpContext.Current.Response.SetCookie(tokenCookie);
            return Task.FromResult(-1);
        }
    }
}