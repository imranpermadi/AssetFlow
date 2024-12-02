using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using FAMS_Models;
using FAMS_Models.Base;
using FAMS_Models.Utilities;
using FAMS_Public_API.Utilities;
using System;
using System.Collections.Generic;

namespace FAMS_Public_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomAuthorize]
    public class CertificateTypeController : Controller
    {
        private string url = "certificateType";

        [Route("list")]
        [HttpPost]
        public ResultData List(IndexParams model = null)
        {
            var result = new ResultData();

            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                result = UUtils.CallMiddlewareAPI($"{url}/list", userHeaders, JsonConvert.SerializeObject(model));
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
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
                result = UUtils.CallMiddlewareAPI($"{url}/" + id.ToString(), userHeaders, "", "GET");
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        [HttpPost]
        public ResultData SaveData(CertificateTypeModel model)
        {
            var result = new ResultData();

            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth(model.mode);
                result = UUtils.CallMiddlewareAPI($"{url}", userHeaders, JsonConvert.SerializeObject(model));
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }
    }
}
