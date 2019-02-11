
using System.Collections.Generic;
using static BusinessEntitties.SightSeeing.SightSeeingCommon;

namespace BusinessEntitties.SightSeeing
{
    public class SightseeingEntity
    {
    }
    public class RequestMode
    {
        [JsonProperty("Currency")]
        public string Currency { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("Mode")]
        public string mode { get; set; }
    }

    public class Destination
    {
        [JsonProperty("DestinationType")]
        public string DestinationType { get; set; }

        [JsonProperty("DestinationCode")]
        public string DestinationCode { get; set; }
    }

    public class SupplierCredentials
    {
        [JsonProperty("ClientID")]
        public string ClientId { get; set; }

        [JsonProperty("EmailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("Language")]
        public string Language { get; set; }

        [JsonProperty("InterfaceUrl")]
        public string InterfaceUrl { get; set; }
    }
    public partial class City
    {
        [JsonProperty("_Code")]
        public string Code { get; set; }

        [JsonProperty("_cdata")]
        public string Cdata { get; set; }
    }
    public partial class SightseeingTypes
    {
        [JsonProperty("SightseeingType")]
        public List<SightseeingType> SightseeingType { get; set; }
    }
    public partial class TourOperations
    {
        [JsonProperty("TourOperation")]
        public List<SightSeeingCommon> TourOperation { get; set; }
    }
    public partial class TourLanguage
    {
        [JsonProperty("_Code")]
        public string Code { get; set; }

        [JsonProperty("_LanguageListCode")]
        public string LanguageListCode { get; set; }

        [JsonProperty("_cdata")]
        public string Cdata { get; set; }
    }


}
