namespace Logic.Interface
{
    using BusinessEntitties;
    using Domain;
    using System.Threading.Tasks;

    public interface IBookingServices
    {
        Task<BookingData> SavingAirBookingFlight(BusinessEntitties.BookFlightModel message, long agencyId, long supplierId);        
        bool CheckUniqIDExistOrNot(string uniqid);
        Task UpdateBookingStatusafterIssueTicket(string bookingStatus, long bookingRefID, long userID);
        string GetPaymentCardType(string agencyCode);
        Paymentinfo GetPaymentCardDetails(string agencyCode);
        Connectiontodbreq GetBookedSupplierInfo(long bookingrefID);
        void UpdateBookingInformatiomFromTripDetails(SupplierTripDetailsResponse supplierTripDetailsResponse, long bookingrefID);
        void UpdatePNRandStatus(BookFlightResponse response, BookingData bookingdata);
        bool CheckBookingRefIDExist(long bookingRefID);
        Task UpdateAllDetailsWithRefID(BusinessEntitties.BookFlightModel model);
        void UpdatePNRandStatus(BookFlightResponse response, BookingData bookingdata, BusinessEntitties.BookFlightModel model, string supplierCode);
        Task UpdateAirBookingAfterIssuedTicket(Itineraryinformation[] itineraryinformation, long bookingrefID, long userid, string pnr, string bookingStatus);
    }
}