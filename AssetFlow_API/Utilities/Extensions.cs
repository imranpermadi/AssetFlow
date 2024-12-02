using FAMS_Data;
using FAMS_Models.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FAMS_API.Utilities
{
    public static class Extensions
    {

        public static string GetUsername(this HttpContext context)
        {
            var user = (User)context.Items["User"];
            return user.Username;
        }

        public static string GetFullname(this HttpContext context)
        {
            var user = (User)context.Items["User"];
            return user.Fullname;
        }

        public static string GetUser(this HttpContext context)
        {
            var user = (User)context.Items["User"];
            return "[" + user.Username + "] " + user.Fullname;
        }

        public static long GetUserId(this HttpContext context)
        {
            var user = (User)context.Items["User"];
            return user.Id;
        }

        public static User GetUserData(this HttpContext context)
        {
            var user = (User)context.Items["User"];
            return user;
        }

        public static string GetCompleteUsername(this HttpContext context)
        {
            var user = (User)context.Items["User"];
            return $"[{user.Username}] {user.Fullname}";
        }

        public static bool GetUserIsAdmin(this HttpContext context)
        {
            var user = (User)context.Items["User"];
            return user.IsAdmin == "Y";
        }
    
        public static UserData UserData(this User user)
        {
            var userData = new UserData();
            BaseProgram.CopyProperties(typeof(User), user, typeof(UserData), userData);
            return userData;
        }

        //public static List<long> GetUserCompany(this HttpContext context, out bool allCompany)
        //{
        //    DataEntities db = new DataEntities();
        //    var userId = context.GetUserId();
        //    var username = context.GetUsername();
        //    allCompany = false;

        //    var user = db.User.Where(r => r.Username == username && r.IsDeleted != "Y").FirstOrDefault();
        //    if (user == null)
        //        return new List<long>();

        //    //if (user.CompanyAll == "Y" || user.IsAdmin == "Y")
        //    //{
        //    //    allCompany = true;
        //    //    return new List<long>();
        //    //}

        //    //var userPOrg = db.UserCompany.Where(r => r.User_Id == userId && r.Is_Deleted != "Y").ToArray();
        //    //if (userPOrg.Length > 0)
        //    //{
        //    //    return userPOrg.Select(r => r.Company_Id).ToList();
        //    //}
        //    //else return new List<long>();
        //}

        //public static List<string> GetUserCompanyCode(this HttpContext context, out bool allCompany)
        //{
        //    DataEntities db = new DataEntities();
        //    var userId = context.GetUserId();
        //    var username = context.GetUsername();
        //    allCompany = false;

        //    var user = db.User.Where(r => r.Username == username && r.Is_Deleted != "Y").FirstOrDefault();
        //    if (user == null)
        //        return new List<string>();

        //    if (user.Company_All || user.Is_Admin)
        //    {
        //        allCompany = true;
        //        return new List<string>();
        //    }

        //    var userPOrg = db.User_Company.Where(r => r.User_Id == userId && r.Is_Deleted != "Y").ToArray();
        //    if (userPOrg.Length > 0)
        //    {
        //        return userPOrg.Select(r => r.Company.Code).ToList();
        //    }
        //    else return new List<string>();
        //}


        public static string GetDisplayName(this User user)
        {
            return "[" + user.Username + "] " + user.Fullname;
        }

        //public static string GetLocationCode(this HttpContext context)
        //{
        //    var user = (User)context.Items["User"];
        //    return user.Location_Code;
        //}
    }
}
