namespace WebApi.Models.Pyton
{
    using System.Collections.Generic;
    using BusinessEntitties;

    public class SearchFlightsPyton : SearchFilghtsBase
    {
        public SearchFlightsPyton()
        {
            CommonRequestSearch = new Commonrequestsearch();
            OriginDestinationInformation = new List<Origindestinationinformation>();
            SupplierAgencyDetails = new List<SupplierAgencyDetails>();
            PassengerTypeQuantity = new WebApi.Models.Passengertypequantity();
        }

        public List<SupplierAgencyDetails> SupplierAgencyDetails { get; set; }
    }
}