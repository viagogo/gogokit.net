using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GogoKit.Models
{
    public class LinkCollection : IEnumerable<Link>
    {
        private readonly IReadOnlyDictionary<string, Link> _links;

        public LinkCollection(IEnumerable<Link> links)
        {
            Requires.ArgumentNotNull(links, "links");

            _links = links.ToDictionary(l => l.Rel);
        }

        public Link this[string rel]
        {
            get { return _links[rel]; }
        }

        public bool TryGetLink(string rel, out Link link)
        {
            return _links.TryGetValue(rel, out link);
        }

        public bool HasLink(string rel)
        {
            return _links.ContainsKey(rel);
        }

        public IEnumerator<Link> GetEnumerator()
        {
            return _links.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
