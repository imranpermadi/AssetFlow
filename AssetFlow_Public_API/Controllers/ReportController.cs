using FAMS_Models;
using FAMS_Models.Utilities;
using FAMS_Public_API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace FAMS_Public_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomAuthorize]
    public class ReportController : ControllerBase
    {
        string controllerCheck = "Report";

   
        //[HttpPost]
        //[Route("generate_freight")]
        //public ResultData FreightReport(FreightCostReportModel model)
        //{
        //    var response = new ResultData();
        //    try
        //    {
        //        var userHeaders = HttpContext.GetMiddlewareAuth(Constants.FORM_MODE_VIEW, controllerCheck, "FreightCostReport");
        //        response = UUtils.CallMiddlewareAPI(
        //            "report/freight_report",
        //            userHeaders,
        //            JsonConvert.SerializeObject(model)
        //            );
        //    }catch(Exception exc)
        //    {

        //    }
        //    return response;
        //}

        //[HttpPost]
        //[Route("generate_order")]
        //public ResultData OrderReport(OrderReportModel model)
        //{
        //    var response = new ResultData();
        //    try
        //    {
        //        var userHeaders = HttpContext.GetMiddlewareAuth(Constants.FORM_MODE_VIEW, controllerCheck, "OrderReport");
        //        response = UUtils.CallMiddlewareAPI(
        //            "report/order_report",
        //            userHeaders,
        //            JsonConvert.SerializeObject(model)
        //            );
        //    }catch(Exception exc)
        //    {

        //    }
        //    return response;
        //}

        //[HttpPost]
        //[Route("generate_shipment")]
        //public ResultData ShipmentReport(ShipmentReportModel model)
        //{
        //    var response = new ResultData();
        //    try
        //    {
        //        var userHeaders = HttpContext.GetMiddlewareAuth(Constants.FORM_MODE_VIEW, controllerCheck, "ShipmentReport");
        //        response = UUtils.CallMiddlewareAPI(
        //            "report/shipment_report",
        //            userHeaders,
        //            JsonConvert.SerializeObject(model)
        //            );
        //    }catch(Exception exc)
        //    {

        //    }
        //    return response;
        //}

        //[HttpPost]
        //[Route("generate_relation_order")]
        //public ResultData RelationOrderReport(OrderReportModel model)
        //{
        //    var response = new ResultData();
        //    try
        //    {
        //        var userHeaders = HttpContext.GetMiddlewareAuth(Constants.FORM_MODE_VIEW, controllerCheck, "RelationOrderReport");
        //        response = UUtils.CallMiddlewareAPI(
        //            "report/relation_order_report",
        //            userHeaders,
        //            JsonConvert.SerializeObject(model)
        //            );
        //    }
        //    catch (Exception exc)
        //    {

        //    }
        //    return response;
        //}

        //[HttpPost]
        //[Route("generate_service_level")]
        //public ResultData ServiceLevelReport(ServiceLevelReportModel model)
        //{
        //    var response = new ResultData();
        //    try
        //    {
        //        var userHeaders = HttpContext.GetMiddlewareAuth(Constants.FORM_MODE_VIEW, controllerCheck, "ServiceLevelReport");
        //        response = UUtils.CallMiddlewareAPI(
        //            "report/service_level_report",
        //            userHeaders,
        //            JsonConvert.SerializeObject(model)
        //            );
        //    }
        //    catch (Exception exc)
        //    {

        //    }
        //    return response;
        //}

        //[HttpPost]
        //[Route("generate_summary_service_level")]
        //public ResultData SummaryServiceLevelReport(SummaryServiceLevelReportModel model)
        //{
        //    var response = new ResultData();
        //    try
        //    {
        //        var userHeaders = HttpContext.GetMiddlewareAuth(Constants.FORM_MODE_VIEW, controllerCheck, "SummaryServiceLevelReport");
        //        response = UUtils.CallMiddlewareAPI(
        //            "report/summary_service_level_report",
        //            userHeaders,
        //            JsonConvert.SerializeObject(model)
        //            );
        //    }
        //    catch (Exception exc)
        //    {

        //    }
        //    return response;
        //}

        //[HttpPost]
        //[Route("generate_transporter_Utilization")]
        //public ResultData TransporterUtilizationReport(TransporterUtilizationReportModel model)
        //{
        //    var response = new ResultData();
        //    try
        //    {
        //        var userHeaders = HttpContext.GetMiddlewareAuth();
        //        response = UUtils.CallMiddlewareAPI(
        //            "report/transporter_utilization_report",
        //            userHeaders,
        //            JsonConvert.SerializeObject(model)
        //            );
        //    }
        //    catch (Exception exc)
        //    {

        //    }
        //    return response;
        //}


        [HttpPost]
        [Route("generate_document_report")]
        public ResultData DocumentReport(ReportModel model)
        {
            var response = new ResultData();
                try
                {
                    var userHeaders = HttpContext.GetMiddlewareAuth();
                    response = UUtils.CallMiddlewareAPI(
                        "report/generate_document_report",
                        userHeaders,
                        JsonConvert.SerializeObject(model)
                        );
                }
                catch (Exception exc)
                {

                }
                return response;
        }

        [HttpPost]
        [Route("generate_movement_report")]
        public ResultData MovementReport(ReportModel model)
        {
            var response = new ResultData();
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                response = UUtils.CallMiddlewareAPI(
                    "report/generate_movement_report",
                    userHeaders,
                    JsonConvert.SerializeObject(model)
                    );
            }
            catch (Exception exc)
            {

            }
            return response;
        }

    }
}
