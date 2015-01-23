using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Models;
using GogoKit.Requests;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface IPaymentMethodClient
    {
        Task<IReadOnlyList<PaymentMethod>> GetAllPaymentMethodsAsync();
        Task<PaymentMethod> GetPaymentMethodAsync(int paymentMethodId);
        Task<PaymentMethod> CreateCreditCardPaymentMethodAsync(NewPaymentMethod paymentMethod);
        Task<PaymentMethod> CreatePaypalPaymentMethodAsync(NewPaymentMethod paymentMethod);
        Task<PaymentMethod> UpdatePaypalPaymentMethodAsync(int paymentMethodId, PaymentMethodUpdate paymentMethodUpdate);
        Task<PaymentMethod> UpdateCreditCardPaymentMethodAsync(int paymentMethodId, PaymentMethodUpdate paymentMethodUpdate);
        Task<IApiResponse> DeletePaymentMethodAsync(int paymentMethodId);
    }
}