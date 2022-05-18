using System.Runtime.Serialization;

namespace GogoKit.Models.Request
{
    [DataContract(Name = "base_barcode")]
    public class BaseBarcodeUpload
    {
        [DataMember(Name = "seat_ordinal")]
        public int? SeatOrdinal { get; set; }

        [DataMember(Name = "seat")]
        public string Seat { get; set; }

        [DataMember(Name = "row")]
        public string Row { get; set; }
    }

    [DataContract(Name = "barcode")]
    public class BarcodeUpload : BaseBarcodeUpload
    {
        [DataMember(Name = "barcode_values")]
        public string[] Values { get; set; }
    }

    [DataContract(Name = "barcode")]
    public class SellerListingBarcodeUpload : BaseBarcodeUpload
    {
        [DataMember(Name = "barcode_values_sha256_hashed")]
        public string[] BarcodeValuesSha256Hashed { get; set; }
    }

    [DataContract(Name = "barcode")]
    public class SaleBarcodeUpload : BaseBarcodeUpload
    {
        [DataMember(Name = "barcode_values")]
        public string[] BarcodeValues { get; set; }
    }
}