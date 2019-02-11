using BusinessEntitties;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Infrastructure.Handlers.Features.Mediation;
using WebApi.Models;
using WebApi.Models.Amadeous;
using WebApi.Models.Mystifly;
using WebApi.Models.Pyton;

namespace WebApi.Infrastructure.Client
{
    public interface IPartnerClient
    {
        Task<PartnerResponse> GetPartnerAllData();
        Task<ResponsePackage> GetPartnerData(string baseUri, string reqUri);

        Task<ResponsePackage> GetPytonData(string baseUri, string reqUri, SearchFlightsPyton model);
        Task<ResponsePackage> GetMystiflyData(string baseUri, string reqUri, SearchFlightsMystifly model);
        Task<ResponsePackage> GetAmadeusData(string baseUri, string reqUri, SearchFlightsAmadeous model);

        Task<ResponsePackage> GetPartnerData(string baseUri, string reqUri, Models.Rootobject model);

        Task<ResponsePackage> Getselectflight(string baseUri, string reqUri, SelectFlightModel model);

        Task<ResponsePackage> GetBookflight(string baseUri, string reqUri, BookFlightEntity model);

        Task<ResponsePackage> GetIssueTicketflight(string baseUri, string reqUri, Models.IssueTickettModel model);
        Task<ResponsePackage> GetCancelPNRStatus(string baseUri, string reqUri, Models.CancelBookingModel message);
        Task<ResponsePackage> GetSupplierTripDetails(string baseUri, string reqUri, Models.SupplierTripDetailsModel message);
    }
}
