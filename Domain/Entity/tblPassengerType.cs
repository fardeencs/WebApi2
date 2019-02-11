namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPassengerType")]
    public partial class tblPassengerType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PaxTypeID { get; set; }

        [StringLength(25)]
        public string PaxType { get; set; }
    }
}
