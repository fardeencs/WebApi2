

namespace WebApi.Infrastructure.Client.Transfer
{
    using Domain;
    using Handlers.Features.Transfer.Book;
    using Handlers.Features.Transfer.Cancel;
    using Handlers.Features.Transfer.Confirm;
    using Handlers.Features.Transfer.Details;
    using Handlers.Features.Transfer.Select;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using WebApi.Infrastructure.Handlers.Features.Transfer.Search;
    public interface ITransferPartnerClient
    {
        Task<ResponsePackage> GetGTASearchData(string baseUri, string reqUri, SearchTransferModel model);
        Task<ResponsePackage> GetGTASelectData(string baseUri, string reqUri, SelectTransferModel model);
        Task<ResponsePackage> GetBookData(string baseUri, string reqUri, BookTransferModel model);
        Task<ResponsePackage> GetConfirmBookData(string baseUri, string reqUri, ConfirmTransferModel model);
        Task<ResponsePackage> DetailsBookData(string baseUri, string reqUri, TransferBookDetailsModel model);
        Task<ResponsePackage> CancelBookData(string baseUri, string reqUri, CancelTransferModel model);
    }
}