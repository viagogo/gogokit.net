using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GogoKit.Enumerations
{

    namespace Viagogo.Api.Enumerations
    {
        [DataContract]
        public enum ApiTicketType
        {
            [EnumMember]
            PaperTicket = 0,

            [EnumMember]
            ETicket = 1,

            [EnumMember]
            TesseraDelTifoso = 2,

            [EnumMember]
            SeasonCard = 3,

            [EnumMember]
            PaperTicketNameChange = 4,

            [EnumMember]
            ETicketNameChange = 5,

            [EnumMember]
            ReservationWithoutFaceValue = 7,

            [EnumMember]
            ReservationWithFaceValue = 8,

            [EnumMember]
            FlashSeatsTicket = 9,

            [EnumMember]
            TicketMasterMobile = 10,

            [EnumMember]
            ETicketUrl = 11,

            [EnumMember]
            WalkIn = 12,

            [EnumMember]
            MobileQRCode = 13,

            [EnumMember]
            FlashseatsAXS = 14,

            [EnumMember]
            UefaTransfer = 15
        }
    }

}
