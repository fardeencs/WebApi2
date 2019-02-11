using BusinessEntitties.SightSeeing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Infrastructure.Handlers.Features.SightSeeing.Search
{
    public class SearchResponseEntity
    {
        [JsonProperty("sightseeingSearchResponse")]
        public SightseeingSearchResponse sightseeingSearchResponse  { get; set; }
    }


    public partial class SightseeingSearchResponse
    {
        [JsonProperty("ResponseDetails")]
        public ResponseDetails ResponseDetails { get; set; }

        [JsonProperty("_ResponseReference")]
        public string ResponseReference { get; set; }

        [JsonProperty("access-token")]
        public string Access_token { get; set; }
    }

    public partial class ResponseDetails
    {
        [JsonProperty("SearchSightseeingPriceResponse")]
        public SearchSightseeingPriceResponse SearchSightseeingPriceResponse { get; set; }

        [JsonProperty("_Language")]
        public string Language { get; set; }
    }

    public partial class SearchSightseeingPriceResponse
    {
        [JsonProperty("SightseeingDetails")]
        public SightseeingDetails SightseeingDetails { get; set; }
    }

    public partial class SightseeingDetails
    {
        [JsonProperty("Sightseeing")]
        public List<Sightseeing> Sightseeing { get; set; }
    }

    public partial class Sightseeing
    {
        [JsonProperty("City")]
        public City City { get; set; }

        [JsonProperty("Item")]
        public Item Item { get; set; }

        [JsonProperty("ImageLinks")]
        public List<ImageLink> ImageLinks { get; set; }

        [JsonProperty("SightseeingTypes")]
        public SightseeingTypes SightseeingTypes { get; set; }

        [JsonProperty("SightseeingCategories")]
        public SightseeingCategories SightseeingCategories { get; set; }

        [JsonProperty("TourOperations")]
        public TourOperations TourOperations { get; set; }

        [JsonProperty("ChargeConditions")]
        public ChargeConditions ChargeConditions { get; set; }

        [JsonProperty("_HasExtraInfo")]
        public bool HasExtraInfo { get; set; }
    }
    public partial class Item
    {
        [JsonProperty("_Code")]
        public string Code { get; set; }

        [JsonProperty("_cdata")]
        public string Cdata { get; set; }
    }

    public partial class ChargeConditions
    {
        [JsonProperty("ChargeCondition")]
        public List<ChargeCondition> ChargeCondition { get; set; }
    }

    public partial class ChargeCondition
    {
        [JsonProperty("Condition")]
        public List<Condition> Condition { get; set; }

        [JsonProperty("_Type")]
        public string Type { get; set; }

        [JsonProperty("_MaximumPossibleChargesShown")]
        public bool MaximumPossibleChargesShown { get; set; }
    }

    public partial class Condition
    {
        [JsonProperty("_Charge")]
        public bool Charge { get; set; }

        [JsonProperty("_ChargeAmount")]
        public double ChargeAmount { get; set; }

        [JsonProperty("_Currency")]
        public string Currency { get; set; }

        [JsonProperty("_FromDay")]
        public string FromDay { get; set; }

        [JsonProperty("_ToDay")]
        public string ToDay { get; set; }
    }
    public partial class SightseeingType
    {
        [JsonProperty("_Code")]
        public string Code { get; set; }

        [JsonProperty("_cdata")]
        public string Cdata { get; set; }
    }
    public partial class SightseeingCategory
    {
        [JsonProperty("_Code")]
        public string Code { get; set; }

        [JsonProperty("_cdata")]
        public string Cdata { get; set; }
    }
    public partial class Confirmation
    {
        [JsonProperty("_Code")]
        public string Code { get; set; }

        [JsonProperty("_cdata")]
        public string Cdata { get; set; }
    }

    public partial class ImageLink
    {
        [JsonProperty("Url")]
        public string Url { get; set; }
    }

    public partial class SightseeingCategories
    {
        [JsonProperty("SightseeingCategory")]
        public List<SightseeingCategory> SightseeingCategory { get; set; }
    }

    public partial class TourOperation
    {
        [JsonProperty("TourLanguages")]
        public TourLanguages TourLanguages { get; set; }

        [JsonProperty("ItemPrice")]
        public ItemPrice ItemPrice { get; set; }

        [JsonProperty("Confirmation")]
        public Confirmation Confirmation { get; set; }
    }

    public partial class ItemPrice
    {
        [JsonProperty("_Currency")]
        public string Currency { get; set; }

        [JsonProperty("_text")]
        public double Text { get; set; }
    }

    public partial class TourLanguages
    {
        [JsonProperty("TourLanguage")]
        public List<TourLanguage> TourLanguage { get; set; }
    }

}