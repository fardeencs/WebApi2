

namespace WebApi.Infrastructure.Handlers.Features.Transfer.Select
{
    using Common;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Threading.Tasks;
    using global::Common;
    using Logic.Interface.Transfer;
    using Client.Transfer;
    using System.Net.Http;
    using System.Net;
    using Newtonsoft.Json;
    public class SelectTransfer : IAsyncRequestHandler<SelectTransferModel, ResponseObject>
    {
        private readonly ITranserSupplierDetails transferSupplierDetails;
        private readonly ITransferPartnerClient tarnsferPartnerClient;
       

        public async Task<ResponseObject> Handle(SelectTransferModel message)
        {
            List<SelectTransferResponseEntity> allsupplierData = new List<SelectTransferResponseEntity>();
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

        private async Task<bool> GetDataFromSightSeeing(List<SelectTransferResponseEntity> list, SelectTransferModel model)
        {
            var supplierAgencyDetails = transferSupplierDetails.GetSupplierRouteBySupplierCodeAndAgencyCode("GAT001"
                    , "GAT001", "select/flights");
            // List<SupplierCredentials> supplierAgencyDetailslist = new List<SupplierCredentials> { SupplierCredentials };
            //model.CommonRequestFarePricer.SupplierAgencyDetails = supplierAgencyDetailslist;
            // string cardType = bookingServices.GetPaymentCardType(model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode);
            // model.CommonRequestFarePricer.Body.AirRevalidate.paymentCardType = cardType;

            string req = JsonConvert.SerializeObject(model);
            var result = await tarnsferPartnerClient.GetGTASelectData("supplierAgencyDetails.BaseUrl", "supplierAgencyDetails.RequestUrl", model);
            string strData = JsonConvert.SerializeObject(result.Data);
            string requestStr = JsonConvert.SerializeObject(model);
            string responseStr = JsonConvert.SerializeObject(result);
            //string agencyCode = model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode;

            SelectTransferResponseEntity partnerResponseEntity = JsonConvert.DeserializeObject<SelectTransferResponseEntity>(strData);
            if (partnerResponseEntity != null)
            {
                list.Add(partnerResponseEntity);
                return true;
            }
            return false;
        }

    }
}