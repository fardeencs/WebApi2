

namespace WebApi.Infrastructure.Client.Insurance
{
    using Domain;
    using Handlers.Features.Insurance.Book;
    using Handlers.Features.Insurance.Cancel;
    using Handlers.Features.Insurance.Confirm;
    using Handlers.Features.Insurance.Details;
    using Handlers.Features.Insurance.Search;
    using Handlers.Features.Insurance.Select;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    public interface IInsurancePartnerClient
    {
        
        Task<ResponsePackage> GetGTASearchData(string baseUri, string reqUri, SearchInsuranceModel model);
        Task<ResponsePackage> GetGTASelectData(string baseUri, string reqUri, SelectInsuranceModel model);
        Task<ResponsePackage> GetBookData(string baseUri, string reqUri, BookInsuranceModel model);
        Task<ResponsePackage> GetConfirmBookData(string baseUri, string reqUri, ConfirmInsuranceRequestModel model);

        Task<ResponsePackage> DetailsBookData(string baseUri, string reqUri, InsuranceDetailsRequestModel model);
        Task<ResponsePackage> CancelBookData(string baseUri, string reqUri, CancelInsuranceBookingModel model);
    }
}