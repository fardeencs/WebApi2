
namespace Logic
{
    using BusinessEntitties;
    using BusinessEntitties.Enumrations;
    using Common;
    using DAL;
    using Domain;
    using Domain.Entity;
    using Logic.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Transactions;
    using System.Xml.Linq;

    public class BookingServices : IBookingServices
    {
        public bool CheckUniqIDExistOrNot(string uniqid)
        {
            bool bookingexist = false;
            using (var _ctx = new MediationEntities())
            {
                var uniqID = _ctx._tblAirBookingData.FirstOrDefault(x => x.UniqueID == uniqid);
                if (uniqID != null)
                {
                    bookingexist = true;
                }
            }
            return bookingexist;
        }
        public async Task<BookingData> SavingAirBookingFlight(BusinessEntitties.BookFlightModel message, Domain.BookFlightResponse reponse, long agencyId, long supplierId)
        {

            long bookingid = 0; long userid = 0;
            BookingData _BookingData = new BookingData();
            try
            {
                using (var _ctx = new MediationEntities())
                {
                    var exsitUser = _ctx.tblUsers.FirstOrDefault(x => x.EmailID == message.BookFlightEntity.BookFlight.Email);
                    using (var scope = new TransactionScope())
                    {
                        if (exsitUser == null)
                        {
                            tblUser user = new tblUser()
                            {
                                EmailID = message.BookFlightEntity.BookFlight.Email,
                                CreatedBy = 1,
                                Mobile = message.BookFlightEntity.BookFlight.PhoneNumber,
                                AgencyID = agencyId,
                                LoginStatus = 1,
                                AdminStatus = 0
                            };
                            _ctx.tblUsers.Add(user);
                            _ctx.SaveChanges();
                            userid = user.UserID;
                        }
                        else
                        {
                            userid = exsitUser.UserID;

                        }
                        tblBooking booking = new tblBooking()
                        {
                            BookingID = 1,
                            Position = 1,
                            UserID = userid,
                            ServiceTypeId = 1,
                            BookingDate = DateTime.Now,
                            SupplierID = supplierId,
                            SupplierBookingReference = CheckedPNRNullornot(reponse.BookFlightResult.Airlinepnr, reponse.BookFlightResult.UniqueID),
                            CancellationDeadline = DateTime.Now,
                            BookingStatusCode = "RQ",
                            AgencyRemarks = "",
                            PaymentStatusID = 1,
                        };
                        _ctx.tblBookings.Add(booking);
                        _ctx.SaveChanges();
                        booking.BookingID = booking.BookingRefID;
                        _ctx.Entry(booking).State = EntityState.Modified;
                        _ctx.SaveChanges();
                        bookingid = booking.BookingRefID;

                        tblBookingHistory bookinghistory = new tblBookingHistory()
                        {
                            BookingRefID = Convert.ToInt64(bookingid.ToString()),
                            BookingStatusCode = "RQ",
                            UserID = userid,
                            ActionDate = DateTime.Now
                        };
                        _ctx.tblBookingHistorys.Add(bookinghistory);
                        _ctx.SaveChanges();

                        tblPayment _tblPayment = new tblPayment()
                        {
                            BookingRefID = bookingid,
                            PaidAmount = Convert.ToDecimal(message.Totalfaregroup.PaidAmount),
                            CurrencyCode = message.Totalfaregroup.SellCurrency,
                            PaidDate = Convert.ToDateTime(message.Totalfaregroup.PaidDate),
                            PaymentTypeID = Convert.ToInt16(message.Totalfaregroup.PaymentTypeID)
                        };
                        _ctx.tblPayment.Add(_tblPayment);
                        _ctx.SaveChanges();

                        List<tblAirFarerules> tblAirFareruleslist = new List<tblAirFarerules>();
                        foreach (var items in message.Fareruleseg)
                        {
                            tblAirFareruleslist.Add(new tblAirFarerules()
                            {
                                BookingRefID = bookingid,
                                FareRule = items.FareRule,
                                Segment = items.Segment,
                                FareRef = items.FareRef,
                                FilingAirline = items.FilingAirline,
                                MarketingAirline = items.MarketingAirline
                            });
                        }
                        _ctx.tblAirFarerules.AddRange(tblAirFareruleslist);
                        _ctx.SaveChanges();

                        tblAirBookingCost _tblAirBookingCost = new tblAirBookingCost()
                        {
                            BookingRefID = bookingid,
                            TotalBaseNet = Convert.ToDecimal(message.Totalfaregroup.TotalBaseNet),
                            TotalTaxNet = Convert.ToDecimal(message.Totalfaregroup.TotalTaxNet),
                            TotalNet = Convert.ToDecimal(message.Totalfaregroup.PaidAmount),
                            NetCurrency = message.Totalfaregroup.NetCurrency,
                            MarkupTypeID = Convert.ToInt16(message.Totalfaregroup.MarkupTypeID),
                            MarkupValue = Convert.ToDouble(message.Totalfaregroup.MarkupValue),
                            MarkupCurrency = message.Totalfaregroup.MarkupCurrency,
                            SellAmount = Convert.ToDecimal(message.Totalfaregroup.SellAmount),
                            CommessionType = (int)message.Totalfaregroup.CommissionType,
                            CommessionValue = (decimal)message.Totalfaregroup.CommissionValue,
                            SellCurrency = message.Totalfaregroup.SellCurrency,
                            AdditionalServiceFee = Convert.ToDecimal(message.Totalfaregroup.AdditionalServiceFee),
                            CancellationAmount = Convert.ToDecimal(message.Totalfaregroup.CancellationAmount),
                            CancellationCurrency = message.Totalfaregroup.CancellationCurrency
                        };
                        _ctx.tblAirBookingCost.Add(_tblAirBookingCost);
                        _ctx.SaveChanges();
                        long bookingCostID = _tblAirBookingCost.BookingCostID;

                        List<tblAirBookingCostBreakup> tblAirBookingCostBreakupList = new List<tblAirBookingCostBreakup>();
                        foreach (var item in message.CostBreakuppax)
                        {
                            tblAirBookingCostBreakupList.Add(new tblAirBookingCostBreakup()
                            {
                                BookingCostID = bookingCostID,
                                BaseNet = Convert.ToDecimal(item.PaxtotalBaseNet),
                                TaxNet = Convert.ToDecimal(item.PaxtotalTaxNet),
                                TotalNet = Convert.ToDecimal(item.PaxpaidAmount),
                                NetCurrency = item.PaxnetCurrency,
                                MarkupTypeID = Convert.ToInt16(item.PaxmarkupTypeID),
                                MarkupValue = double.Parse(item.PaxmarkupValue),
                                MarkupCurrency = item.PaxmarkupCurrency,
                                SellAmount = Convert.ToDecimal(item.PaxsellAmount),
                                SellCurrency = item.PaxsellCurrency,
                                AdditionalServiceFee = Convert.ToDecimal(item.PaxadditionalServiceFee),
                                PaxType = item.PaxType,
                                NoOfPAx = Convert.ToInt16(item.TotalPaxQuantity)
                            });
                        }
                        _ctx.tblAirBookingCostBreakup.AddRange(tblAirBookingCostBreakupList);
                        _ctx.SaveChanges();

                        List<tblAirbaggageDetails> tblAirbaggageDetailsList = new List<tblAirbaggageDetails>();
                        foreach (var items in message.AirBagDetails)
                        {
                            tblAirbaggageDetailsList.Add(new tblAirbaggageDetails()
                            {
                                BookingRefID = bookingid,
                                PAXType = items.PAXTyp,
                                CabinBaggageQuantity = Convert.ToInt16(items.CabinBaggageQuantity),
                                CabinBaggageUnit = items.CabinBaggageUnit,
                                CheckinBaggageQuantity = Convert.ToInt16(items.CheckinBaggageQuantity),
                                CheckinBaggageUnit = items.CheckinBaggageUnit,
                                FromSeg = items.fromSeg,
                                ToSeg = items.toseg
                            });
                        }
                        _ctx.tblAirbaggageDetails.AddRange(tblAirbaggageDetailsList);
                        _ctx.SaveChanges();

                        var SeqNumber = 0;
                        foreach (var items in message.BookFlightEntity.BookFlight.FLLegGroup)
                        {
                            tblAirOriginDestinationOption _tblAirOriginDestinationOption = new tblAirOriginDestinationOption()
                            {
                                BookingRefID = bookingid,
                                RefNumber = 0,
                                DirectionID = SeqNumber,
                                ElapsedTime = items.ElapsedTime
                            };
                            _ctx.tblAirOriginDestinationOption.Add(_tblAirOriginDestinationOption);
                            _ctx.SaveChanges();
                            foreach (var segitem in items.Segments)
                            {
                                string departureDateTimeAppHH = segitem.DepartureTime.Substring(0, 2);
                                string departureDateTimeAppMM = segitem.DepartureTime.Substring(2, 2);
                                string departureDateTimeApp = segitem.DepartureDate.Replace('-', '/') + " " + departureDateTimeAppHH + ":" + departureDateTimeAppMM + ":00";
                                string arrDateTimeAppHH = segitem.ArrivalTime.Substring(0, 2);
                                string arrDateTimeAppMM = segitem.ArrivalTime.Substring(2, 2);
                                string arrDateTimeAppNew = segitem.ArrivalDate.Replace('-', '/') + " " + arrDateTimeAppHH + ":" + arrDateTimeAppMM + ":00";
                                tblAirSegment _tblAirSegment = new tblAirSegment()
                                {
                                    OriginDestinationID = _tblAirOriginDestinationOption.OriginDestinationID,
                                    DepartureDateTime = CommonFunctions.ConvertDateTimeFromString(departureDateTimeApp, "hi-IN"),// Convert.ToDateTime(departureDateTimeApp, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat),
                                    ArrivalDateTime = CommonFunctions.ConvertDateTimeFromString(arrDateTimeAppNew, "hi-IN"),// Convert.ToDateTime(arrDateTimeAppNew, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat),
                                    FlightNumber = segitem.FlightNumber,
                                    Status = 1,
                                    DepartureAirportLocationCode = segitem.DepartureFrom,
                                    DepartureAirportTerminal = segitem.TerminalFrom,
                                    ArrivalAirportLocationCode = segitem.DepartureTo,
                                    ArrivalAirportTerminal = segitem.TerminalTo,
                                    OperatingAirlineCode = segitem.OperatingCompany,
                                    EquipmentAirEquipType = segitem.Flightequip,
                                    MarketingAirlineCode = segitem.MarketingCompany
                                };
                                _ctx.tblAirSegment.Add(_tblAirSegment);
                                _ctx.SaveChanges();

                                Int64 segid = Convert.ToInt64(_tblAirSegment.SegmentID);
                                tblAirSegmentBookingAvail _tblAirSegmentBookingAvail = new tblAirSegmentBookingAvail()
                                {
                                    SegmentID = segid,
                                    ResBookDesigCode = "",
                                    AvailablePTC = "",
                                    ResBookDesigCabinCode = segitem.BookingClass,
                                    FareBasis = segitem.FareBasis
                                };
                                _ctx._tblAirSegmentBookingAvail.Add(_tblAirSegmentBookingAvail);
                                _ctx.SaveChanges();
                            }
                            ++SeqNumber;
                        }
                        if (reponse.BookFlightResult.Errors != null)
                        {
                            List<tblAirBookingError> tblAirBookingErrorList = new List<tblAirBookingError>();
                            foreach (var item in reponse.BookFlightResult.Errors)
                            {
                                tblAirBookingErrorList.Add(new tblAirBookingError()
                                {
                                    BookingRefID = bookingid,
                                    ErrorCode = item.Code,
                                    ErrorMessage = item.Message
                                });
                            }
                            _ctx._tblAirBookingError.AddRange(tblAirBookingErrorList);
                            _ctx.SaveChanges();
                        }


                        AddAirPassengers(_ctx, message.BookFlightEntity.BookFlight.TravelerInfo, reponse, bookingid, message.BookFlightEntity.BookFlight.CustomerInfo.Email, message.BookFlightEntity.BookFlight.CustomerInfo.PhoneNumber, message.BookFlightEntity.BookFlight.CustomerInfo.PhoneCountry, userid);

                        //List<tblAirbookingcostMarkUp> tblAirbookingcostMarkUpList = AddAdvancelevelMarkup(message.costAirMarkUp, bookingCostID, decimal.Parse(message.Totalfaregroup.PaidAmount));
                        //_ctx._tblAirbookingcostMarkUp.AddRange(tblAirbookingcostMarkUpList);
                        //_ctx.SaveChanges();

                        scope.Complete();
                    }
                    _BookingData.BookingRefID = Convert.ToInt64(bookingid);
                    _BookingData.userID = Convert.ToInt64(exsitUser.UserID);
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _BookingData;
        }
        private List<tblAirbookingcostMarkUp> AddAdvancelevelMarkup(List<costAirMarkUp> _costAirMarkUp, long bookingCostID, decimal totalAmount)
        {
            List<tblAirbookingcostMarkUp> tblAirbookingcostMarkUpList = new List<tblAirbookingcostMarkUp>();
            int MarkSeq = 0;
            foreach (var item in _costAirMarkUp)
            {
                decimal agencyCommonMarkUpAmount = 0;
                decimal agencyMarkUpAmount = getMarkupAmount(totalAmount, Convert.ToDecimal(item.v1), Convert.ToInt16(item.t1));
                int levelType = 1;
                if (MarkSeq == _costAirMarkUp.Count - 1)
                {
                    levelType = 2;
                }
                if (MarkSeq == 0)
                {
                    agencyCommonMarkUpAmount = getCommonMarkupAmount(totalAmount, decimal.Parse(item.v1), int.Parse(item.v2), 0, 0);
                }
                else
                {
                    agencyCommonMarkUpAmount = getCommonMarkupAmount(totalAmount, decimal.Parse(item.v1), int.Parse(item.v2), decimal.Parse(_costAirMarkUp[MarkSeq - 1].v2), int.Parse(_costAirMarkUp[MarkSeq - 1].t2));
                }

                tblAirbookingcostMarkUpList.Add(new tblAirbookingcostMarkUp
                {
                    BookingCostID = bookingCostID,
                    CommonMarkUpAmount = agencyCommonMarkUpAmount,
                    MarkUpAmount = agencyMarkUpAmount,
                    LevelID = long.Parse(item.agencyID),
                    LevelType = levelType,

                });
                ++MarkSeq;
            }
            return tblAirbookingcostMarkUpList;
        }
        private decimal getCommonMarkupAmount(decimal totalAmount, decimal markupamount, int markuptype, decimal commonmarkup, int commonmarkuptype)
        {
            decimal totalmarkupamount = 0;
            switch (markuptype)
            {
                case 1:
                    totalmarkupamount = (totalAmount * markupamount) / 100;
                    break;
                case 2:
                    totalmarkupamount = markupamount;
                    break;
                default:
                    break;
            }

            switch (commonmarkuptype)
            {
                case 1:
                    totalmarkupamount = totalmarkupamount + (totalAmount * commonmarkup) / 100;
                    break;
                case 2:
                    totalmarkupamount = totalmarkupamount + commonmarkup;
                    break;
                default:
                    break;
            }

            return totalmarkupamount;
        }

        private decimal getMarkupAmount(decimal totalAmount, decimal markupamount, int type)
        {
            decimal amountWithMarkUp = 0;
            switch (type)
            {
                case 1:
                    amountWithMarkUp = (totalAmount * markupamount) / 100;
                    break;
                case 2:
                    amountWithMarkUp = markupamount;
                    break;
                default:
                    amountWithMarkUp = 0;
                    break;
            }
            return amountWithMarkUp;
        }
        private void AddAirPassengers(MediationEntities _ctx, List<Travelerinfo> message, BookFlightResponse reponse, long bookingId, string emailid, string telphone, string locationcode, long UserID)
        {
            bool isUpdated = true;
            var xDoc = GetXDocTravelInfoProcesses(message, bookingId, emailid, telphone, locationcode, UserID, reponse.BookFlightResult.Airlinepnr);
            const string sql = @"EXEC spAddAirPassengersDetails @xmlData";
            var result = _ctx.Database
                .SqlQuery<SpBaseEntity>(
                          sql
                        , new SqlParameter("xmlData", new SqlXml(xDoc.Root.CreateReader()))

                ).FirstOrDefault();

            if (result.StatusCode != "OK-200")
            {
                isUpdated = false;
            }
        }
        private static XDocument GetXDocTravelInfoProcesses(List<Travelerinfo> list, long bookingId, string emailid, string telphone, string locationcode, long UserID, string pnr)
        {
            return new XDocument(
                new XElement("EVENTS",
                    from xevent in list
                    select new XElement("EVENT"
                            , new XElement("GivenName", xevent.GivenName)
                            , new XElement("BookingRefID", bookingId.ToString())
                            , new XElement("TitleID", GetTitleID(xevent.NamePrefix))
                            , new XElement("PaxTypeID", Getpaxtype(xevent.PassengerTypeQuantity))
                            , new XElement("Surname", xevent.Surname)
                            , new XElement("EmailID", emailid)
                            , new XElement("TelPhoneType", telphone)
                            , new XElement("LocationCode", locationcode)
                            , new XElement("DateofBirth", DateTime.ParseExact(xevent.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                            , new XElement("PNR", pnr)
                            , new XElement("ETicketNo", "")
                            , new XElement("UserID", UserID.ToString())
                            , new XElement("DocTypeID", getDocId(xevent._DocType))
                            , new XElement("DocumentNumber", xevent._DocID)
                            , new XElement("ExpiryDate", GetDate(xevent._ExpireDate))
                            , new XElement("IssueLocation", "")
                            , new XElement("IssueCountry", xevent._DocIssueCountry)
                            , new XElement("AirLineCode", xevent.FFAirlineCode)
                            , new XElement("FFNumber", xevent.FrequentFlyerNumber)
                        )));
        }
        public static DateTime GetDate(string Date)
        {
            DateTime validadate;
            if (Date == "")
            {
                validadate = DateTime.Parse("01/01/1900");
            }
            else
            {
                validadate = DateTime.ParseExact(Date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            return validadate;
        }
        public static string GetTitleID(string title)
        {
            int titleID = 0;
            switch ((Titles)Enum.Parse(typeof(Titles), title, true))
            {
                case Titles.Mr:
                    titleID = (int)Titles.Mr;
                    break;
                case Titles.Mrs:
                    titleID = (int)Titles.Mrs;
                    break;
                case Titles.Ms:
                    titleID = (int)Titles.Ms;
                    break;
                case Titles.Miss:
                    titleID = (int)Titles.Miss;
                    break;
                case Titles.Mstr:
                    titleID = (int)Titles.Mstr;
                    break;
                case Titles.Dr:
                    titleID = (int)Titles.Dr;
                    break;
                case Titles.HE:
                    titleID = (int)Titles.HE;
                    break;
                case Titles.HH:
                    titleID = (int)Titles.HH;
                    break;
            }
            return titleID.ToString();
        }
        public static string Getpaxtype(string paxtype)
        {
            int paxtypeID = 0;
            switch ((PaxTypes)Enum.Parse(typeof(PaxTypes), paxtype, true))
            {
                case PaxTypes.ADT:
                    paxtypeID = (int)PaxTypes.ADT;
                    break;
                case PaxTypes.CHD:
                    paxtypeID = (int)PaxTypes.CHD;
                    break;
                case PaxTypes.INF:
                    paxtypeID = (int)PaxTypes.INF;
                    break;
                default:
                    break;
            }
            return paxtypeID.ToString();
        }
        public static string getDocId(string doctype)
        {
            string typeid = "";
            doctype = doctype.ToLower();
            if (doctype == "passport")
            {
                typeid = "1";
            }
            else if (doctype == "govt issued Identity card")
            {
                typeid = "2";
            }

            return typeid;
        }
        public async Task UpdateAirBookingAfterIssuedTicket(Itineraryinformation[] itineraryinformation, long bookingrefID, long userid, string pnr, string bookingStatus)
        {
            using (var _ctx = new MediationEntities())
            {
                using (var scope = new TransactionScope())
                {
                    tblBooking _tblBooking = new tblBooking()
                    {
                        BookingRefID = bookingrefID,
                        SupplierBookingReference = pnr,
                        BookingStatusCode = bookingStatus
                    };
                    _ctx.tblBookings.Attach(_tblBooking);
                    _ctx.Entry(_tblBooking).Property(x => x.SupplierBookingReference).IsModified = true;
                    _ctx.Entry(_tblBooking).Property(x => x.BookingStatusCode).IsModified = true;
                    _ctx.SaveChanges();

                    tblBookingHistory _tblBookingHistory = new tblBookingHistory()
                    {
                        BookingRefID = bookingrefID,
                        BookingStatusCode = bookingStatus,
                        UserID = userid,
                        ActionDate = DateTime.Now
                    };
                    _ctx.tblBookingHistorys.Add(_tblBookingHistory);
                    _ctx.SaveChanges();

                    long _bookref = bookingrefID;
                    var airpassengerdetails = (from sm in _ctx.tblAirPassengers
                                               where sm.BookingRefID == _bookref
                                               select new
                                               {
                                                   PaxID = sm.PaxID
                                               }).ToList()
                                                .Select(x => new tblAirPassengers()
                                                {
                                                    PaxID = x.PaxID
                                                }).ToList();
                    int i = 0;
                    foreach (var item in airpassengerdetails)
                    {
                        tblAirPassengers _tblAirPassenger = new tblAirPassengers()
                        {
                            PaxID = item.PaxID,
                            PNR = pnr,
                            ETicketNo = itineraryinformation[i].ETickets.eTicketNumber
                        };
                        _ctx.tblAirPassengers.Attach(_tblAirPassenger);
                        _ctx.Entry(_tblAirPassenger).Property(x => x.PNR).IsModified = true;
                        _ctx.Entry(_tblAirPassenger).Property(x => x.ETicketNo).IsModified = true;
                        _ctx.SaveChanges();
                        ++i;
                    }
                    scope.Complete();
                }
            }
        }
        public async Task UpdateBookingStatusafterIssueTicket(string bookingStatus, long bookingRefID, long userID)
        {
            using (var _ctx = new MediationEntities())
            {
                using (var scope = new TransactionScope())
                {
                    tblBooking _tblBooking = new tblBooking()
                    {
                        BookingRefID = bookingRefID,
                        BookingStatusCode = bookingStatus
                    };
                    _ctx.tblBookings.Attach(_tblBooking);
                    _ctx.Entry(_tblBooking).Property(x => x.BookingStatusCode).IsModified = true;
                    _ctx.SaveChanges();

                    tblBookingHistory _tblBookingHistory = new tblBookingHistory()
                    {
                        BookingRefID = bookingRefID,
                        BookingStatusCode = bookingStatus,
                        UserID = userID,
                        ActionDate = DateTime.Now
                    };
                    _ctx.tblBookingHistorys.Add(_tblBookingHistory);
                    _ctx.SaveChanges();

                    scope.Complete();
                }
            }
        }
        public string CheckedPNRNullornot(string pnr, string uniqID)
        {
            if (pnr == "")
            {
                return uniqID;
            }
            else if (pnr == "NA")
            {
                return uniqID;
            }
            else if (pnr == "NIL")
            {
                return uniqID;
            }
            else
            {
                return pnr;
            }
        }
        public string GetPaymentCardType(string agencyCode)
        {
            string cardName = "";
            using (var _ctx = new MediationEntities())
            {
                var cardTypes = (from cn in _ctx._tblCardDetail
                                 join onl in _ctx._tblOnlinePaymentOption on cn.CardID equals onl.CardID
                                 join ag in _ctx.tblAgencies on onl.AgencyID equals ag.AgencyID
                                 where ag.AgencyCode == agencyCode && onl.Status == 1
                                 select new
                                 {
                                     cardTypes = cn.Name
                                 }).FirstOrDefault();
                if (cardTypes != null)
                {
                    cardName = cardTypes.cardTypes;
                }
            }
            return cardName;
        }
        public Paymentinfo GetPaymentCardDetails(string agencyCode)
        {
            using (var _ctx = new MediationEntities())
            {
                var cardTypes = (from cn in _ctx._tblCardDetail
                                 join onl in _ctx._tblOnlinePaymentOption on cn.CardID equals onl.CardID
                                 join ag in _ctx.tblAgencies on onl.AgencyID equals ag.AgencyID
                                 where ag.AgencyCode == agencyCode && onl.Status == 1
                                 select new Paymentinfo
                                 {
                                     CVC = onl.CVV,
                                     Expiry = onl.Expiry,
                                     PaymentCode = cn.Name,
                                     Holder = onl.CradHolderName,
                                     Number = onl.CardNo
                                 }).FirstOrDefault();
                return cardTypes;
            }
        }
        public Connectiontodbreq GetBookedSupplierInfo(long bookingrefID)
        {
            using (var _ctx = new MediationEntities())
            {
                try
                {
                    var bookedsupplerInfo = (from bk in _ctx.tblBookings
                                             join bkd in _ctx._tblAirBookingData on bk.BookingRefID equals bkd.BookingRefID
                                             join sp in _ctx.tblSupplierMasters on bk.SupplierID equals sp.SupplierId
                                             join us in _ctx.tblUsers on bk.UserID equals us.UserID
                                             join ag in _ctx.tblAgencies on us.AgencyID equals ag.AgencyID
                                             where bk.BookingRefID == bookingrefID
                                             select new Connectiontodbreq
                                             {
                                                 SupplierCodeDb = sp.SupplierCode,
                                                 SupplierPnr = bkd.PNR,
                                                 SupplierUniqueId = bkd.UniqueID,
                                                 AgencyCodeDb = ag.AgencyCode,
                                                 BookingStatusCode = bk.BookingStatusCode
                                             }).FirstOrDefault();
                    return bookedsupplerInfo;
                }
                catch (Exception ex) { throw ex; }
            }
        }
        public void UpdateBookingInformatiomFromTripDetails(SupplierTripDetailsResponse supplierTripDetailsResponse, long bookingrefID)
        {
            using (var _ctx = new MediationEntities())
            {
                string spBookingStatus = supplierTripDetailsResponse.data.travelItinerary.bookingStatus;
                var bookingInfo = _ctx.tblBookings.Where(x => x.BookingRefID == bookingrefID).FirstOrDefault();
                if (bookingInfo != null && bookingInfo.BookingStatusCode != spBookingStatus)
                {
                    string uniqID = ""; string pnr = ""; string ticketnumebr = "";
                    if (supplierTripDetailsResponse.data.travelItinerary.uniqueID != "")
                    {
                        uniqID = supplierTripDetailsResponse.data.travelItinerary.uniqueID;
                    }
                    if (supplierTripDetailsResponse.data.travelItinerary.itineraryInfo.reservationItems.Length > 0)
                    {
                        if (supplierTripDetailsResponse.data.travelItinerary.itineraryInfo.reservationItems[0].airlinePNR != "")
                        {
                            pnr = supplierTripDetailsResponse.data.travelItinerary.itineraryInfo.reservationItems[0].airlinePNR;
                        }
                    }
                    if (supplierTripDetailsResponse.data.travelItinerary.itineraryInfo.customerInfos.Length > 0)
                    {
                        if (supplierTripDetailsResponse.data.travelItinerary.itineraryInfo.customerInfos[0].eTickets.Length > 0)
                        {
                            ticketnumebr = supplierTripDetailsResponse.data.travelItinerary.itineraryInfo.customerInfos[0].eTickets[0].eTicketNumber;
                        }
                    }
                    using (var scope = new TransactionScope())
                    {

                        bookingInfo.BookingRefID = bookingrefID;
                        bookingInfo.BookingStatusCode = spBookingStatus;
                        bookingInfo.SupplierBookingReference = pnr;
                        _ctx.Entry(bookingInfo).State = EntityState.Modified;
                        _ctx.SaveChanges();

                        tblBookingHistory _tblBookingHistory = new tblBookingHistory()
                        {
                            BookingRefID = bookingrefID,
                            BookingStatusCode = spBookingStatus,
                            UserID = bookingInfo.UserID,
                            ActionDate = DateTime.Now
                        };
                        _ctx.tblBookingHistorys.Add(_tblBookingHistory);
                        _ctx.SaveChanges();

                        var airBooking = _ctx._tblAirBookingData.Where(x => x.BookingRefID == bookingrefID).FirstOrDefault();
                        if (airBooking != null && airBooking.ID > 0)
                        {
                            airBooking.PNR = pnr;
                            airBooking.UniqueID = uniqID;

                            _ctx.Entry(airBooking).State = EntityState.Modified;
                            _ctx.SaveChanges();
                        }
                        var passengerExsitDetails = _ctx.tblAirPassengers.Where(w => w.BookingRefID == bookingrefID);
                        if (passengerExsitDetails != null && passengerExsitDetails.ToList().Count() > 0)
                        {
                            foreach (var detail in passengerExsitDetails)
                            {
                                detail.PNR = pnr;
                                detail.ETicketNo = ticketnumebr;
                                _ctx.Entry(detail).State = EntityState.Modified;
                                _ctx.SaveChanges();
                            }
                        }
                        scope.Complete();
                    }
                }
            }
        }
        public void UpdatePNRandStatus(BookFlightResponse response, BookingData bookingdata)
        {
            using (var _ctx = new MediationEntities())
            {
                using (var scope = new TransactionScope())
                {
                    tblBookingHistory bookinghistory = new tblBookingHistory()
                    {
                        BookingRefID = bookingdata.BookingRefID,
                        BookingStatusCode = "HK",
                        UserID = bookingdata.userID,
                        ActionDate = DateTime.Now
                    };
                    _ctx.tblBookingHistorys.Add(bookinghistory);
                    _ctx.SaveChanges();

                    tblBooking _tblBooking = new tblBooking()
                    {
                        BookingRefID = bookingdata.BookingRefID,
                        BookingStatusCode = "HK",
                        SupplierBookingReference = response.BookFlightResult.Airlinepnr
                    };
                    _ctx.tblBookings.Attach(_tblBooking);
                    _ctx.Entry(_tblBooking).Property(x => x.BookingStatusCode).IsModified = true;
                    _ctx.Entry(_tblBooking).Property(x => x.SupplierBookingReference).IsModified = true;
                    _ctx.SaveChanges();

                    tblAirBookingData _tblAirBookingData = new tblAirBookingData()
                    {
                        BookingRefID = bookingdata.BookingRefID,
                        PNR = response.BookFlightResult.Airlinepnr,
                        UniqueID = response.BookFlightResult.UniqueID
                    };
                    _ctx._tblAirBookingData.Add(_tblAirBookingData);
                    _ctx.SaveChanges();

                    var airpassengers = (from ap in _ctx.tblAirPassengers
                                         where ap.BookingRefID == bookingdata.BookingRefID
                                         select new
                                         {
                                             PaxID = ap.PaxID
                                         }).ToList();

                    foreach (var item in airpassengers)
                    {
                        tblAirPassengers _tblAirPassengers = new tblAirPassengers()
                        {
                            PaxID = item.PaxID,
                            PNR = response.BookFlightResult.Airlinepnr,
                        };
                        _ctx.tblAirPassengers.Attach(_tblAirPassengers);
                        _ctx.Entry(_tblAirPassengers).Property(x => x.PNR).IsModified = true;
                        _ctx.SaveChanges();
                    }
                    scope.Complete();
                }
            }
        }
        public bool CheckBookingRefIDExist(long bookingRefID)
        {
            bool Exist = false;
            using (var _ctx = new MediationEntities())
            {
                var BookingRecord = (from bk in _ctx.tblBookings
                                     where bk.BookingRefID == bookingRefID && bk.BookingStatusCode == "RQ"
                                     select new
                                     {
                                         BookingRefID = bk.BookingRefID
                                     }).FirstOrDefault();
                if (BookingRecord != null)
                {
                    Exist = true;
                }
            }
            return Exist;
        }
        public async Task UpdateAllDetailsWithRefID(BusinessEntitties.BookFlightModel model)
        {
            var bookingrefid = long.Parse(model.BookFlightEntity.BookFlight.BookingId);
            using (var _ctx = new MediationEntities())
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        var airbookingcost = _ctx.tblAirBookingCost.Where(x => x.BookingRefID == bookingrefid).ToList();
                        long bookingcostid = airbookingcost[0].BookingCostID;
                        var airbookingcostbreak = _ctx.tblAirBookingCostBreakup.Where(x => x.BookingCostID == bookingcostid).ToList();
                        _ctx.tblAirBookingCostBreakup.RemoveRange(airbookingcostbreak);
                        _ctx.SaveChanges();
                        _ctx.tblAirBookingCost.RemoveRange(airbookingcost);
                        _ctx.SaveChanges();

                        tblAirBookingCost _tblAirBookingCost = new tblAirBookingCost()
                        {
                            BookingRefID = bookingrefid,
                            TotalBaseNet = Convert.ToDecimal(model.Totalfaregroup.TotalBaseNet),
                            TotalTaxNet = Convert.ToDecimal(model.Totalfaregroup.TotalTaxNet),
                            TotalNet = Convert.ToDecimal(model.Totalfaregroup.PaidAmount),
                            NetCurrency = model.Totalfaregroup.NetCurrency,
                            MarkupTypeID = Convert.ToInt16(model.Totalfaregroup.MarkupTypeID),
                            MarkupValue = Convert.ToDouble(model.Totalfaregroup.MarkupValue),
                            MarkupCurrency = model.Totalfaregroup.MarkupCurrency,
                            SellAmount = Convert.ToDecimal(model.Totalfaregroup.SellAmount),
                            CommessionType = (int)model.Totalfaregroup.CommissionType,
                            CommessionValue = (decimal)model.Totalfaregroup.CommissionValue,
                            SellCurrency = model.Totalfaregroup.SellCurrency,
                            AdditionalServiceFee = Convert.ToDecimal(model.Totalfaregroup.AdditionalServiceFee),
                            CancellationAmount = Convert.ToDecimal(model.Totalfaregroup.CancellationAmount),
                            CancellationCurrency = model.Totalfaregroup.CancellationCurrency,
                            ExchangeCurrency = model.BookFlightEntity.BookFlight.SupplierAgencyDetails[0].ToCurrency,
                            RateofExchange = model.BookFlightEntity.BookFlight.SupplierAgencyDetails[0].ToROEValue
                        };
                        _ctx.tblAirBookingCost.Add(_tblAirBookingCost);
                        _ctx.SaveChanges();

                        List<tblAirBookingCostBreakup> tblAirBookingCostBreakupList = new List<tblAirBookingCostBreakup>();
                        foreach (var item in model.CostBreakuppax)
                        {
                            tblAirBookingCostBreakupList.Add(new tblAirBookingCostBreakup()
                            {
                                BookingCostID = _tblAirBookingCost.BookingCostID,
                                BaseNet = Convert.ToDecimal(item.PaxtotalBaseNet),
                                TaxNet = Convert.ToDecimal(item.PaxtotalTaxNet),
                                TotalNet = Convert.ToDecimal(item.PaxpaidAmount),
                                NetCurrency = item.PaxnetCurrency,
                                MarkupTypeID = Convert.ToInt16(item.PaxmarkupTypeID),
                                MarkupValue = double.Parse(item.PaxmarkupValue),
                                MarkupCurrency = item.PaxmarkupCurrency,
                                SellAmount = Convert.ToDecimal(item.PaxsellAmount),
                                SellCurrency = item.PaxsellCurrency,
                                AdditionalServiceFee = Convert.ToDecimal(item.PaxadditionalServiceFee),
                                PaxType = item.PaxType,
                                NoOfPAx = Convert.ToInt16(item.TotalPaxQuantity)
                            });
                        }
                        _ctx.tblAirBookingCostBreakup.AddRange(tblAirBookingCostBreakupList);
                        _ctx.SaveChanges();

                        scope.Complete();

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        public async Task<BookingData> SavingAirBookingFlight(BusinessEntitties.BookFlightModel message, long agencyId, long supplierId)
        {
            long bookingid = 0; long userid = 0;
            BookingData _BookingData = new BookingData();
            try
            {
                using (var _ctx = new MediationEntities())
                {
                    var exsitUser = _ctx.tblUsers.FirstOrDefault(x => x.EmailID == message.BookFlightEntity.BookFlight.Email);
                    using (var scope = new TransactionScope())
                    {
                        if (exsitUser == null)
                        {
                            tblUser user = new tblUser()
                            {
                                EmailID = message.BookFlightEntity.BookFlight.Email,
                                CreatedBy = 1,
                                Mobile = message.BookFlightEntity.BookFlight.PhoneNumber,
                                AgencyID = agencyId,
                                LoginStatus = 1,
                                AdminStatus = 0
                            };
                            _ctx.tblUsers.Add(user);
                            _ctx.SaveChanges();
                            userid = user.UserID;
                        }
                        else
                        {
                            userid = exsitUser.UserID;

                        }
                        tblBooking booking = new tblBooking()
                        {
                            BookingID = 1,
                            Position = 1,
                            UserID = userid,
                            ServiceTypeId = 1,
                            BookingDate = DateTime.Now,
                            SupplierID = supplierId,
                            SupplierBookingReference = "",
                            CancellationDeadline = DateTime.Now,
                            BookingStatusCode = "RQ",
                            AgencyRemarks = "",
                            PaymentStatusID = 1,
                        };
                        _ctx.tblBookings.Add(booking);
                        _ctx.SaveChanges();
                        booking.BookingID = booking.BookingRefID;
                        _ctx.Entry(booking).State = EntityState.Modified;
                        _ctx.SaveChanges();
                        bookingid = booking.BookingRefID;

                        tblBookingHistory bookinghistory = new tblBookingHistory()
                        {
                            BookingRefID = Convert.ToInt64(bookingid.ToString()),
                            BookingStatusCode = "RQ",
                            UserID = userid,
                            ActionDate = DateTime.Now
                        };
                        _ctx.tblBookingHistorys.Add(bookinghistory);
                        _ctx.SaveChanges();

                        tblPayment _tblPayment = new tblPayment()
                        {
                            BookingRefID = bookingid,
                            PaidAmount = Convert.ToDecimal(message.Totalfaregroup.PaidAmount),
                            CurrencyCode = message.Totalfaregroup.SellCurrency,
                            PaidDate = Convert.ToDateTime(message.Totalfaregroup.PaidDate),
                            PaymentTypeID = Convert.ToInt16(message.Totalfaregroup.PaymentTypeID)
                        };
                        _ctx.tblPayment.Add(_tblPayment);
                        _ctx.SaveChanges();

                        List<tblAirFarerules> tblAirFareruleslist = new List<tblAirFarerules>();
                        foreach (var items in message.Fareruleseg)
                        {
                            tblAirFareruleslist.Add(new tblAirFarerules()
                            {
                                BookingRefID = bookingid,
                                FareRule = items.FareRule,
                                Segment = items.Segment,
                                FareRef = items.FareRef,
                                FilingAirline = items.FilingAirline,
                                MarketingAirline = items.MarketingAirline
                            });
                        }
                        _ctx.tblAirFarerules.AddRange(tblAirFareruleslist);
                        _ctx.SaveChanges();

                        tblAirBookingCost _tblAirBookingCost = new tblAirBookingCost()
                        {
                            BookingRefID = bookingid,
                            TotalBaseNet = Convert.ToDecimal(message.Totalfaregroup.TotalBaseNet),
                            TotalTaxNet = Convert.ToDecimal(message.Totalfaregroup.TotalTaxNet),
                            TotalNet = Convert.ToDecimal(message.Totalfaregroup.PaidAmount),
                            NetCurrency = message.Totalfaregroup.NetCurrency,
                            MarkupTypeID = Convert.ToInt16(message.Totalfaregroup.MarkupTypeID),
                            MarkupValue = Convert.ToDouble(message.Totalfaregroup.MarkupValue),
                            MarkupCurrency = message.Totalfaregroup.MarkupCurrency,
                            SellAmount = Convert.ToDecimal(message.Totalfaregroup.SellAmount),
                            CommessionType = (int)message.Totalfaregroup.CommissionType,
                            CommessionValue = (decimal)message.Totalfaregroup.CommissionValue,
                            SellCurrency = message.Totalfaregroup.SellCurrency,
                            AdditionalServiceFee = Convert.ToDecimal(message.Totalfaregroup.AdditionalServiceFee),
                            CancellationAmount = Convert.ToDecimal(message.Totalfaregroup.CancellationAmount),
                            CancellationCurrency = message.Totalfaregroup.CancellationCurrency,
                            ExchangeCurrency = message.BookFlightEntity.BookFlight.SupplierAgencyDetails[0].ToCurrency,
                            RateofExchange = message.BookFlightEntity.BookFlight.SupplierAgencyDetails[0].ToROEValue,
                        };
                        _ctx.tblAirBookingCost.Add(_tblAirBookingCost);
                        _ctx.SaveChanges();
                        long bookingCostID = _tblAirBookingCost.BookingCostID;

                        List<tblAirBookingCostBreakup> tblAirBookingCostBreakupList = new List<tblAirBookingCostBreakup>();
                        foreach (var item in message.CostBreakuppax)
                        {
                            tblAirBookingCostBreakupList.Add(new tblAirBookingCostBreakup()
                            {
                                BookingCostID = bookingCostID,
                                BaseNet = Convert.ToDecimal(item.PaxtotalBaseNet),
                                TaxNet = Convert.ToDecimal(item.PaxtotalTaxNet),
                                TotalNet = Convert.ToDecimal(item.PaxpaidAmount),
                                NetCurrency = item.PaxnetCurrency,
                                MarkupTypeID = Convert.ToInt16(item.PaxmarkupTypeID),
                                MarkupValue = double.Parse(item.PaxmarkupValue),
                                MarkupCurrency = item.PaxmarkupCurrency,
                                SellAmount = Convert.ToDecimal(item.PaxsellAmount),
                                SellCurrency = item.PaxsellCurrency,
                                AdditionalServiceFee = Convert.ToDecimal(item.PaxadditionalServiceFee),
                                PaxType = item.PaxType,
                                NoOfPAx = Convert.ToInt16(item.TotalPaxQuantity)
                            });
                        }
                        _ctx.tblAirBookingCostBreakup.AddRange(tblAirBookingCostBreakupList);
                        _ctx.SaveChanges();

                        List<tblAirbaggageDetails> tblAirbaggageDetailsList = new List<tblAirbaggageDetails>();
                        foreach (var items in message.AirBagDetails)
                        {
                            tblAirbaggageDetailsList.Add(new tblAirbaggageDetails()
                            {
                                BookingRefID = bookingid,
                                PAXType = items.PAXTyp,
                                CabinBaggageQuantity = Convert.ToInt16(items.CabinBaggageQuantity),
                                CabinBaggageUnit = items.CabinBaggageUnit,
                                CheckinBaggageQuantity = Convert.ToInt16(items.CheckinBaggageQuantity),
                                CheckinBaggageUnit = items.CheckinBaggageUnit,
                                FromSeg = items.fromSeg,
                                ToSeg = items.toseg
                            });
                        }
                        _ctx.tblAirbaggageDetails.AddRange(tblAirbaggageDetailsList);
                        _ctx.SaveChanges();

                        var SeqNumber = 0;
                        foreach (var items in message.BookFlightEntity.BookFlight.FLLegGroup)
                        {
                            tblAirOriginDestinationOption _tblAirOriginDestinationOption = new tblAirOriginDestinationOption()
                            {
                                BookingRefID = bookingid,
                                RefNumber = 0,
                                DirectionID = SeqNumber,
                                ElapsedTime = items.ElapsedTime
                            };
                            _ctx.tblAirOriginDestinationOption.Add(_tblAirOriginDestinationOption);
                            _ctx.SaveChanges();
                            foreach (var segitem in items.Segments)
                            {
                                string departureDateTimeAppHH = segitem.DepartureTime.Substring(0, 2);
                                string departureDateTimeAppMM = segitem.DepartureTime.Substring(2, 2);
                                string departureDateTimeApp = segitem.DepartureDate.Replace('-', '/') + " " + departureDateTimeAppHH + ":" + departureDateTimeAppMM + ":00";
                                string arrDateTimeAppHH = segitem.ArrivalTime.Substring(0, 2);
                                string arrDateTimeAppMM = segitem.ArrivalTime.Substring(2, 2);
                                string arrDateTimeAppNew = segitem.ArrivalDate.Replace('-', '/') + " " + arrDateTimeAppHH + ":" + arrDateTimeAppMM + ":00";
                                tblAirSegment _tblAirSegment = new tblAirSegment()
                                {
                                    OriginDestinationID = _tblAirOriginDestinationOption.OriginDestinationID,
                                    DepartureDateTime = CommonFunctions.ConvertDateTimeFromString(departureDateTimeApp, "hi-IN"),// Convert.ToDateTime(departureDateTimeApp, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat),
                                    ArrivalDateTime = CommonFunctions.ConvertDateTimeFromString(arrDateTimeAppNew, "hi-IN"),// Convert.ToDateTime(arrDateTimeAppNew, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat),
                                    FlightNumber = segitem.FlightNumber,
                                    Status = 1,
                                    DepartureAirportLocationCode = segitem.DepartureFrom,
                                    DepartureAirportTerminal = segitem.TerminalFrom,
                                    ArrivalAirportLocationCode = segitem.DepartureTo,
                                    ArrivalAirportTerminal = segitem.TerminalTo,
                                    OperatingAirlineCode = segitem.OperatingCompany,
                                    EquipmentAirEquipType = segitem.Flightequip,
                                    MarketingAirlineCode = segitem.MarketingCompany
                                };
                                _ctx.tblAirSegment.Add(_tblAirSegment);
                                _ctx.SaveChanges();

                                Int64 segid = Convert.ToInt64(_tblAirSegment.SegmentID);
                                tblAirSegmentBookingAvail _tblAirSegmentBookingAvail = new tblAirSegmentBookingAvail()
                                {
                                    SegmentID = segid,
                                    ResBookDesigCode = "",
                                    AvailablePTC = "",
                                    ResBookDesigCabinCode = segitem.BookingClass,
                                    FareBasis = segitem.FareBasis
                                };
                                _ctx._tblAirSegmentBookingAvail.Add(_tblAirSegmentBookingAvail);
                                _ctx.SaveChanges();
                            }
                            ++SeqNumber;
                        }

                        AddAirPassengers(_ctx, message.BookFlightEntity.BookFlight.TravelerInfo, bookingid, message.BookFlightEntity.BookFlight.CustomerInfo.Email, message.BookFlightEntity.BookFlight.CustomerInfo.PhoneNumber, message.BookFlightEntity.BookFlight.CustomerInfo.PhoneCountry, userid);

                        //List<tblAirbookingcostMarkUp> tblAirbookingcostMarkUpList = AddAdvancelevelMarkup(message.costAirMarkUp, bookingCostID, decimal.Parse(message.Totalfaregroup.PaidAmount));
                        //_ctx._tblAirbookingcostMarkUp.AddRange(tblAirbookingcostMarkUpList);
                        //_ctx.SaveChanges();

                        scope.Complete();
                    }
                    _BookingData.BookingRefID = Convert.ToInt64(bookingid);
                    _BookingData.userID = Convert.ToInt64(exsitUser.UserID);
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _BookingData;
        }
        private void AddAirPassengers(MediationEntities _ctx, List<Travelerinfo> message, long bookingId, string emailid, string telphone, string locationcode, long UserID)
        {
            bool isUpdated = true;
            var xDoc = GetXDocTravelInfoProcesses(message, bookingId, emailid, telphone, locationcode, UserID);
            const string sql = @"EXEC spAddAirPassengersupdatedNew @xmlData";

            var result = _ctx.Database
                .SqlQuery<SpBaseEntity>(
                          sql
                        , new SqlParameter("xmlData", new SqlXml(xDoc.Root.CreateReader()))

                ).FirstOrDefault();

            if (result.StatusCode != "OK-200")
            {
                isUpdated = false;
            }
        }
        private static XDocument GetXDocTravelInfoProcesses(List<Travelerinfo> list, long bookingId, string emailid, string telphone, string locationcode, long UserID)
        {
            return new XDocument(
                new XElement("EVENTS",
                    from xevent in list
                    select new XElement("EVENT"
                            , new XElement("GivenName", xevent.GivenName)
                            , new XElement("BookingRefID", bookingId.ToString())
                            , new XElement("TitleID", GetTitleID(xevent.NamePrefix))
                            , new XElement("PaxTypeID", Getpaxtype(xevent.PassengerTypeQuantity))
                            , new XElement("Surname", xevent.Surname)
                            , new XElement("EmailID", emailid)
                            , new XElement("TelPhoneType", telphone)
                            , new XElement("LocationCode", locationcode)
                            , new XElement("DateofBirth", DateTime.ParseExact(xevent.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                            , new XElement("PNR", "")
                            , new XElement("ETicketNo", "")
                            , new XElement("UserID", UserID.ToString())
                            , new XElement("DocTypeID", getDocId(xevent._DocType))
                            , new XElement("DocumentNumber", xevent._DocID)
                            , new XElement("ExpiryDate", GetDate(xevent._ExpireDate))
                            , new XElement("IssueLocation", "")
                            , new XElement("IssueCountry", xevent._DocIssueCountry)
                            , new XElement("AirLineCode", xevent.FFAirlineCode)
                            , new XElement("FFNumber", xevent.FrequentFlyerNumber)
                        )));
        }
        public void UpdatePNRandStatus(BookFlightResponse response, BookingData bookingdata, BusinessEntitties.BookFlightModel model, string supplierCode)
        {
            using (var _ctx = new MediationEntities())
            {
                using (var scope = new TransactionScope())
                {
                    string bookingstatusCode = response.BookFlightResult.Status;
                    tblBookingHistory bookinghistory = new tblBookingHistory()
                    {
                        BookingRefID = bookingdata.BookingRefID,
                        BookingStatusCode = bookingstatusCode,
                        UserID = bookingdata.userID,
                        ActionDate = DateTime.Now
                    };
                    _ctx.tblBookingHistorys.Add(bookinghistory);
                    _ctx.SaveChanges();

                    string canceldedline = DateTime.Now.ToString();
                    if (response.BookFlightResult.TktTimeLimit != "")
                    {
                        canceldedline = DateTime.ParseExact(response.BookFlightResult.TktTimeLimit, "dd-MM-yyyy HH:mm", CultureInfo.CreateSpecificCulture("nl-NL")).ToString("MM/dd/yyyy HH:mm");
                    }

                    tblBooking _tblBooking = new tblBooking();
                    _tblBooking.BookingRefID = bookingdata.BookingRefID;
                    _tblBooking.BookingStatusCode = bookingstatusCode;
                    _tblBooking.SupplierBookingReference = response.BookFlightResult.Airlinepnr;
                    _tblBooking.CancellationDeadline = DateTime.Parse(canceldedline);
                    _ctx.tblBookings.Attach(_tblBooking);
                    _ctx.Entry(_tblBooking).Property(x => x.BookingStatusCode).IsModified = true;
                    _ctx.Entry(_tblBooking).Property(x => x.SupplierBookingReference).IsModified = true;
                    _ctx.Entry(_tblBooking).Property(x => x.CancellationDeadline).IsModified = true;
                    _ctx.SaveChanges();

                    tblAirBookingData _tblAirBookingData = new tblAirBookingData()
                    {
                        BookingRefID = bookingdata.BookingRefID,
                        PNR = response.BookFlightResult.Airlinepnr,
                        UniqueID = response.BookFlightResult.UniqueID
                    };
                    _ctx._tblAirBookingData.Add(_tblAirBookingData);
                    _ctx.SaveChanges();

                    var airpassengers = (from ap in _ctx.tblAirPassengers
                                         where ap.BookingRefID == bookingdata.BookingRefID
                                         select new
                                         {
                                             PaxID = ap.PaxID
                                         }).ToList();

                    foreach (var item in airpassengers)
                    {
                        tblAirPassengers _tblAirPassengers = new tblAirPassengers()
                        {
                            PaxID = item.PaxID,
                            PNR = response.BookFlightResult.Airlinepnr,
                        };
                        _ctx.tblAirPassengers.Attach(_tblAirPassengers);
                        _ctx.Entry(_tblAirPassengers).Property(x => x.PNR).IsModified = true;
                        _ctx.SaveChanges();
                    }

                    if (response.BookFlightResult.Errors != null)
                    {
                        List<tblAirBookingError> tblAirBookingErrorList = new List<tblAirBookingError>();
                        foreach (var item in response.BookFlightResult.Errors)
                        {
                            tblAirBookingErrorList.Add(new tblAirBookingError()
                            {
                                BookingRefID = bookingdata.BookingRefID,
                                ErrorCode = item.Code,
                                ErrorMessage = item.Message
                            });
                        }
                        _ctx._tblAirBookingError.AddRange(tblAirBookingErrorList);
                        _ctx.SaveChanges();
                    }
                    scope.Complete();
                }
            }
        }
    }
}
