namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblAirBookingError
    {
        [Key]
        public long ErrorID { get; set; }

        public long? BookingRefID { get; set; }

        [StringLength(20)]
        public string ErrorCode { get; set; }

        [StringLength(100)]
        public string ErrorMessage { get; set; }
    }
}
