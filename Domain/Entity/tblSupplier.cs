namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSupplier")]
    public partial class tblSupplier
    {
        [Key]
        public long SupplierID { get; set; }

        [StringLength(30)]
        public string SupplierName { get; set; }

        [StringLength(5)]
        public string SupplierCode { get; set; }
    }
}
