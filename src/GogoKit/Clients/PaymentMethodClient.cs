using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Http;
using GogoKit.Models;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class PaymentMethodClient : IPaymentMethodClient
    {
        private readonly IUserClient _userClient;
        private readonly IApiConnection _connection;

        public PaymentMethodClient(IUserClient userClient, IApiConnection connection)
        {
            _userClient = userClient;
            _connection = connection;
        }

        public async Task<IReadOnlyList<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            var user = await _userClient.GetAsync();
            return await _connection.GetAllPagesAsync<PaymentMethod>(user.Links["user:paymentmethods"], null);
        }

        public async Task<PaymentMethod> GetPaymentMethodAsync(Link paymentMethodLink)
        {
            return await _connection.GetAsync<PaymentMethod>(paymentMethodLink, null);
        }
    }
}