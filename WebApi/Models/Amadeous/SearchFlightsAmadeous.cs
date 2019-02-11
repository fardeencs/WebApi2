namespace WebApi.Models.Amadeous
{
    using System.Collections.Generic;
    using BusinessEntitties;

    public class SearchFlightsAmadeous : SearchFilghtsBase
    {
        public SearchFlightsAmadeous()
        {
            CommonRequestSearch = new Commonrequestsearch();
            OriginDestinationInformation = new List<Origindestinationinformation>();
            SupplierAgencyDetails = new List<SupplierAgencyDetails>();
            PassengerTypeQuantity = new WebApi.Models.Passengertypequantity();
        }

        public List<SupplierAgencyDetails> SupplierAgencyDetails { get; set; }

    }

}