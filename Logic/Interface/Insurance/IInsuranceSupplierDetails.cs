

namespace Logic.Interface.Insurance
{
    using BusinessEntitties.Insurance;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public interface IInsuranceSupplierDetails
    {
        Task<InsuranceSupplierCredentials> GeBasicDetailsOfIsuranceSupplier(string agencyCode, string supplierCode, string status);
        InsuranceSupplierCredentials GetSupplierRouteBySupplierCodeAndAgencyCode(string agencyCode, string supplierCode, string status);
    }
}
