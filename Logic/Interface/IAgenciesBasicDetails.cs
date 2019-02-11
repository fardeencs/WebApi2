

namespace Logic.Interface
{
    using BusinessEntitties;
    using System.Threading.Tasks;

    public interface IAgenciesBasicDetails
    {
        Task<SupplierAgencyDetails> GeBasicDetailsOfSupplier(string agencyCode, string supplierCode, string status);
        SupplierAgencyDetails GeBasicDetailsOfAmadeous(string agencyCode, string status);
        SupplierAgencyDetails GetBasicDetailsOfMystifly(string agencyCode, string status);
        SupplierAgencyDetails GetBasicDetailsOfPyton(string agencyCode, string status);
    }
}