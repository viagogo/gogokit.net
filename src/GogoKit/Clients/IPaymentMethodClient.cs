using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface IPaymentMethodClient
    {
        Task<IReadOnlyList<PaymentMethod>> GetAllPaymentMethodsAsync();
        Task<PaymentMethod> GetPaymentMethodAsync(int paymentMethodId);
        Task<PaymentMethod> CreateCreditCardPaymentMethod(PaymentMethodCreate paymentMethod);
        Task<PaymentMethod> CreatePaypalPaymentMethod(PaymentMethodCreate paymentMethod);
        Task<PaymentMethod> UpdatePaypalPaymentMethod(int paymentMethodId, PaymentMethodUpdate paymentMethodUpdate);
        Task<PaymentMethod> UpdateCreditCardPaymentMethod(int paymentMethodId, PaymentMethodUpdate paymentMethodUpdate);
    }
}