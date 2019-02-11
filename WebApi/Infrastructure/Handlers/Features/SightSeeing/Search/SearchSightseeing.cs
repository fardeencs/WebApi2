
#pragma warning disable 1998
#pragma warning disable 618
namespace WebApi.Infrastructure.Handlers.Features.SightSeeing.Search
{
    using Common;
    using MediatR;
    using Common;
    using BusinessEntitties.SightSeeing;
    using Logic.Interface.sightSeeing;
    using global::Common;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using BusinessEntitties.Enumrations;
    using System.Net;
    using System.Net.Http;
    using Newtonsoft.Json;
    using Logic.SightSeeing;
    using Client.Sightseeing;
    using Web.Core.Client;

    public class SearchSightseeing: IAsyncRequestHandler<SightseeingSearch, ResponseObject>
    {
        private const string ReqUrlGTA = "requrlGTA";
        private readonly ISightSeeingPartnerClient sightSeeingPartnerClient;
        private readonly ISightseeingSupplierDetails sightseeingSupplierDetails;

        public SearchSightseeing()
        {
            var apiClient = new ApiClient();
            sightSeeingPartnerClient = new SightSeeingPartnerClient(apiClient);
        }


        public async Task<ResponseObject> Handle(SightseeingSearch message)
        {
           

            List<SearchResponseEntity> allsupplierData = new List<SearchResponseEntity>();

           bool isCompletedGta = await GetDataFromGTA(allsupplierData, message);

            //Task<bool> mystiflyResponse = GetDataFromGTA(allsupplierData, message);   

            // await mystiflyResponse;





            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = allsupplierData,
                Message = "Data retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }


        private async Task<bool> GetDataFromGTA(List<SearchResponseEntity> list, SightseeingSearch model)
        {
            try
            {
                string baseUri = "http://192.168.1.18:9500/";
                string strData = string.Empty;
                string reqUri = "gta-partner/search-sightseeing";
                bool isFetchedFromDb = false;

                string req = JsonConvert.SerializeObject(model);
                if (string.IsNullOrEmpty(strData))
                {

                    var result = await sightSeeingPartnerClient.GetGTASearchData(baseUri, reqUri, model);
                    strData = JsonConvert.SerializeObject(result.Data);
                    isFetchedFromDb = true;
                }
                SearchResponseEntity partnerResponseEntity = JsonConvert.DeserializeObject<SearchResponseEntity>(strData);
                if (partnerResponseEntity != null)
                {
                    list.Add(partnerResponseEntity);
                    return true;

                }

                return false;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
               
        }


      



    }
}