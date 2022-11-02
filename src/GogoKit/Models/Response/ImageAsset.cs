using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    [DataContract(Name = "image_asset")]
    public class ImageAsset
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "template")]
        public string Template { get; set; }

        [DataMember(Name = "placeholders")]
        public string[] PlaceHolders { get; set; }
    }
}
