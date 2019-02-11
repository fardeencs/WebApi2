using Common;
using Logic.Interface.sightSeeing;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using WebApi.Infrastructure.Client.Sightseeing;

namespace WebApi.Infrastructure.Handlers.Features.SightSeeing.Confirm
{
    public class ConfirmSightSeeingBooking : IAsyncRequestHandler<ConfirmSightSeeingBookingModel, ResponseObject>
    {
        private readonly ISightseeingSupplierDetails sightseeingSupplierDetails;
        private readonly ISightSeeingPartnerClient sightSeeingPartnerClient;
        //
        public async Task<ResponseObject> Handle(ConfirmSightSeeingBookingModel message)
        {
            List<ConfirmSightSeeingBookingResponseEntity> allsupplierData = new List<ConfirmSightSeeingBookingResponseEntity>();
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

        private async Task<bool> GetDataFromSightSeeing(List<ConfirmSightSeeingBookingResponseEntity> list, ConfirmSightSeeingBookingModel model)
        {
            var supplierAgencyDetails = sightseeingSupplierDetails.GetSupplierRouteBySupplierCodeAndAgencyCode("GAT001"
                    , "GAT001", "select/flights");
            // List<SupplierCredentials> supplierAgencyDetailslist = new List<SupplierCredentials> { SupplierCredentials };
            //model.CommonRequestFarePricer.SupplierAgencyDetails = supplierAgencyDetailslist;
            // string cardType = bookingServices.GetPaymentCardType(model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode);
            // model.CommonRequestFarePricer.Body.AirRevalidate.paymentCardType = cardType;

            string req = JsonConvert.SerializeObject(model);
            var result = await sightSeeingPartnerClient.GetConfirmBookData("supplierAgencyDetails.BaseUrl", "supplierAgencyDetails.RequestUrl", model);
            string strData = JsonConvert.SerializeObject(result.Data);
            string requestStr = JsonConvert.SerializeObject(model);
            string responseStr = JsonConvert.SerializeObject(result);
            //string agencyCode = model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode;

            ConfirmSightSeeingBookingResponseEntity partnerResponseEntity = JsonConvert.DeserializeObject<ConfirmSightSeeingBookingResponseEntity>(strData);
            if (partnerResponseEntity != null)
            {
                list.Add(partnerResponseEntity);
                return true;
            }
            return false;
        }
    }
}