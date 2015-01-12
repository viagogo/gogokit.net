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
        Task<PaymentMethod> CreatePaymentMethod(PaymentMethodCreate paymentMethod, string paymentMethodType);
    }
}