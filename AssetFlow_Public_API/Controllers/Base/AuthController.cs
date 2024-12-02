using FAMS_Models;
using FAMS_Models.Utilities;
using FAMS_Public_API.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace FAMS_Public_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class AuthController : ControllerBase
    {


        [Route("login")]
        [HttpPost]
        public ResultData Authenticate(LoginModel model)
        {
            var result = new ResultData();

            try
            {
                //https://localhost:44332/
                result = UUtils.CallMiddlewareAPI("auth/login", null,JsonConvert.SerializeObject(model));

                if (result != null)
                {
                    if(result.success && string.IsNullOrEmpty(result.message_code))
                    {
                        var data = JsonConvert.DeserializeObject<WebLoginResultModel>(result.data.ToString());

                        //var tokenDate = DateTime.Now.AddHours(12);
                        //AuthUtils.CreateUserData(data, tokenDate);
                        //result.data = new
                        //{
                        //    token_expiry = tokenDate,
                        //    user_data = data
                        //};

                        result.data = data;
                    }

                }                
            }
            catch(Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }


        //[Route("menu/{menu}/{action}")]
        [Route("menu")]
        [HttpPost]
        [CustomAuthorize]
        public ResultData AuthorizeMenu(MenuData model)
        {
            var result = new ResultData();

            try
            {
                var user = HttpContext.GetMiddlewareAuth();

                result = UUtils.CallMiddlewareAPI("auth/menu/", user, JsonConvert.SerializeObject(model));

            }
            catch(Exception ex)
            {

            }

            return result;

        }

        [Route("change_password")]
        [HttpPost]
        public ResultData ChangePassword(ChangePasswordModel model)
        {
            var result = new ResultData();

            //result.success = true;
            //result.message = Constants.OK;
            try
            {
                var user = HttpContext.GetMiddlewareAuth();
                result = UUtils.CallMiddlewareAPI("auth/change_password", user,JsonConvert.SerializeObject(model));

            }catch(Exception exc)
            {
                result.success = false;
                result.message = exc.Message;
            }
            return result;
        }


        [HttpGet]
        [Route("download_mobile")]
        public FileStreamResult DownloadMobileApp()
        {
            try
            {
                var user = HttpContext.GetMiddlewareAuth();
                var result = UUtils.CallMiddlewareAPI($"auth/download_mobile", user, "", "GET");

                var byteArray = result.data;

                BinaryFormatter bf = new BinaryFormatter();
                var ms = new MemoryStream();
                bf.Serialize(ms, byteArray);
                //return ms.ToArray();
                ms.Position = 0;
                ms.Flush();
                return File(ms, "application/octet-stream", result.message);
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
