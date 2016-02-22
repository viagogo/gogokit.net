using GogoKit;
using GogoKit.Exceptions;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using ManagingSales.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ManagingSales.Controllers
{
    public class ETicketsController : Controller
    {
        private readonly IViagogoClient _viagogoClient;

        public ETicketsController(IViagogoClient viagogoClient)
        {
            _viagogoClient = viagogoClient;
        }

        [Route("sales/{saleId}/etickets")]
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

            if (sale == null ||
                sale.ETicketUploadsLink == null ||
                sale.UploadETicketsLink == null ||
                sale.SaveETicketsLink == null)
            {
                return HttpNotFound();
            }

            var eticketUploads = await _viagogoClient.Hypermedia.GetAllPagesAsync<ETicketUpload>(sale.ETicketUploadsLink);
            var etickets = eticketUploads.SelectMany(u => u.ETickets)
                                         .OrderBy(e => e.Id)
                                         .Select(e => new SelectListItem
                                                      {
                                                           Text = $"ETicket #{e.Id}",
                                                           Value = e.Id.Value.ToString()
                                                      })
                                         .ToList();
            var viewModel = new ETicketUploadsViewModel {Sale = sale, ETickets = etickets};

            return View(viewModel);
        }

        [Route("sales/{saleId}/etickets")]
        [HttpPost]
        public async Task<ActionResult> Index(int saleId, string[] selectedFiles)
        {
            Sale sale;
            try
            {
                sale = await _viagogoClient.Sales.GetAsync(saleId, new SaleRequest());
            }
            catch (ResourceNotFoundException)
            {
                return HttpNotFound();
            }

            await _viagogoClient.Sales.SaveETicketsAsync(saleId, selectedFiles.Select(int.Parse), new SaleRequest());

            return RedirectToAction("Details", "Sales",  new { saleId = saleId });
        }

        [Route("uploadetickets")]
        [HttpPost]
        public async Task<ActionResult> Upload(int saleId, HttpPostedFileBase eticketFile)
        {
            Sale sale;
            try
            {
                sale = await _viagogoClient.Sales.GetAsync(saleId, new SaleRequest());
            }
            catch (ResourceNotFoundException)
            {
                return HttpNotFound();
            }

            using (var fileMemoryStream = new MemoryStream())
            {
                await eticketFile.InputStream.CopyToAsync(fileMemoryStream);
                var pdfFileBytes = fileMemoryStream.ToArray();

                await _viagogoClient.Sales.UploadETicketsAsync(
                    sale,
                    eticketFile.FileName,
                    pdfFileBytes,
                    new ETicketUploadRequest());
            }

            return RedirectToAction("Index", new { saleId = saleId });
        }
    }
}