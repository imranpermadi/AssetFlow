using FAMS_Models.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FAMS_Public_API.Utilities
{
    public static class Extensions
    {
        public static UserData GetUser(this HttpContext context)
        {
            try
            {
                var user = (UserData)context.Items["User"];

                return user;
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        public static Dictionary<string, string> GetMiddlewareAuth(this HttpContext context, string mode = "", string override_controller = "", string override_action = "", string type = "")
        {
            try
            {
                var user = context.GetUser();

                if (user == null)
                    throw new Exception("ERROR");

                var headers = new Dictionary<string, string>();

                //var language = context.Request.Headers["language"].ToString();
                var menuAction = context.Items["menu_action"];
                var menuController = context.Items["menu_controller"];
                var menuCheck = context.Items["menu_check"];

                //headers.Add("language", language);
                headers.Add("username", user.Username);
                headers.Add("fullname", user.Fullname);

                if (string.IsNullOrEmpty(override_controller))
                {
                    headers.Add("menu_controller", menuController != null ? menuController.ToString() : "");
                }
                else
                {
                    headers.Add("menu_controller", override_controller);
                }

                if (string.IsNullOrEmpty(override_action))
                {
                    headers.Add("menu_action", menuAction != null ? menuAction.ToString() : "");
                }
                else
                {
                    headers.Add("menu_action", override_action);
                }

                    headers.Add("type_index", type);
                

                headers.Add("menu_check", menuCheck != null ? menuCheck.ToString() : "");
                headers.Add("mode", mode);
                headers.Add("auth", user.Token);

                return headers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
