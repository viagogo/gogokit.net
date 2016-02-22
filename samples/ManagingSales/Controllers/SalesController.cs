using GogoKit;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using ManagingSales.Attributes;
using ManagingSales.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ManagingSales.Controllers
{
    [CookieAuthorize]
    public class SalesController : Controller
    {
        private IViagogoClient _viagogoClient;

        public SalesController(IViagogoClient viagogoClient)
        {
            _viagogoClient = viagogoClient;
        }

        [Route("", Name = "Sales")]
        public async Task<ActionResult> Index(int? page = 1)
        {
            var sales = await _viagogoClient.Sales.GetAsync(new SaleRequest { Page = page });
            var salesViewModel = new SalesViewModel
                                 {
                                    Sales = sales.Items.Select(CreateSaleViewModel).ToList(),
                                    CurrentPage = sales.Page.Value,
                                    NextPage = sales.NextLink != null ? sales.Page.Value + 1 : (int?)null,
                                    PreviousPage = sales.PrevLink != null ? sales.Page.Value - 1 : (int?)null,
                                    NumberOfPages = (int)Math.Ceiling((double)sales.TotalItems.Value / sales.PageSize.Value)
                                 };
            return View(salesViewModel);
        }

        private SaleViewModel CreateSaleViewModel(Sale sale)
        {
            string title = null;
            if (sale.ConfirmLink != null)
            {
                title = sale.ConfirmLink.Title;
            }
            if (sale.UploadETicketsLink != null)
            {
                title = sale.UploadETicketsLink.Title;
            }

            return new SaleViewModel
            {
                Resource = sale,
                ActionTitle = title
            };
        }
    }
}