
namespace WebApi.Infrastructure.Handlers.Features.SightSeeing.Search
{




    using Common;
    using MediatR;
    using System.Collections.Generic;
    using Domain;
    using BusinessEntitties;
    using BusinessEntitties.SightSeeing;
    using global::Common;
    using Newtonsoft.Json;
    public class SightseeingSearch : IAsyncRequest<ResponseObject>
        {
        [JsonProperty("sightseeingSearchRequest")]
        public SightseeingSearchRequest SightseeingSearchRequest { get; set; }
        }




    public class SightseeingSearchRequest
    {
        [JsonProperty("SupplierCode")]
        public List<string> SupplierCode { get; set; }

        [JsonProperty("SupplierCredentials")]
        public SupplierCredentials SupplierCredentials { get; set; }

        [JsonProperty("RequestMode")]
        public RequestMode requestMode { get; set; }

        [JsonProperty("Destination")]
        public Destination Destination { get; set; }

        [JsonProperty("Travellers")]
        public Travellers Travellers { get; set; }

        [JsonProperty("TourDate")]
        public string TourDate { get; set; }

        [JsonProperty("Access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("SightseeingType")]
        public List<string> SightseeingType { get; set; }

        [JsonProperty("SightseeingCategory")]
        public List<string> SightseeingCategory { get; set; }

        [JsonProperty("IsChangeConditions")]
        public bool IsChangeConditions { get; set; }

        [JsonProperty("IsRecommended")]
        public bool IsRecommended { get; set; }

        [JsonProperty("IsImmediateConfirmation")]
        public bool IsImmediateConfirmation { get; set; }
    }

    public class Travellers
    {
        [JsonProperty("ADT")]
        public long Adt { get; set; }

        [JsonProperty("CHD")]
        public long Chd { get; set; }

        [JsonProperty("INF")]
        public long Inf { get; set; }

        [JsonProperty("ChildrenAge")]
        public List<long> ChildrenAge { get; set; }
    }

}
