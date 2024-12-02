using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using FAMS_Models;
using FAMS_Models.Base;
using FAMS_Models.Utilities;
using FAMS_Public_API.Utilities;
using System;
using System.Collections.Generic;

namespace FAMS_Public_API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizationController : ControllerBase
    {
        string url = "localization";

        [Route("list")]
        [HttpGet]
        public ResultData List()
        {
            var result = new ResultData();

            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                result = UUtils.CallMiddlewareAPI($"{url}/list", userHeaders, "", "GET");
                //result.data = JsonConvert.DeserializeObject<List<CompanyModel>>(result.data.ToString());
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }


        [HttpPost]
        public ResultData SaveData(List<LanguageLocalizationModel> model)
        {
            var result = new ResultData();

            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth(Constants.FORM_MODE_EDIT);
                result = UUtils.CallMiddlewareAPI($"{url}", userHeaders, JsonConvert.SerializeObject(model));
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        [HttpGet]
        [Route("export")]
        public FileContentResult Export()
        {
            var result = new ResultByteArrayData();
            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth();
                //var byteArray = UUtils.CallMiddlewareAPIFile($"{apiUrl}download_pod/{pod_id}", userHeaders, "", "GET");

                result = UUtils.CallMiddlewareAPIByteArray($"{url}/export", userHeaders, "", "GET");

                var byteArray = result.data_byte;

                return File(byteArray, "application/octet-stream", result.message);

            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return null;
            //return result;
        }

        [HttpPut]
        [Route("import")]
        public ResultData Import(IFormCollection formCollection)
        {
            var result = new ResultData();

            try
            {
                var userHeaders = HttpContext.GetMiddlewareAuth(Constants.FORM_MODE_EDIT);
                var files = formCollection.Files;

                Dictionary<string, string> fileParameters = new Dictionary<string, string>();

                result = UUtils.CallMiddlewareAPIFormData(url + "/import", userHeaders, null, files, "PUT", "multipart/form-data");
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
