


namespace WebApi.Infrastructure.Handlers.Features.SightSeeing.Details
{
    using Common;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Threading.Tasks;
    using global::Common;
    using Logic.Interface.sightSeeing;
    using Client.Sightseeing;
    using System.Net.Http;
    using System.Net;
    using Newtonsoft.Json;
    public class SightSeeingDetails : IAsyncRequestHandler<SightSeeingDetailsModel, ResponseObject>
    {
        private readonly ISightseeingSupplierDetails sightseeingSupplierDetails;
        private readonly ISightSeeingPartnerClient sightSeeingPartnerClient;

        public async Task<ResponseObject> Handle(SightSeeingDetailsModel message)
        {
            List<SightSeeingDetailsResponseEntity> allsupplierData = new List<SightSeeingDetailsResponseEntity>();
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

        private async Task<bool> GetDataFromSightSeeing(List<SightSeeingDetailsResponseEntity> list, SightSeeingDetailsModel model)
        {
            var supplierAgencyDetails = sightseeingSupplierDetails.GetSupplierRouteBySupplierCodeAndAgencyCode("GAT001"
                    , "GAT001", "select/flights");
            // List<SupplierCredentials> supplierAgencyDetailslist = new List<SupplierCredentials> { SupplierCredentials };
            //model.CommonRequestFarePricer.SupplierAgencyDetails = supplierAgencyDetailslist;
            // string cardType = bookingServices.GetPaymentCardType(model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode);
            // model.CommonRequestFarePricer.Body.AirRevalidate.paymentCardType = cardType;

            string req = JsonConvert.SerializeObject(model);
            var result = await sightSeeingPartnerClient.DetailsBookData("supplierAgencyDetails.BaseUrl", "supplierAgencyDetails.RequestUrl", model);
            string strData = JsonConvert.SerializeObject(result.Data);
            string requestStr = JsonConvert.SerializeObject(model);
            string responseStr = JsonConvert.SerializeObject(result);
            //string agencyCode = model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode;

            SightSeeingDetailsResponseEntity partnerResponseEntity = JsonConvert.DeserializeObject<SightSeeingDetailsResponseEntity>(strData);
            if (partnerResponseEntity != null)
            {
                list.Add(partnerResponseEntity);
                return true;
            }
            return false;
        }

    }
}