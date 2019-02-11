using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitties
{
    public class RateOFExchange
    {
        public string CurrencyCode { get; set; }
        public decimal? RateofAmount { get; set; }
    }
    public class Connectiontodbreq
    {
        public string AgencyCodeDb { get; set; }
        public string SupplierCodeDb { get; set; }
        public string BookingRefID { get; set; }
        public string SupplierUniqueId { get; set; }
        public string SupplierPnr { get; set; }
        public bool Issupplier { get; set; }
        public string Target { get; set; }
        public string BookingStatusCode { get; set; }
    }
}
