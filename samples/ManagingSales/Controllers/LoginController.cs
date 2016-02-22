using GogoKit;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ManagingSales.Controllers
{
    public class LoginController : Controller
    {
        private static readonly Uri RedirectUri = new Uri("https://localhost:44300/oauth2/callback");

        private static readonly string[] Scopes = { "read:user", "write:user" };

        private readonly IViagogoClient _viagogoClient;

        public LoginController(IViagogoClient viagogoClient)
        {
            _viagogoClient = viagogoClient;
        }

        [Route("login", Name = "Login")]
        public ActionResult Index()
        {
            var authorizationUrl = _viagogoClient.OAuth2.GetAuthorizationUrl(RedirectUri, Scopes);

            return Redirect(authorizationUrl.ToString());
        }

        [Route("oauth2/callback")]
        public async Task<ActionResult> Callback(string code, string state)
        {
            var token = await _viagogoClient.OAuth2.GetAuthorizationCodeAccessTokenAsync(code, RedirectUri, Scopes);
            await _viagogoClient.TokenStore.SetTokenAsync(token);

            return RedirectToRoute("Sales");
        }
    }
}