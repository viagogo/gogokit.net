using System;
using System.Globalization;

namespace GogoKit.Clients
{
    internal static class StringExtensions
    {
        public static Uri FormatUri(this string pattern, params object[] args)
        {
            Requires.ArgumentNotNullOrEmpty(pattern, "pattern");

            return new Uri(string.Format(CultureInfo.InvariantCulture, pattern, args), UriKind.Relative);
        }
    }
}