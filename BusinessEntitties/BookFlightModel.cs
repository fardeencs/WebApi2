namespace BusinessEntitties
{
    using System.Collections.Generic;

    public partial class BookFlightModel //: IBookFlightModel
    {
        public BookFlightModel()
        {
            BookFlightEntity = new BookFlightEntity();
            Totalfaregroup = new TotalFareGroup();
            AirBagDetails = new List<AirBagDetails>();
            Fareruleseg = new List<Fareruleseg>();
            CostBreakuppax = new List<CostBreakuppax>();
        }
        public BookFlightEntity BookFlightEntity { get; set; }
        public TotalFareGroup Totalfaregroup { get; set; }
        public List<AirBagDetails> AirBagDetails { get; set; }
        public List<Fareruleseg> Fareruleseg { get; set; }
        public List<CostBreakuppax> CostBreakuppax { get; set; }
        public List<costAirMarkUp> costAirMarkUp { get; set; }
    }

    public partial class BookFlightEntity
    {
        public BookFlightEntity()
        {
            BookFlight = new BookFlight();
        }
        public BookFlight BookFlight { get; set; }

    }

    public partial class BookFlight
    {
        public BookFlight()
        {
            SupplierAgencyDetails = new List<SupplierAgencyDetails>();
            Fdetails = new Fdetails();
            CustomerInfo = new Customerinfo();
            PaymentInfo = new Paymentinfo();
            TravelerInfo = new List<Travelerinfo>();
            FLLegGroup = new List<Flleggroup>();
        }
        public string AgencyCode { get; set; }
        public string BookingId { get; set; }
        public string SupplierCode { get; set; }
        public List<SupplierAgencyDetails> SupplierAgencyDetails { get; set; }
        public string Faresourcecode { get; set; }
        public string SessionId { get; set; }
        public string Target { get; set; }
        public Fdetails Fdetails { get; set; }
        public Customerinfo CustomerInfo { get; set; }
        public Paymentinfo PaymentInfo { get; set; }
        public List< Travelerinfo> TravelerInfo { get; set; }
        public List<Flleggroup> FLLegGroup { get; set; }
        public string AreaCityCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string[] AddressLine { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public int ADT { get; set; }
        public int CHD { get; set; }
        public int INF { get; set; }
    }

   
}


