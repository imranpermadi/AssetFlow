using FAMS_Data;
using FAMS_Models.Resources;
using FAMS_Models.Utilities;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_ViewModels.Utilities
{
    public class UAuth
    {


        public static string Login(DataEntities db, string username, string password, string platform, string fcm_token, string token, out UserPrincipal userData, out User user, out bool correctPass)
        {
            string result = "";
            userData = null;
            user = null;
            correctPass = false;

            try
            {
                user = db.User.Where(r => r.Username == username).FirstOrDefault();
                if (user == null)
                    throw new Exception(Resources.USER_PASS_INVALID);

                if (user.IsDeleted == "Y")
                    throw new Exception(Resources.USER_PASS_INVALID);

                if (user.UseAD == "Y")
                {
                    result = ADSignIn(username, password);
                    if (result == Constants.OK)
                    {
                        user.LastAccessDate = DateTime.Now;
                        db.Entry(user).State = EntityState.Modified;
                        correctPass = true;
                    }
                }
                else
                {
                    var a = UEncryption.ComputeSha256Hash(password);
                    if (user.Password == UEncryption.ComputeSha256Hash(password))
                    {
                        correctPass = true;
                        if (user.FirstLogin == "Y")
                        {
                            return "Please change your password on first login";
                        }

                        int duration = 180;
                        var durationData = db.Lookup.Where(r => r.LookupGroup == Constants.LookupGroup.CHANGE_PASS_DURATION && r.IsDeleted != "Y").FirstOrDefault();
                        if (durationData == null)
                        {
                            duration= int.Parse(durationData.LookupValue);
                        }

                        var userId = user.Id;
                        var lastPassChange = db.UserPassLog.Where(r => r.UserId == userId).OrderByDescending(r => r.Id).FirstOrDefault();
                        if (lastPassChange != null)
                        {
                            if (lastPassChange.CreatedDate.AddDays(duration) < DateTime.Now)
                                return "CHANGE_PASS";
                        }
                        else
                            return "CHANGE_PASS";

                        user.LastAccessDate = DateTime.Now;
                        db.Entry(user).State = EntityState.Modified;
                        //db.SaveChanges();

                        //UUtils.CreateUserLog(db, username, platform, "LOGIN");

                        return "OK";
                    }
                    else
                    {
                        return Resources.USER_PASS_INVALID;
                    }
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public static string ADSignIn(string user, string password)
        {
            string result = Constants.OK;
            try
            {
                using (DirectoryEntry adsEntry = new DirectoryEntry($"LDAP://{Constants.AD_DOMAIN}", user, password, AuthenticationTypes.Secure))
                {
                    try
                    {
                        //Code to validate connection, if user is authenticated then conversion to object will be succeed otherwise it will failed.
                        object validateConn = adsEntry.NativeObject;
                    }
                    catch (DirectoryServicesCOMException exc)
                    {
                        string extendedErrorMessage = exc.ExtendedErrorMessage;
                        string[] codes = extendedErrorMessage.Split(',');
                        string errorCode = codes.Where(r => r.Contains("data")).Select(r => r.Replace("data", "").Trim()).LastOrDefault();

                        //All error code prefixed with a 'HEX: 0X..' for example 'HEX: 0X525'
                        switch (errorCode)
                        {
                            case "525":
                                result = Resources.USER_PASS_INVALID;
                                break;
                            case "52e":
                                result = Resources.USER_PASS_INVALID;
                                break;
                            case "530":
                                result = $"Your account not permitted to logon at this time. {Environment.NewLine} Please contact local IT";
                                break;
                            case "531":
                                result = $"Your account not permitted to logon to this computer. {Environment.NewLine} Please contact local IT";
                                break;
                            case "532":
                                result = $"Your password has expired. {Environment.NewLine}"
                                    + @"<a href=""https://adpass.company.co.id"" target=""_blank""> Please click this link to change your password. </a>";
                                break;
                            case "533":
                                result = $"Your account has been disabled. {Environment.NewLine} Please contact local IT.";
                                break;
                            case "701":
                                result = "Your account is expired. Please contact local IT";
                                break;
                            case "773":
                                result = $"You must change password before logging on the first time. {Environment.NewLine}"
                                    + @"<a href=""https://adpass.company.co.id"" target=""_blank""> Please click this link to change your password. </a>";
                                break;
                            case "775":
                                result = $"Account is locked. {Environment.NewLine} You entered the wrong password 3 times, Please wait 15 minutes or contact local IT";
                                break;
                            default:
                                result = $"Unspecified error: {errorCode}";
                                break;
                        }
                    }
                    adsEntry.Close();
                }
            }
            catch (Exception exc)
            {
                result = exc.Message;
            }

            return result;
        }

        public static bool CheckPassword(string password, out string message, out string newPassword)
        {
            var complexity = false;
            message = "";
            newPassword = "";

            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    newPassword = "WILdms123!";
                    complexity = true;
                }
                else
                {
                    complexity = CheckPasswordComplexity(password);
                    if (!complexity)
                        throw new Exception("Password does not meet minimum security requirements. It must be at least 8(eight) characters long, minimum of 1 capital letter and number.");

                    newPassword = password;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return complexity;
        }

        public static bool CheckPasswordComplexity(string passWord)
        {
            int validConditions = 0;
            //foreach (char c in passWord)
            //{
            //    if (c >= 'a' && c <= 'z')
            //    {
            //        validConditions++;
            //        break;
            //    }
            //}
            if (passWord.Length < 8) return false;

            foreach (char c in passWord)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 0) return false;
            foreach (char c in passWord)
            {
                if (c >= '0' && c <= '9')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 1) return false;
            //if (validConditions == 2)
            //{
            //    char[] special = { '@', '#', '$', '%', '^', '&', '+', '=' }; // or whatever    
            //    if (passWord.IndexOfAny(special) == -1) return false;
            //}
            return true;
        }

        //public static List<string> GetUserLocation(string username,out bool allLocation)
        //{
        //    DMSEntities db = new DMSEntities();
        //    allLocation = false;

        //    var user = db.Users.Where(r => r.Employee_Number == username && r.Is_Deleted != "Y").FirstOrDefault();
        //    if (user == null)
        //        return new List<string>();

        //    if (user.Is_Admin)
        //    {
        //        allLocation = true;
        //        return new List<string>();
        //    }

        //    if (user.Location_All)
        //    {
        //        allLocation = true;
        //        return new List<string>();
        //    }

        //    var userPOrg = db.User_Location.Where(r => r.User_Id == username && !r.Is_Deleted).ToArray();
        //    if (userPOrg.Length > 0)
        //    {
        //        return userPOrg.Select(r => r.Location_Code).ToList();
        //    }
        //    else return new List<string>();
        //}

        //public static List<String> GetTransporterLocation(string username)
        //{
        //    DMSEntities db = new DMSEntities();
        //    var userPOrg = db.User_Location.Where(r => r.User_Id == username && !r.Is_Deleted).ToArray();
        //    if (userPOrg.Length > 0)
        //    {
        //        return userPOrg.Select(r => r.Location_Code).ToList();
        //    }
        //    else return new List<string>();
        //}
    }
}
