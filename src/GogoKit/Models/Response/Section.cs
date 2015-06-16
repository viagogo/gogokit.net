using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class Section
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "free_text_row")]
        public bool IsFreeTextRow { get; set; }

        [DataMember(Name = "rows")]
        public IList<Row> Rows { get; set; }
    }
}
