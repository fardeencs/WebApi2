
namespace WebApi.Infrastructure.Client.Sightseeing
{
    using Domain;
    using Handlers.Features.SightSeeing.Cancel;
    using Handlers.Features.SightSeeing.Details;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using WebApi.Infrastructure.Handlers.Features.SightSeeing.Book;
    using WebApi.Infrastructure.Handlers.Features.SightSeeing.Confirm;
    using WebApi.Infrastructure.Handlers.Features.SightSeeing.Search;
    using WebApi.Infrastructure.Handlers.Features.SightSeeing.Select;

    public interface ISightSeeingPartnerClient
    {

        Task<ResponsePackage> GetGTASearchData(string baseUri, string reqUri, SightseeingSearch model);
        Task<ResponsePackage> GetGTASelectData(string baseUri, string reqUri, SelectSigntseeingModel model);
        Task<ResponsePackage> GetBookData(string baseUri, string reqUri, BookSightSeeingModel model);
        Task<ResponsePackage> GetConfirmBookData(string baseUri, string reqUri, ConfirmSightSeeingBookingModel model);
        Task<ResponsePackage> DetailsBookData(string baseUri, string reqUri, SightSeeingDetailsModel model);
        Task<ResponsePackage> CancelBookData(string baseUri, string reqUri, CancelSightSeeingModel model);
    }
}