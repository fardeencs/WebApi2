namespace Logic.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusinessEntitties;
    using Domain;
    using Domain.Enumrations;

    public interface ISupplierAgencyServices
    {
        Task<List<SupplierAgencyDetails>> GetSupplierAgencyBasicDetails(string agencyCode, string status);
        Task<SupplierAgencyDetails> GetSupplierAgencyBasicDetails(string agencyCode, string status, string supplierCode);
        Task<List<SupplierAgencyDetails>> GetSupplierAgencyBasicDetailswithsuppliercode(string agencyCode, string status, string supplierCode);
        SupplierAgencyDetails GetSupplierRouteBySupplierCodeAndAgencyCode(string agencyCode, string supplierCode, string routeFlag);        
        Task<bool> AddSupplierResponse(List<AgencySupplierResponseEntity> message);
        Task<string> GetAgencySupplierResponse(AgencySupplierResponseEntity message);       
        Task<GetTripDetailsModelRS> GetTravellerDetailsfromDB(string bookingRefID);
        Task<string> GetUserIDfromRefID(string BookingRefID);
        Task SaveLog(string logName, string agency, string rQ, string rS);        
    }
}
