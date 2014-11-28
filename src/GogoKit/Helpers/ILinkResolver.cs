using System;
using System.Collections.Generic;
using GogoKit.Models;

namespace GogoKit.Helpers
{
    public interface ILinkResolver
    {
        Uri ResolveLink(Link link, IDictionary<string, string> parameters);
    }
}
