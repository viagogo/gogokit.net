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
    }
}