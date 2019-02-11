
using Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Domain;
using Logic.Interface.sightSeeing;
using WebApi.Infrastructure.Client.Sightseeing;
using Newtonsoft.Json;

namespace WebApi.Infrastructure.Handlers.Features.SightSeeing.Book
{
    public class BookSightSeeing : IAsyncRequestHandler<BookSightSeeingModel, ResponseObject>
    {
        private readonly ISightseeingSupplierDetails sightseeingSupplierDetails;
        private readonly ISightSeeingPartnerClient sightSeeingPartnerClient;


        public async Task<ResponseObject> Handle(BookSightSeeingModel message)
        {
            List<BookSightSeeingResponseEntity> allsupplierData = new List<BookSightSeeingResponseEntity>();

            bool mystiflyResponse = await GetDataFromSightSeeing(allsupplierData, message);

            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = allsupplierData,
                Message = "Data retrieved Successfully",
                IsSuccessful = true
            };
            return response;

        }

        private async Task<bool> GetDataFromSightSeeing(List<BookSightSeeingResponseEntity> list, BookSightSeeingModel model)
        {
            var supplierAgencyDetails = sightseeingSupplierDetails.GetSupplierRouteBySupplierCodeAndAgencyCode("GAT001"
                    , "GAT001", "select/flights");
            // List<SupplierCredentials> supplierAgencyDetailslist = new List<SupplierCredentials> { SupplierCredentials };
            //model.CommonRequestFarePricer.SupplierAgencyDetails = supplierAgencyDetailslist;
            // string cardType = bookingServices.GetPaymentCardType(model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode);
            // model.CommonRequestFarePricer.Body.AirRevalidate.paymentCardType = cardType;

            string req = JsonConvert.SerializeObject(model);
            var result = await sightSeeingPartnerClient.GetBookData("supplierAgencyDetails.BaseUrl", "supplierAgencyDetails.RequestUrl", model);
            string strData = JsonConvert.SerializeObject(result.Data);
            string requestStr = JsonConvert.SerializeObject(model);
            string responseStr = JsonConvert.SerializeObject(result);
            //string agencyCode = model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode;

            BookSightSeeingResponseEntity partnerResponseEntity = JsonConvert.DeserializeObject<BookSightSeeingResponseEntity>(strData);
            if (partnerResponseEntity != null)
            {
                list.Add(partnerResponseEntity);
                return true;
            }
            return false;
        }
    }
}