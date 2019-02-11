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

namespace WebApi.Infrastructure.Handlers.Features.Mediation
{
    public class CancelBooking : IAsyncRequestHandler<Models.CancelBookingModel, ResponseObject>
    {
        private ISupplierAgencyServices supplierAgencyServices;
        private IBookingServices bookingServices;
        private readonly IPartnerClient partnerClient;
        public CancelBooking(ISupplierAgencyServices _supplierAgencyServices, IBookingServices _bookingServices)
        {
            this.supplierAgencyServices = _supplierAgencyServices;
            this.bookingServices = _bookingServices;
            var apiClient = new ApiClient();
            partnerClient = new PartnerClient(apiClient);
        }
        public async Task<ResponseObject> Handle(CancelBookingModel message)
        {
            List<CancelPNRResponse> cancelbookinpnrresponse = new List<CancelPNRResponse>();
            bool mystiflyResponse = await CancelbookingfromSupplier(cancelbookinpnrresponse, message);
            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = cancelbookinpnrresponse,
                Message = "Data retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }
        private async Task<bool> CancelbookingfromSupplier(List<CancelPNRResponse> list, Models.CancelBookingModel model)
        {
            var supplierAgencyDetails = supplierAgencyServices.GetSupplierRouteBySupplierCodeAndAgencyCode(model.AgencyCode, model.SupplierCode, "cancel-pnr");
            if (supplierAgencyDetails != null)
            {
                List<SupplierAgencyDetails> SupplierAgencyDetailsList = new List<SupplierAgencyDetails>() { supplierAgencyDetails };
                model.supplierAgencyDetails = SupplierAgencyDetailsList;
                string req = JsonConvert.SerializeObject(model);
                var result = await partnerClient.GetCancelPNRStatus(supplierAgencyDetails.BaseUrl, supplierAgencyDetails.RequestUrl, model);
                string requestStr = JsonConvert.SerializeObject(model);
                string resposneStr = JsonConvert.SerializeObject(result);
                string agencyCode = model.AgencyCode;
                await supplierAgencyServices.SaveLog("cancel-Flight", agencyCode, requestStr, resposneStr);
                if (result != null)
                {
                    string strData = JsonConvert.SerializeObject(result.Data);
                    Domain.CancelPNRResponse partnerResponseEntity = null;
                    partnerResponseEntity = JsonConvert.DeserializeObject<Domain.CancelPNRResponse>(strData);
                    partnerResponseEntity.BookingRefID = model.BookingRefID;
                    partnerResponseEntity.UserID = model.UserID;
                    if (partnerResponseEntity != null)
                    {
                        list.Add(partnerResponseEntity);
                        if (partnerResponseEntity.success == true)
                        {
                            await bookingServices.UpdateBookingStatusafterIssueTicket("XX", model.BookingRefID, model.UserID);
                        }
                        else
                        {
                            
                        }
                        return true;
                    }
                    else
                    {
                        list.Add(partnerResponseEntity);
                        return true;
                    }
                }
                else
                {
                    Error[] errors = new Error[1];
                    Error error = new Error()
                    {
                        Code = "",
                        Message = "Supplier not responding"
                    };
                    errors[0] = error;
                    CancelPNRResponse cancelPNRResponse = new CancelPNRResponse()
                    {
                        BookingRefID = model.BookingRefID,
                        success = false,
                        uniqueID = model.UniqueId,
                        UserID = model.UserID,
                        errors = errors
                    };
                    list.Add(cancelPNRResponse);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}