namespace Logic
{
    using BusinessEntitties;
    using BusinessEntitties.Enumrations;
    using DAL;
    using Logic.Interface;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    public class AgenciesBasicDetails : IAgenciesBasicDetails
    {

        public async Task<SupplierAgencyDetails> GeBasicDetailsOfSupplier(string agencyCode, string supplierCode, string status)
        {
            using (var _ctx = new MediationEntities())
            {
                const string sql = @"EXEC spGetSupplierBasicDetails @agencyCode, @supplierCode, @status";
                var result = await _ctx.Database
                    .SqlQuery<SupplierAgencyDetails>(
                              sql
                            , new SqlParameter("agencyCode", agencyCode)
                            , new SqlParameter("supplierCode", supplierCode)
                            , new SqlParameter("status", status)

                    ).FirstOrDefaultAsync();
               

                return result;
            }
        }
        public SupplierAgencyDetails GeBasicDetailsOfAmadeous(string agencyCode, string status)
        {
            using (var _ctx = new MediationEntities())
            {
                var supplierBasicDetails = (from sm in _ctx.tblSupplierMasters
                                            join ag in _ctx.tblAgencies on sm.AgencyID equals ag.AgencyID
                                            join roe in _ctx._tblRateofExchange on sm.AgencyID equals roe.AgencyID
                                            where sm.Status == status && ag.AgencyCode == agencyCode
                                            && sm.SupplierCode == SupplierCode.AMA001.ToString() && roe.Status == 1
                                            select new SupplierAgencyDetails
                                            {
                                                AccountNumber = sm.AccountNumber,
                                                AgencyID = ag.AgencyID,
                                                Password = sm.Password,
                                                Status = sm.Status,
                                                SupplierCode = sm.SupplierCode,
                                                // SupplierName = sm.SupplierName,
                                                UserName = sm.UserName,
                                                BaseUrl = sm.BaseUrl,
                                                ToCurrency = roe.CurrencyCode,
                                                ToROEValue = roe.ReteofExchange,
                                                AgencyCode = ag.AgencyCode
                                            }).FirstOrDefault();

                return supplierBasicDetails;
            }
        }
        public SupplierAgencyDetails GetBasicDetailsOfMystifly(string agencyCode, string status)
        {
            using (var _ctx = new MediationEntities())
            {
                var supplierBasicDetails = (from sm in _ctx.tblSupplierMasters
                                            join ag in _ctx.tblAgencies on sm.AgencyID equals ag.AgencyID
                                            join roe in _ctx._tblRateofExchange on sm.AgencyID equals roe.AgencyID
                                            where sm.Status == status && ag.AgencyCode == agencyCode
                                            && sm.SupplierCode == SupplierCode.MIS001.ToString() && roe.Status == 1
                                            select new SupplierAgencyDetails
                                            {
                                                AccountNumber = sm.AccountNumber,
                                                AgencyID = ag.AgencyID,
                                                Password = sm.Password,
                                                Status = sm.Status,
                                                SupplierCode = sm.SupplierCode,
                                                // SupplierName = sm.SupplierName,
                                                UserName = sm.UserName,
                                                BaseUrl = sm.BaseUrl,
                                                ToCurrency = roe.CurrencyCode,
                                                ToROEValue = roe.ReteofExchange,
                                                AgencyCode = ag.AgencyCode
                                            }).FirstOrDefault();

                return supplierBasicDetails;
            }
        }
        public SupplierAgencyDetails GetBasicDetailsOfPyton(string agencyCode, string status)
        {
            using (var _ctx = new MediationEntities())
            {
                var supplierBasicDetails = (from sm in _ctx.tblSupplierMasters
                                            join ag in _ctx.tblAgencies on sm.AgencyID equals ag.AgencyID
                                            join roe in _ctx._tblRateofExchange on sm.AgencyID equals roe.AgencyID
                                            where sm.Status == status && ag.AgencyCode == agencyCode
                                            && sm.SupplierCode == SupplierCode.PYT001.ToString() && roe.Status == 1
                                            select new SupplierAgencyDetails
                                            {
                                                AccountNumber = sm.AccountNumber,
                                                AgencyID = ag.AgencyID,
                                                Password = sm.Password,
                                                Status = sm.Status,
                                                SupplierCode = sm.SupplierCode,
                                                // SupplierName = sm.SupplierName,
                                                UserName = sm.UserName,
                                                BaseUrl = sm.BaseUrl,
                                                ToCurrency = roe.CurrencyCode,
                                                ToROEValue = roe.ReteofExchange,
                                                AgencyCode = ag.AgencyCode
                                            }).FirstOrDefault();

                return supplierBasicDetails;
            }
        }
    }
}
