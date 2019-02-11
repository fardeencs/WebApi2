using Common;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Web.Core.Client;
using WebApi.Infrastructure.Client;
using Domain;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Logic.Interface;
using WebApi.Infrastructure.Common;
using System;
using WebApi.Models;
using BusinessEntitties;


#pragma warning disable 1998
#pragma warning disable 618

namespace WebApi.Infrastructure.Handlers.Features.Mediation
{
    public class IssueTicket : IAsyncRequestHandler<Models.IssueTickettModel, ResponseObject>
    {

        private const string BaseUrlMystyfly = "baseurlMystyfly";
        private const string FselecturlMystyfly = "FTicketurlMystyfly";
        private readonly IPartnerClient partnerClient;
        private readonly ISupplierAgencyServices supplierAgencyServices;
        private readonly IBookingServices bookingServices;
        public IssueTicket(ISupplierAgencyServices _supplierAgencyServices, IBookingServices _bookingServices)
        {
            this.supplierAgencyServices = _supplierAgencyServices;
            this.bookingServices = _bookingServices;
            var apiClient = new ApiClient();
            partnerClient = new PartnerClient(apiClient);
        }
        private async Task<bool> GetDataFromMystifly(List<Domain.IssueTicketEntity> list, Models.IssueTickettModel model)
        {
            var supplierAgencyDetails = supplierAgencyServices.GetSupplierRouteBySupplierCodeAndAgencyCode(model.ticketCreateTSTFromPricing.AgencyCode
               , model.ticketCreateTSTFromPricing.SupplierCode, "issueticket");
            List<SupplierAgencyDetails> SupplierAgencyDetailslist = new List<SupplierAgencyDetails>() { supplierAgencyDetails };
            model.SupplierAgencyDetails = SupplierAgencyDetailslist;
            //code to add supplier details in to request
            //var allSupplierBasicDetails = await supplierAgencyServices.GetSupplierAgencyBasicDetailswithsuppliercode(model.ticketCreateTSTFromPricing.AgencyCode, "T", model.ticketCreateTSTFromPricing.SupplierCode);
            // model.SupplierAgencyDetails = allSupplierBasicDetails;

            string req = JsonConvert.SerializeObject(model);
            var result = await partnerClient.GetIssueTicketflight(supplierAgencyDetails.BaseUrl, supplierAgencyDetails.RequestUrl, model);
            string strData = JsonConvert.SerializeObject(result.Data);
            string requestStr = JsonConvert.SerializeObject(model);
            string resposneStr = JsonConvert.SerializeObject(result);
            string agencyCode = model.ticketCreateTSTFromPricing.AgencyCode;
            await supplierAgencyServices.SaveLog("issue-Flight", agencyCode, requestStr, resposneStr);
            if (strData != "null")
            {
                Domain.IssueTicketEntity partnerResponseEntity = null;
                partnerResponseEntity = JsonConvert.DeserializeObject<Domain.IssueTicketEntity>(strData);

                if (partnerResponseEntity != null)
                {
                    if (partnerResponseEntity.TripDetailsResult.ItineraryInformation.Length > 0)
                    {
                        string bookingStatus = partnerResponseEntity.TripDetailsResult.BookingStatus;
                        //update details here strat     
                        await bookingServices.UpdateAirBookingAfterIssuedTicket(partnerResponseEntity.TripDetailsResult.ItineraryInformation,
                            long.Parse(model.ticketCreateTSTFromPricing.BookingRefID),
                            long.Parse(model.ticketCreateTSTFromPricing.UserID),
                            partnerResponseEntity.TripDetailsResult.ReservationItem[0].AirlinePNR,
                            bookingStatus);
                        //update details here end
                        partnerResponseEntity.TripDetailsResult.BookingId = model.ticketCreateTSTFromPricing.BookingRefID;
                        list.Add(partnerResponseEntity);
                    }
                    else
                    {
                        list.Add(partnerResponseEntity);
                    }

                    return true;
                }
            }
            else
            {
                Tkerror error = new Tkerror()
                {
                    Code = "0000",
                    Meassage = "Supplier not responding"
                };
                Tkerror[] errors = new Tkerror[] { error };
                Domain.IssueTicketEntity partnerResponseEntity = new IssueTicketEntity();
                Itineraryinformation[] Itineraryinformations = new Itineraryinformation[0];
                Triptotalfare triptotalfare = new Triptotalfare()
                {
                    Currency = "",
                    EquiFare = "",
                    Tax = "",
                    TotalFare = ""
                };
                Domain.Reservationitem[] reservationitems = new Domain.Reservationitem[0];
                Tripdetailsresult tripdetailsresult = new Tripdetailsresult()
                {
                    BookingId = model.ticketCreateTSTFromPricing.BookingRefID,
                    Success = "false",
                    Target = model.ticketCreateTSTFromPricing.Target,
                    UniqueID = model.ticketCreateTSTFromPricing.UniqueID,
                    ItineraryInformation = Itineraryinformations,
                    ReservationItem = reservationitems,
                    TripTotalFare = triptotalfare,
                    BookingStatus = "",
                    TicketStatus = "",
                    TKErrors = errors
                };
                partnerResponseEntity.TripDetailsResult = tripdetailsresult;
                partnerResponseEntity.TripDetailsResult.TKErrors = errors;
                list.Add(partnerResponseEntity);
                return true;
            }
            return false;
        }




        public async Task<ResponseObject> Handle(IssueTickettModel message)
        {
            List<Domain.IssueTicketEntity> allsupplierData = new List<Domain.IssueTicketEntity>();
            bool mystiflyResponse = await GetDataFromMystifly(allsupplierData, message);
            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = allsupplierData,
                Message = "Data retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }
    }
}