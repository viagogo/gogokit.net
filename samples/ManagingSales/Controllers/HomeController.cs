using GogoKit;
using System.Web.Mvc;

namespace ManagingSales.Controllers
{
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