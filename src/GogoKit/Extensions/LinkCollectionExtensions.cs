using HalKit.Models;

namespace GogoKit
{
    public static class LinkCollectionExtensions
    {
        public static Link TryGetLink(this LinkCollection links, string rel)
        {
            Link link;
            links.TryGetLink(rel, out link);

            return link;
        }
    }
}
