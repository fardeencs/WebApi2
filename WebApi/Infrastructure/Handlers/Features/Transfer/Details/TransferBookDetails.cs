

namespace WebApi.Infrastructure.Handlers.Features.Transfer.Details
{
    using Confirm;
    using global::Common;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Threading.Tasks;
    using Client.Transfer;
    using Logic.Interface.Transfer;
    using System.Net.Http;
    using Newtonsoft.Json;
    using System.Net;
    public class TransferBookDetails : IAsyncRequestHandler<TransferBookDetailsModel, ResponseObject>
    {
        private readonly ITransferPartnerClient transferPartnerClient;
        private readonly ITranserSupplierDetails transferSupplierDetails;

        public async Task<ResponseObject> Handle(TransferBookDetailsModel message)
        {
            List<TransferBookdetailsresponseEntity> allsupplierData = new List<TransferBookdetailsresponseEntity>();

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

        private async Task<bool> GetDataFromSightSeeing(List<TransferBookdetailsresponseEntity> list, TransferBookDetailsModel model)
        {
            var supplierAgencyDetails = transferSupplierDetails.GetSupplierRouteBySupplierCodeAndAgencyCode("GAT001"
                    , "GAT001", "select/flights");
            // List<SupplierCredentials> supplierAgencyDetailslist = new List<SupplierCredentials> { SupplierCredentials };
            //model.CommonRequestFarePricer.SupplierAgencyDetails = supplierAgencyDetailslist;
            // string cardType = bookingServices.GetPaymentCardType(model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode);
            // model.CommonRequestFarePricer.Body.AirRevalidate.paymentCardType = cardType;

            string req = JsonConvert.SerializeObject(model);
            var result = await transferPartnerClient.DetailsBookData("supplierAgencyDetails.BaseUrl", "supplierAgencyDetails.RequestUrl", model);
            string strData = JsonConvert.SerializeObject(result.Data);
            string requestStr = JsonConvert.SerializeObject(model);
            string responseStr = JsonConvert.SerializeObject(result);
            //string agencyCode = model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode;

            TransferBookdetailsresponseEntity partnerResponseEntity = JsonConvert.DeserializeObject<TransferBookdetailsresponseEntity>(strData);
            if (partnerResponseEntity != null)
            {
                list.Add(partnerResponseEntity);
                return true;
            }
            return false;
        }


    }
}