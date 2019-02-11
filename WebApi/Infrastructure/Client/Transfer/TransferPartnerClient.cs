

namespace WebApi.Infrastructure.Client.Transfer
{
    using Domain;
    using Handlers.Features.Transfer.Book;
    using Handlers.Features.Transfer.Cancel;
    using Handlers.Features.Transfer.Confirm;
    using Handlers.Features.Transfer.Details;
    using Handlers.Features.Transfer.Select;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;
    using WebApi.Infrastructure.Handlers.Features.Transfer.Search;
    public class TransferPartnerClient
    {
        public async Task<ResponsePackage> GetGTASearchData(string baseUri, string reqUri, SearchTransferModel message)
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
        public async Task<ResponsePackage> GetGTASelectData(string baseUri, string reqUri, SelectTransferModel message)
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
        public async Task<ResponsePackage> GetBookData(string baseUri, string reqUri, BookTransferModel message)
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
        public async Task<ResponsePackage> GetConfirmBookData(string baseUri, string reqUri, ConfirmTransferModel message)
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
        public async Task<ResponsePackage> DetailsBookData(string baseUri, string reqUri, TransferBookDetailsModel message)
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
        public async Task<ResponsePackage> CancelBookData(string baseUri, string reqUri, CancelTransferModel message)
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