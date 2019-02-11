using System.Configuration;
using System.Data.Entity;
using DAL.Mappings;
using Domain;
using Domain.Entity;

namespace DAL
{
    public class MediationEntities : DbContext
    {
        public MediationEntities()
        {
            Configuration.ProxyCreationEnabled = true;

        }

        public MediationEntities(string connectionString) : base(connectionString)
        {
        }

        // public DbSet<User> Users { get; set; }
        public virtual DbSet<tblAgency> tblAgencies { get; set; }
        public virtual DbSet<tblAgencyType> tblAgencyTypes { get; set; }
        public virtual DbSet<tblCity> tblCities { get; set; }
        public virtual DbSet<tblCountry> tblCountries { get; set; }
        public virtual DbSet<tblSupplierMaster> tblSupplierMasters { get; set; }
        public virtual DbSet<tblSupplierApiInfo> tblSupplierApiInfoes { get; set; }
        public virtual DbSet<tblTitle> tblTitles { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<tblBooking> tblBookings { get; set; }
        public virtual DbSet<tblBookingHistory> tblBookingHistorys { get; set; }
        public virtual DbSet<tblBookingStatu> tblBookingStatus { get; set; }
        public virtual DbSet<tblPaymentStatu> tblPaymentStatus { get; set; }
        public virtual DbSet<tblPaymentType> tblPaymentTypes { get; set; }
        public virtual DbSet<tblAirPassengers> tblAirPassengers { get; set; }
        public virtual DbSet<tblAgencySeacrhDetail> tblAgencySeacrhDetails { get; set; }
        public virtual DbSet<tblAirBookingCost> tblAirBookingCost { get; set; }
        public virtual DbSet<tblAirBookingCost_1> tblAirBookingCost_1 { get; set; }
        public virtual DbSet<tblAirBookingCostBreakup> tblAirBookingCostBreakup { get; set; }
        public virtual DbSet<tblPayment> tblPayment { get; set; }
        public virtual DbSet<tblAirOriginDestinationOption> tblAirOriginDestinationOption { get; set; }
        public virtual DbSet<tblAirSegment> tblAirSegment { get; set; }
        public virtual DbSet<tblAirbaggageDetails> tblAirbaggageDetails { get; set; }
        public virtual DbSet<tblAirFarerules> tblAirFarerules { get; set; }
        public virtual DbSet<tblDocType> _tblDocType { get; set; }
        public virtual DbSet<tblAirPassengerDoc> _tblAirPassengerDoc { get; set; }
        public virtual DbSet<tblAirBookingData> _tblAirBookingData { get; set; }
        public virtual DbSet<tblAirSegmentBookingAvail> _tblAirSegmentBookingAvail { get; set; }
        public virtual DbSet<tblAirbookingcostMarkUp> _tblAirbookingcostMarkUp { get; set; }
        public virtual DbSet<tblPassengerType> _tblPassengerType { get; set; }
        public virtual DbSet<tblRateofExchange> _tblRateofExchange { get; set; }
        public virtual DbSet<tblCardDetail> _tblCardDetail { get; set; }
        public virtual DbSet<tblOnlinePaymentOption> _tblOnlinePaymentOption { get; set; }
        public virtual DbSet<tblSupplier> _tblSupplier { get; set; }
        public virtual DbSet<tblAirBookingError> _tblAirBookingError { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new UserMapping());
            // Database.SetInitializer<MediationEntities>(new DBInitializer());
        }

        protected override void Dispose(bool disposing)
        {
            Configuration.LazyLoadingEnabled = false;
            base.Dispose(disposing);
        }
    }
}