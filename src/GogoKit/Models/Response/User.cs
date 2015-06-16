using System.Runtime.Serialization;
using HalKit.Models.Response;
using HalKit.Json;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class User : Resource
    {
        [DataMember(Name = "full_name")]
        public string FullName { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "primary_phone")]
        public string PrimaryPhone { get; set; }

        [DataMember(Name = "email_optin")]
        public bool EmailOptIn { get; set; }

        [Rel("user:update")]
        public Link UpdateLink { get; set; }

        [Rel("user:addresses")]
        public Link AddressesLink { get; set; }

        [Rel("user:paymentmethods")]
        public Link PaymentMethodsLink { get; set; }

        [Rel("user:purchases")]
        public Link PurchasesLink { get; set; }

        [Rel("user:sales")]
        public Link SalesLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the current user's
        /// <see cref="SellerListing"/>s.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#usersellerlistings.</remarks>
        [Rel("user:sellerlistings")]
        public Link SellerListingsLink { get; set; }
    }
}
