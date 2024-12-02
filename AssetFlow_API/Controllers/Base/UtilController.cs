using FAMS_API.Utilities;
using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Resources;
using FAMS_Models.Utilities;
using FAMS_ViewModels;
using FAMS_ViewModels.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using RabbitTester.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FAMS_API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuthorize]
    public class UtilController : ControllerBase
    {
        DataEntities db = new DataEntities();

        [HttpGet]
        [Route("version")]
        public ResultData GetWebVersion()
        {
            var result = new ResultData();
            result.success = true;
            result.message = Constants.OK;

            try
            {
                DataEntities db = new DataEntities();

                var version = "";
                var environment = ConfigurationManager.AppSettings["Environment"].ToString();

                //var changeLog = db.Changelogs.Where(r => r.Active == "Y" && !r.Is_Deleted).FirstOrDefault();
                //if (changeLog != null)
                //    version = changeLog.Version;

                var setting = db.Setting.Where(r => r.Code == Constants.SettingsCode.WEB_VERSION).FirstOrDefault();
                if (setting != null)
                {
                    version = setting.Value;
                }

                result.data = $"[{environment}] v.{version}";
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        [HttpGet]
        [Route("ping")]
        public string PingBackend()
        {
            var result = new ResultData();
            bool success = false;
            string message = "";

            try
            {
                //var user = HttpContext.GetUserData();
                var rpc = new RpcClient();

                var res = rpc.CallPing(BaseProgram.CreateRoutingKey("util", "ping"), "", false);

                result = JsonConvert.DeserializeObject<ResultData>(res);

                message = result.message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }

        [HttpGet]
        [Route("localization")]
        public ResultData Localizations()
        {
            var result = new ResultData();

            try
            {
                var fallback = "";
                Dictionary<string, object> messageList = new Dictionary<string, object>();
                var languagesData = new[]{ new { id="", text="" } }.ToList();

                var languages = db.Lookup.Where(r => r.LookupGroup == Constants.LookupGroup.LANGUAGE && r.IsDeleted != "Y").ToList();
                if(languages != null && languages.Count > 0)
                {
                    var fallbackLangaugeData = languages.FirstOrDefault(r => r.LookupInfo1 == "Y");
                    if (fallbackLangaugeData != null)
                        fallback = fallbackLangaugeData.LookupKey;

                    languagesData = languages.Select(r => new
                    {
                        id = r.LookupKey,
                        text = $"({r.LookupKey}) {r.LookupValue}"
                    }).ToList();

                }
                var localizations = db.Localization.Where(r => r.IsDeleted != "Y" /*&& r.Category == Constants.LocalizationType.LABEL*/).ToList();
                if(localizations != null && languages.Count > 0)
                {
                    var distinctLocs = localizations.Select(r => r.Code).Distinct().ToArray();

                    foreach(var language in languages)
                    {
                        Dictionary<string, object> locale = new Dictionary<string, object>();
                        Dictionary<string, Dictionary<string, object>> level1Locale = new Dictionary<string, Dictionary<string, object>>();

                        foreach(var loc in distinctLocs)
                        {
                            var localeLang = "";
                            var localeLangData = localizations.FirstOrDefault(r=> r.Code == loc && r.LanguageCode == language.LookupKey);
                            if(localeLangData != null)
                            {
                                localeLang = localeLangData.Label;
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(fallback))
                                {
                                    localeLang = loc;
                                }
                                else
                                {
                                    var fallackLangData = localizations.FirstOrDefault(r => r.Code == loc && r.LanguageCode == fallback);
                                }
                            }


                            if (loc.Contains("."))
                            {
                                string cat = loc.Split('.')[0];
                                string catCode = loc.Split('.')[1];

                                Dictionary<string, object> catCodeLang = new Dictionary<string, object>();
                                catCodeLang.Add(catCode, localeLang);

                                if (!level1Locale.ContainsKey(cat))
                                {
                                    level1Locale.Add(cat, catCodeLang);
                                }
                                else
                                {
                                    (level1Locale[cat] as Dictionary<string, object>).Add(catCode, localeLang);
                                }
                            }
                            else
                            {
                                locale.Add(loc, localeLang);
                            }

                        }

                        foreach(var level1 in level1Locale)
                        {
                            locale.Add(level1.Key, level1.Value);
                        }

                        messageList.Add(language.LookupKey, locale);
                    }
                }

                if (languagesData[0].id == "")
                    languagesData.Clear();

                result.data = new
                {
                    languages = languagesData,
                    localizations = messageList
                };

                result.success = true;

            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        [HttpPost]
        [Route("change_language")]
        public ResultData ChangeLanguage(ChangeLanguageModel modelData)
        {
            var result = new ResultData();
            result.success = true;
            result.message = Constants.OK;

            try
            {
                DataEntities db = new DataEntities();

                User model;
                long userId = HttpContext.GetUserId();

                model = db.User.Where(r => r.Id == userId).FirstOrDefault();
                if (model == null)
                    throw new Exception("User not found");

                model.Language = modelData.language;
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
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
