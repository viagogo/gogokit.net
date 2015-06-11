using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Extensions;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using GogoKit.Services;
using HalKit;
using HalKit.Http;
using HalKit.Models.Response;

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

        public Task<PaymentMethod> GetAsync(int paymentMethodId)
        {
            return GetAsync(paymentMethodId, new PaymentMethodRequest());
        }

        public async Task<PaymentMethod> GetAsync(int paymentMethodId, PaymentMethodRequest request)
        {
            var paymentMethodLink = await _linkFactory.CreateLinkAsync(
                                            "paymentMethods/{0}",
                                            paymentMethodId).ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PaymentMethod>(paymentMethodLink, request).ConfigureAwait(_halClient);
        }

        public async Task<PaymentMethods> GetAsync(PaymentMethodRequest request)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAsync<PaymentMethods>(user.PaymentMethodsLink, request).ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<PaymentMethod>> GetAllAsync()
        {
            return GetAllAsync(new PaymentMethodRequest());
        }

        public async Task<IReadOnlyList<PaymentMethod>> GetAllAsync(PaymentMethodRequest request)
        {
            var user = await _userClient.GetAsync().ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<PaymentMethod>(user.PaymentMethodsLink, request).ConfigureAwait(_halClient);
        }

        public Task<PaymentMethod> CreateAsync(NewCreditCard creditCard)
        {
            return CreatePaymentMethodAsync(creditCard, p => p.CreateCreditCardLink);
        }

        public Task<PaymentMethod> CreateAsync(NewPayPal paypal)
        {
            return CreatePaymentMethodAsync(paypal, p => p.CreatePayPalLink);
        }

        private async Task<PaymentMethod> CreatePaymentMethodAsync(
            NewPaymentMethod paymentMethod,
            Func<PaymentMethods, Link> getLinkFunc)
        {
            var paymentMethodsPage = await GetAsync(new PaymentMethodRequest {PageSize = 1}).ConfigureAwait(_halClient);
            return await _halClient.PostAsync<PaymentMethod>(getLinkFunc(paymentMethodsPage),
                                                             paymentMethod).ConfigureAwait(_halClient);
        }

        public Task<PaymentMethod> UpdateAsync(int paymentMethodId, NewCreditCard creditCard)
        {
            return PutPaymentMethodAsync(paymentMethodId, creditCard, p => p.UpdateCreditCardLink);
        }

        public Task<PaymentMethod> UpdateAsync(int paymentMethodId, NewPayPal paypal)
        {
            return PutPaymentMethodAsync(paymentMethodId, paypal, p => p.UpdatePayPalLink);
        }

        private Task<PaymentMethod> PutPaymentMethodAsync(
            int paymentMethodId,
            NewPaymentMethod paymentMethod,
            Func<PaymentMethod, Link> getLinkFunc)
        {
            return GetPaymentMethodAndFollowLinkAsync(
                paymentMethodId,
                getLinkFunc,
                putLink => _halClient.PutAsync<PaymentMethod>(putLink, paymentMethod));
        }

        public Task<PaymentMethod> UpdateAsync(int paymentMethodId, PaymentMethodUpdate paymentMethod)
        {
            return GetPaymentMethodAndFollowLinkAsync(
                paymentMethodId,
                p => p.UpdateDefaultsLink,
                link => _halClient.PatchAsync<PaymentMethod>(link, paymentMethod));
        }

        public Task<IApiResponse> DeleteAsync(int paymentMethodId)
        {
            return GetPaymentMethodAndFollowLinkAsync(
                paymentMethodId,
                p => p.DeleteLink,
                deleteLink => _halClient.DeleteAsync(deleteLink));
        }

        private async Task<T> GetPaymentMethodAndFollowLinkAsync<T>(
            int paymentMethodId,
            Func<PaymentMethod, Link> getLinkFunc,
            Func<Link, Task<T>> followLinkFunc)
        {
            var paymentMethod = await GetAsync(paymentMethodId).ConfigureAwait(_halClient);
            return await followLinkFunc(getLinkFunc(paymentMethod)).ConfigureAwait(_halClient);
        }
    }
}