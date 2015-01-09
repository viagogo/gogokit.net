using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Models;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class PaymentMethodClient : IPaymentMethodClient
    {
        private readonly IUserClient _userClient;
        private readonly IApiConnection _connection;
        private readonly IResourceLinkComposer _resourceLinkComposer;

        public PaymentMethodClient(IUserClient userClient, IApiConnection connection, IResourceLinkComposer resourceLinkComposer)
        {
            _userClient = userClient;
            _connection = connection;
            _resourceLinkComposer = resourceLinkComposer;
        }

        public async Task<IReadOnlyList<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            var user = await _userClient.GetAsync();
            return await _connection.GetAllPagesAsync<PaymentMethod>(user.Links["user:paymentmethods"], null);
        }

        public async Task<PaymentMethod> GetPaymentMethodAsync(int paymentMethodId)
        {
            var updatePaymentMethodUrl = await _resourceLinkComposer.ComposeLinkWithAbsolutePathForResource(ApiUrls.GetPaymentMethod(paymentMethodId));
            return await _connection.GetAsync<PaymentMethod>(updatePaymentMethodUrl, null);
        }
    }
}