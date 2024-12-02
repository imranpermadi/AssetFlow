
using FAMS_API.Utilities;
using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Resources;
using FAMS_Models.Utilities;
using FAMS_ViewModels.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    //[ApiAuthorize]
    public class AuthController : ControllerBase
    {
        DataEntities db = new DataEntities();

        [Route("login")]
        [HttpPost]
        public ResultData Login(LoginModel model)
        {
            var result = new ResultData();
            bool success = false;
            string message = "";

            try
            {
                var username = model.Username;
                var password = model.Password;


                UserPrincipal userPrincipal;
                User user;
                bool correctPass = false;

                string loginResult = UAuth.Login(db, username, model.Password, "WEB", "", "", out userPrincipal, out user, out correctPass);

                if(user != null && user.UseAD != "Y")
                {
                    //CAPTCHA
                }

                if(loginResult == "OK")
                {
                    success = true;
                }
                else
                {
                    if(user != null)
                    {
                        if(correctPass && user.FirstLogin == "Y" && user.UseAD != "Y")
                        {
                            result.message_code = "FIRST_LOGIN";
                        }
                        else
                        {
                            if(loginResult == "CHANGE_PASS")
                            {
                                loginResult = "Password expired. Please change your current password";

                                result.message_code = "CHANGE_PASS";
                            }
                        }
                    }
                }
                message = loginResult;

                if (success)
                {
                    if (string.IsNullOrEmpty(result.message_code))
                    {
                        var userData = new UserData()
                        {
                            Id = user.Id,
                            Username = user.Username,
                            Fullname = user.Fullname,
                            Email = user.Email,
                            IsAdmin = user.IsAdmin,
                            UseAD = user.UseAD,
                            Menus = UMenu.GetUserMenu(user.Username, user.IsAdmin == "Y"),
                            LocationCode = user.LocationCode,
                            Language = user.Language,
                            SERVER = ConfigurationManager.AppSettings["Environment"].ToString(),
                        };

                        var tokenDate = DateTime.Now.AddHours(12);
                        AuthUtils.CreateUserData(userData, tokenDate);
                        result.data = new WebLoginResultModel()
                        {
                            token_expiry = tokenDate,
                            user_data = userData
                        };

                        AuthUtils.DeactivatePastTokens(db, userData);

                        UserToken newToken = new UserToken();
                        newToken.Username = user.Username;
                        newToken.Token = userData.Token;
                        newToken.ExpiryDate = tokenDate;
                        newToken.CreatedDate = DateTime.Now;
                        newToken.Active = "Y";
                        newToken.Type = "WEB";
                        db.Entry(newToken).State = System.Data.Entity.EntityState.Added;

                        db.SaveChanges();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(result.message_code))
                        result.message_code = "CAPTCHA";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            result.success = success;
            result.message = message;

            return result;
        }


        //[Route("menu/{menu}/{action}")]
        [Route("menu")]
        [HttpPost]
        [ApiAuthorize]
        public ResultData AuthorizeMenu(MenuData model)
        {
            var result = new ResultData();
            bool success = false;
            string message = "";

            try
            {
                var username = HttpContext.GetUsername();
                var isAdmin = HttpContext.GetUserIsAdmin();
                GroupMenu menuAuth = new GroupMenu();
                bool auth = false;

                if (isAdmin)
                {
                    auth = true;
                    menuAuth = new GroupMenu()
                    {
                        Create = true,
                        View = true,
                        Delete = true,
                        Edit = true,
                        Print = true
                    };
                }
                else
                {
                    auth = UMenu.UserHasAuthorization(username, model.menu_action, model.menu_controller, out menuAuth);
                }

                if (auth)
                {
                    success = auth;
                    result.data = menuAuth;
                }
                else
                {
                    throw new Exception("Unauthorized");
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            result.success = success;
            result.message = message;

            return result;

        }

        [HttpPost]
        [Route("change_password")]
        //[ValidateAntiForgeryToken]
        public ResultData ChangeUserPassword(ChangePasswordModel model)
        {
            var result = new ResultData();
            string message = "";
            bool success = false;

            try
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var user = db.User.Where(r => r.Username == model.username && r.IsDeleted != "Y").FirstOrDefault();
                        if (user != null)
                        {
                            if (user.UseAD == "Y")
                                throw new Exception("Unable to change password");

                            if (user.Password != UEncryption.ComputeSha256Hash(model.currentPass))
                            {
                                throw new Exception("Incorrect Old Password");
                            }
                            //Maximize password security here
                            string nPass = "";
                            string passMessage = "";
                            var complexity = UAuth.CheckPassword(model.newPass, out passMessage, out nPass);
                            if (!complexity)
                            {
                                throw new Exception(passMessage);
                            }
                            string hashedNewPass = UEncryption.ComputeSha256Hash(nPass);
                            if (hashedNewPass == user.Password)
                                throw new Exception("New password must be different from Old Password");

                            if (!model.changeFirstLogin)
                            {
                                var passHistory = db.UserPassLog.Where(r => r.UserId == user.Id).OrderByDescending(r => r.CreatedDate).Take(5).ToArray();
                                if (passHistory.Length > 0)
                                {
                                    var checkSamePassword = passHistory.Where(r => r.Password == hashedNewPass).FirstOrDefault();
                                    if (checkSamePassword != null)
                                        throw new Exception("Password must be different from recently changed password");
                                }
                                user.FirstLogin = "N";
                            }
                            else
                                user.FirstLogin = "Y";

                            
                            user.Password = UEncryption.ComputeSha256Hash(nPass);
                            user.EditedBy = model.username;
                            user.EditedDate = DateTime.Now;
                            db.Entry(user).State = System.Data.Entity.EntityState.Modified;

                            if (!model.changeFirstLogin)
                            {
                                UserPassLog passLog = new UserPassLog();
                                passLog.UserId = user.Id;
                                passLog.Password = hashedNewPass;
                                passLog.CreatedDate = DateTime.Now;
                                db.Entry(passLog).State = System.Data.Entity.EntityState.Added;
                            }

                            db.SaveChanges();
                            trans.Commit();

                            message = "Password succesfully changed";
                            success = true;
                        }
                        else throw new Exception("User not found");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            result.message = message;
            result.success = success;

            return result;
        }

        [HttpGet]
        [Route("download_mobile")]
        public ResultData DownloadMobileApp()
        {
            var result = new ResultData();
            var message = "";
            var success = false;

            try
            {


                success = true;
                message = Constants.OK;
            }
            catch(Exception ex)
            {
                message = ex.Message;
            }

            result.success = success;
            result.message = message;

            return result;
        }
    }
}
