using System;
using System.Runtime.Serialization;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// Represents a window of time when a courier pickup can be arranged.
    /// </summary>
    /// <remarks>See http://developer.viagogo.net/#pickupwindow</remarks>
    [DataContract(Name = "pickup_window")]
    public class PickupWindow
    {
        /// <summary>
        /// The date when the window starts.
        /// </summary>
        [DataMember(Name = "start_date")]
        public DateTimeOffset? StartDate { get; set; }

        /// <summary>
        /// The date when the window ends.
        /// </summary>
        [DataMember(Name = "end_date")]
        public DateTimeOffset? EndDate { get; set; }
    }
}
