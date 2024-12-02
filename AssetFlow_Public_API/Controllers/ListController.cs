using FAMS_Models;
using FAMS_Models.Utilities;
using FAMS_Public_API.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FAMS_Public_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomAuthorize]
    public class ListController : ControllerBase
    {
        private const string API_URL = "list/";

        [NonAction]
        private Dictionary<string, object> GetDefaultParameter(string q="", int page =0, bool init = false)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("q", q);
            parameters.Add("page", page.ToString());
            parameters.Add("init", init);
            return parameters;
        }



        #region Company
        [HttpGet]
        [Route("company_code")]
        public object CompanyByCode(string q = "", int page = 0, bool init = false, string exclude_ids = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("exclude_ids", exclude_ids);

                return UUtils.CallMiddlewareListAPI(API_URL + $"company_code", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("company")]
        public object Company(string q = "", int page = 0, bool init = false, string exclude_ids = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("exclude_ids", exclude_ids);
                return UUtils.CallMiddlewareListAPI(API_URL + $"company", userHeaders,parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region Business Segment
        [HttpGet]
        [Route("business_segment")]
        public object BusinessSegment(string q = "", int page = 0, bool init = false, string exclude_ids = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("exclude_ids", exclude_ids);
                return UUtils.CallMiddlewareListAPI(API_URL + $"business_segment", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("business_segment_code")]
        public object BusinessSegmentCode(string q = "", int page = 0, bool init = false, string exclude_ids = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("exclude_ids", exclude_ids);
                return UUtils.CallMiddlewareListAPI(API_URL + $"business_segment_code", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Location
        [HttpGet]
        [Route("location_code")]
        public object LocationCode(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"location_code", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("location_id")]
        public object Location(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"location_id", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Order Type
        [HttpGet]
        [Route("order_type")]
        public object OrderType(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"order_type", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("group")]
        public object Group(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"group?q={q}&page={page}&init={init}", userHeaders, parameters,"", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("order_type_code")]
        public object OrderTypeByCode(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"order_type_code?q={q}&page={page}&init={init}", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        [HttpGet]
        [Route("lookup")]
        public object Lookup(string q = "", int page = 0, bool init = false, string group = "", bool allow_all = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("group", group);
                parameters.Add("allow_all", allow_all);
                return UUtils.CallMiddlewareListAPI(API_URL + $"lookup", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        [Route("truck_type")]
        public object TruckType(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"truck_type", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("vehicle")]
        public object Vehicle(string q = "", int page = 0, bool init = false, string transporter_code = "", string exclude_ids = "", string location = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("transporter_code", transporter_code);
                parameters.Add("exclude_ids", exclude_ids);
                parameters.Add("location", location);
                return UUtils.CallMiddlewareListAPI(API_URL + $"vehicle", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("vendor")]
        public object Vendor(string q="",int page=0,bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"vendor?q={q}&page={page}&init={init}", userHeaders, parameters, "", "GET");
            }
            catch(Exception exc)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("driver")]
        public object Driver(string q = "", int page = 0, bool init = false, string transporter_code = "", string exclude_ids = "", string location = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("transporter_code", transporter_code);
                parameters.Add("exclude_ids", exclude_ids);
                parameters.Add("location", location);
                return UUtils.CallMiddlewareListAPI(API_URL + $"driver", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
       

        [HttpGet]
        [Route("warehouse")]
        public object Warehouse(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"warehouse", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        
        [HttpGet]
        [Route("release_status_all")]
        public object ReleaseStatusAll(string q="",int page=0,bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"release_status_all", userHeaders, parameters, "", "GET");
            }catch(Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        [Route("sales_organization")]
        public object SalesOrganization(string q = "", int page = 0, bool init = false, string company = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("company", company);
                return UUtils.CallMiddlewareListAPI(API_URL + $"sales_organization", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        [Route("sales_organization_all")]
        public object SalesOrganizationAll(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"sales_organization_all?q={q}&page={page}&init={init}", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("sales_organization_code")]
        public object SalesOrganizationCode(string q = "", int page = 0, bool init = false, string company = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("company", company);
                return UUtils.CallMiddlewareListAPI(API_URL + $"sales_organization_code", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("purchase_organization")]
        public object PurchaseOrganization(string q = "", int page = 0, bool init = false, string company = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("company", company);
                return UUtils.CallMiddlewareListAPI(API_URL + $"purchase_organization", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        [Route("transporter_freight")]
        public object TransporterFreight(TransporterFreightModel model)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                return UUtils.CallMiddlewareListAPI(API_URL + "transporter_freight", userHeaders, null, JsonConvert.SerializeObject(model));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("appointment_slot")]
        public object AppointmentSlot(RequestSlotModel model)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                return UUtils.CallMiddlewareListAPI(API_URL + "appointment_slot", userHeaders, null, JsonConvert.SerializeObject(model));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        

        [HttpGet]
        [Route("shipment_warehouse")]
        public object ShipmentWarehouse(string q = "", int page = 0, bool init = false, long shipment_id = 0)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("shipment_id", shipment_id);
                return UUtils.CallMiddlewareListAPI(API_URL + $"shipment_warehouse", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("shipment_business_segment")]
        public object ShipmentBusinessSegment(string q = "", int page = 0, bool init = false, long shipment_id = 0, string warehouse_code = "", long business_segment_id = 0, string appointment_date = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("shipment_id", shipment_id);
                parameters.Add("warehouse_code", warehouse_code);
                parameters.Add("appointment_date", appointment_date);
                parameters.Add("business_segment_id", business_segment_id);
                return UUtils.CallMiddlewareListAPI(API_URL + $"shipment_business_segment", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("shipment_progress_status_all")]
        public object ShipmentProgressStatusAll(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"shipment_progress_status_all", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("transporter_all")]
        public object TransporterAll(string q ="",int page = 0,bool init=false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"transporter_all", userHeaders, parameters, "", "GET");
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        
        [HttpGet]
        [Route("material_all")]
        public object MaterialAll(string q="",int page=0,bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"material_all",userHeaders, parameters, "", "GET");
            }catch(Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("material_code_v2")]
        public object MaaterialByCodeV2(string q = "", int page = 0, bool init = false, string company_code = "", string plant_code = "", string business_segment_code = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("company_code", company_code);
                parameters.Add("plant_code", plant_code);
                parameters.Add("business_segment_code", business_segment_code);
                return UUtils.CallMiddlewareListAPI(API_URL + $"material_code_v2", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("commodity_group")]
        public object CommodityGroupList(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"commodity_group", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("trans_type")]
        public object TransTypeList(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"trans_type", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("wb_trans_type")]
        public object WBTransTypeList(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"wb_trans_type?q={q}&page={page}&init={init}", userHeaders, parameters,"", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("menu")]
        public object MenuList(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"menu?q={q}&page={page}&init={init}", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("customer_code")]
        public object CustomerListCode(string q = "", int page = 0, bool init = false, string company_code = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("company_code", company_code);
                return UUtils.CallMiddlewareListAPI(API_URL + $"customer_code", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("vendor_code")]
        public object VendorListCode(string q = "", int page = 0, bool init = false, string company_code = "", string purchase_org = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("company_code", company_code);
                parameters.Add("purchase_org", purchase_org);
                return UUtils.CallMiddlewareListAPI(API_URL + $"vendor_code", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("incoterm")]
        public object IncotermList(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"incoterm", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("incoterm_code")]
        public object IncotermListCode(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"incoterm_code", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("transporter_code")]
        public object TransporterListCode(string q = "", int page = 0, bool init = false,string company_code = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("company_code", company_code);
                return UUtils.CallMiddlewareListAPI(API_URL + $"transporter_code", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        [Route("user_code")]
        public object UserListCode(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"user_code", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("available_shipments/{order_id}")]
        public object AvailableShipments(long order_id = 0)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                return UUtils.CallMiddlewareListAPI(API_URL + $"available_shipments/{order_id}", userHeaders, null, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        [Route("status")]
        public object StatusListGeneral(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"status", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("shipment_status")]
        public object ShipmentStatusList(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"shipment_status", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("packing_type")]
        public object PackingType(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"packing_type", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("department_code")]
        public object DepartmentCode(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"department_code", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("storage_location")]
        public object StorageLocation(string q = "", int page = 0, bool init = false, bool isInternal = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("isInternal", isInternal);
                return UUtils.CallMiddlewareListAPI(API_URL + $"storage_location", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("parentmaintid")]
        public object ParentId(string q = "", int page = 0, bool init = false, bool isMain = false, long id = 0, long location_id = 0)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("isMain", isMain);
                parameters.Add("id", id);
                parameters.Add("location_id", location_id);
                return UUtils.CallMiddlewareListAPI(API_URL + $"parentmaintid", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("company_id")]
        public object CompanyId(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"company_id", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        [Route("department_id")]
        public object DepartmentId(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"department_id", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("doc_category_code")]
        public object DocCategoryCode(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"doc_category_code", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("company_doc_id")]
        public object CompanyDocId(string q = "", int page = 0, bool init = false, bool is_only_document = false, long only_company_id = 0)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("only_company_id", only_company_id);
                parameters.Add("is_only_document", is_only_document);
                return UUtils.CallMiddlewareListAPI(API_URL + $"company_doc_id", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("packing_detail_id")]
        public object PackingDetailId(string q = "", int page = 0, bool init = false, long company = 0, long location = 0, long department = 0, string menu = "", long id_from = 0, long storage_id = 0)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("company", company);
                parameters.Add("location", location);
                parameters.Add("department", department);
                parameters.Add("menu", menu);
                parameters.Add("id_from", id_from);
                parameters.Add("storage_id", storage_id);
                return UUtils.CallMiddlewareListAPI(API_URL + $"packing_detail_id", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("packing_document")]
        public object PackingDocumentList(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"packing_document", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("packing")]
        public object PackingList(string q = "", int page = 0, bool init = false, string exclude_packings = "", long packing_id = 0)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("exclude_packings", exclude_packings);
                parameters.Add("packing_id", packing_id);
                return UUtils.CallMiddlewareListAPI(API_URL + $"packing", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        
        
        }

        [HttpGet]
        [Route("document")]
        public object DocumentList(string q = "", int page = 0, bool init = false, long packing_detail_id = 0, string exclude_documents = "", string exclude_packings = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("packing_detail_id", packing_detail_id);
                parameters.Add("exclude_documents", exclude_documents);
                parameters.Add("exclude_packings", exclude_packings);
                return UUtils.CallMiddlewareListAPI(API_URL + $"document", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        [Route("relocation_no")]
        public object RelocationNo(string q = "", int page = 0, bool init = false)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                return UUtils.CallMiddlewareListAPI(API_URL + $"relocation_no", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        [Route("remarks_document_movement")]
        public object RemarksDocumentMovement(string q = "", int page = 0, bool init = false, string report_type = "")
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("report_type", report_type);
                return UUtils.CallMiddlewareListAPI(API_URL + $"remarks_document_movement", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("destination")]
        public object Destination(string q = "", int page = 0, bool init = false, string transfer_type = "", long idStorageExclude = 0)
        {
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                Dictionary<string, object> parameters = GetDefaultParameter(q, page, init);
                parameters.Add("transfer_type", transfer_type);
                parameters.Add("idStorageExclude", idStorageExclude);
                return UUtils.CallMiddlewareListAPI(API_URL + $"destination", userHeaders, parameters, "", "GET");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
