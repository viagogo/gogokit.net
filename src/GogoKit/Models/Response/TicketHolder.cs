using System;
using System.Runtime.Serialization;
using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    [DataContract(Name = "ticket_holder")]
    public class TicketHolderResource : Resource
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "full_name")]
        public string FullName { get; set; }

        [DataMember(Name = "date_of_birth")]
        public DateTimeOffset? DateOfBirth { get; set; }

        [DataMember(Name = "city_of_birth")]
        public string CityOfBirth { get; set; }

        [DataMember(Name = "province_of_birth")]
        public string ProvinceOfBirth { get; set; }

        [DataMember(Name = "city_of_residence")]
        public string CityOfResidence { get; set; }

        [DataMember(Name = "province_of_residence")]
        public string ProvinceOfResidence { get; set; }

        [DataMember(Name = "club_issuer")]
        public string ClubIssuer { get; set; }

        [DataMember(Name = "card_number")]
        public string CardNumber { get; set; }

        [DataMember(Name = "document_type")]
        public string DocumentType { get; set; }

        [DataMember(Name = "document_number")]
        public string DocumentNumber { get; set; }

        [DataMember(Name = "fiscal_code")]
        public string FiscalCode { get; set; }

        [DataMember(Name = "security_code")]
        public string SecurityCode { get; set; }

        [DataMember(Name = "generic_answer")]
        public string GenericAnswer { get; set; }

        [DataMember(Name = "email_address")]
        public string EmailAddress { get; set; }

        [Embedded("nationality_country")]
        public Country NationalityCountry { get; set; }

        [Embedded("document_issuing_country")]
        public Country DocumentIssuingCountry { get; set; }
    }
}
