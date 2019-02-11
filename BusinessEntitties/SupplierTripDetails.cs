
namespace BusinessEntitties
{  
    public class Travelitinerary
    {
        public object[] bookingNotes { get; set; }
        public string bookingStatus { get; set; }
        public Itineraryinfo itineraryInfo { get; set; }
        public string ticketStatus { get; set; }
        public string uniqueID { get; set; }
    }

    public class Itineraryinfo
    {
        public object[] bookedReservedItems { get; set; }
        public double clientUTCOffset { get; set; }
        public Customerinfo1[] customerInfos { get; set; }
        public Itinerarypricing itineraryPricing { get; set; }
        public Reservationitem[] reservationItems { get; set; }
        public Tripdetailsptc_Farebreakdowns[] tripDetailsPTC_FareBreakdowns { get; set; }
    }

    public class Itinerarypricing
    {
        public Equifare equiFare { get; set; }
        public Tax tax { get; set; }
        public Totalfare totalFare { get; set; }
    }

    public class Equifare
    {
        public string amount { get; set; }
        public string currencyCode { get; set; }
        public int decimalPlaces { get; set; }
    }

    public class Tax
    {
        public string amount { get; set; }
        public string currencyCode { get; set; }
        public int decimalPlaces { get; set; }
    }

    public class Totalfare
    {
        public string amount { get; set; }
        public string currencyCode { get; set; }
        public int decimalPlaces { get; set; }
    }

    public class Customerinfo1
    {
        public Customer customer { get; set; }
        public Eticket[] eTickets { get; set; }
        public Ssr[] ssRs { get; set; }
    }

    public class Customer
    {
        public Age age { get; set; }
        public string dateOfBirth { get; set; }
        public string emailAddress { get; set; }
        public string gender { get; set; }
        public string knownTravelerNo { get; set; }
        public string nameNumber { get; set; }
        public string nationalID { get; set; }
        public string passengerNationality { get; set; }
        public string passengerType { get; set; }
        public string passportExpiresOn { get; set; }
        public string passportIssuanceCountry { get; set; }
        public string passportNationality { get; set; }
        public string passportNumber { get; set; }
        public Paxname paxName { get; set; }
        public string phoneNumber { get; set; }
        public string postCode { get; set; }
        public string redressNo { get; set; }
    }

    public class Age
    {
        public string months { get; set; }
        public string years { get; set; }
    }

    public class Paxname
    {
        public string passengerFirstName { get; set; }
        public string passengerLastName { get; set; }
        public string passengerTitle { get; set; }
    }

    public class Eticket
    {
        public int itemRPH { get; set; }
        public string eTicketNumber { get; set; }
    }

    public class Ssr
    {
        public int itemRPH { get; set; }
        public string mealPreference { get; set; }
        public string seatPreference { get; set; }
    }

    public class Reservationitem
    {
        public string airEquipmentType { get; set; }
        public string airlinePNR { get; set; }
        public string arrivalAirportLocationCode { get; set; }
        public string arrivalDateTime { get; set; }
        public string arrivalTerminal { get; set; }
        public string baggage { get; set; }
        public string cabinClassText { get; set; }
        public string departureAirportLocationCode { get; set; }
        public string departureDateTime { get; set; }
        public string departureTerminal { get; set; }
        public string flightNumber { get; set; }
        public int itemRPH { get; set; }
        public string journeyDuration { get; set; }
        public string marketingAirlineCode { get; set; }
        public int numberInParty { get; set; }
        public string operatingAirlineCode { get; set; }
        public object resBookDesigCode { get; set; }
        public object resBookDesigText { get; set; }
        public int stopQuantity { get; set; }
    }

    public class Tripdetailsptc_Farebreakdowns
    {
        public Passengertypequantity passengerTypeQuantity { get; set; }
        public Tripdetailspassengerfare tripDetailsPassengerFare { get; set; }
    }

    public class Passengertypequantity
    {
        public int code { get; set; }
        public int quantity { get; set; }
    }

    public class Tripdetailspassengerfare
    {
        public Equifare1 equiFare { get; set; }
        public Tax1 tax { get; set; }
        public Totalfare1 totalFare { get; set; }
    }

    public class Equifare1
    {
        public string amount { get; set; }
        public string currencyCode { get; set; }
        public int decimalPlaces { get; set; }
    }

    public class Tax1
    {
        public string amount { get; set; }
        public string currencyCode { get; set; }
        public int decimalPlaces { get; set; }
    }

    public class Totalfare1
    {
        public string amount { get; set; }
        public string currencyCode { get; set; }
        public int decimalPlaces { get; set; }
    }

}
