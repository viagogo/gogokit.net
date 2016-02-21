using GogoKit;
using ManagingSales.Attributes;
using System.Web.Mvc;

namespace ManagingSales.Controllers
{
    [CookieAuthorize]
    public class HomeController : Controller
    {
        private IViagogoClient _viagogoClient;

        public HomeController(IViagogoClient viagogoClient)
        {
            _viagogoClient = viagogoClient;
        }

        [Route("", Name = "Sales")]
        public ActionResult Index()
        {
            return View();
        }
    }
}