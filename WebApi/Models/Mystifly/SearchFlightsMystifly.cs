namespace WebApi.Models.Mystifly
{
    using System.Collections.Generic;
    using BusinessEntitties;

    public class SearchFlightsMystifly : SearchFilghtsBase
    {
        public SearchFlightsMystifly()
        {
            CommonRequestSearch = new Commonrequestsearch();
            OriginDestinationInformation = new List<Origindestinationinformation>();
            SupplierAgencyDetails = new List<SupplierAgencyDetails>();
            PassengerTypeQuantity = new WebApi.Models.Passengertypequantity();
        }
  
        public List<SupplierAgencyDetails> SupplierAgencyDetails { get; set; }
               
    }
}