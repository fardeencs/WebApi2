﻿

namespace WebApi.Infrastructure.Handlers.Features.Insurance.Cancel
{
    using Client.Insurance;
    using global::Common;
    using Logic.Interface.Insurance;
    using MediatR;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;


    public class CancelInsuranceBooking : IAsyncRequestHandler<CancelInsuranceBookingModel, ResponseObject>
    {
        private readonly IInsurancePartnerClient insurancePartnerClient;
        private readonly IInsuranceSupplierDetails insuranceSupplierDetails;

        public async Task<ResponseObject> Handle(CancelInsuranceBookingModel message)
        {
            List<CancelInsuranceBookingResponseEntity> allsupplierData = new List<CancelInsuranceBookingResponseEntity>();
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

        private async Task<bool> GetDataFromSightSeeing(List<CancelInsuranceBookingResponseEntity> list, CancelInsuranceBookingModel model)
        {
            var supplierAgencyDetails = insuranceSupplierDetails.GetSupplierRouteBySupplierCodeAndAgencyCode("GAT001"
                    , "GAT001", "select/flights");
            // List<SupplierCredentials> supplierAgencyDetailslist = new List<SupplierCredentials> { SupplierCredentials };
            //model.CommonRequestFarePricer.SupplierAgencyDetails = supplierAgencyDetailslist;
            // string cardType = bookingServices.GetPaymentCardType(model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode);
            // model.CommonRequestFarePricer.Body.AirRevalidate.paymentCardType = cardType;

            string req = JsonConvert.SerializeObject(model);
            var result = await insurancePartnerClient.CancelBookData("supplierAgencyDetails.BaseUrl", "supplierAgencyDetails.RequestUrl", model);
            string strData = JsonConvert.SerializeObject(result.Data);
            string requestStr = JsonConvert.SerializeObject(model);
            string responseStr = JsonConvert.SerializeObject(result);
            //string agencyCode = model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode;

            CancelInsuranceBookingResponseEntity partnerResponseEntity = JsonConvert.DeserializeObject<CancelInsuranceBookingResponseEntity>(strData);
            if (partnerResponseEntity != null)
            {
                list.Add(partnerResponseEntity);
                return true;
            }
            return false;
        }


    }
}