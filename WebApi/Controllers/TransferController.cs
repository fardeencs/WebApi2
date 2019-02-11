

namespace WebApi.Controllers
{
    using Infrastructure.Handlers.Features.Transfer.Book;
    using Infrastructure.Handlers.Features.Transfer.Cancel;
    using Infrastructure.Handlers.Features.Transfer.Confirm;
    using Infrastructure.Handlers.Features.Transfer.Details;
    using Infrastructure.Handlers.Features.Transfer.Search;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/transfer")]
    public class TransferController :  BaseController
    {
        public TransferController(IMediator mediatR)
        {
            MediatR = mediatR;
        }

        [HttpGet]
        [Route("version")]
        public string Version()
        {
            return $"{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        [HttpPost]
        [Route("search/transfer")]
        public async Task<IHttpActionResult> SearchTransfer(SearchTransferModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }

        [HttpPost]
        [Route("select/transfer")]
        public async Task<IHttpActionResult> SelectTransfer(SearchTransferModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }

        [Route("book/transfer")]
        public async Task<IHttpActionResult> BookTransfer(BookTransferModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }
        [Route("confirm/transfer")]
        public async Task<IHttpActionResult> ConfirmBookTransfer(ConfirmTransferModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }
        [Route("details/transfer")]
        public async Task<IHttpActionResult> DetailsTransfer(TransferBookDetailsModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }
        [Route("cancel/transfer")]
        public async Task<IHttpActionResult> CancelTransfer(CancelTransferModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }

    }
}
