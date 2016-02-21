using GogoKit;
using GogoKit.Models.Request;
using ManagingSales.Attributes;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Index(int? page = 1)
        {
            var sales = await _viagogoClient.Sales.GetAsync(new SaleRequest { Page = page });
            return View(sales);
        }
    }
}