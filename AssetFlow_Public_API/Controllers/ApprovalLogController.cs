using FAMS_Models;
using FAMS_Models.Utilities;
using FAMS_Public_API.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using FAMS_Models.Resources;

namespace FAMS_Public_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomAuthorize]
    public class ApprovalLogController : ControllerBase
    {
        string url = "approvallog";
        string controllerCheck = "approvallog";

        [Route("list")]
        [HttpPost]
        public ResultData List(IndexParams model = null)
        {
            var result = new ResultData();

            try
            {
                //https://localhost:44332/
                var userHeaders = HttpContext.GetMiddlewareAuth(Constants.FORM_MODE_VIEW, controllerCheck);
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

    }
}
