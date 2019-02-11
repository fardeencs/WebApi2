

namespace WebApi.Infrastructure.Client.Sightseeing
{
    using Domain;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;
    using Web.Core.Client;
    using WebApi.Infrastructure.Handlers.Features.SightSeeing.Book;
    using WebApi.Infrastructure.Handlers.Features.SightSeeing.Cancel;
    using WebApi.Infrastructure.Handlers.Features.SightSeeing.Confirm;
    using WebApi.Infrastructure.Handlers.Features.SightSeeing.Details;
    using WebApi.Infrastructure.Handlers.Features.SightSeeing.Search;
    using WebApi.Infrastructure.Handlers.Features.SightSeeing.Select;


    public class SightSeeingPartnerClient : ClientBase, ISightSeeingPartnerClient
    {
        public SightSeeingPartnerClient(IApiClient apiClient) : base(apiClient)
        {
        }

        public async Task<ResponsePackage> GetGTASearchData(string baseUri, string reqUri, SightseeingSearch message)
        {
            ResponsePackage responsePackage = new ResponsePackage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestObject = JsonConvert.SerializeObject(message);
                using (HttpResponseMessage Res = await client.PostAsJsonAsync(reqUri, message))
                {
                    if (Res.IsSuccessStatusCode)
                    {
                        var partnerResponse = Res.Content.ReadAsStringAsync().Result;
                        responsePackage = JsonConvert.DeserializeObject<ResponsePackage>(partnerResponse);
                    }
                    return responsePackage;
                }
            }
        }
        public async Task<ResponsePackage> GetGTASelectData(string baseUri, string reqUri, SelectSigntseeingModel message)
        {
            ResponsePackage responsePackage = new ResponsePackage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestObject = JsonConvert.SerializeObject(message);
                using (HttpResponseMessage Res = await client.PostAsJsonAsync(reqUri, message))
                {
                    if (Res.IsSuccessStatusCode)
                    {
                        var partnerResponse = Res.Content.ReadAsStringAsync().Result;
                        responsePackage = JsonConvert.DeserializeObject<ResponsePackage>(partnerResponse);
                    }
                    return responsePackage;
                }
            }
        }
        public async Task<ResponsePackage> GetBookData(string baseUri, string reqUri, BookSightSeeingModel message)
        {
            ResponsePackage responsePackage = new ResponsePackage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestObject = JsonConvert.SerializeObject(message);
                using (HttpResponseMessage Res = await client.PostAsJsonAsync(reqUri, message))
                {
                    if (Res.IsSuccessStatusCode)
                    {
                        var partnerResponse = Res.Content.ReadAsStringAsync().Result;
                        responsePackage = JsonConvert.DeserializeObject<ResponsePackage>(partnerResponse);
                    }
                    return responsePackage;
                }
            }
        }
        public async Task<ResponsePackage> GetConfirmBookData(string baseUri, string reqUri, ConfirmSightSeeingBookingModel message)
        {
            ResponsePackage responsePackage = new ResponsePackage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestObject = JsonConvert.SerializeObject(message);
                using (HttpResponseMessage Res = await client.PostAsJsonAsync(reqUri, message))
                {
                    if (Res.IsSuccessStatusCode)
                    {
                        var partnerResponse = Res.Content.ReadAsStringAsync().Result;
                        responsePackage = JsonConvert.DeserializeObject<ResponsePackage>(partnerResponse);
                    }
                    return responsePackage;
                }
            }
        }
        public async Task<ResponsePackage> DetailsBookData(string baseUri, string reqUri, SightSeeingDetailsModel message)
        {
            ResponsePackage responsePackage = new ResponsePackage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestObject = JsonConvert.SerializeObject(message);
                using (HttpResponseMessage Res = await client.PostAsJsonAsync(reqUri, message))
                {
                    if (Res.IsSuccessStatusCode)
                    {
                        var partnerResponse = Res.Content.ReadAsStringAsync().Result;
                        responsePackage = JsonConvert.DeserializeObject<ResponsePackage>(partnerResponse);
                    }
                    return responsePackage;
                }
            }
        }
        public async Task<ResponsePackage> CancelBookData(string baseUri, string reqUri, CancelSightSeeingModel message)
        {
            ResponsePackage responsePackage = new ResponsePackage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestObject = JsonConvert.SerializeObject(message);
                using (HttpResponseMessage Res = await client.PostAsJsonAsync(reqUri, message))
                {
                    if (Res.IsSuccessStatusCode)
                    {
                        var partnerResponse = Res.Content.ReadAsStringAsync().Result;
                        responsePackage = JsonConvert.DeserializeObject<ResponsePackage>(partnerResponse);
                    }
                    return responsePackage;
                }
            }
        }
    }
}