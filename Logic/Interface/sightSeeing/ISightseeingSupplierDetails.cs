
namespace Logic.Interface.sightSeeing
{
    using BusinessEntitties.SightSeeing;
    using System.Threading.Tasks;
   public interface ISightseeingSupplierDetails
    {
        
        Task<SupplierCredentials> GeBasicDetailsOfsightseeingSupplier(string agencyCode, string supplierCode, string status);
        SupplierCredentials GetSupplierRouteBySupplierCodeAndAgencyCode(string agencyCode, string supplierCode, string status);
    }
}
