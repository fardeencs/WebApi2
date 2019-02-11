namespace Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DAL;
    using Domain;
    using Logic.Interface;
    using System.Transactions;
    using Domain.Entity;
    using System;
    using Domain.Enumrations;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Xml.Linq;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Configuration;
    using System.Globalization;
    using Common;
    using BusinessEntitties;
    using System.IO;

    public class SupplierAgencyServices : ISupplierAgencyServices
    {
        public async Task<List<SupplierAgencyDetails>> GetSupplierAgencyBasicDetails(string agencyCode, string status)
        {
            using (var _ctx = new MediationEntities())
            {
                var supplierBasicDetails = await (from sm in _ctx.tblSupplierMasters
                                                  join ag in _ctx.tblAgencies on sm.AgencyID equals ag.AgencyID
                                                  //join sapi in _ctx.tblSupplierApiInfoes on sm.SupplierId equals                                  sapi.SupplierId
                                                  where sm.Status == status && ag.AgencyCode == agencyCode
                                                  select new SupplierAgencyDetails
                                                  {
                                                      AccountNumber = sm.AccountNumber,
                                                      AgencyID = ag.AgencyID,
                                                      AgencyCode = ag.AgencyCode,
                                                      Password = sm.Password,
                                                      Status = sm.Status,
                                                      SupplierCode = sm.SupplierCode,
                                                      SupplierName = sm.SupplierName,
                                                      UserName = sm.UserName,
                                                  }).ToListAsync();

                return supplierBasicDetails;
            }
        }
        public async Task<SupplierAgencyDetails> GetSupplierAgencyBasicDetails(string agencyCode, string status, string supplierCode)
        {
            using (var _ctx = new MediationEntities())
            {
                var supplierBasicDetails = (from sm in _ctx.tblSupplierMasters
                                            join ag in _ctx.tblAgencies on sm.AgencyID equals ag.AgencyID
                                            join roe in _ctx._tblRateofExchange on sm.AgencyID equals roe.AgencyID
                                            where sm.Status == status && ag.AgencyCode == agencyCode && roe.Status == 1
                                            && sm.SupplierCode == supplierCode
                                            select new SupplierAgencyDetails
                                            {
                                                AccountNumber = sm.AccountNumber,
                                                AgencyID = ag.AgencyID,
                                                Password = sm.Password,
                                                Status = sm.Status,
                                                SupplierCode = sm.SupplierCode,
                                                SupplierName = sm.SupplierName,
                                                UserName = sm.UserName,
                                                BaseUrl = sm.BaseUrl,
                                                ToCurrency = roe.CurrencyCode,
                                                ToROEValue = roe.ReteofExchange,
                                                AgencyCode = ag.AgencyCode
                                            }).FirstOrDefault();

                return supplierBasicDetails;
            }
        }

        public async Task<List<SupplierAgencyDetails>> GetSupplierAgencyBasicDetailswithsuppliercode(string agencyCode, string status, string supplierCode)
        {
            using (var _ctx = new MediationEntities())
            {
                var supplierBasicDetails = await (from sm in _ctx.tblSupplierMasters
                                                  join ag in _ctx.tblAgencies on sm.AgencyID equals ag.AgencyID
                                                  //join sapi in _ctx.tblSupplierApiInfoes on sm.SupplierId equals                                  sapi.SupplierId
                                                  where sm.Status == status && ag.AgencyCode == agencyCode && sm.SupplierCode == supplierCode
                                                  select new SupplierAgencyDetails
                                                  {
                                                      AccountNumber = sm.AccountNumber,
                                                      AgencyID = ag.AgencyID,
                                                      Password = sm.Password,
                                                      Status = sm.Status,
                                                      SupplierCode = sm.SupplierCode,
                                                      SupplierName = sm.SupplierName,
                                                      UserName = sm.UserName,
                                                      AgencyCode = ag.AgencyCode
                                                  }).ToListAsync();

                return supplierBasicDetails;

            }
        }

        public SupplierAgencyDetails GetSupplierRouteBySupplierCodeAndAgencyCode(string agencyCode, string supplierCode, string routeFlag)
        {
            using (var _ctx = new MediationEntities())
            {
                var supplierBasicDetails = (from sm in _ctx.tblSupplierMasters
                                            join ag in _ctx.tblAgencies on sm.AgencyID equals ag.AgencyID
                                            join sapi in _ctx.tblSupplierApiInfoes on sm.SupplierId equals sapi.SupplierId
                                            join roe in _ctx._tblRateofExchange on sm.AgencyID equals roe.AgencyID
                                            where sm.Status == "T" && ag.AgencyCode == agencyCode && sapi.SupplierRoute == routeFlag && sm.SupplierCode == supplierCode && roe.Status == 1
                                            select new SupplierAgencyDetails
                                            {
                                                AccountNumber = sm.AccountNumber,
                                                Password = sm.Password,
                                                UserName = sm.UserName,
                                                BaseUrl = sm.BaseUrl,
                                                RequestUrl = sapi.RequestUrl,
                                                AgencyID = ag.AgencyID,
                                                AgencyCode = ag.AgencyCode,
                                                SupplierId = sm.SupplierId,
                                                ToCurrency = roe.CurrencyCode,
                                                ToROEValue = roe.ReteofExchange,
                                                SupplierCode = sm.SupplierCode
                                            }).FirstOrDefault();

                return supplierBasicDetails;

            }
        }




        public async Task<bool> AddSupplierResponse(List<AgencySupplierResponseEntity> message)
        {
            bool isUpdated = false;
            try
            {
                using (var _ctx = new MediationEntities())
                {
                    message.ForEach(_ =>
                    {
                        _ctx.tblAgencySeacrhDetails.Add(new tblAgencySeacrhDetail()
                        {
                            AgencyCode = _.AgencyCode,
                            SearchDateTime = _.SearchDateTime,
                            Status = _.Status,
                            SupplierCode = _.SupplierCode,
                            Search = _.Search,
                            Key = _.Key

                        });
                    });

                    await _ctx.SaveChangesAsync();

                    isUpdated = true;
                    return isUpdated;
                }
            }
            catch (DbEntityValidationException e)
            {
                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                return isUpdated;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<string> GetAgencySupplierResponse(AgencySupplierResponseEntity message)
        {
            try
            {
                message.SearchDateTime = DateTime.Now;
                int minToAdd = 20;
                using (var _ctx = new MediationEntities())
                {
                    const string sql = @"EXEC [dbo].[spGetAgencySupplierResponse]
                                                 @AgencyCode
				                                ,@SearchDateTime
				                                ,@Key
				                                ,@Status
				                                ,@SupplierCode
                                                ,@MinutesToAdd
                                                ";


                    var result = _ctx.Database
                        .SqlQuery<SpBaseEntity>(
                                  sql
                                , new SqlParameter("AgencyCode", message.AgencyCode)
                                , new SqlParameter("SearchDateTime", message.SearchDateTime)
                                , new SqlParameter("Key", message.Key)
                                , new SqlParameter("Status", message.Status)
                                , new SqlParameter("SupplierCode", message.SupplierCode)
                                , new SqlParameter("MinutesToAdd", minToAdd)
                        ).FirstOrDefault();

                    if (result == null)
                        result.Data = null;

                    if (result.StatusCode != "OK-200")
                        result.Data = null;

                    return result.Data;
                }
            }
            catch (Exception ex)
            {

                return null;
            }

        }
        public async Task<GetTripDetailsModelRS> GetTravellerDetailsfromDB(string bookingRefID)
        {
            GetTripDetailsModelRS _GetTripDetailsModelRS = new GetTripDetailsModelRS();
            using (var _ctx = new MediationEntities())
            {
                long _bookref = Convert.ToInt64(bookingRefID);
                var BookingRefIDBasicData = (from sm in _ctx.tblBookings
                                             join bs in _ctx.tblBookingStatus on sm.BookingStatusCode equals bs.BookingStatusCode
                                             join ur in _ctx.tblUsers on sm.UserID equals ur.UserID
                                             join ag in _ctx.tblAgencies on ur.AgencyID equals ag.AgencyID
                                             join bc in _ctx.tblAirBookingCost on sm.BookingRefID equals bc.BookingRefID
                                             join bd in _ctx._tblAirBookingData on sm.BookingRefID equals bd.BookingRefID
                                             join sp in _ctx.tblSupplierMasters on sm.SupplierID equals sp.SupplierId
                                             where sm.BookingRefID == _bookref
                                             select new
                                             {
                                                 BookingDate = sm.BookingDate,
                                                 UserID = sm.UserID,
                                                 BookingID = sm.BookingRefID,
                                                 SupplierBookingReference = sm.SupplierBookingReference,
                                                 CancellationDeadline = sm.CancellationDeadline,
                                                 BookingStatusCode = sm.BookingStatusCode,
                                                 BookingStatus = bs.BookingStatus,
                                                 AgencyCode = ag.AgencyCode,
                                                 TotalBaseNet = bc.TotalBaseNet,
                                                 TotalTaxNet = bc.TotalTaxNet,
                                                 TotalNet = bc.TotalNet,
                                                 NetCurrency = bc.NetCurrency,
                                                 MarkupTypeID = bc.MarkupTypeID,
                                                 MarkupValue = bc.MarkupValue,
                                                 MarkupCurrency = bc.MarkupCurrency,
                                                 SellAmount = bc.SellAmount,
                                                 SellCurrency = bc.SellCurrency,
                                                 AdditionalServiceFee = bc.AdditionalServiceFee,
                                                 CancellationAmount = bc.CancellationAmount,
                                                 CancellationCurrency = bc.CancellationCurrency,
                                                 SupplierCode = sp.SupplierCode,
                                                 UniqueID = bd.UniqueID
                                             }).ToList();
                //To Response Json Start
                if (BookingRefIDBasicData.Count > 0)
                {
                    Dbconnectionresponse _Dbconnectionresponse = new Dbconnectionresponse()
                    {
                        AgencyCode = BookingRefIDBasicData[0].AgencyCode,
                        BookingId = BookingRefIDBasicData[0].BookingID.ToString(),
                        SupplierCode = BookingRefIDBasicData[0].SupplierCode,
                        BookingStatus = BookingRefIDBasicData[0].BookingStatusCode,
                        TicketLimit = DateTime.Parse(BookingRefIDBasicData[0].CancellationDeadline.ToString()).ToString("dd-MM-yyyy hh:mm:ss"),
                        BookingDate = DateTime.Parse(BookingRefIDBasicData[0].BookingDate.ToString()).ToString("dd-MM-yyyy hh:mm:ss"),
                        UniqueID = BookingRefIDBasicData[0].UniqueID
                    };
                    Dbtotalfaregroup _Dbtotalfaregroup = new Dbtotalfaregroup()
                    {
                        totalBaseNet = BookingRefIDBasicData[0].TotalBaseNet.ToString(),
                        totalTaxNet = BookingRefIDBasicData[0].TotalTaxNet.ToString(),
                        markupTypeID = BookingRefIDBasicData[0].MarkupTypeID.ToString(),
                        netCurrency = BookingRefIDBasicData[0].NetCurrency.ToString(),
                        markupValue = BookingRefIDBasicData[0].MarkupValue.ToString(),
                        markupCurrency = BookingRefIDBasicData[0].MarkupCurrency,
                        sellAmount = BookingRefIDBasicData[0].SellAmount.ToString(),
                        sellCurrency = BookingRefIDBasicData[0].SellCurrency.ToString(),
                        additionalServiceFee = BookingRefIDBasicData[0].AdditionalServiceFee.ToString(),
                        cancellationAmount = BookingRefIDBasicData[0].CancellationAmount.ToString(),
                        cancellationCurrency = BookingRefIDBasicData[0].CancellationCurrency,
                        paidDate = "",
                        paidAmount = BookingRefIDBasicData[0].TotalNet.ToString(),
                        paymentTypeID = ""
                    };
                    _Dbconnectionresponse.DBTotalfaregroup = _Dbtotalfaregroup;
                    //To Response Json End
                    var RefIDBaggDetails = (from bg in _ctx.tblAirbaggageDetails
                                            where bg.BookingRefID == _bookref
                                            select new
                                            {
                                                paxType = bg.PAXType,
                                                cabinBaggageQuantity = bg.CabinBaggageQuantity,
                                                cabinBaggageUnit = bg.CabinBaggageUnit,
                                                checkinBaggageQuantity = bg.CheckinBaggageQuantity,
                                                checkinBaggageUnit = bg.CheckinBaggageUnit
                                            }).ToList();
                    //To Response Json Start
                    int totalbaggdetails = RefIDBaggDetails.Count;
                    if (totalbaggdetails > 0)
                    {
                        Dbairbagdetail[] _Dbairbagdetails = new Dbairbagdetail[totalbaggdetails];
                        int bagSeq = 0;
                        foreach (var item in RefIDBaggDetails)
                        {
                            Dbairbagdetail _Dbairbagdetail = new Dbairbagdetail()
                            {
                                paxType = item.paxType,
                                cabinBaggageQuantity = item.cabinBaggageQuantity.ToString(),
                                cabinBaggageUnit = item.cabinBaggageUnit,
                                checkinBaggageQuantity = item.checkinBaggageQuantity.ToString(),
                                checkinBaggageUnit = item.checkinBaggageUnit
                            };
                            _Dbairbagdetails[bagSeq] = _Dbairbagdetail;
                            ++bagSeq;
                        }
                        _Dbconnectionresponse.DBairBagDetails = _Dbairbagdetails;
                    }
                    else
                    {
                        Dbairbagdetail[] _Dbairbagdetails = new Dbairbagdetail[0];
                        _Dbconnectionresponse.DBairBagDetails = _Dbairbagdetails;
                    }
                    //To Response Json End                    
                    var RefIDCostbreakup = (from br in _ctx.tblAirBookingCostBreakup
                                            join bc in _ctx.tblAirBookingCost on br.BookingCostID equals bc.BookingCostID
                                            where bc.BookingRefID == _bookref
                                            select new
                                            {
                                                paxType = br.PaxType,
                                                paxtotalTaxNet = br.TaxNet,
                                                paxtotalBaseNet = br.BaseNet,
                                                paxmarkupTypeID = br.MarkupTypeID,
                                                paxnetCurrency = br.NetCurrency,
                                                paxmarkupValue = br.MarkupValue,
                                                paxmarkupCurrency = br.MarkupCurrency,
                                                paxsellAmount = br.SellAmount,
                                                paxsellCurrency = br.SellCurrency,
                                                paxadditionalServiceFee = br.AdditionalServiceFee,
                                                paxquantity = br.NoOfPAx,
                                                paxpaidAmount = br.SellAmount
                                            }).ToList();
                    //To Response Json Start
                    int totalpricebk = RefIDCostbreakup.Count;
                    if (totalpricebk > 0)
                    {
                        int costSeq = 0;
                        Dbcostbreakuppax[] _Dbcostbreakuppaxs = new Dbcostbreakuppax[totalpricebk];
                        foreach (var item in RefIDCostbreakup)
                        {
                            Dbcostbreakuppax _Dbcostbreakuppax = new Dbcostbreakuppax()
                            {
                                paxType = item.paxType,
                                paxquantity = item.paxquantity.ToString(),
                                paxtotalBaseNet = item.paxtotalBaseNet.ToString(),
                                paxtotalTaxNet = item.paxtotalTaxNet.ToString(),
                                paxmarkupTypeID = item.paxmarkupTypeID.ToString(),
                                paxnetCurrency = item.paxnetCurrency.ToString(),
                                paxmarkupValue = item.paxmarkupValue.ToString(),
                                paxmarkupCurrency = item.paxmarkupCurrency.ToString(),
                                paxsellAmount = item.paxsellAmount.ToString(),
                                paxsellCurrency = item.paxsellCurrency.ToString(),
                                paxadditionalServiceFee = item.paxadditionalServiceFee.ToString(),
                                paxpaidAmount = item.paxpaidAmount.ToString()
                            };
                            _Dbcostbreakuppaxs[costSeq] = _Dbcostbreakuppax;
                            ++costSeq;
                        }
                        _Dbconnectionresponse.DBcostBreakuppax = _Dbcostbreakuppaxs;
                    }
                    else
                    {
                        Dbcostbreakuppax[] _Dbcostbreakuppaxs = new Dbcostbreakuppax[0];
                        _Dbconnectionresponse.DBcostBreakuppax = _Dbcostbreakuppaxs;
                    }
                    //To Response Json End                    
                    var RefIDfarerule = (from fr in _ctx.tblAirFarerules
                                         where fr.BookingRefID == _bookref
                                         select new
                                         {
                                             FareRule = fr.FareRule,
                                             Segment = fr.Segment,
                                             FareRef = fr.FareRef,
                                             FilingAirline = fr.FilingAirline,
                                             MarketingAirline = fr.MarketingAirline
                                         }).ToList();
                    //To Response Json Start
                    int totRefIDfarerule = RefIDfarerule.Count;
                    if (totRefIDfarerule > 0)
                    {
                        int ruleSeq = 0;
                        Dbfareruleseg[] _Dbfarerulesegs = new Dbfareruleseg[totRefIDfarerule];
                        foreach (var item in RefIDfarerule)
                        {
                            Dbfareruleseg _Dbfareruleseg = new Dbfareruleseg()
                            {
                                fareRule = item.FareRule,
                                Segment = item.Segment,
                                FareRef = item.FareRef,
                                FilingAirline = item.FilingAirline,
                                MarketingAirline = item.MarketingAirline
                            };
                            _Dbfarerulesegs[ruleSeq] = _Dbfareruleseg;
                            ++ruleSeq;
                        }
                        _Dbconnectionresponse.DBfareruleseg = _Dbfarerulesegs;
                    }
                    else
                    {
                        Dbfareruleseg[] _Dbfarerulesegs = new Dbfareruleseg[0];
                        _Dbconnectionresponse.DBfareruleseg = _Dbfarerulesegs;
                    }
                    //To Response Json End                    
                    var RefIDTravellers = (from ap in _ctx.tblAirPassengers
                                           join tl in _ctx.tblTitles on ap.TitleID equals tl.TitleID
                                           join pd in _ctx._tblAirPassengerDoc on ap.PaxID equals pd.PaxID
                                           join doc in _ctx._tblDocType on pd.DocTypeID equals doc.DocTypeID
                                           join pax in _ctx._tblPassengerType on ap.PaxTypeID equals pax.PaxTypeID
                                           where ap.BookingRefID == _bookref
                                           select new
                                           {
                                               Title = tl.Title,
                                               GivenName = ap.GivenName,
                                               Surname = ap.Surname,
                                               BirthDate = ap.DateofBirth,
                                               DocType = doc.DocType,
                                               DocumentNumber = pd.DocumentNumber,
                                               DocIssueCountry = pd.IssueCountry,
                                               ExpireDate = pd.ExpiryDate,
                                               AirPnr = ap.PNR,
                                               AirTicketNo = ap.ETicketNo,
                                               paxtype = pax.PaxType
                                           }).ToList();
                    //To Response Json Start
                    int totRefIDTravellers = RefIDTravellers.Count;
                    if (totRefIDTravellers > 0)
                    {
                        int travellerSeq = 0;
                        Dbtravelerinfo[] _Dbtravelerinfos = new Dbtravelerinfo[totRefIDTravellers];
                        foreach (var item in RefIDTravellers)
                        {
                            Dbtravelerinfo _Dbtravelerinfo = new Dbtravelerinfo()
                            {
                                PassengerType = item.paxtype.ToString(),
                                GivenName = item.GivenName,
                                NamePrefix = item.Title,
                                Surname = item.Surname,
                                BirthDate = Convert.ToDateTime(item.BirthDate).ToString("dd-MM-yyyy"),
                                DocType = item.DocType,
                                DocumentNumber = item.DocumentNumber,
                                DocIssueCountry = item.DocIssueCountry,
                                ExpireDate = Convert.ToDateTime(item.ExpireDate).ToString("dd-MM-yyyy"),
                                AirPnr = item.AirPnr,
                                AirTicketNo = item.AirTicketNo
                            };
                            _Dbtravelerinfos[travellerSeq] = _Dbtravelerinfo;
                            ++travellerSeq;
                        }
                        _Dbconnectionresponse.DBTravelerInfo = _Dbtravelerinfos;
                    }
                    else
                    {
                        Dbtravelerinfo[] _Dbtravelerinfos = new Dbtravelerinfo[0];
                        _Dbconnectionresponse.DBTravelerInfo = _Dbtravelerinfos;
                    }
                    //To Response Json End                    
                    var RefiDLegs = (from lg in _ctx.tblAirOriginDestinationOption
                                     where lg.BookingRefID == _bookref
                                     select new
                                     {
                                         OriginDestinationID = lg.OriginDestinationID,
                                         RefNumber = lg.RefNumber,
                                         DirectionID = lg.RefNumber,
                                         ElapsedTime = lg.ElapsedTime
                                     }).ToList();
                    int totRefiDLegs = RefiDLegs.Count;
                    if (totRefiDLegs > 0)
                    {
                        Dbflleggroup[] _Dbflleggroups = new Dbflleggroup[totRefiDLegs];
                        int legSeq = 0;
                        foreach (var item in RefiDLegs)
                        {
                            Dbflleggroup _Dbflleggroup = new Dbflleggroup();
                            _Dbflleggroup.elapsedtime = item.ElapsedTime;
                            long _LegID = item.OriginDestinationID;
                            var RefIDSegmant = (from seg in _ctx.tblAirSegment
                                                join segf in _ctx._tblAirSegmentBookingAvail on seg.SegmentID equals segf.SegmentID
                                                where seg.OriginDestinationID == _LegID
                                                select new
                                                {
                                                    FareBasis = "",
                                                    DepartureDate = seg.DepartureDateTime,
                                                    ArrivalDate = seg.ArrivalDateTime,
                                                    DepartureFrom = seg.DepartureAirportLocationCode,
                                                    DepartureTo = seg.ArrivalAirportLocationCode,
                                                    MarketingCompany = seg.MarketingAirlineCode,
                                                    OperatingCompany = seg.OperatingAirlineCode,
                                                    FlightNumber = seg.FlightNumber,
                                                    BookingClass = "",
                                                    terminalTo = seg.ArrivalAirportTerminal,
                                                    terminalFrom = seg.DepartureAirportTerminal,
                                                    flightequip = seg.EquipmentAirEquipType,
                                                    cabin = segf.ResBookDesigCabinCode
                                                }).ToList();
                            int totRefIDSegmant = RefIDSegmant.Count;
                            if (totRefIDSegmant > 0)
                            {
                                int segSeq = 0;
                                Dbsegment[] _Dbsegments = new Dbsegment[totRefIDSegmant];
                                foreach (var segitem in RefIDSegmant)
                                {
                                    Dbsegment _Dbsegment = new Dbsegment();
                                    _Dbsegment.DepartureDate = Convert.ToDateTime(segitem.DepartureDate).ToString("dd-MM-yyyy");
                                    _Dbsegment.DepartureTime = Convert.ToDateTime(segitem.DepartureDate).ToString("HHmm");
                                    _Dbsegment.ArrivalDate = Convert.ToDateTime(segitem.ArrivalDate).ToString("dd-MM-yyyy");
                                    _Dbsegment.ArrivalTime = Convert.ToDateTime(segitem.ArrivalDate).ToString("HHmm");
                                    _Dbsegment.DepartureFrom = segitem.DepartureFrom;
                                    _Dbsegment.DepartureTo = segitem.DepartureTo;
                                    _Dbsegment.MarketingCompany = segitem.MarketingCompany;
                                    _Dbsegment.OperatingCompany = segitem.OperatingCompany;
                                    _Dbsegment.FlightNumber = segitem.FlightNumber;
                                    _Dbsegment.BookingClass = segitem.BookingClass;
                                    _Dbsegment.terminalTo = segitem.terminalTo;
                                    _Dbsegment.terminalFrom = segitem.terminalFrom;
                                    _Dbsegment.flightequip = segitem.flightequip;
                                    _Dbsegment.cabin = segitem.cabin;
                                    _Dbsegments[segSeq] = _Dbsegment;
                                    ++segSeq;
                                }
                                _Dbflleggroup.from = RefIDSegmant[0].DepartureFrom;
                                _Dbflleggroup.to = RefIDSegmant[totRefIDSegmant - 1].DepartureTo;
                                _Dbflleggroup.DBsegments = _Dbsegments;
                            }
                            else
                            {
                                Dbsegment[] _Dbsegments = new Dbsegment[0];
                                _Dbflleggroup.DBsegments = _Dbsegments;
                            }
                            _Dbflleggroups[legSeq] = _Dbflleggroup;
                            ++legSeq;
                        }
                        _Dbconnectionresponse.DBFLLegGroup = _Dbflleggroups;
                    }
                    else
                    {
                        Dbflleggroup[] _Dbflleggroups = new Dbflleggroup[0];
                        _Dbconnectionresponse.DBFLLegGroup = _Dbflleggroups;
                    }

                    var refidPaxAdtcound = (from px in _ctx.tblAirBookingCostBreakup
                                            join pc in _ctx.tblAirBookingCost_1 on px.BookingCostID equals pc.BookingCostID
                                            where pc.BookingRefID == _bookref && px.PaxType == "ADT"
                                            select new
                                            {
                                                PaxType = px.PaxType,
                                                quantity = px.NoOfPAx
                                            }).ToList();

                    var refidPaxchdcound = (from px in _ctx.tblAirBookingCostBreakup
                                            join pc in _ctx.tblAirBookingCost_1 on px.BookingCostID equals pc.BookingCostID
                                            where pc.BookingRefID == _bookref && px.PaxType == "CHD"
                                            select new
                                            {
                                                PaxType = px.PaxType,
                                                quantity = px.NoOfPAx
                                            }).ToList();

                    var refidPaxinfcound = (from px in _ctx.tblAirBookingCostBreakup
                                            join pc in _ctx.tblAirBookingCost_1 on px.BookingCostID equals pc.BookingCostID
                                            where pc.BookingRefID == _bookref && px.PaxType == "INF"
                                            select new
                                            {
                                                PaxType = px.PaxType,
                                                quantity = px.NoOfPAx
                                            }).ToList();

                    if (refidPaxAdtcound.Count > 0)
                    {
                        _Dbconnectionresponse.ADT = Convert.ToInt16(refidPaxAdtcound[0].quantity);
                    }
                    else
                    {
                        _Dbconnectionresponse.ADT = 0;
                    }
                    if (refidPaxchdcound.Count > 0)
                    {
                        _Dbconnectionresponse.CHD = Convert.ToInt16(refidPaxchdcound[0].quantity);
                    }
                    else
                    {
                        _Dbconnectionresponse.CHD = 0;
                    }
                    if (refidPaxinfcound.Count > 0)
                    {
                        _Dbconnectionresponse.INF = Convert.ToInt16(refidPaxinfcound[0].quantity);
                    }
                    else
                    {
                        _Dbconnectionresponse.INF = 0;
                    }
                    _GetTripDetailsModelRS.DBConnectionResponse = _Dbconnectionresponse;
                }
                else
                {

                }

            }
            return _GetTripDetailsModelRS;
        }

        public async Task<string> GetUserIDfromRefID(string BookingRefID)
        {
            string userID = "";
            long _bookrefid = Convert.ToInt64(BookingRefID);
            using (var _ctx = new MediationEntities())
            {
                var refIDuser = (from us in _ctx.tblBookings
                                 where us.BookingRefID == _bookrefid
                                 select new
                                 {
                                     UserID = us.UserID
                                 }).ToList();
                userID = refIDuser[0].UserID.ToString();
            }
            return userID;
        }
        public async Task SaveLog(string logName, string agency, string rQ, string rS)
        {
            try
            {
                string date = DateTime.Now.ToString("dd-MM-yyyy");
                string storePath = @"E:\API\logs\Mediation-Server\LOGRQRS\" + date + " / " + agency + " / " + logName + "/";
                //string storePath = @"C:\Inetpub\vhosts\oneviewitsolutions.com\API\logs\Mediation-Server\LOGRQRS\" + agency + " / " + date + " / " + logName + "/";
                if (!Directory.Exists(storePath))
                    Directory.CreateDirectory(storePath);
                string timeStamp = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-ffff");
                string filenameRq = storePath + "/" + logName + "_" + timeStamp + "_RQ.json";
                File.WriteAllText(filenameRq, rQ);
                string filenameRs = storePath + "/" + logName + "_" + timeStamp + "_RS.json";
                File.WriteAllText(filenameRs, rS);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
