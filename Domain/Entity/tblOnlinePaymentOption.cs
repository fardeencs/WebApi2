namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblOnlinePaymentOption
    {
        [Key]
        public long OptionsID { get; set; }

        public long? AgencyID { get; set; }

        public long? CardID { get; set; }

        [StringLength(50)]
        public string CradHolderName { get; set; }

        [StringLength(50)]
        public string CardNo { get; set; }

        [StringLength(10)]
        public string CVV { get; set; }

        [StringLength(20)]
        public string Expiry { get; set; }

        public int? Status { get; set; }

        public virtual tblCardDetail tblCardDetail { get; set; }
    }
}
