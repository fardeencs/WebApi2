namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRateofExchange")]
    public partial class tblRateofExchange
    {
        [Key]
        [Column(Order = 0)]
        public long ExchangeID { get; set; }

        public long? AgencyID { get; set; }

        [Column(TypeName = "money")]
        public decimal? ReteofExchange { get; set; }

        [StringLength(5)]
        public string CurrencyCode { get; set; }

        public int? Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AddedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ModifyDate { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AddedBy { get; set; }
    }
}
