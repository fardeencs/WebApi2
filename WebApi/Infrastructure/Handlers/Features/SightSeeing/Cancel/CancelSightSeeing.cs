using Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Logic.Interface.sightSeeing;
using WebApi.Infrastructure.Client.Sightseeing;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace WebApi.Infrastructure.Handlers.Features.SightSeeing.Cancel
{
    public class CancelSightSeeing : IAsyncRequestHandler<CancelSightSeeingModel, ResponseObject>
    {
        private readonly ISightseeingSupplierDetails sightseeingSupplierDetails;
        private readonly ISightSeeingPartnerClient sightSeeingPartnerClient;
        private object list;

        public async Task<ResponseObject> Handle(CancelSightSeeingModel message)
        {
            List<CancelSightSeeingResponseEntity> allsupplierData = new List<CancelSightSeeingResponseEntity>();
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

        private async Task<bool> GetDataFromSightSeeing(List<CancelSightSeeingResponseEntity> list, CancelSightSeeingModel model)
        {
            var supplierAgencyDetails = sightseeingSupplierDetails.GetSupplierRouteBySupplierCodeAndAgencyCode("GAT001"
                    , "GAT001", "select/flights");
            // List<SupplierCredentials> supplierAgencyDetailslist = new List<SupplierCredentials> { SupplierCredentials };
            //model.CommonRequestFarePricer.SupplierAgencyDetails = supplierAgencyDetailslist;
            // string cardType = bookingServices.GetPaymentCardType(model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode);
            // model.CommonRequestFarePricer.Body.AirRevalidate.paymentCardType = cardType;

            string req = JsonConvert.SerializeObject(model);
            var result = await sightSeeingPartnerClient.CancelBookData("supplierAgencyDetails.BaseUrl", "supplierAgencyDetails.RequestUrl", model);
            string strData = JsonConvert.SerializeObject(result.Data);
            string requestStr = JsonConvert.SerializeObject(model);
            string responseStr = JsonConvert.SerializeObject(result);
            //string agencyCode = model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode;

            CancelSightSeeingResponseEntity partnerResponseEntity = JsonConvert.DeserializeObject<CancelSightSeeingResponseEntity>(strData);
            if (partnerResponseEntity != null)
            {
                list.Add(partnerResponseEntity);
                return true;
            }
            return false;
        }

    }
}