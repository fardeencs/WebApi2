
namespace WebApi.Infrastructure.Handlers.Features.Insurance.Select
{
    using Client.Insurance;
    using Common;
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

    public class SelectInsurance : IAsyncRequestHandler<SelectInsuranceModel, ResponseObject>
    {
        private readonly IInsurancePartnerClient insurancePartnerClient;
        private readonly IInsuranceSupplierDetails insuranceSupplierDetails;

        public async Task<ResponseObject> Handle(SelectInsuranceModel message)
        {
            List<SelectInsuranceResponseEntity> allsupplierData = new List<SelectInsuranceResponseEntity>();
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

        private async Task<bool> GetDataFromSightSeeing(List<SelectInsuranceResponseEntity> list, SelectInsuranceModel model)
        {
            var supplierAgencyDetails = insuranceSupplierDetails.GetSupplierRouteBySupplierCodeAndAgencyCode("GAT001"
                    , "GAT001", "select/flights");
            // List<SupplierCredentials> supplierAgencyDetailslist = new List<SupplierCredentials> { SupplierCredentials };
            //model.CommonRequestFarePricer.SupplierAgencyDetails = supplierAgencyDetailslist;
            // string cardType = bookingServices.GetPaymentCardType(model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode);
            // model.CommonRequestFarePricer.Body.AirRevalidate.paymentCardType = cardType;

            string req = JsonConvert.SerializeObject(model);
            var result = await insurancePartnerClient.GetGTASelectData("supplierAgencyDetails.BaseUrl", "supplierAgencyDetails.RequestUrl", model);
            string strData = JsonConvert.SerializeObject(result.Data);
            string requestStr = JsonConvert.SerializeObject(model);
            string responseStr = JsonConvert.SerializeObject(result);
            //string agencyCode = model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode;

            SelectInsuranceResponseEntity partnerResponseEntity = JsonConvert.DeserializeObject<SelectInsuranceResponseEntity>(strData);
            if (partnerResponseEntity != null)
            {
                list.Add(partnerResponseEntity);
                return true;
            }
            return false;
        }


    }
}