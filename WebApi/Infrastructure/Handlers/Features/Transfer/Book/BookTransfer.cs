

namespace WebApi.Infrastructure.Handlers.Features.Transfer.Book
{
    using Client.Transfer;
    using global::Common;
    using Insurance.Book;
    using Logic.Interface.Transfer;
    using MediatR;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    public class BookTransfer : IAsyncRequestHandler<BookTransferModel, ResponseObject>
    {
        private readonly ITransferPartnerClient transferPartnerClient;
        private readonly ITranserSupplierDetails transferSupplierDetails;

        public async Task<ResponseObject> Handle(BookTransferModel message)
        {
            List<BooktransferResponseEntity> allsupplierData = new List<BooktransferResponseEntity>();

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

        private async Task<bool> GetDataFromSightSeeing(List<BooktransferResponseEntity> list, BookTransferModel model)
        {
            var supplierAgencyDetails = transferSupplierDetails.GetSupplierRouteBySupplierCodeAndAgencyCode("GAT001"
                    , "GAT001", "select/flights");
            // List<SupplierCredentials> supplierAgencyDetailslist = new List<SupplierCredentials> { SupplierCredentials };
            //model.CommonRequestFarePricer.SupplierAgencyDetails = supplierAgencyDetailslist;
            // string cardType = bookingServices.GetPaymentCardType(model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode);
            // model.CommonRequestFarePricer.Body.AirRevalidate.paymentCardType = cardType;

            string req = JsonConvert.SerializeObject(model);
            var result = await transferPartnerClient.GetBookData("supplierAgencyDetails.BaseUrl", "supplierAgencyDetails.RequestUrl", model);
            string strData = JsonConvert.SerializeObject(result.Data);
            string requestStr = JsonConvert.SerializeObject(model);
            string responseStr = JsonConvert.SerializeObject(result);
            //string agencyCode = model.CommonRequestFarePricer.Body.AirRevalidate.ARAgencyCode;

            BooktransferResponseEntity partnerResponseEntity = JsonConvert.DeserializeObject<BooktransferResponseEntity>(strData);
            if (partnerResponseEntity != null)
            {
                list.Add(partnerResponseEntity);
                return true;
            }
            return false;
        }


    }
}