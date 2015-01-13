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
        private readonly IApiConnection _connection;
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
            var updatePaymentMethodUrl = await _resourceLinkComposer.ComposeLinkWithAbsolutePathForResource(GetPaymentMethodUri(paymentMethodId));
            return await _connection.GetAsync<PaymentMethod>(updatePaymentMethodUrl, null);
        }

        public async Task<PaymentMethod> CreatePaypalPaymentMethod(PaymentMethodCreate paymentMethod)
        {
            return await CreatePaymentMethod(paymentMethod, "PayPal");
        }

        private async Task<PaymentMethod> CreatePaymentMethod(PaymentMethodCreate paymentMethod, string paymentMethodType)
        {
            var user = await _userClient.GetAsync();
            return await _connection.PostAsync<PaymentMethod>(user.Links["user:paymentmethods"], new Dictionary<string, string> {{"paymentMethodType", paymentMethodType}}, paymentMethod);
        }

        public async Task<PaymentMethod> CreateCreditCardPaymentMethod(PaymentMethodCreate paymentMethod)
        {
            return await CreatePaymentMethod(paymentMethod, "CreditCard");
        }

        public async Task<PaymentMethod> UpdatePaypalPaymentMethod(int paymentMethodId, PaymentMethodUpdate paymentMethodUpdate)
        {
            return await UpdatePaymentMethod(paymentMethodId, paymentMethodUpdate, "PayPal");
        }

        public async Task<PaymentMethod> UpdateCreditCardPaymentMethod(int paymentMethodId, PaymentMethodUpdate paymentMethodUpdate)
        {
            return await UpdatePaymentMethod(paymentMethodId, paymentMethodUpdate, "CreditCard");
        }

        private async Task<PaymentMethod> UpdatePaymentMethod(int paymentMethodId, PaymentMethodUpdate paymentMethodUpdate, string paymentMethodType)
        {
            var updatePaymentMethodUri = await _resourceLinkComposer.ComposeLinkWithAbsolutePathForResource(UpdatePaymentMethodUri(paymentMethodId));
            return await _connection.PatchAsync<PaymentMethod>(updatePaymentMethodUri, new Dictionary<string, string> { { "paymentMethodType", paymentMethodType } }, paymentMethodUpdate);
        }

        public async Task<IApiResponse> DeletePaymentMethod(int paymentMethodId)
        {
            var deletePaymentMethodLink = await _resourceLinkComposer.ComposeLinkWithAbsolutePathForResource(DeletePaymentMethodUri(paymentMethodId));
            return await _connection.DeleteAsync(deletePaymentMethodLink, null);
        }
    }
}