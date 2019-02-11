namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblCardDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblCardDetail()
        {
            tblOnlinePaymentOptions = new HashSet<tblOnlinePaymentOption>();
        }

        [Key]
        public long CardID { get; set; }

        [StringLength(25)]
        public string Name { get; set; }

        public int? Status { get; set; }

        [StringLength(10)]
        public string Type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOnlinePaymentOption> tblOnlinePaymentOptions { get; set; }
    }
}
