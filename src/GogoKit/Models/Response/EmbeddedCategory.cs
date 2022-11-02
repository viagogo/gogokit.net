using System.Runtime.Serialization;
using HalKit.Models.Response;
using HalKit.Json;
using System.Collections.Generic;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// A Category represents a grouping of events or other categories. Examples are “Concerts”, “Rock and Pop” and “Lady Gaga”.
    /// </summary>
    [DataContract]
    public class EmbeddedCategory : Resource
    {
        /// <summary>
        /// The category identifier.
        /// </summary>
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// The name of the category.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The external mappings for this category.
        /// </summary>
        [Embedded("external_mappings")]
        public EmbeddedExternalMappingResource[] ExternalMappings { get; set; }

        /// <summary>
        /// The categories that have been merged into this category.
        /// </summary>
        [Embedded("merged_categories")]
        public IReadOnlyList<MergedEntity> MergedCategories { get; set; }

        /// <summary>
        /// The image asset of the category.
        /// </summary>
        [Embedded("image_assets")]
        public IReadOnlyList<ImageAsset> ImageAssets { get; set; }
    }
}
