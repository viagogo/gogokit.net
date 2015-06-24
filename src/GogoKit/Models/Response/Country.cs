using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class Country : Resource
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the local viagogo website
        /// for a country.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#countrylocalwebpage.</remarks>
		[Rel("country:localwebpage")]
        public Link LocalWebPageLink { get; set; }
    }
}