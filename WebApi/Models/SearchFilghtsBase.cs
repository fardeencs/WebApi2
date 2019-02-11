

namespace WebApi.Models
{
    using System.Collections.Generic;
    using BusinessEntitties;

    public class SearchFilghtsBase
    {
        public Commonrequestsearch CommonRequestSearch { get; set; }
        public List<Origindestinationinformation> OriginDestinationInformation { get; set; }
        public WebApi.Models.Passengertypequantity PassengerTypeQuantity { get; set; }

        public string Currency { get; set; }
        public string PreferredAirline { get; set; }
        public string NonStop { get; set; }
        public string cabin { get; set; }
        public string IsRefundable { get; set; }
        public string Maxstopquantity { get; set; }
        public string Triptype { get; set; }
        public string PreferenceLevel { get; set; }
        public string Target { get; set; }
        public string RequestOption { get; set; }
        public string PricingSource { get; set; }
    }
}