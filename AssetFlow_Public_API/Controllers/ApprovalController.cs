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
using FAMS_Models.Resources;

namespace FAMS_Public_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomAuthorize]
    public class ApprovalController : ControllerBase
    {
        private string apiUrl = "approval/";

        [Route("list")]
        [HttpPost]
        public ResultData List(IndexParams model = null)
        {
            var result = new ResultData();

            try
            {
                //https://localhost:44332/
                var userHeaders = HttpContext.GetMiddlewareAuth();
                result = UUtils.CallMiddlewareAPI("approval/list", userHeaders, JsonConvert.SerializeObject(model));
                //result.data = JsonConvert.DeserializeObject<List<CompanyModel>>(result.data.ToString());
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Resources.INTERNAL_ERROR;
            }

            return result;
        }

        [HttpGet("{id}/{mode}")]
        public ResultData GetData(long id, string mode)
        {
            var result = new ResultData();

            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth(mode);
                result = UUtils.CallMiddlewareAPI("approval/" + id.ToString(), userHeaders, "", "GET");
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Resources.INTERNAL_ERROR;
            }

            return result;
        }

        [Route("status")]
        [HttpGet]
        public ResultData GetStatus()
        {
            var result = new ResultData();

            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth("Index");
                result = UUtils.CallMiddlewareAPI(apiUrl + "status", userHeaders, "", "GET");
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Resources.INTERNAL_ERROR;
            }

            return result;
        }


        [Route("approval_details/{id}/{approval_id}")]
        [HttpGet]
        public ResultData GetApprovalDetails(long id, long approval_id)
        {
            var result = new ResultData();

            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth("Index");
                result = UUtils.CallMiddlewareAPI(apiUrl + $"approval_details/{id}/{approval_id}", userHeaders, "", "GET");
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Resources.INTERNAL_ERROR;
            }

            return result;
        }

        [HttpPut]
        [Route("process")]
        public ResultData Process(ApprovalHelperModel model)
        {
            var result = new ResultData();

            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth("Index");
                result = UUtils.CallMiddlewareAPI(apiUrl + "process", userHeaders, JsonConvert.SerializeObject(model), "PUT");
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Resources.INTERNAL_ERROR;
            }

            return result;
        }


    }
}
