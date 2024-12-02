using FAMS_Models.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FAMS_Public_API.Utilities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorize : Attribute, IAuthorizationFilter
    {
        private string menuAction = "", menuController = "";

        public CustomAuthorize(string action = "", string controller = "")
        {
            menuAction = action;
            menuController = controller;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (UserData)context.HttpContext.Items["User"];


            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (actionDescriptor.ControllerName == "Util" && (actionDescriptor.ActionName == "Ping" || actionDescriptor.ActionName == "Localizations"))
                return;

            menuAction = string.IsNullOrEmpty(menuAction) ? "Index" : menuAction;
            var requests = context.HttpContext.Request.Query;
            if (requests.Count > 0)
            {
                string queries = "";
                var listParam = menuAction.Substring(menuAction.IndexOf('?') + 1).Split('&');
                foreach (var key in requests.Keys)
                {
                    var keyValue = requests[key];
                    if (menuAction.Contains(key))
                    {
                        if (listParam.Any(r => r.Contains('=')))
                        {
                            var param = listParam
                                .Where(r => r.Substring(0, r.IndexOf('=')) == key)
                                .Select(r => new
                                {
                                    currKey = r.Substring(0, r.IndexOf('=')),
                                    currValue = r.Substring(r.IndexOf('=') + 1)
                                }).FirstOrDefault();

                            if (param == null || param.currKey == null || (param.currValue == null || param.currValue == ""))
                                continue;

                            if (param.currValue != keyValue)
                            {
                                menuAction = menuAction.Replace(param.currValue, keyValue);
                            }
                        }
                    }
                    else
                    {
                        if (!queries.Contains(key))
                        {
                            queries += $"{(string.IsNullOrEmpty(queries) ? "" : "&")}{key}={keyValue}";
                        }
                    }
                }

                menuAction = string.IsNullOrEmpty(queries) ? menuAction : $"{menuAction}?{queries}";
            }

            if (user == null)
            {

                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }
            else
            {

                context.HttpContext.Items.Add("menu_action", menuAction);
                context.HttpContext.Items.Add("menu_controller", string.IsNullOrEmpty(menuController) ? actionDescriptor.ControllerName : menuController);
            }

        }
    }
}
