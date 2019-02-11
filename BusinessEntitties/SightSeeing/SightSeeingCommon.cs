namespace BusinessEntitties.SightSeeing
{
    public class SightSeeingCommon
    {
        [JsonProperty("TourOperations")]
        public TourOperations TourOperations { get; set; }
        public partial class SightseeingType
        {
            [JsonProperty("_Code")]
            public string Code { get; set; }

            [JsonProperty("_cdata")]
            public string Cdata { get; set; }
        }
    }
}
   