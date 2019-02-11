

namespace WebApi.Controllers
{
    using Infrastructure.Handlers.Features.Insurance.Book;
    using Infrastructure.Handlers.Features.Insurance.Cancel;
    using Infrastructure.Handlers.Features.Insurance.Confirm;
    using Infrastructure.Handlers.Features.Insurance.Details;
    using Infrastructure.Handlers.Features.Insurance.Search;
    using Infrastructure.Handlers.Features.Insurance.Select;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Http;


    [RoutePrefix("api/insurance")]
    public class InsuranceController : BaseController
    {

        public InsuranceController(IMediator mediatR)
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
        [Route("search/insurance")]
        public async Task<IHttpActionResult> SearchInsurance(SearchInsuranceModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }
        [HttpPost]
        [Route("select/insurance")]
        public async Task<IHttpActionResult> SelectInsurance(SelectInsuranceModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }
        [HttpPost]
        [Route("book/insurance")]
        public async Task<IHttpActionResult> BookInsurance(BookInsuranceModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }
        [HttpPost]
        [Route("confirm/insurance")]
        public async Task<IHttpActionResult> ConfirmBooking(ConfirmInsuranceRequestModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }
        [HttpPost]
        [Route("details/insurance")]
        public async Task<IHttpActionResult> DetailsBooking(InsuranceDetailsRequestModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }
        [HttpPost]
        [Route("cancel/insurance")]
        public async Task<IHttpActionResult> CancelInsurance(CancelInsuranceBookingModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }

    }
}
