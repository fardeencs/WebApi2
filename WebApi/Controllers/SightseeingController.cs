
namespace WebApi.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Http;
    using MediatR;
    using WebApi.Infrastructure.Handlers.Features.Mediation;
    using WebApi.Models;
    using Domain;
    using System;
    using WebApi.Infrastructure.Handlers.Features.SightSeeing.Search;
    using WebApi.Infrastructure.Handlers.Features.SightSeeing.Select;
    using WebApi.Infrastructure.Handlers.Features.SightSeeing.Book;
    using WebApi.Infrastructure.Handlers.Features.SightSeeing.Confirm;
    using Infrastructure.Handlers.Features.SightSeeing.Details;
    using Infrastructure.Handlers.Features.SightSeeing.Cancel;
    [RoutePrefix("api/sightseeing")]
    public class SightseeingController : BaseController
    {
        public SightseeingController(IMediator mediatR)
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
        [Route("search/sightseeing")]
        public async Task<IHttpActionResult> SearchSightseeing(SightseeingSearch model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }



        [HttpPost]
        [Route("select/sightseeing")]
        public async Task<IHttpActionResult> SelectSightseeing(SelectSigntseeingModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }

        [HttpPost]
        [Route("book/sightseeing")]
        public async Task<IHttpActionResult> BookSightseeing(BookSightSeeingModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }

        [HttpPost]
        [Route("Confirm/sightseeing")]
        public async Task<IHttpActionResult> ConfirmBookSightseeing(ConfirmSightSeeingBookingModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }
        [HttpPost]
        [Route("details/sightseeing")]
        public async Task<IHttpActionResult> DetailsSightseeing(SightSeeingDetailsModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }
        [HttpPost]
        [Route("details/sightseeing")]
        public async Task<IHttpActionResult> CancelSightseeing(CancelSightSeeingModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }

    }
}
