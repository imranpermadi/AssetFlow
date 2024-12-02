using FAMS_API.Utilities;
using FAMS_Data;
using FAMS_Models.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FAMS_API
{

    public class ApiAuthorize : Attribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                DataEntities db = new DataEntities();

                string language = context.HttpContext.Request.Headers["language"];
                string username = context.HttpContext.Request.Headers["username"];
                string fullname = context.HttpContext.Request.Headers["fullname"];
                //string menu = context.HttpContext.Request.Headers["menu"];

                string cActionName = context.ActionDescriptor.DisplayName;

                if (cActionName.Contains("PingBackend"))
                    return;

                //string actionName = string.IsNullOrEmpty(currentAction) ? filterContext.ActionDescriptor.ActionName : currentAction;
                //var requests = context.ActionDescriptor.GetParameters()
                //            .Select(r => new
                //            {
                //                Key = r.ParameterName,
                //                Value = context.HttpContext.Request[r.ParameterName]
                //            }).ToDictionary(r => r.Key);

                string actionName = context.HttpContext.Request.Headers["menu_action"].ToString();
                string mode = "";
                string controllerName = context.HttpContext.Request.Headers["menu_controller"].ToString();
                string actionCheck = context.HttpContext.Request.Headers["menu_check"].ToString();

                if (actionCheck != "Y")
                {
                    string authHeader = context.HttpContext.Request.Headers["Authorization"];

                    string authType = authHeader.Split(' ')[0];
                    string authParam = authHeader.Split(' ')[1];

                    var apiSetting = db.APISetting.Where(r => r.Credential == authParam && r.IsDeleted != "Y").FirstOrDefault();
                    if (apiSetting == null)
                    {
                        throw new Exception("Unauthorized");
                    }
                }

                List<string> excludeControllers = new List<string>()
                {
                    "Util",
                    "Dashboard",
                    "Menu",
                    "Auth",
                    "List",
                };

                if (string.IsNullOrEmpty(username))
                {
                    throw new Exception("Unauthorized");
                }

                var auth = context.HttpContext.Request.Headers["auth"].ToString();

                //var userToken = db.User_Token.Where(r => r.Token == auth && r.Active && r.Username == username).FirstOrDefault();
                //if(userToken == null)
                //{
                //    throw new Exception("Unauthorized");
                //}
                //else
                //{
                //    if (userToken.Expiry_Date < DateTime.Now)
                //        throw new Exception("Unauthorized");
                //}

                var user = db.User.Where(r => r.Username == username && r.IsDeleted != "Y").FirstOrDefault();
                if (user == null)
                    throw new Exception("Unauthorized");

                bool valid = false;

                if(!excludeControllers.Contains(controllerName))
                {
                    valid = CheckAuthorization(username, actionName, controllerName, mode);
                }
                else
                {
                    valid = true;
                }

                //bool valid = true;

                if (!valid)
                    throw new Exception("Unauthorized");

                //user.language = language;

                context.HttpContext.Items.Add("User", user);

                var lang = "en-US";
                if (language == "en") lang = "en-US";
                else if (language == "id") lang = "id-ID";

                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);

            }
            catch (Exception ex)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }

        public static bool CheckAuthorization(string username = "", string actionName = "", string controllerName = "", string mode = "")
        {
            GroupMenu groupMenu = null;
            if (actionName == "Editor") actionName = "Index";

            bool hasAuth = UMenu.UserHasAuthorization(username, actionName, controllerName, out groupMenu);

            if (hasAuth)
            {
                if (string.IsNullOrEmpty(mode))
                {
                    return true;
                }
                else
                {
                    if (mode == Constants.FORM_MODE_CREATE) return groupMenu.Create;
                    else if (mode == Constants.FORM_MODE_EDIT) return groupMenu.Edit;
                    else if (mode == Constants.FORM_MODE_DELETE) return groupMenu.Delete;
                    else if (mode == Constants.FORM_MODE_VIEW) return groupMenu.View;
                }
            }

            return false;
        }

    }
}
