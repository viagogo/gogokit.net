using GogoKit;
using GogoKit.Exceptions;
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

        [Route("")]
        public async Task<ActionResult> Index(int? page = 1)
        {
            var sales = await _viagogoClient.Sales.GetAsync(new SaleRequest { Page = page });
            var salesViewModel = new SalesViewModel
                                 {
                                    Sales = sales.Items,
                                    CurrentPage = sales.Page.Value,
                                    NextPage = sales.NextLink != null ? sales.Page.Value + 1 : (int?)null,
                                    PreviousPage = sales.PrevLink != null ? sales.Page.Value - 1 : (int?)null,
                                    NumberOfPages = (int)Math.Ceiling((double)sales.TotalItems.Value / sales.PageSize.Value)
                                 };
            return View(salesViewModel);
        }

        [Route("sales/{saleId}")]
        public async Task<ActionResult> Details(int saleId)
        {
            Sale sale;
            try
            {
                sale = await _viagogoClient.Sales.GetAsync(saleId);
                return View("SaleDetails", sale);
            }
            catch (ResourceNotFoundException)
            {
                return HttpNotFound();
            }
        }

        [Route("sales/{saleId}/confirm")]
        [HttpPost]
        public async Task<ActionResult> ConfirmSale(int saleId)
        {
            Sale sale;
            try
            {
                sale = await _viagogoClient.Sales.ConfirmSaleAsync(saleId, new SaleRequest());
                return View("SaleDetails", sale);
            }
            catch (ResourceNotFoundException)
            {
                return HttpNotFound();
            }
        }
    }
}