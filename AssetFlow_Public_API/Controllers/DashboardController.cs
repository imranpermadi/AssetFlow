using FAMS_Models;
using FAMS_Models.Utilities;
using FAMS_Public_API.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using FAMS_Models.Resources;

namespace FAMS_Public_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomAuthorize]
    public class DashboardController : ControllerBase
    {
        public string url = "dashboard";

        //[Route("list")]
        //[HttpPost]
        //public ResultData List(IndexParams model = null)
        //{
        //    var result = new ResultData();

        //    try
        //    {
        //        //https://localhost:44332/
        //        var userHeaders = HttpContext.GetMiddlewareAuth();
        //        result = UUtils.CallMiddlewareAPI($"{url}/list", userHeaders, JsonConvert.SerializeObject(model));
        //        //result.data = JsonConvert.DeserializeObject<List<CompanyModel>>(result.data.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        result.success = false;
        //        result.message = ex.Message;
        //    }

        //    return result;
        //}

        //[HttpGet]
        //[Route("completed")]
        //public ResultData CompletedChart()
        //{
        //    var result = new ResultData();

        //    try
        //    {
        //        var userHeaders = HttpContext.GetMiddlewareAuth(Constants.FORM_MODE_VIEW);
        //        result = UUtils.CallMiddlewareAPI("dashboard/completed" , userHeaders, "", "GET");
        //    }
        //    catch (Exception ex)
        //    {
        //        result.success = false;
        //        result.message = Resources.INTERNAL_ERROR;
        //    }

        //    return result;
        //}

        //[HttpGet]
        //[Route("status_count")]
        //public ResultData StatusCountChart()
        //{
        //    var result = new ResultData();

        //    try
        //    {
        //        var userHeaders = HttpContext.GetMiddlewareAuth(Constants.FORM_MODE_VIEW);
        //        result = UUtils.CallMiddlewareAPI("dashboard/status_count", userHeaders, "", "GET");
        //    }
        //    catch (Exception ex)
        //    {
        //        result.success = false;
        //        result.message = Resources.INTERNAL_ERROR;
        //    }

        //    return result;
        //}

       

    }
}
