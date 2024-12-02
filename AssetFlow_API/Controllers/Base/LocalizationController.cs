using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FAMS_API.Utilities;
using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Base;
using FAMS_Models.Utilities;
using FAMS_ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace FAMS_API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuthorize]
    public class LocalizationController : ControllerBase
    {
        DataEntities db = new DataEntities();

        [HttpGet]
        [Route("list")]
        public ResultData List()
        {
            var response = new ResultData();
            bool success = false;
            string message = "";

            try
            {
                var listData = new List<LanguageLocalizationModel>();
                var newDetail = new LanguageLocalizationModel();

                var localizations = db.Localization.Where(r => r.IsDeleted != "Y").ToList();
                var languages = db.Lookup.Where(r => r.LookupGroup == Constants.LookupGroup.LANGUAGE && r.IsDeleted != "Y").OrderBy(r=> r.LookupValue).ToList();

                if(languages.Count > 0)
                {
                    //var fallback = languages.FirstOrDefault(r => r.LookupInfo1 == "Y");
                    if(localizations.Count > 0)
                    {
                        var distinctCodes = localizations.Select(r => r.Code).Distinct().ToList();

                        foreach (var distinct in distinctCodes)
                        {
                            var header = new LanguageLocalizationModel();

                            header.Code = distinct;
                            header.Details = new List<LocalizationModel>();

                            foreach (var language in languages)
                            {
                                var detail = new LocalizationModel();

                                detail.LanguageCode = language.LookupKey;
                                detail.language_name = language.LookupValue;
                                detail.Code = distinct;

                                var hasLoc = localizations.FirstOrDefault(r => r.LanguageCode == language.LookupKey && r.IsDeleted != "Y" && r.Code == distinct);
                                if (hasLoc != null)
                                {
                                    detail.Label = hasLoc.Label;
                                    detail.mode = Constants.FORM_MODE_UNCHANGED;
                                }
                                else
                                {
                                    detail.mode = Constants.FORM_MODE_CREATE;
                                }

                                header.Details.Add(detail);
                            }
                            listData.Add(header);
                        }
                    }

                    newDetail.Details = new List<LocalizationModel>();
                   
                    foreach(var language in languages)
                    {
                        var newLang = new LocalizationModel();
                        newLang.LanguageCode = language.LookupKey;
                        newLang.language_name = language.LookupValue;
                        newLang.mode = Constants.FORM_MODE_CREATE;
                        
                        newDetail.Details.Add(newLang);
                    }


                }

                response.data = new
                {
                    header = listData,
                    languages = languages,
                    header_model = newDetail,
                    detail_model = new LocalizationModel()
                };
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            response.success = success;
            response.message = message;

            return response;

        }

        [HttpGet("{id}")]
        public ResultData GetData(long id)
        {
            var result = new ResultData();

            try
            {
                var header = new LocalizationViewModel();

                if (id != 0)
                {
                    header = new LocalizationViewModel(db, id);
                }

                //result.data = new EditorHelper()
                //{
                //    header = header,

                //};
            }
            catch(UException ex)
            {
                result.success = false;
                result.message = ex.Message;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        [HttpPost]
        public ResultData Save(List<LanguageLocalizationModel> modelData)
        {
            var result = new ResultData();
            bool success = false;
            string message = "";

            try
            {
                var user = HttpContext.GetUserData().UserData();

                if (modelData != null && modelData.Count > 0)
                {
                    foreach (var model in modelData)
                    {
                        var newModel = new Localization();

                        var localizationData = new List<Localization>();

                        if (model.mode != Constants.FORM_MODE_CREATE)
                        {
                            localizationData = db.Localization.Where(r => r.Code == model.Code && r.IsDeleted != "Y").ToList();
                        }
                        else
                        {
                            localizationData = db.Localization.Where(r => r.Code == model.Code && r.IsDeleted != "Y").ToList();
                            if (localizationData.Count > 0)
                                model.mode = Constants.FORM_MODE_EDIT;
                        }

                        if (model.mode == Constants.FORM_MODE_DELETE && localizationData.Count > 0)
                        {
                            foreach(var locale in localizationData)
                            {
                                locale.IsDeleted = "Y";
                                locale.DeletedDate = DateTime.Now;
                                locale.DeletedBy = user.GetDisplayName();

                                db.Entry(locale).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        else
                        {
                            foreach (var localization in model.Details)
                            {
                                var newData = new Localization();
                             
                                if(localization.mode != Constants.FORM_MODE_UNCHANGED)
                                {
                                    if (localization.mode == Constants.FORM_MODE_CREATE)
                                    {
                                        newData = new Localization();
                                        newData.CreatedDate = DateTime.Now;
                                        newData.CreatedBy = user.GetDisplayName();

                                        db.Entry(newData).State = System.Data.Entity.EntityState.Added;
                                    }
                                    else if (localization.mode == Constants.FORM_MODE_EDIT)
                                    {
                                        newData = localizationData.FirstOrDefault(r => r.Code == model.Code && r.LanguageCode == localization.LanguageCode);

                                        if (newData == null)
                                            continue;

                                        newData.EditedBy = user.GetDisplayName();
                                        newData.EditedDate = DateTime.Now;

                                        db.Entry(newData).State = System.Data.Entity.EntityState.Modified;
                                    }

                                    newData.LanguageCode = localization.LanguageCode;
                                    newData.Code = model.Code;
                                    newData.Category = model.Category;
                                    newData.Label = localization.Label;
                                }
                                
                                
                            }
                        }

                    }

                    db.SaveChanges();
                    success = true;
                    message = "OK";
                }
                else throw new UException("Model data invalid");
            }
            catch(UException ex)
            {
                result.success = false;
                result.message = ex.Message;
            }
            catch (Exception ex)
            {
                success = false;
                message = ex.Message;
            }

            result.success = success;
            result.message = message;

            return result;
        }

        [HttpGet]
        [Route("export")]
        public FileStreamResult Export()
        {
            var result = new ResultByteArrayData();
            bool success = false;
            string message = "";

            try
            {
                var fileName = "Localization.xlsx";

                var languages = db.Lookup.Where(r => r.LookupGroup == Constants.LookupGroup.LANGUAGE).OrderBy(r=> r.LookupValue).ToList();
                var localizationData = db.Localization.Where(r => r.IsDeleted != "Y").Distinct().ToList();

                var distinctCodes = localizationData.Select(r=> r.Code).ToList();

                //Prepare template header
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Localization");
                ws.Cell(1, 1).Value = "Code";
               
                for(int i=0; i<languages.Count;i++)
                {
                    var language = languages[i];
                    ws.Cell(1, (2 + i)).Value = language.LookupValue + " (" + language.LookupKey + ")";
                }

                for(int i=0; i< distinctCodes.Count;i++)
                {
                    var code = distinctCodes[i];

                    ws.Cell(2 + i, 1).Value = code;


                    for (int x = 0; x < languages.Count; x++)
                    {
                        var languageCode = languages[x].LookupKey;

                        var langLocalization = localizationData.FirstOrDefault(r=> r.LanguageCode == languageCode && r.Code == code); 

                        if (langLocalization != null)
                        {
                            ws.Cell(2 + i, 2 + x).Value = langLocalization.Label;
                        }
                        else
                        {
                            ws.Cell(2 + i, 2 + x).Value = "";
                        }
                    }
                }

                var fileStream = new MemoryStream();

                wb.SaveAs(fileStream);

                fileStream.Seek(0, SeekOrigin.Begin);
                fileStream.Position = 0;

                return File(fileStream, "application/octet-stream", fileName);

                success = true;
                message = fileName;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return null;

            //result.success = success;
            //result.message = message;

            //return result;
        }

        [HttpPut]
        [Route("import")]
        public ResultData Import(IFormCollection formCollection)
        {
            var result = new ResultData();
            var message = "";
            bool success = false;

            try
            {
                var user = HttpContext.GetUserData().UserData();

                var files = formCollection.Files;

                if(files != null && files.Count > 0)
                {
                    var file = files[0];
                    var stream = file.OpenReadStream();

                    var wb = new XLWorkbook(stream);
                    var ws = wb.Worksheet(1);

                    var localizations = db.Localization.Where(r => r.IsDeleted != "Y").ToList();
                    var languageList = new List<String>();

                    var col = 0;
                    while (true)
                    {
                        var lang = ws.Cell(1, 2 + col).Value;

                        if (string.IsNullOrEmpty(lang.ToString()))
                            break;

                        col++;
                        languageList.Add(lang.ToString());
                    }

                    for(int i = 2; i< ws.LastRowUsed().RowNumber(); i++)
                    {
                        var code = ws.Cell(i, 1).Value;

                        if (!string.IsNullOrEmpty(code.ToString()))
                        {
                            var codeLocalizations = localizations.Where(r => r.Code == code.ToString()).ToList();

                            for (int x = 0; x < languageList.Count; x++)
                            {
                                var languageData = languageList[x];
                                var lang = languageData.Split(' ')[1].TrimStart('(').TrimEnd(')');

                                var label = ws.Cell(i, 2 + x).Value;

                                if (!string.IsNullOrEmpty(label.ToString()))
                                {
                                    var model = new Localization();
                                    model = codeLocalizations.FirstOrDefault(r => r.LanguageCode == lang);
                                    
                                    if (model != null)
                                    {
                                        model.Label = label.ToString();
                                        model.EditedBy = user.GetDisplayName();
                                        model.EditedDate = DateTime.Now;
                                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                    }
                                    else
                                    {
                                        model = new Localization();
                                        model.Label = label.ToString();
                                        model.LanguageCode = lang;
                                        model.CreatedDate = DateTime.Now;
                                        model.CreatedBy = user.GetDisplayName();
                                        model.IsDeleted = "N";
                                        db.Entry(model).State = System.Data.Entity.EntityState.Added;
                                    }

                                }
                            }
                        }
                    }

                    db.SaveChanges();
                    success = true;
                    message = "OK";
                }
                
            }
            catch (Exception ex)
            {
                success = false;
                message = ex.Message;
            }

            result.success = success;
            result.message = message;

            return result;
        }


        public class EditorHelper
        {
            public LanguageLocalizationModel header { get; set; }

            public LocalizationModel detail { get; set; }
        }
    }
}
