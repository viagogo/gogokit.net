using System;
using GogoKit.Clients;

namespace GogoKit.Helpers
{
    public static class ApiUrls
    {
        public static Uri UpdateAddress(int addressId)
        {
            return "addresses/{0}".FormatUri(addressId);
        }

        public static Uri DeleteAddress(int addressId)
        {
            return "addresses/{0}".FormatUri(addressId);
        }

        public static Uri GetPaymentMethod(int paymentMethodId)
        {
            return "paymentMethods/{0}".FormatUri(paymentMethodId);
        }

        public static Uri GetCategoryChildren(int categoryId)
        {
            return "categories/{0}/children".FormatUri(categoryId);
        }
    }
}