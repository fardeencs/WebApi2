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
    public class BookFlight : IAsyncRequestHandler<Models.BookFlightModel, ResponseObject>
    {

        private readonly IPartnerClient partnerClient;
        private readonly ISupplierAgencyServices supplierAgencyServices;
        private readonly IBookingServices bookingServices;

        public BookFlight(ISupplierAgencyServices _supplierAgencyServices, IBookingServices _bookingServices)
        {
            this.supplierAgencyServices = _supplierAgencyServices;
            this.bookingServices = _bookingServices;
            var apiClient = new ApiClient();
            partnerClient = new PartnerClient(apiClient);
        }


        public async Task<ResponseObject> Handle(Models.BookFlightModel message)
        {
            List<Domain.BookFlightResponse> allsupplierData = new List<Domain.BookFlightResponse>();

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


        private async Task<bool> GetDataFromMystifly(List<Domain.BookFlightResponse> list, Models.BookFlightModel model)
        {
            try
            {
                var supplierAgencyDetails = supplierAgencyServices.GetSupplierRouteBySupplierCodeAndAgencyCode(model.BookFlightEntity.BookFlight.AgencyCode

               , model.BookFlightEntity.BookFlight.SupplierCode, "book/flights");
                List<SupplierAgencyDetails> _supplierAgencyDetailslist = new List<SupplierAgencyDetails> { supplierAgencyDetails };
                model.BookFlightEntity.BookFlight.SupplierAgencyDetails = _supplierAgencyDetailslist;
                Paymentinfo paymentinfo = bookingServices.GetPaymentCardDetails(model.BookFlightEntity.BookFlight.AgencyCode);
                model.BookFlightEntity.BookFlight.PaymentInfo = paymentinfo;


                BookingData _BookingData = new BookingData();
                BusinessEntitties.BookFlightModel bookFlightModel = new BusinessEntitties.BookFlightModel();
                bookFlightModel.AirBagDetails = model.AirBagDetails;
                bookFlightModel.CostBreakuppax = model.CostBreakuppax;
                bookFlightModel.Fareruleseg = model.Fareruleseg;
                bookFlightModel.Totalfaregroup = model.Totalfaregroup;
                bookFlightModel.BookFlightEntity.BookFlight = model.BookFlightEntity.BookFlight;
                bookFlightModel.costAirMarkUp = model.costAirMarkUp;

                //Check RefID Exist in Database
                bool Exist = false;
                if (model.BookFlightEntity.BookFlight.BookingId != "")
                {
                    Exist = bookingServices.CheckBookingRefIDExist(long.Parse(model.BookFlightEntity.BookFlight.BookingId));
                }
                if (Exist)
                {
                    //Update DataBase With New Price Details and Change Password
                    await bookingServices.UpdateAllDetailsWithRefID(bookFlightModel);
                }
                else
                {
                    // Add New Record in Database
                    _BookingData = await bookingServices.SavingAirBookingFlight(bookFlightModel, supplierAgencyDetails.AgencyID, supplierAgencyDetails.SupplierId);
                }

                //Send Booking Request To Supplier
                string modelStr = JsonConvert.SerializeObject(model.BookFlightEntity);
                var result = await partnerClient.GetBookflight(supplierAgencyDetails.BaseUrl, supplierAgencyDetails.RequestUrl, model.BookFlightEntity);
                string responseStr = JsonConvert.SerializeObject(result.Data);
                string jsonData = JsonConvert.SerializeObject(result.Data);
                string requestStr = JsonConvert.SerializeObject(model);
                string agencyCode = model.BookFlightEntity.BookFlight.AgencyCode;
                await supplierAgencyServices.SaveLog("book-Flight", agencyCode, requestStr, jsonData);
                if (jsonData != "null")
                {
                    Domain.BookFlightResponse partnerResponseEntity = JsonConvert.DeserializeObject<Domain.BookFlightResponse>(responseStr);
                    string bookStatus = partnerResponseEntity.BookFlightResult.Status;
                    if (bookStatus == "PRICECHANGED")
                    {
                        //Send Status to website with new bookingRefID
                        partnerResponseEntity.BookFlightResult.BookingId = _BookingData.BookingRefID.ToString();
                        list.Add(partnerResponseEntity);
                        return true;
                    }
                    else
                    {
                        //Check PNR is Successfully Generated
                        bool pnrstatus = CheckPNRorUniqIDexistornot(partnerResponseEntity);
                        if (pnrstatus)
                        {
                            //Update PNR,BookingStatus and UniqID
                            //Add Errors To Database
                            bookingServices.UpdatePNRandStatus(partnerResponseEntity, _BookingData, bookFlightModel, supplierAgencyDetails.SupplierCode);
                            partnerResponseEntity.BookFlightResult.BookingId = _BookingData.BookingRefID.ToString();
                            list.Add(partnerResponseEntity);
                        }
                        else
                        {
                            list.Add(partnerResponseEntity);
                        }
                    }
                }
                else
                {
                    //Send Error Message to website 
                    // Error message = Supplier Note Responding
                    Domain.BookFlightResponse bookFlightResponse = GetErrorTag("0000", "Supplier not responding");
                    list.Add(bookFlightResponse);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BookFlightResponse GetErrorTag(string errorcode, string errormessage)
        {
            Domain.Bookflightresult bookflightresult = new Bookflightresult();
            Domain.Error[] errors = new Error[1];
            Domain.Error error = new Error()
            {
                Code = errorcode,
                Message = errormessage
            };
            errors[0] = error;
            bookflightresult.Errors = errors;

            Domain.BookFlightResponse bookFlightResponse = new BookFlightResponse()
            {
                BookFlightResult = bookflightresult
            };
            bookFlightResponse.BookFlightResult.Success = "False";
            return bookFlightResponse;
        }
        public bool CheckPNRorUniqIDexistornot(BookFlightResponse resposne)
        {
            bool status = false;
            string airlinePNR = resposne.BookFlightResult.Airlinepnr;
            string uniqID = resposne.BookFlightResult.UniqueID;
            if (airlinePNR == "NA")
            {
                airlinePNR = "";
            }
            else if (airlinePNR == "NIL")
            {
                airlinePNR = "";
            }
            if (uniqID == "NA")
            {
                uniqID = "";
            }
            else if (uniqID == "NIL")
            {
                uniqID = "";
            }

            if (airlinePNR != "")
            {
                status = true;
            }
            else
            {
                if (uniqID != "")
                {
                    status = true;
                }
            }

            return status;
        }
    }
}