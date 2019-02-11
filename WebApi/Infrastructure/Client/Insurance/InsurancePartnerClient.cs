

namespace WebApi.Infrastructure.Client.Insurance
{
    using Domain;
    using Handlers.Features.Insurance.Book;
    using Handlers.Features.Insurance.Cancel;
    using Handlers.Features.Insurance.Confirm;
    using Handlers.Features.Insurance.Details;
    using Handlers.Features.Insurance.Search;
    using Handlers.Features.Insurance.Select;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;
    public class InsurancePartnerClient
    {
        public async Task<ResponsePackage> GetGTASearchData(string baseUri, string reqUri, SearchInsuranceModel message)
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

        public async Task<ResponsePackage> GetGTASelectData(string baseUri, string reqUri, SelectInsuranceModel message)
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
        public async Task<ResponsePackage> GetBookData(string baseUri, string reqUri, BookInsuranceModel message)
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
        public async Task<ResponsePackage> GetConfirmBookData(string baseUri, string reqUri, ConfirmInsuranceRequestModel message)
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
        public async Task<ResponsePackage> DetailsBookData(string baseUri, string reqUri, InsuranceDetailsRequestModel message)
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
        public async Task<ResponsePackage> CancelBookData(string baseUri, string reqUri, CancelInsuranceBookingModel message)
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