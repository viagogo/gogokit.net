using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    public class PaymentMethods : PagedResource<PaymentMethod>
    {
        [Rel("paymentmethod:createcreditcard")]
        public Link CreateCreditCardLink { get; set; }

        [Rel("paymentmethod:createpaypal")]
        public Link CreatePayPalLink { get; set; }
    }
}
