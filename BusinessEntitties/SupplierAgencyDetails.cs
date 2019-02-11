﻿namespace BusinessEntitties
{
    public class SupplierAgencyDetails
    {
        public long AgencyID { get; set; }
        public string AgencyCode { get; set; }
        public long SupplierId { get; set; }
        public string BaseUrl { get; set; }
        public string RequestUrl { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string AccountNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string ToCurrency { get; set; }
        public decimal? ToROEValue { get; set; }

    }
}