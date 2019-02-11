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
    public class SupplierTripDetails : IAsyncRequestHandler<Models.SupplierTripDetailsModel, ResponseObject>
    {
        private ISupplierAgencyServices supplierAgencyServices;
        private readonly IPartnerClient partnerClient;
        public SupplierTripDetails(ISupplierAgencyServices _supplierAgencyServices)
        {
            this.supplierAgencyServices = _supplierAgencyServices;
            var apiClient = new ApiClient();
            partnerClient = new PartnerClient(apiClient);
        }
        public async Task<ResponseObject> Handle(SupplierTripDetailsModel message)
        {
            List<SupplierTripDetailsResponse> suppliertropdetails = new List<SupplierTripDetailsResponse>();
            bool mystiflyResponse = await GetSupplierTripDetails(suppliertropdetails, message);
            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = suppliertropdetails,
                Message = "Data retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }
        private async Task<bool> GetSupplierTripDetails(List<SupplierTripDetailsResponse> list, SupplierTripDetailsModel model)
        {
            var supplierAgencyDetails = supplierAgencyServices.GetSupplierRouteBySupplierCodeAndAgencyCode(model.AgencyCode, model.SupplierCode, "supplier-Tripdetails");
            if (supplierAgencyDetails != null)
            {
                List<SupplierAgencyDetails> SupplierAgencyDetailsList = new List<SupplierAgencyDetails>() { supplierAgencyDetails };
                model.supplierAgencyDetails = SupplierAgencyDetailsList;
                string req = JsonConvert.SerializeObject(model);
                var result = await partnerClient.GetSupplierTripDetails(supplierAgencyDetails.BaseUrl, supplierAgencyDetails.RequestUrl, model);
                string requestStr = JsonConvert.SerializeObject(model);
                string resposneStr = JsonConvert.SerializeObject(result);
                string agencyCode = model.AgencyCode;
                await supplierAgencyServices.SaveLog("supplier-trip-details", agencyCode, requestStr, resposneStr);
                if (result != null)
                {
                    string strData = JsonConvert.SerializeObject(result.Data);
                    SupplierTripDetailsResponse partnerResponseEntity = null;
                    partnerResponseEntity = JsonConvert.DeserializeObject<SupplierTripDetailsResponse>(strData);
                    if (partnerResponseEntity != null)
                    {
                        list.Add(partnerResponseEntity);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}