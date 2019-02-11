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
using Newtonsoft.Json;
using Logic.Interface;
using WebApi.Infrastructure.Common;
using System.Web.Script.Serialization;
using System;
using System.IO;
using System.Xml;
using BusinessEntitties;
using BusinessEntitties.Enumrations;


#pragma warning disable 1998
#pragma warning disable 618

namespace WebApi.Infrastructure.Handlers.Features.Mediation
{
    using WebApi.Models.Amadeous;
    using WebApi.Models.Mystifly;
    using WebApi.Models.Pyton;

    public class SearchFlight : IAsyncRequestHandler<Models.Rootobject, ResponseObject>
    {
        #region --private members--
        private const string BaseUrlMystyfly = "baseurlMystyfly";
        private const string BaseUrlPython = "baseurlPython";
        private const string BaseUrlAmadeous = "baseurlAmadeous";
        private const string amadeusjsonurl = "Amadeousjsonurl";


        private const string ReqUrlMystyfly = "requrlMystyfly";
        private const string ReqUrlPython = "requrlPython";
        private const string ReqUrlAmadeous = "requrlAmadeous";

        private const string SupplierCodeMystyfly = "supplierCodeMystyfly";
        private const string SupplierCodePython = "supplierCodePython";
        private const string SupplierCodeAmadeous = "supplierCodeAmadeous";
        //private string _SupplierCodeMystyfly = "";
        //private string _SupplierCodePython = "";
        //private string _SupplierCodeAmadeous = "";


        private readonly IPartnerClient partnerClient;
        private readonly ISupplierAgencyServices supplierAgencyServices;
        private readonly IAgenciesBasicDetails agenciesBasicDetails;

        #endregion

        #region -- comment code--
        //private readonly IApiClient apiClient;
        //public SearchFlight(IPartnerClient _partnerClient, IApiClient _apiClient)
        //{
        //    var apiClient = new ApiClient();
        //    partnerClient = new PartnerClient(apiClient);
        //    //apiClient = _apiClient;
        //    //partnerClient = _partnerClient;
        //}
        #endregion

        #region --constructor--
        public SearchFlight(ISupplierAgencyServices _supplierAgencyServices, IAgenciesBasicDetails _agenciesBasicDetails)
        {
            this.supplierAgencyServices = _supplierAgencyServices;
            this.agenciesBasicDetails = _agenciesBasicDetails;
            var apiClient = new ApiClient();
            partnerClient = new PartnerClient(apiClient);
            InitializeSupplierCode();
        }

        private void InitializeSupplierCode()
        {
            //_SupplierCodeMystyfly = ConficBase.GetConfigAppValue(SupplierCodeMystyfly);
            //_SupplierCodePython = ConficBase.GetConfigAppValue(SupplierCodePython);
            //_SupplierCodeAmadeous = ConficBase.GetConfigAppValue(SupplierCodeAmadeous);

        }

        #endregion

