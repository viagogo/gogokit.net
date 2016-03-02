using GogoKit;
using GogoKit.Exceptions;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using HalKit.Models.Response;
using ManagingSales.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ManagingSales.Controllers
{
    public class ShipmentsController : Controller
    {
        private readonly IViagogoClient _viagogoClient;

        public ShipmentsController(IViagogoClient viagogoClient)
        {
            _viagogoClient = viagogoClient;
        }

        [Route("sales/{saleId}/shipments")]
        [HttpGet]
        public async Task<ActionResult> Index(int saleId)
        {
            Sale sale;
            try
            {
                sale = await _viagogoClient.Sales.GetAsync(saleId, new SaleRequest());
            }
            catch (ResourceNotFoundException)
            {
                sale = null;
            }

            if (sale == null || sale.ShipmentsLink == null)
            {
                return HttpNotFound();
            }

            var shipments = await _viagogoClient.Shipments.GetAsync(saleId);

            return View(new ShipmentsViewModel { Sale = sale, Shipments = shipments });
        }

        [Route("sales/{saleId}/createshipment")]
        public async Task<ActionResult> CreateShipment(int saleId)
        {
            Sale sale;
            try
            {
                sale = await _viagogoClient.Sales.GetAsync(saleId, new SaleRequest());
            }
            catch (ResourceNotFoundException)
            {
                sale = null;
            }

            if (sale == null || sale.ShipmentsLink == null)
            {
                return HttpNotFound();
            }

            var shipment = await _viagogoClient.Shipments.CreateAsync(saleId);
            return RedirectToAction("Index", new { saleId = saleId });
        }

        [Route("sales/{saleId}/shippinglabel")]
        public async Task<ActionResult> DownloadShippingLabel(int saleId, string labelHref)
        {
            byte[] label;
            try
            {
                label = await _viagogoClient.Shipments.GetShippingLabelAsync(new Link { HRef = labelHref });
            }
            catch (ResourceNotFoundException)
            {
                return HttpNotFound();
            }

            return File(label, "application/pdf");
        }
    }
}