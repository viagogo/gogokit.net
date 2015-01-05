using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public interface IPaymentMethodClient
    {
        Task<IReadOnlyList<PaymentMethod>> GetAllPaymentMethodsAsync();
        Task<PaymentMethod> GetPaymentMethodAsync(Link paymentMethodLink);
    }
}