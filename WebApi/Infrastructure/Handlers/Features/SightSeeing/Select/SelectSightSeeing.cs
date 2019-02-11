

namespace WebApi.Infrastructure.Handlers.Features.SightSeeing.Select
{
    using MediatR;
    using Common;
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Net.Http;
    using BusinessEntitties.SightSeeing;
    using Logic.Interface.sightSeeing;
    using System.Net;
    using Newtonsoft.Json;
    using WebApi.Infrastructure.Client.Sightseeing;
    using global::Common;

    public class SelectSightSeeing : IAsyncRequestHandler<SelectSigntseeingModel, ResponseObject>
    {

        private readonly ISightseeingSupplierDetails sightseeingSupplierDetails;
        private readonly ISightSeeingPartnerClient sightSeeingPartnerClient;

        public async Task<ResponseObject> Handle(SelectSigntseeingModel message)
        {
            List<SelectSightSeeingResponseEntity> allsupplierData = new List<SelectSightSeeingResponseEntity>();
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

        private async Task<bool> GetDataFromSightSeeing(List<SelectSightSeeingResponseEntity> list, SelectSigntseeingModel model)
        {
            var supplierAgencyDetails = sightseeingSupplierDetails.GetSupplierRouteBySupplierCodeAndAgencyCode("GAT001"
                    , "GAT001", "select/flights");
           // List<SupplierCredentials> supplierAgencyDetailslist = new List<SupplierCredentials> { SupplierCredentials };
           //model.CommonRequestFarePricer.SupplierAgencyDetails = supplierAgencyDetailslist;
           // string cardType = bookingServices.GetPaymentCardType(model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode);
           // model.CommonRequestFarePricer.Body.AirRevalidate.paymentCardType = cardType;

            string req = JsonConvert.SerializeObject(model);
            var result = await sightSeeingPartnerClient.GetGTASelectData("supplierAgencyDetails.BaseUrl", "supplierAgencyDetails.RequestUrl", model);
            string strData = JsonConvert.SerializeObject(result.Data);
            string requestStr = JsonConvert.SerializeObject(model);
            string responseStr = JsonConvert.SerializeObject(result);
            //string agencyCode = model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode;

            SelectSightSeeingResponseEntity partnerResponseEntity = JsonConvert.DeserializeObject<SelectSightSeeingResponseEntity>(strData);
            if (partnerResponseEntity != null)
            {
                list.Add(partnerResponseEntity);
                return true;
            }
            return false;
        }

    }
}