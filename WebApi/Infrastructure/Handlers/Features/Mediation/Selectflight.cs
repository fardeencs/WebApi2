using Common;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Web.Core.Client;
using WebApi.Infrastructure.Client;
using Domain;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Logic.Interface;
using WebApi.Infrastructure.Common;
using System;
using WebApi.Models;
using BusinessEntitties;


#pragma warning disable 1998
#pragma warning disable 618

namespace WebApi.Infrastructure.Handlers.Features.Mediation
{
    public class SelectFlights : IAsyncRequestHandler<SelectFlightModel, ResponseObject>
    {



        private readonly IPartnerClient partnerClient;
        private readonly ISupplierAgencyServices supplierAgencyServices;
        private readonly IBookingServices bookingServices;

        public SelectFlights(ISupplierAgencyServices _supplierAgencyServices, IBookingServices _bookingServices)
        {
            this.supplierAgencyServices = _supplierAgencyServices;
            this.bookingServices = _bookingServices;
            var apiClient = new ApiClient();
            partnerClient = new PartnerClient(apiClient);
        }
        public async Task<ResponseObject> Handle(SelectFlightModel message)
        {
            List<Domain.SelectFlightResponse> allsupplierData = new List<Domain.SelectFlightResponse>();
            bool mystiflyResponse = await GetDataFromMystifly(allsupplierData, message);

            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = allsupplierData,
                Message = "Data retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }

        private async Task<bool> GetDataFromMystifly(List<Domain.SelectFlightResponse> list, SelectFlightModel model)
        {
            var supplierAgencyDetails = supplierAgencyServices.GetSupplierRouteBySupplierCodeAndAgencyCode(model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode
                    , model.CommonRequestFarePricer.Body.AirRevalidate.ARSupplierCode, "select/flights");
            List<SupplierAgencyDetails> supplierAgencyDetailslist = new List<SupplierAgencyDetails> { supplierAgencyDetails };
            model.CommonRequestFarePricer.SupplierAgencyDetails = supplierAgencyDetailslist;
            string cardType = bookingServices.GetPaymentCardType(model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode);
            model.CommonRequestFarePricer.Body.AirRevalidate.paymentCardType = cardType;

            string req = JsonConvert.SerializeObject(model);
            var result = await partnerClient.Getselectflight(supplierAgencyDetails.BaseUrl, supplierAgencyDetails.RequestUrl, model);
            string strData = JsonConvert.SerializeObject(result.Data);
            string requestStr = JsonConvert.SerializeObject(model);
            string responseStr = JsonConvert.SerializeObject(result);
            string agencyCode = model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode;
            await supplierAgencyServices.SaveLog("Select-Flight", agencyCode, requestStr, responseStr);
            Domain.SelectFlightResponse partnerResponseEntity = JsonConvert.DeserializeObject<Domain.SelectFlightResponse>(strData);
            if (partnerResponseEntity != null)
            {
                list.Add(partnerResponseEntity);
                return true;
            }
            return false;
        }
    }
}
