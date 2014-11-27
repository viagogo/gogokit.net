using System;
using System.Collections.Generic;
using Viagogo.Sdk.Models;

namespace Viagogo.Sdk.Helpers
{
    public interface ILinkResolver
    {
        Uri ResolveLink(Link link, IDictionary<string, string> parameters);
    }
}
