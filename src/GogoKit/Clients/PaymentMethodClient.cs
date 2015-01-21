using System;
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
        private readonly IHypermediaConnection _connection;
        private readonly IResourceLinkComposer _resourceLinkComposer;

        private static Uri GetPaymentMethodUri(int paymentMethodId)
        {
            return "paymentMethods/{0}".FormatUri(paymentMethodId);
        }

        private static Uri UpdatePaymentMethodUri(int paymentMethodId)
        {
            return "paymentMethods/{0}".FormatUri(paymentMethodId);
        }

        private static Uri DeletePaymentMethodUri(int paymentMethodId)
        {
            return "paymentMethods/{0}".FormatUri(paymentMethodId);
        }

        public PaymentMethodClient(IUserClient userClient,
                                   IHypermediaConnection connection,
                                   IResourceLinkComposer resourceLinkComposer)
        {
            _userClient = userClient;
            _connection = connection;
            _resourceLinkComposer = resourceLinkComposer;
        }

        public async Task<IReadOnlyList<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAllPagesAsync<PaymentMethod>(user.Links["user:paymentmethods"], null).ConfigureAwait(_connection);
        }

        public async Task<PaymentMethod> GetPaymentMethodAsync(int paymentMethodId)
        {
            var updatePaymentMethodUrl = await _resourceLinkComposer.ComposeLinkWithAbsolutePathForResource(GetPaymentMethodUri(paymentMethodId))
                                                                    .ConfigureAwait(_connection);
            return await _connection.GetAsync<PaymentMethod>(updatePaymentMethodUrl, null).ConfigureAwait(_connection);
        }

        public Task<PaymentMethod> CreatePaypalPaymentMethodAsync(PaymentMethodCreate paymentMethod)
        {
            return CreatePaymentMethod(paymentMethod, "PayPal");
        }

        private async Task<PaymentMethod> CreatePaymentMethod(PaymentMethodCreate paymentMethod, string paymentMethodType)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.PostAsync<PaymentMethod>(
                user.Links["user:paymentmethods"],
                new Dictionary<string, string> {{"paymentMethodType", paymentMethodType}},
                paymentMethod).ConfigureAwait(_connection);
        }

        public Task<PaymentMethod> CreateCreditCardPaymentMethodAsync(PaymentMethodCreate paymentMethod)
        {
            return CreatePaymentMethod(paymentMethod, "CreditCard");
        }

        public Task<PaymentMethod> UpdatePaypalPaymentMethodAsync(int paymentMethodId, PaymentMethodUpdate paymentMethodUpdate)
        {
            return UpdatePaymentMethod(paymentMethodId, paymentMethodUpdate, "PayPal");
        }

        public Task<PaymentMethod> UpdateCreditCardPaymentMethodAsync(int paymentMethodId, PaymentMethodUpdate paymentMethodUpdate)
        {
            return UpdatePaymentMethod(paymentMethodId, paymentMethodUpdate, "CreditCard");
        }

        private async Task<PaymentMethod> UpdatePaymentMethod(
            int paymentMethodId,
            PaymentMethodUpdate paymentMethodUpdate,
            string paymentMethodType)
        {
            var updatePaymentMethodUri = await _resourceLinkComposer.ComposeLinkWithAbsolutePathForResource(UpdatePaymentMethodUri(paymentMethodId))
                                                                    .ConfigureAwait(_connection);
            return await _connection.PatchAsync<PaymentMethod>(
                updatePaymentMethodUri,
                new Dictionary<string, string> { { "paymentMethodType", paymentMethodType } },
                paymentMethodUpdate).ConfigureAwait(_connection);
        }

        public async Task<IApiResponse> DeletePaymentMethodAsync(int paymentMethodId)
        {
            var deletePaymentMethodLink = await _resourceLinkComposer.ComposeLinkWithAbsolutePathForResource(DeletePaymentMethodUri(paymentMethodId))
                                                                     .ConfigureAwait(_connection);
            return await _connection.DeleteAsync(deletePaymentMethodLink, null).ConfigureAwait(_connection);
        }
    }
}