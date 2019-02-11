
namespace Logic.Interface.Transfer
{
    using BusinessEntitties.Transfer;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ITranserSupplierDetails
    {
        Task<TransferSupplierCredentials> GeBasicDetailsOfTransferSupplier(string agencyCode, string supplierCode, string status);
        TransferSupplierCredentials GetSupplierRouteBySupplierCodeAndAgencyCode(string agencyCode, string supplierCode, string status);
    }
}
