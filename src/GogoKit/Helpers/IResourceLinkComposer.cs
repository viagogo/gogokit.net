using System;
using System.Threading.Tasks;
using GogoKit.Models;

namespace GogoKit.Helpers
{
    public interface IResourceLinkComposer
    {
        Task<Link> ComposeLinkWithAbsolutePathForResource(Uri relativeUri);
    }
}