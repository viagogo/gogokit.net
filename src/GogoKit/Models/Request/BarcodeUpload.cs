using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GogoKit.Models.Request
{
    [DataContract(Name = "barcode")]
    public class BarcodeUpload
    {
        [DataMember(Name = "seat_ordinal")]
        public int? SeatOrdinal { get; set; }

        [DataMember(Name = "seat")]
        public string Seat { get; set; }

        [DataMember(Name = "row")]
        public string Row { get; set; }

        [DataMember(Name = "barcode_values")]
        public string[] Values { get; set; }
    }
}
