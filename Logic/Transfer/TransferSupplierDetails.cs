
using BusinessEntitties.Transfer;
using DAL;
using Logic.Interface.Transfer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Transfer
{


    public class   TransferSupplierDetails : ITranserSupplierDetails
    {

        public async Task<TransferSupplierCredentials> GeBasicDetailsOfTransferSupplier(string agencyCode, string supplierCode, string status)
        {
            using (var _ctx = new MediationEntities())
            {
                const string sql = @"EXEC spGetSupplierBasicDetails @agencyCode, @supplierCode, @status";
                var result = await _ctx.Database
                    .SqlQuery<TransferSupplierCredentials>(
                              sql
                            , new SqlParameter("agencyCode", agencyCode)
                            , new SqlParameter("supplierCode", supplierCode)
                            , new SqlParameter("status", status)

                    ).FirstOrDefaultAsync();


                return result;
            }
        }

        public TransferSupplierCredentials GetSupplierRouteBySupplierCodeAndAgencyCode(string agencyCode, string supplierCode, string routeFlag)
        {
            using (var _ctx = new MediationEntities())
            {
                var supplierBasicDetails = (from sm in _ctx.tblSupplierMasters
                                            join ag in _ctx.tblAgencies on sm.AgencyID equals ag.AgencyID
                                            join sapi in _ctx.tblSupplierApiInfoes on sm.SupplierId equals sapi.SupplierId
                                            join roe in _ctx._tblRateofExchange on sm.AgencyID equals roe.AgencyID
                                            where sm.Status == "T" && ag.AgencyCode == agencyCode && sapi.SupplierRoute == routeFlag && sm.SupplierCode == supplierCode && roe.Status == 1
                                            select new TransferSupplierCredentials
                                            {
                                                //AccountNumber = sm.AccountNumber,
                                                //Password = sm.Password,
                                                //UserName = sm.UserName,
                                                //BaseUrl = sm.BaseUrl,
                                                //RequestUrl = sapi.RequestUrl,
                                                //AgencyID = ag.AgencyID,
                                                //AgencyCode = ag.AgencyCode,
                                                //SupplierId = sm.SupplierId,
                                                //ToCurrency = roe.CurrencyCode,
                                                //ToROEValue = roe.ReteofExchange,
                                                //SupplierCode = sm.SupplierCode
                                            }).FirstOrDefault();

                return supplierBasicDetails;

            }
        }

        TransferSupplierCredentials ITranserSupplierDetails.GetSupplierRouteBySupplierCodeAndAgencyCode(string agencyCode, string supplierCode, string status)
        {
            throw new NotImplementedException();
        }
    }
}
