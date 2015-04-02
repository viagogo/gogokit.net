using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Extensions;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using GogoKit.Services;
using HalKit;
using HalKit.Http;
using HalKit.Models;

namespace GogoKit.Clients
{
    public class PaymentMethodsClient : IPaymentMethodsClient
    {
        private readonly IUserClient _userClient;
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public PaymentMethodsClient(IUserClient userClient,
                                    IHalClient halClient,
                                    ILinkFactory linkFactory)
        {
            _userClient = userClient;
            _halClient = halClient;
            _linkFactory = linkFactory;
        }

        public async Task<PagedResource<PaymentMethod>> GetAsync(int page, int pageSize)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PagedResource<PaymentMethod>>(
                user.Links["user:paymentmethods"],
                null).ConfigureAwait(_halClient);
        }

        public async Task<IReadOnlyList<PaymentMethod>> GetAllAsync()
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<PaymentMethod>(
                user.Links["user:paymentmethods"],
                null).ConfigureAwait(_halClient);
        }

        public async Task<PaymentMethod> GetAsync(int paymentMethodId)
        {
            var paymentMethodLink = await _linkFactory.CreateLinkAsync("paymentMethods/{0}", paymentMethodId).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PaymentMethod>(paymentMethodLink, null).ConfigureAwait(_halClient);
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
            var paymentMethodsPage = await GetAsync(1, 1).ConfigureAwait(_halClient);
            return await _halClient.PostAsync<PaymentMethod>(
                paymentMethodsPage.Links[createLinkRel],
                paymentMethod).ConfigureAwait(_halClient);
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
                putLink => _halClient.PutAsync<PaymentMethod>(putLink, paymentMethod));
        }

        public Task<PaymentMethod> UpdateAsync(int paymentMethodId, PaymentMethodUpdate paymentMethod)
        {
            return GetPaymentMethodAndFollowLinkAsync(
                paymentMethodId,
                "paymentmethod:updatedefaults",
                link => _halClient.PatchAsync<PaymentMethod>(link, paymentMethod));
        }

        public Task<IApiResponse> DeleteAsync(int paymentMethodId)
        {
            return GetPaymentMethodAndFollowLinkAsync(
                paymentMethodId,
                "paymentmethod:delete",
                deleteLink => _halClient.DeleteAsync(deleteLink));
        }

        private async Task<T> GetPaymentMethodAndFollowLinkAsync<T>(
            int paymentMethodId,
            string linkRel,
            Func<Link, Task<T>> followLinkFunc)
        {
            var paymentMethod = await GetAsync(paymentMethodId).ConfigureAwait(_halClient);
            return await followLinkFunc(paymentMethod.Links[linkRel]).ConfigureAwait(_halClient);
        }
    }
}