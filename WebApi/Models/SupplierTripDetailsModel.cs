using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessEntitties;
using Common;
using MediatR;

namespace WebApi.Models
{

    public class SupplierTripDetailsModel : IAsyncRequest<ResponseObject>
    {
        //public TripDetails tripDetails { get; set; }
        public string UniqueID { get; set; }
        public string Target { get; set; }
        public string AgencyCode { get; set; }
        public string SupplierCode { get; set; }
        public List<SupplierAgencyDetails> supplierAgencyDetails { get; set; }
    }
    public class TripDetails
    {
        public string UniqueID { get; set; }
        public string Target { get; set; }
        public string AgencyCode { get; set; }
        public string SupplierCode { get; set; }
        public List<SupplierAgencyDetails> supplierAgencyDetails { get; set; }
    }
}