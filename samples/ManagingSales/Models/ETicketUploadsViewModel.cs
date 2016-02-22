using GogoKit.Models.Response;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ManagingSales.Models
{
    public class ETicketUploadsViewModel
    {
        public Sale Sale { get; set; }
        public List<SelectListItem> ETickets { get; set; }
    }
}