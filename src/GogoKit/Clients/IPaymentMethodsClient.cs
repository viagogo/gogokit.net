using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Clients
{
    public interface IPaymentMethodsClient
    {
        Task<PagedResource<PaymentMethod>> GetAsync(int page, int pageSize);
        Task<IReadOnlyList<PaymentMethod>> GetAllAsync();
        Task<PaymentMethod> GetAsync(int paymentMethodId);
        Task<PaymentMethod> CreateAsync(NewCreditCard creditCard);
        Task<PaymentMethod> CreateAsync(NewPayPal paypal);
        Task<PaymentMethod> UpdateAsync(int paymentMethodId, NewCreditCard creditCard);
        Task<PaymentMethod> UpdateAsync(int paymentMethodId, NewPayPal paypal);
        Task<PaymentMethod> UpdateAsync(int paymentMethodId, PaymentMethodUpdate paymentMethod);
        Task<IApiResponse> DeleteAsync(int paymentMethodId);
    }
}