        #region --handler--
        public async Task<ResponseObject> Handle(Models.Rootobject message)
        {
            string key = GenrateSavedResponseKey(message);

            List<Domain.Rootobject> allsupplierData = new List<Domain.Rootobject>();
            if (SupplierCode.MIS001.ToString().ToUpper() == message.CommonRequestSearch.SupplierCode.Trim().ToUpper())
            {
                bool mystiflyResponse = await GetDataFromMystifly(allsupplierData, message, key);
            }
            else if (SupplierCode.PYT001.ToString().ToUpper() == message.CommonRequestSearch.SupplierCode.Trim().ToUpper())
            {
                bool pythonResponse = await GetDataFromPython(allsupplierData, message, key);
            }
            else if (SupplierCode.AMA001.ToString().ToUpper() == message.CommonRequestSearch.SupplierCode.Trim().ToUpper())
            {
                bool amadiousResponse = await GetDataFromAmadeus(allsupplierData, message, key);
            }
            else
            {
                #region sync reequest
                //bool pythonResponse = await GetDataFromPython(allsupplierData, message, key);
                //bool mystiflyResponse = await GetDataFromMystifly(allsupplierData, message, key);
                //bool amadiousResponse = await GetDataFromAmadeus(allsupplierData, message, key);
                #endregion

                #region -- async req--

                //CreateResponseTime("Amedious");
                Task<bool> amadiousResponse = GetDataFromAmadeus(allsupplierData, message, key);
                //CreateResponseTime("Python");
                Task<bool> pythonResponse = GetDataFromPython(allsupplierData, message, key);
                //CreateResponseTime("Mystifly");
                Task<bool> mystiflyResponse = GetDataFromMystifly(allsupplierData, message, key);

                await amadiousResponse;
                await pythonResponse;
                await mystiflyResponse;

                //int timeout = 20000;
                //amadiousResponse.Wait(timeout);
                //pythonResponse.Wait(timeout);
                //mystiflyResponse.Wait(timeout);

                #endregion
            }

            if (allsupplierData == null || allsupplierData.Count() <= 0) return null;

            Domain.Rootobject filteredData = FilteredData(allsupplierData, message.CommonRequestSearch.IsFiltered);

            //Models.TestData allData = new Models.TestData();
            //allData.NonFilteredData = allsupplierData;
            //allData.FilteredData = filteredData;

            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = filteredData,
                Message = "Data retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }

        #endregion

        #region --request to indivisual partners api's-- 
        private async Task<bool> GetDataFromMystifly(List<Domain.Rootobject> list, Models.Rootobject model, string key)
        {
            //bool isAdded = await AddSupplierDetailsToRequestObject(model, this._SupplierCodeMystyfly);
            // SupplierAgencyDetails supplierDetails = supplierAgencyServices.GetBasicDetailsOfMystifly(model.CommonRequestSearch.AgencyCode, "T");
            //SupplierAgencyDetails supplierDetails = agenciesBasicDetails.GetBasicDetailsOfMystifly(model.CommonRequestSearch.AgencyCode, "T");
            SupplierAgencyDetails supplierDetails = await agenciesBasicDetails.GeBasicDetailsOfSupplier(model.CommonRequestSearch.AgencyCode, SupplierCode.MIS001.ToString(), "T");
            if (supplierDetails != null)
            {
                supplierDetails.AgencyCode = model.CommonRequestSearch.AgencyCode;
                List<SupplierAgencyDetails> supplierAgencyDetails = new List<SupplierAgencyDetails>();
                supplierAgencyDetails.Add(supplierDetails);
                //model.SupplierAgencyDetails = supplierAgencyDetails;

                //string baseUri = model.SupplierAgencyDetails.FirstOrDefault().BaseUrl;
                SearchFlightsMystifly requestModel = new SearchFlightsMystifly();
                requestModel.cabin = model.cabin;
                requestModel.CommonRequestSearch = model.CommonRequestSearch;
                requestModel.Currency = model.Currency;
                requestModel.IsRefundable = model.IsRefundable;
                requestModel.Maxstopquantity = model.Maxstopquantity;
                requestModel.NonStop = model.NonStop;
                requestModel.OriginDestinationInformation = model.OriginDestinationInformation;
                requestModel.PassengerTypeQuantity = model.PassengerTypeQuantity;
                requestModel.PreferenceLevel = model.PreferenceLevel;
                requestModel.PreferredAirline = model.PreferredAirline;
                requestModel.PricingSource = model.PricingSource;
                requestModel.RequestOption = model.RequestOption;
                requestModel.Target = model.Target;
                requestModel.Triptype = model.Triptype;
                requestModel.SupplierAgencyDetails = supplierAgencyDetails;
                string baseUri = supplierDetails.BaseUrl;

                string strData = string.Empty;

                string reqUri = ConficBase.GetConfigAppValue(ReqUrlMystyfly);
                bool isFetchedFromDb = false;
                strData = await GetSupplierResponseFromDb(model, SupplierCode.MIS001.ToString(), key);
                string req = JsonConvert.SerializeObject(model);
                if (string.IsNullOrEmpty(strData))
                {
                    // var result = await partnerClient.GetMystiflyData(baseUri, reqUri, model);
                    var result = await partnerClient.GetMystiflyData(baseUri, reqUri, requestModel);
                    strData = JsonConvert.SerializeObject(result.Data);
                    isFetchedFromDb = true;
                }
                Domain.Rootobject partnerResponseEntity = JsonConvert.DeserializeObject<Domain.Rootobject>(strData);
                if (partnerResponseEntity != null)
                {
                    if (partnerResponseEntity.fareMasterPricerTravelBoardSearchReply.flightIndex != null
                                    && partnerResponseEntity.fareMasterPricerTravelBoardSearchReply.flightIndex.ToList().Count() > 0)
                    {
                        list.Add(partnerResponseEntity);

                        if (isFetchedFromDb)
                        {
                            AddSearchDetails(model, strData, ConficBase.GetConfigAppValue(SupplierCodeMystyfly), key);
                        }

                        return true;
                    }
                }
            }
            return false;
        }

        private async Task<bool> GetDataFromPython(List<Domain.Rootobject> list, Models.Rootobject model, string key)
        {
            string strData = string.Empty;
            //SupplierAgencyDetails supplierDetails = agenciesBasicDetails.GetBasicDetailsOfPyton(model.CommonRequestSearch.AgencyCode, "T");
            SupplierAgencyDetails supplierDetails = await agenciesBasicDetails.GeBasicDetailsOfSupplier(model.CommonRequestSearch.AgencyCode, SupplierCode.PYT001.ToString(), "T");
            if (supplierDetails != null)
            {
                supplierDetails.AgencyCode = model.CommonRequestSearch.AgencyCode;
                List<SupplierAgencyDetails> supplierAgencyDetails = new List<SupplierAgencyDetails>();
                supplierAgencyDetails.Add(supplierDetails);
                // model.SupplierAgencyDetails = supplierAgencyDetails;
                // string baseUri = model.SupplierAgencyDetails.FirstOrDefault().BaseUrl;

                SearchFlightsPyton requestModel = new SearchFlightsPyton();
                requestModel.cabin = model.cabin;
                requestModel.CommonRequestSearch = model.CommonRequestSearch;
                requestModel.Currency = model.Currency;
                requestModel.IsRefundable = model.IsRefundable;
                requestModel.Maxstopquantity = model.Maxstopquantity;
                requestModel.NonStop = model.NonStop;
                requestModel.OriginDestinationInformation = model.OriginDestinationInformation;
                requestModel.PassengerTypeQuantity = model.PassengerTypeQuantity;
                requestModel.PreferenceLevel = model.PreferenceLevel;
                requestModel.PreferredAirline = model.PreferredAirline;
                requestModel.PricingSource = model.PricingSource;
                requestModel.RequestOption = model.RequestOption;
                requestModel.Target = model.Target;
                requestModel.Triptype = model.Triptype;
                requestModel.SupplierAgencyDetails = supplierAgencyDetails;
                string baseUri = supplierDetails.BaseUrl;

                string reqUri = ConficBase.GetConfigAppValue(ReqUrlPython);

                bool isFetchedFromDb = false;
                strData = await GetSupplierResponseFromDb(model, SupplierCode.PYT001.ToString(), key);
                if (string.IsNullOrEmpty(strData))
                {
                    //  string strData1 = JsonConvert.SerializeObject(model);
                    var result = await partnerClient.GetPytonData(baseUri, reqUri, requestModel);
                    strData = JsonConvert.SerializeObject(result.Data);
                    isFetchedFromDb = true;
                }
                Domain.Rootobject partnerResponseEntity = JsonConvert.DeserializeObject<Domain.Rootobject>(strData);
                if (partnerResponseEntity != null)
                {
                    if (partnerResponseEntity.fareMasterPricerTravelBoardSearchReply.flightIndex != null && partnerResponseEntity.fareMasterPricerTravelBoardSearchReply.flightIndex.ToList().Count() > 0)
                    {
                        list.Add(partnerResponseEntity);
                        if (isFetchedFromDb)
                        {
                            AddSearchDetails(model, strData, ConficBase.GetConfigAppValue(SupplierCodePython), key);
                        }

                        return true;
                    }
                }
            }
            return false;
        }

        private async Task<bool> GetDataFromAmadeus(List<Domain.Rootobject> list, Models.Rootobject model, string key)
        {
            string strData = string.Empty;
            //bool isAdded = await AddSupplierDetailsToRequestObject(model, this._SupplierCodeAmadeous);
            try
            {
                SupplierAgencyDetails supplierDetails = await agenciesBasicDetails.GeBasicDetailsOfSupplier(model.CommonRequestSearch.AgencyCode, SupplierCode.AMA001.ToString(), "T");
                if (supplierDetails != null)
                {
                    supplierDetails.AgencyCode = model.CommonRequestSearch.AgencyCode;
                    List<SupplierAgencyDetails> supplierAgencyDetails = new List<SupplierAgencyDetails>();
                    supplierAgencyDetails.Add(supplierDetails);
                    // model.SupplierAgencyDetails = supplierAgencyDetails;


                    SearchFlightsAmadeous requestModel = new SearchFlightsAmadeous();
                    requestModel.cabin = model.cabin;
                    requestModel.CommonRequestSearch = model.CommonRequestSearch;
                    requestModel.Currency = model.Currency;
                    requestModel.IsRefundable = model.IsRefundable;
                    requestModel.Maxstopquantity = model.Maxstopquantity;
                    requestModel.NonStop = model.NonStop;
                    requestModel.OriginDestinationInformation = model.OriginDestinationInformation;
                    requestModel.PassengerTypeQuantity = model.PassengerTypeQuantity;
                    requestModel.PreferenceLevel = model.PreferenceLevel;
                    requestModel.PreferredAirline = model.PreferredAirline;
                    requestModel.PricingSource = model.PricingSource;
                    requestModel.RequestOption = model.RequestOption;
                    requestModel.Target = model.Target;
                    requestModel.Triptype = model.Triptype;
                    requestModel.SupplierAgencyDetails = supplierAgencyDetails;
                    string baseUri = supplierDetails.BaseUrl;

                    string reqUri = ConficBase.GetConfigAppValue(ReqUrlAmadeous);
                    bool isFetchedFromDb = false;
                    strData = await GetSupplierResponseFromDb(model, SupplierCode.AMA001.ToString(), key);
                    if (string.IsNullOrEmpty(strData))
                    {
                        // string jsonData = JsonConvert.SerializeObject(model);
                        var result = await partnerClient.GetAmadeusData(baseUri, reqUri, requestModel);
                        strData = JsonConvert.SerializeObject(result.Data);
                        isFetchedFromDb = true;
                    }
                    Domain.Rootobject partnerResponseEntity = JsonConvert.DeserializeObject<Domain.Rootobject>(strData);
                    if (partnerResponseEntity != null)
                    {
                        if (partnerResponseEntity.fareMasterPricerTravelBoardSearchReply.flightIndex != null
                                        && partnerResponseEntity.fareMasterPricerTravelBoardSearchReply.flightIndex.ToList().Count() > 0)
                        {
                            list.Add(partnerResponseEntity);
                            if (isFetchedFromDb)
                            {
                                AddSearchDetails(model, strData, ConficBase.GetConfigAppValue(SupplierCodeAmadeous), key);
                            }

                            return true;
                        }
                    }
                }
            }catch(Exception ex)
            {
                throw ex;
            }
            
            return false;
        }

        #endregion

        #region --add supplier details to request object--

        private async Task<bool> AddSupplierDetailsToRequestObject(Models.Rootobject model, string supplierCode)
        {
            bool isAdded = false;
            List<SupplierAgencyDetails> supplierAgencyDetails = new List<SupplierAgencyDetails>();
            SupplierAgencyDetails supplierDetails = await supplierAgencyServices.GetSupplierAgencyBasicDetails(model.CommonRequestSearch.AgencyCode, "T", supplierCode);
            supplierAgencyDetails.Add(supplierDetails);
            if (supplierDetails != null)
            {
                model.SupplierAgencyDetails = supplierAgencyDetails;
                isAdded = true;
            }
            return isAdded;
        }
        #endregion

        #region --manupulation by madiation after reciveing response from partners--
        private Domain.Rootobject FilteredData(List<Domain.Rootobject> allsupplierData, bool IsFiltered)
        {
            var allsupplieFlightIndexData = allsupplierData.SelectMany(x => x.fareMasterPricerTravelBoardSearchReply.flightIndex).ToList();

            Faremasterpricertravelboardsearchreply fistRooTObject = allsupplierData.First().fareMasterPricerTravelBoardSearchReply;
            Domain.Rootobject rootObject = new Domain.Rootobject();

            Faremasterpricertravelboardsearchreply faremasterpricertravelboardsearchreply = GetCommonRootObject(fistRooTObject);

            List<Flightindex> flightindexList = new List<Flightindex>();

            /*during debugg uncomment this code and varify your correct data*/
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string jsonString = serializer.Serialize(allsupplieFlightIndexData);
            if (IsFiltered)
            {
                var groupedFlightsByKey = allsupplieFlightIndexData
                                                        .GroupBy(item => item.SegmentRef.key)
                                                        .ToDictionary(grp => grp.Key, grp => grp.ToList());

                foreach (var item in groupedFlightsByKey)
                {
                    Flightindex flightindex = item.Value.OrderBy(x => x.fare.amount).FirstOrDefault();
                    flightindexList.Add(flightindex);
                }
            }
            else
            {
                flightindexList.AddRange(allsupplieFlightIndexData);
            }


            faremasterpricertravelboardsearchreply.flightIndex = flightindexList.OrderBy(x => x.fare.amount).ToList();

            rootObject.fareMasterPricerTravelBoardSearchReply = faremasterpricertravelboardsearchreply;

            return rootObject;

        }
        private static Faremasterpricertravelboardsearchreply GetCommonRootObject(Faremasterpricertravelboardsearchreply fistRooTObject)
        {
            Conversionrate conversionrate = new Conversionrate();
            Conversionratedetail conversionratedetail = new Conversionratedetail()
            {
                currency = fistRooTObject.conversionRate.conversionRateDetail.currency
            };
            conversionrate.conversionRateDetail = conversionratedetail;

            Replystatus replystatus = new Replystatus();
            Status status = new Status()
            {
                advisoryTypeInfo = fistRooTObject.replyStatus.status.advisoryTypeInfo
            };
            replystatus.status = status;

            Faremasterpricertravelboardsearchreply faremasterpricertravelboardsearchreply = new Faremasterpricertravelboardsearchreply();
            faremasterpricertravelboardsearchreply.conversionRate = conversionrate;
            faremasterpricertravelboardsearchreply.replyStatus = replystatus;
            return faremasterpricertravelboardsearchreply;
        }

        #endregion

        #region --get supplier response from db--
        private async Task<string> GetSupplierResponseFromDb(Models.Rootobject model, string supplierCode, string key)
        {
            AgencySupplierResponseEntity addSupplierDetails = new AgencySupplierResponseEntity();
            try
            {
                DateTime currentDateTime = DateTime.Now;
                addSupplierDetails.AgencyCode = model.CommonRequestSearch.AgencyCode;
                addSupplierDetails.SearchDateTime = currentDateTime;
                addSupplierDetails.Key = key;
                addSupplierDetails.Status = 1;
                addSupplierDetails.SupplierCode = supplierCode;
                string response = await supplierAgencyServices.GetAgencySupplierResponse(addSupplierDetails);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region --save supplier response in db--
        private void AddSearchDetails(Models.Rootobject model, string response, string supplierCode, string key)
        {
            List<AgencySupplierResponseEntity> addSupplierDetails = new List<AgencySupplierResponseEntity>();
            try
            {
                //DateTime depDate = DateTime.ParseExact(_.DepartureDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime currentDateTime = DateTime.Now;
                addSupplierDetails.Add(new AgencySupplierResponseEntity()
                {
                    AgencyCode = model.CommonRequestSearch.AgencyCode,
                    SearchDateTime = currentDateTime,
                    Key = key,
                    Search = response,
                    Status = 1,
                    SupplierCode = supplierCode
                });
                supplierAgencyServices.AddSupplierResponse(addSupplierDetails);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region --genrate response key--
        private static string GenrateSavedResponseKey(Models.Rootobject message)
        {
            string key = "";
            string seginf = "";
            string passinf = "";
            int count = message.OriginDestinationInformation.Count();

            for (int i = 0; i < count; i++)
            {
                seginf += message.OriginDestinationInformation[i].OriginLocation + "-" + message.OriginDestinationInformation[i].DestinationLocation + "-" + message.OriginDestinationInformation[i].DepartureDate + "-";
            }
            passinf = message.PassengerTypeQuantity.ADT + "-" + message.PassengerTypeQuantity.CHD + "-" + message.PassengerTypeQuantity.INF + "-" + message.cabin;

            key = seginf + passinf;
            return key;
        }
        #endregion

        #region--other code--
        public void CreateJSONDoc(string JSONDATA, string Agency, string Supplier, string key)
        {
            try
            {
                string date = DateTime.Now.ToString();
                string storePath = System.Web.HttpContext.Current.Server.MapPath("") + "/" + Agency + "/" + Supplier + "/SearchResponse/";
                if (!Directory.Exists(storePath))
                    Directory.CreateDirectory(storePath);
                string filename = key + ".json";
                string tempfilename = storePath + "/" + filename;
                File.WriteAllText(tempfilename, JSONDATA);
            }
            catch (Exception ex)
            {
                // Response.Write("XmlException: " + xmlEx.Message);
            }
        }

        public void CreateResponseTime(string partnerIdentity)
        {
            string content = "";
            try
            {
                string date = DateTime.Now.ToString();
                string storePath = System.Web.HttpContext.Current.Server.MapPath("") + "/ResponseTime/";
                if (!Directory.Exists(storePath))
                    Directory.CreateDirectory(storePath);
                string filename = "responsetime-" + partnerIdentity + ".txt";
                string tempfilename = storePath + "/" + filename;
                using (StreamWriter writetext = new StreamWriter(tempfilename))
                {
                    content = "=> " + date + "-" + partnerIdentity + Environment.NewLine;
                    writetext.WriteLine(content);
                }

                //  File.WriteAllText(tempfilename, content);
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

    }
}