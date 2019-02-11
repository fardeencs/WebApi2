
namespace WebApi.Models
{

    using Common;
    using MediatR;
    using System.Collections.Generic;
    using Domain.ViewModels;
    using Domain;
    using BusinessEntitties;

    public class CancelBookingModel : IAsyncRequest<ResponseObject>
    {
        //public cancelBookedPNR cancelBookedPNR { get; set; }
        public string UniqueId { get; set; }
        public string Target { get; set; }
        public string AgencyCode { get; set; }
        public string SupplierCode { get; set; }
        public long BookingRefID { get; set; }
        public long UserID { get; set; }
        public List<SupplierAgencyDetails> supplierAgencyDetails { get; set; }
    }
    public class cancelBookedPNR
    {
        public string UniqueId { get; set; }
        public string Target { get; set; }
        public string AgencyCode { get; set; }
        public string SupplierCode { get; set; }
        public long BookingRefID { get; set; }
        public long UserID { get; set; }
    }
}