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
    public class GetTripDetails : IAsyncRequestHandler<GetTripDetailsModel, ResponseObject>
    {
        private readonly IPartnerClient partnerClient;
        private readonly ISupplierAgencyServices supplierAgencyServices;
        private readonly IBookingServices bookingServices;
        public GetTripDetails(ISupplierAgencyServices _supplierAgencyServices, IBookingServices _bookingServices)
        {
            this.supplierAgencyServices = _supplierAgencyServices;
            this.bookingServices = _bookingServices;
            var apiClient = new ApiClient();
            partnerClient = new PartnerClient(apiClient);
        }
        private async Task<GetTripDetailsModelRS> GetTraveldetails(List<Domain.GetTripDetailsModelRS> list, Models.GetTripDetailsModel model)
        {
            GetTripDetailsModelRS _GetTripDetailsModelRS = new GetTripDetailsModelRS();
            SupplierTripDetailsModel supplierTripDetailsModel = new SupplierTripDetailsModel();
            if (model.ConnectiontoDBreq.Issupplier == true)
            {
                var bookedSupplierInfo = bookingServices.GetBookedSupplierInfo(long.Parse(model.ConnectiontoDBreq.BookingRefID));
                if (bookedSupplierInfo != null)
                {
                    model.ConnectiontoDBreq.SupplierCodeDb = bookedSupplierInfo.SupplierCodeDb;
                    model.ConnectiontoDBreq.SupplierPnr = bookedSupplierInfo.SupplierPnr;
                    model.ConnectiontoDBreq.SupplierUniqueId = bookedSupplierInfo.SupplierUniqueId;
                }
                if (bookedSupplierInfo.BookingStatusCode == "HK" || bookedSupplierInfo.BookingStatusCode == "TP")
                {
                    var supplierAgencyDetails = supplierAgencyServices.GetSupplierRouteBySupplierCodeAndAgencyCode(model.ConnectiontoDBreq.AgencyCodeDb, model.ConnectiontoDBreq.SupplierCodeDb, "getDBtripdetails");
                    if (supplierAgencyDetails != null)
                    {
                        List<SupplierAgencyDetails> SupplierAgencyDetailsList = new List<SupplierAgencyDetails>() { supplierAgencyDetails };
                        supplierTripDetailsModel = new SupplierTripDetailsModel()
                        {
                            AgencyCode = model.ConnectiontoDBreq.AgencyCodeDb,
                            SupplierCode = model.ConnectiontoDBreq.SupplierCodeDb,
                            Target = model.ConnectiontoDBreq.Target,
                            UniqueID = model.ConnectiontoDBreq.SupplierUniqueId,
                            supplierAgencyDetails = SupplierAgencyDetailsList
                        };
                        string req = JsonConvert.SerializeObject(supplierTripDetailsModel);
                        var result = await partnerClient.GetSupplierTripDetails(supplierAgencyDetails.BaseUrl, supplierAgencyDetails.RequestUrl, supplierTripDetailsModel);
                        string requestStr = JsonConvert.SerializeObject(model);
                        string resposneStr = JsonConvert.SerializeObject(result);
                        string agencyCode = model.ConnectiontoDBreq.AgencyCodeDb;
                        await supplierAgencyServices.SaveLog("supplier-trip-details", agencyCode, requestStr, resposneStr);
                        if (result != null)
                        {
                            string strData = JsonConvert.SerializeObject(result);
                            Domain.SupplierTripDetailsResponse supplierTripDetailsResponse = new Domain.SupplierTripDetailsResponse();
                            try
                            {
                                supplierTripDetailsResponse = JsonConvert.DeserializeObject<Domain.SupplierTripDetailsResponse>(strData);
                                if (supplierTripDetailsResponse.data.success == true)
                                {
                                    bookingServices.UpdateBookingInformatiomFromTripDetails(supplierTripDetailsResponse, long.Parse(model.ConnectiontoDBreq.BookingRefID));
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }

                           
                        }
                    }
                }
            }
            _GetTripDetailsModelRS = await supplierAgencyServices.GetTravellerDetailsfromDB(model.ConnectiontoDBreq.BookingRefID.ToString());
            return _GetTripDetailsModelRS;
        }
        public async Task<ResponseObject> Handle(GetTripDetailsModel message)
        {
            List<Domain.GetTripDetailsModelRS> getTripDetailsModelRS = new List<Domain.GetTripDetailsModelRS>();
            GetTripDetailsModelRS _GetTripDetailsModelRS = new GetTripDetailsModelRS();
            _GetTripDetailsModelRS = await GetTraveldetails(getTripDetailsModelRS, message);

            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = _GetTripDetailsModelRS,
                Message = "Data retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }
    }
}