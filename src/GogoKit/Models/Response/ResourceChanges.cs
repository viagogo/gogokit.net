using System.Collections.Generic;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// Represents the resources that have changed since a previous request made
    /// by your application.
    /// </summary>
    /// <remarks>See http://developer.viagogo.net/#getting-resource-changes-since-your-last-request</remarks>
    public class ChangedResources<T> where T : Resource
    {
        public ChangedResources(IReadOnlyList<T> newOrUpdatedResources,
                                IReadOnlyList<T> deletedResources,
                                Link nextLink)
        {
            Requires.ArgumentNotNull(newOrUpdatedResources, nameof(newOrUpdatedResources));
            Requires.ArgumentNotNull(deletedResources, nameof(deletedResources));

            NewOrUpdatedResources = newOrUpdatedResources;
            DeletedResources = deletedResources;
            NextLink = nextLink;
        }

        /// <summary>
        /// The new or updated <typeparamref name="T"/> resources.
        /// </summary>
        public IReadOnlyList<T> NewOrUpdatedResources { get; }

        /// <summary>
        /// The deleted <typeparamref name="T"/> resources.
        /// </summary>
        public IReadOnlyList<T> DeletedResources { get; }

        /// <summary>
        /// The <see cref="Link"/> that should be used to get subsequent
        /// <typeparamref name="T"/> resource changes.
        /// </summary>
        public Link NextLink { get; }
    }
}
