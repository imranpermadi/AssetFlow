using FAMS_Models.Utilities;
using FAMS_Public_API.Utilities;
using FAMS_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FAMS_Public_API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomAuthorize]
    public class UtilController : ControllerBase
    {
        string mode = Constants.FORM_MODE_UTIL;

        [HttpGet]
        [Route("version")]
        public ResultData GetVersion()
        {
            var result = new ResultData();

            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth(mode);
                result = UUtils.CallMiddlewareAPI($"util/version", userHeaders, "", "GET");
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        [HttpGet]
        [Route("ping")] //For Probes
        public string Ping()
        {
            return "OK";
        }

        [HttpGet]
        [Route("changelog")]
        public ResultData GetChangelog()
        {
            var result = new ResultData();

            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth(mode);
                result = UUtils.CallMiddlewareAPI($"util/changelog", userHeaders, "", "GET");
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        [HttpGet]
        [Route("localization")]
        public ResultData Localizations()
        {
            var result = new ResultData();

            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth(mode);
                result = UUtils.CallMiddlewareAPI($"util/localization", userHeaders, "", "GET");
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        [HttpPost]
        [Route("change_language")]
        public ResultData ChangeLanguage(ChangeLanguageModel model)
        {
            var result = new ResultData();
            bool success = false;
            string message = "";

            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth(mode);
                result = UUtils.CallMiddlewareAPI("util/change_language", userHeaders, JsonConvert.SerializeObject(model));
            }
            catch (Exception ex)
            {
                result.success = success;
                result.message = ex.Message;

            }

            return result;
        }

    }
}
