using BusinessEntitties;
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
using WebApi.Infrastructure.ApiModels;
using WebApi.Infrastructure.Handlers.Features.Mediation;
using WebApi.Models;
using WebApi.Models.Amadeous;
using WebApi.Models.Mystifly;
using WebApi.Models.Pyton;

namespace WebApi.Infrastructure.Client
{
    public class PartnerClient : ClientBase, IPartnerClient
    {
        private const string baseUri = "http://localhost:18313/";
        private const string IdKey = "id";

        public PartnerClient(IApiClient apiClient) : base(apiClient)
        {
        }

        public async Task<PartnerResponse> GetPartnerAllData()
        {
            string reqUri = baseUri + "partnerone/all/async";
            //  var idPair = IdKey.AsPair(productId.ToString());
            return await GetAllJsonDecodedContent<PartnerResponse, PartnerApiModel>(reqUri);
        }

        //public async Task<ResponsePackage> GetSupplierDataAsPost(string baseUri, string reqUri, Models.Rootobject message)
        //{
        //    //return await GetSupplierDataAsPost(baseUri, reqUri, message);
        //    //return await GetJsonDecodedContentFromPostReq<>(baseUri, reqUri, message);
        //}


        public async Task<ResponsePackage> GetPartnerData(string baseUri, string reqUri)
        {
            ResponsePackage responsePackage = new ResponsePackage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync(reqUri);
                if (Res.IsSuccessStatusCode)
                {
                    var partnerResponse = Res.Content.ReadAsStringAsync().Result;
                    responsePackage = JsonConvert.DeserializeObject<ResponsePackage>(partnerResponse);
                }
                return responsePackage;
            }
        }

        public async Task<ResponsePackage> GetPartnerData(string baseUri, string reqUri, Models.Rootobject message)
        {
            ResponsePackage responsePackage = new ResponsePackage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string requestObject = JsonConvert.SerializeObject(message);
                HttpResponseMessage Res = await client.PostAsJsonAsync(reqUri, message);

                if (Res.IsSuccessStatusCode)
                {
                    var partnerResponse = Res.Content.ReadAsStringAsync().Result;
                    responsePackage = JsonConvert.DeserializeObject<ResponsePackage>(partnerResponse);
                }
                return responsePackage;
            }
        }

        public async Task<ResponsePackage> GetPytonData(string baseUri, string reqUri, SearchFlightsPyton message)
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

        public async Task<ResponsePackage> GetMystiflyData(string baseUri, string reqUri, SearchFlightsMystifly message)
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
        public async Task<ResponsePackage> GetAmadeusData(string baseUri, string reqUri, SearchFlightsAmadeous message)
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

        public async Task<ResponsePackage> Getselectflight(string baseUri, string reqUri, SelectFlightModel message)
        {
            ResponsePackage responsePackage = new ResponsePackage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PostAsJsonAsync(reqUri, message);

                if (Res.IsSuccessStatusCode)
                {
                    var partnerResponse = Res.Content.ReadAsStringAsync().Result;
                    responsePackage = JsonConvert.DeserializeObject<ResponsePackage>(partnerResponse);
                }
                return responsePackage;
            }
        }

        public async Task<ResponsePackage> GetBookflight(string baseUri, string reqUri, BookFlightEntity message)
        {
            ResponsePackage responsePackage = new ResponsePackage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PostAsJsonAsync(reqUri, message);

                if (Res.IsSuccessStatusCode)
                {
                    var partnerResponse = Res.Content.ReadAsStringAsync().Result;
                    responsePackage = JsonConvert.DeserializeObject<ResponsePackage>(partnerResponse);
                }
                return responsePackage;
            }
        }

        public async Task<ResponsePackage> GetIssueTicketflight(string baseUri, string reqUri, Models.IssueTickettModel message)
        {
            ResponsePackage responsePackage = new ResponsePackage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PostAsJsonAsync(reqUri, message);

                if (Res.IsSuccessStatusCode)
                {
                    var partnerResponse = Res.Content.ReadAsStringAsync().Result;
                    responsePackage = JsonConvert.DeserializeObject<ResponsePackage>(partnerResponse);
                }
                return responsePackage;
            }
        }
        public async Task<ResponsePackage> GetCancelPNRStatus(string baseUri, string reqUri, Models.CancelBookingModel message)
        {
            ResponsePackage responsePackage = new ResponsePackage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PostAsJsonAsync(reqUri, message);

                if (Res.IsSuccessStatusCode)
                {
                    var partnerResponse = Res.Content.ReadAsStringAsync().Result;
                    responsePackage = JsonConvert.DeserializeObject<ResponsePackage>(partnerResponse);
                }
                return responsePackage;
            }
        }
        public async Task<ResponsePackage> GetSupplierTripDetails(string baseUri, string reqUri, Models.SupplierTripDetailsModel message)
        {
            ResponsePackage responsePackage = new ResponsePackage();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PostAsJsonAsync(reqUri, message);

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