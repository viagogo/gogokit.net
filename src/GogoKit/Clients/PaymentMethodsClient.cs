using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Models;
using GogoKit.Requests;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class PaymentMethodsClient : IPaymentMethodsClient
    {
        private readonly IUserClient _userClient;
        private readonly IHypermediaConnection _connection;
        private readonly IResourceLinkComposer _linkHelper;

        public PaymentMethodsClient(IUserClient userClient,
                                    IHypermediaConnection connection,
                                    IResourceLinkComposer linkHelper)
        {
            _userClient = userClient;
            _connection = connection;
            _linkHelper = linkHelper;
        }

        public async Task<PagedResource<PaymentMethod>> GetAsync(int page, int pageSize)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAsync<PagedResource<PaymentMethod>>(
                user.Links["user:paymentmethods"],
                null).ConfigureAwait(_connection);
        }

        public async Task<IReadOnlyList<PaymentMethod>> GetAllAsync()
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_connection);
            return await _connection.GetAllPagesAsync<PaymentMethod>(
                user.Links["user:paymentmethods"],
                null).ConfigureAwait(_connection);
        }

        public async Task<PaymentMethod> GetAsync(int paymentMethodId)
        {
            var paymentMethodLink = await _linkHelper.ComposeLinkWithAbsolutePathForResource(
                                        "paymentMethods/{0}".FormatUri(paymentMethodId)).ConfigureAwait(_connection);
            return await _connection.GetAsync<PaymentMethod>(paymentMethodLink, null).ConfigureAwait(_connection);
        }

        public Task<PaymentMethod> CreateAsync(NewCreditCard creditCard)
        {
            return CreatePaymentMethodAsync(creditCard, "paymentmethod:createcreditcard");
        }

        public Task<PaymentMethod> CreateAsync(NewPayPal paypal)
        {
            return CreatePaymentMethodAsync(paypal, "paymentmethod:createpaypal");
        }

        private async Task<PaymentMethod> CreatePaymentMethodAsync(
            NewPaymentMethod paymentMethod,
            string createLinkRel)
        {
            var paymentMethodsPage = await GetAsync(1, 1).ConfigureAwait(_connection);
            return await _connection.PostAsync<PaymentMethod>(
                paymentMethodsPage.Links[createLinkRel],
                null,
                paymentMethod).ConfigureAwait(_connection);
        }

        public Task<PaymentMethod> UpdateAsync(int paymentMethodId, NewCreditCard creditCard)
        {
            return PutPaymentMethodAsync(paymentMethodId, creditCard, "paymentmethod:updatecreditcard");
        }

        public Task<PaymentMethod> UpdateAsync(int paymentMethodId, NewPayPal paypal)
        {
            return PutPaymentMethodAsync(paymentMethodId, paypal, "paymentmethod:updatepaypal");
        }

        private Task<PaymentMethod> PutPaymentMethodAsync(
            int paymentMethodId,
            NewPaymentMethod paymentMethod,
            string linkRel)
        {
            return GetPaymentMethodAndFollowLinkAsync(
                paymentMethodId,
                linkRel,
                putLink => _connection.PutAsync<PaymentMethod>(putLink, null, paymentMethod));
        }

        public Task<PaymentMethod> UpdateAsync(int paymentMethodId, PaymentMethodUpdate paymentMethod)
        {
            return GetPaymentMethodAndFollowLinkAsync(
                paymentMethodId,
                "paymentmethod:updatedefaults",
                link => _connection.PatchAsync<PaymentMethod>(link, null, paymentMethod));
        }

        public Task<IApiResponse> DeleteAsync(int paymentMethodId)
        {
            return GetPaymentMethodAndFollowLinkAsync(
                paymentMethodId,
                "paymentmethod:delete",
                deleteLink => _connection.DeleteAsync(deleteLink, null));
        }

        private async Task<T> GetPaymentMethodAndFollowLinkAsync<T>(
            int paymentMethodId,
            string linkRel,
            Func<Link, Task<T>> followLinkFunc)
        {
            var paymentMethod = await GetAsync(paymentMethodId).ConfigureAwait(_connection);
            return await followLinkFunc(paymentMethod.Links[linkRel]).ConfigureAwait(_connection);
        }
    }
}