
namespace WebApi.Infrastructure.Handlers.Features.Transfer.Search
{
    using BusinessEntitties.Enumrations;
    using Common;
    using global::Common;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Net;
    using Newtonsoft.Json;
    using BusinessEntitties.Transfer;
    using Client.Transfer;
    using Logic.Interface.Transfer;
    public class SearchTransfer : IAsyncRequestHandler<SearchTransferModel, ResponseObject>
    {

        private const string ReqUrlGTA = "requrlGTA";
        private readonly ITransferPartnerClient transferPartnerClient;
        private readonly ITranserSupplierDetails transferSupplierDetails;

        public async System.Threading.Tasks.Task<ResponseObject> Handle(SearchTransferModel message)
        {
            List<SearchTransferResponseEntity> allsupplierData = new List<SearchTransferResponseEntity>();
            if (SupplierCode.GTA001.ToString().ToUpper() == "GTA001")//message.SightseeingSearchRequest.SupplierCode[0].Trim().ToUpper())
            {
                bool mystiflyResponse = await GetDataFromGTA(allsupplierData, message);
            }
            else
            {

                //CreateResponseTime("Mystifly");
                Task<bool> mystiflyResponse = GetDataFromGTA(allsupplierData, message);
                await mystiflyResponse;


            }



            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = allsupplierData,
                Message = "Data retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }

        private async Task<bool> GetDataFromGTA(List<SearchTransferResponseEntity> list, SearchTransferModel model)
        {

            TransferSupplierCredentials supplierCredentials = await transferSupplierDetails.GeBasicDetailsOfTransferSupplier("GTA001", SupplierCode.MIS001.ToString(), "T");
            if (supplierCredentials != null)
            {
                //supplierCredentials.AgencyCode = model.SightseeingSearchRequest.AgencyCode;
                List<TransferSupplierCredentials> supplierAgencyDetails = new List<TransferSupplierCredentials>();
                supplierAgencyDetails.Add(supplierCredentials);

                //string baseUri = model.SupplierAgencyDetails.FirstOrDefault().BaseUrl;
                SearchTransferModel requestModel = new SearchTransferModel();

                string baseUri = supplierCredentials.BaseUrl;

                string strData = string.Empty;

                string reqUri = ConficBase.GetConfigAppValue(ReqUrlGTA);
                bool isFetchedFromDb = false;

                string req = JsonConvert.SerializeObject(model);
                if (string.IsNullOrEmpty(strData))
                {
                    // var result = await partnerClient.GetMystiflyData(baseUri, reqUri, model);
                    var result = await transferPartnerClient.GetGTASearchData(baseUri, reqUri, requestModel);
                    strData = JsonConvert.SerializeObject(result.Data);
                    isFetchedFromDb = true;
                }
                SearchTransferResponseEntity partnerResponseEntity = JsonConvert.DeserializeObject<SearchTransferResponseEntity>(strData);
                if (partnerResponseEntity != null)
                {
                    list.Add(partnerResponseEntity);
                    return true;

                }
            }
            return false;
        }

    }
}