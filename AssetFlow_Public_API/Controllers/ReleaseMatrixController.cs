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
    public class ReleaseMatrixController : ControllerBase
    {
        private string url = "releasematrix";

        [Route("list")]
        [HttpPost]
        public ResultData List(IndexParams model = null)
        {
            var result = new ResultData();

            try
            {
                //https://localhost:44332/
                var userHeaders = HttpContext.GetMiddlewareAuth();
                result = UUtils.CallMiddlewareAPI($"{url}/list", userHeaders, JsonConvert.SerializeObject(model));
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
                result = UUtils.CallMiddlewareAPI($"{url}/" + id.ToString(), userHeaders, "", "GET");
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Resources.INTERNAL_ERROR;
            }

            return result;
        }

        [HttpPost]
        public ResultData SaveData(ReleaseMatrixModel model)
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
                result.message = Resources.INTERNAL_ERROR;
            }

            return result;
        }
    }
}
