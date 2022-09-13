using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// An entity that has been merged into another entity.
    /// </summary>
    [DataContract]
    public class MergedEntity
    {
        /// <summary>
        /// The identifier for this entity.
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }
    }
}
