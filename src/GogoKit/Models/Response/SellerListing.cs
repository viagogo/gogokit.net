using HalKit.Json;
using HalKit.Models.Response;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    [DataContract]
    public class SellerListing : Resource
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name = "created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [DataMember(Name = "number_of_tickets")]
        public int? NumberOfTickets { get; set; }

        [DataMember(Name = "display_number_of_tickets")]
        public int? DisplayNumberOfTickets { get; set; }

        [DataMember(Name = "seating")]
        public Seating Seating { get; set; }

        [DataMember(Name = "face_value")]
        public Money FaceValue { get; set; }

        [DataMember(Name = "ticket_price")]
        public Money TicketPrice { get; set; }

        [DataMember(Name = "ticket_proceeds")]
        public Money TicketProceeds { get; set; }

        [Embedded("event")]
        public EmbeddedEvent Event { get; set; }

        [Embedded("venue")]
        public EmbeddedVenue Venue { get; set; }

        [Embedded("ticket_type")]
        public TicketType TicketType { get; set; }

        [Embedded("split_type")]
        public SplitType SplitType { get; set; }

        [Embedded("listing_notes")]
        public IList<ListingNote> ListingNotes { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the <see cref="ListingConstraints"/>
        /// resource for updating a <see cref="SellerListing"/>.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#sellerlistingconstraints.</remarks>
        [Rel("sellerlisting:constraints")]
        public Link ConstraintsLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the <see cref="Address"/>
        /// where the ticket(s) are located.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#sellerlistingticketlocation.</remarks>
        [Rel("sellerlisting:ticketlocation")]
        public Link TicketLocationLink { get; set; }

        /// <summary>
        /// You can use a PATCH request on the href of this link to update the
        /// type of tickets in a listing.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#sellerlistingupdatetickettype.</remarks>
        [Rel("sellerlisting:updatetickettype")]
        public Link UpdateTicketTypeLink { get; set; }

        /// <summary>
        /// You can use a PATCH request on the href of this link to update the
        /// way that tickets in the listing are allowed be split.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#sellerlistingupdatesplittype.</remarks>
        [Rel("sellerlisting:updatesplittype")]
        public Link UpdateSplitTypeLink { get; set; }

        /// <summary>
        /// You can use a PATCH request on the href of this link to update the
        /// price of each ticket in a listing.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#sellerlistingupdateticketprice.</remarks>
        [Rel("sellerlisting:updateticketprice")]
        public Link UpdateTicketPriceLink { get; set; }

        /// <summary>
        /// You can use a PATCH request on the href of this link to update the
        /// number of tickets available for purchase.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#sellerlistingupdatenumberoftickets.</remarks>
        [Rel("sellerlisting:updatenumberoftickets")]
        public Link UpdateNumberOfTicketsLink { get; set; }

        /// <summary>
        /// You can use a PATCH request on the href of this link to update the
        /// location where the tickets are located.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#sellerlistingupdateticketlocation.</remarks>
        [Rel("sellerlisting:updateticketlocation")]
        public Link UpdateTicketLocationLink { get; set; }

        /// <summary>
        /// You can use a PATCH request on the href of this link to update the
        /// notes for a listing.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#sellerlistingupdateticketlocation.</remarks>
        [Rel("sellerlisting:updatenotes")]
        public Link UpdateNotesLink { get; set; }

        /// <summary>
        /// You can use a PATCH request on the href of this link to update the
        /// the face value of each ticket in a listing.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#sellerlistingupdatefacevalue.</remarks>
        [Rel("sellerlisting:updatefacevalue")]
        public Link UpdateFaceValueLink { get; set; }

        /// <summary>
        /// You can use a PATCH request on the href of this link to make a
        /// listing available for purchase on the viagogo marketplace.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#sellerlistingpublish.</remarks>
        [Rel("sellerlisting:publish")]
        public Link PublishLink { get; set; }

        /// <summary>
        /// You can use a PATCH request on the href of this link to make a
        /// listing unavailable for purchase on the viagogo marketplace.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#sellerlistingunpublish.</remarks>
        [Rel("sellerlisting:unpublish")]
        public Link UnpublishLink { get; set; }

        /// <summary>
        /// You can use a DELETE request on the href of this link to delete
        /// a <see cref="SellerListing"/> for the current user.
        /// </summary>
        [Rel("sellerlisting:delete")]
        public Link DeleteLink { get; set; }
    }

    [DataContract]
    public class SellerListingPreview : SellerListing
    {
        /// <summary>
        /// You can use a POST request on the href of this link to create a
        /// <see cref="SellerListing"/> for this event.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#eventcreatesellerlisting.</remarks>
        [Rel("event:createsellerlisting")]
        public Link CreateListingLink { get; set; }
    }
}
