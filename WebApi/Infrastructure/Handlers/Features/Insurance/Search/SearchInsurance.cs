

namespace WebApi.Infrastructure.Handlers.Features.Insurance.Search
{
    using BusinessEntitties.Enumrations;
    using BusinessEntitties.Insurance;
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
    public class SearchInsurance : IAsyncRequestHandler<SearchInsuranceModel, ResponseObject>
    {
        private const string ReqUrlGTA = "requrlGTA";
        private readonly IInsurancePartnerClient insurancePartnerClient;
        private readonly IInsuranceSupplierDetails insuranceSupplierDetails;


        public async System.Threading.Tasks.Task<ResponseObject> Handle(SearchInsuranceModel message)
        {
            List<SearchInsuranceResponseEntity> allsupplierData = new List<SearchInsuranceResponseEntity>();
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


        private async Task<bool> GetDataFromGTA(List<SearchInsuranceResponseEntity> list, SearchInsuranceModel model)
        {

            InsuranceSupplierCredentials supplierCredentials = await insuranceSupplierDetails.GeBasicDetailsOfIsuranceSupplier("GTA001", SupplierCode.MIS001.ToString(), "T");
            if (supplierCredentials != null)
            {
                //supplierCredentials.AgencyCode = model.SightseeingSearchRequest.AgencyCode;
                List<InsuranceSupplierCredentials> supplierAgencyDetails = new List<InsuranceSupplierCredentials>();
                supplierAgencyDetails.Add(supplierCredentials);

                //string baseUri = model.SupplierAgencyDetails.FirstOrDefault().BaseUrl;
                SearchInsuranceModel requestModel = new SearchInsuranceModel();

                string baseUri = supplierCredentials.BaseUrl;

                string strData = string.Empty;

                string reqUri = ConficBase.GetConfigAppValue(ReqUrlGTA);
                bool isFetchedFromDb = false;

                string req = JsonConvert.SerializeObject(model);
                if (string.IsNullOrEmpty(strData))
                {
                    // var result = await partnerClient.GetMystiflyData(baseUri, reqUri, model);
                    var result = await insurancePartnerClient.GetGTASearchData(baseUri, reqUri, requestModel);
                    strData = JsonConvert.SerializeObject(result.Data);
                    isFetchedFromDb = true;
                }
                SearchInsuranceResponseEntity partnerResponseEntity = JsonConvert.DeserializeObject<SearchInsuranceResponseEntity>(strData);
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