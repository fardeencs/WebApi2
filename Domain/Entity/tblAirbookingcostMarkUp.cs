namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAirbookingcostMarkUp")]
    public partial class tblAirbookingcostMarkUp
    {
        [Key]
        public long CostMakrUpID { get; set; }

        public long? BookingCostID { get; set; }

        [Column(TypeName = "money")]
        public decimal? MarkUpAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? CommonMarkUpAmount { get; set; }

        public long? LevelID { get; set; }

        public int? LevelType { get; set; }
    }
}
