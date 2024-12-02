using FAMS_API.Utilities;
using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Utilities;
using FAMS_ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FAMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuthorize]
    public class RunningNumberController : ControllerBase
    {
        DataEntities db = new DataEntities();

        [HttpPost]
        [Route("list")]
        public ResultData List(IndexParams request = null)
        {
            var response = new ResultData();
            bool success = false;
            string message = "";
            int totalRecords = 0;
            int pageSize = 10;
            int skip = request == null ? request.start : 0;
            string orderBy = "A.Id DESC";
            List<SqlParameter> parameters = new List<SqlParameter>(), parametert = new List<SqlParameter>();

            try
            {
                string query = @"SELECT DISTINCT *
                                        FROM RunningNumber A
                                        WHERE 1=1 {0}
                                        ORDER BY {3} {2} {1}";
                string whereQuery = "";
                string totalQuery = @"SELECT COUNT(A.Id) 
                                        From RunningNumber A 
                                        WHERE 1=1  {0}";

                if (request != null)
                {
                    if (request.filters != null && request.filters.Count > 0)
                    {
                        for (int i = 0; i < request.filters.Count; i++)
                        {
                            var filter = request.filters[i];

                            if (!string.IsNullOrEmpty(filter.value))
                            {
                                string columnName = filter.field;
                                string colName = columnName;
                                string tableAlias = "A.";
                                string filterValue = filter.value;

                                if (columnName.Contains("Date"))
                                {
                                    whereQuery += " AND FORMAT(" + tableAlias + columnName + ", 'dd/MM/yyyy HH:mm') LIKE @" + colName;
                                }
                                else
                                {
                                    if (columnName.Contains("Company_Name"))
                                    {
                                        tableAlias = "C.";
                                        columnName = "Name";
                                    }

                                    if (columnName.Contains("Company_Code"))
                                    {
                                        tableAlias = "C.";
                                        columnName = "Code";
                                    }

                                    whereQuery += " AND " + tableAlias + columnName + " LIKE @" + colName;
                                }

                                parameters.Add(new SqlParameter("@" + colName, "%" + filter.value + "%"));
                            }
                        }
                    }

                    if (request.sorts != null && request.sorts.Count > 0)
                    {
                        List<string> sortList = new List<string>();
                        for (int i = 0; i < request.sorts.Count; i++)
                        {
                            var sort = request.sorts[i];
                            string columnName = sort.field;
                            string tableAlias = "A.";
                            string sortBy = sort.order;

                            if (columnName.Contains("Company_Name"))
                            {
                                tableAlias = "C.";
                                columnName = "Name";
                            }

                            if (columnName.Contains("Company_Code"))
                            {
                                tableAlias = "C.";
                                columnName = "Code";
                            }

                            sortList.Add(tableAlias + columnName + " " + sortBy);
                        }
                        orderBy = String.Join(", ", sortList);
                    }
                }

                string fQuery = string.Format(query,
                                        whereQuery,
                                        (pageSize > 0 ? "FETCH NEXT " + pageSize.ToString() + " ROWS ONLY" : ""),
                                        (skip > -1 ? "OFFSET " + skip.ToString() + " ROWS" : ""),
                                        (string.IsNullOrEmpty(orderBy) ? "CreatedDate DESC" : orderBy)
                                    );

                var data = db.Database.SqlQuery<RunningNumberModel>(fQuery, parameters.ToArray()).ToList();

                foreach (SqlParameter prm in parameters)
                    parametert.Add(new SqlParameter(prm.ParameterName, prm.Value));

                var qt = db.Database.SqlQuery<Int32>(string.Format(totalQuery, whereQuery), parametert.ToArray()).ToArray();
                totalRecords = qt.Length > 0 ? qt[0] : 0;

                success = true;
                response.data = data;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            response.success = success;
            response.message = message;
            response.totalRecords = totalRecords;

            return response;

        }

        [HttpGet("{id}")]
        public ResultData GetData(long id)
        {
            var result = new ResultData();
            var detail = new RunningNumberDetailModel();

            try
            {
                var header = new RunningNumberModel();

                if (id != 0)
                {
                    header = new RunningNumberViewModel(db, id);
                }

                result.data = new EditorHelper()
                {
                    header = header,
                    detail = detail
                };
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        //[HttpPost]
        //public ResultData Save(RunningNumberModel modelData)
        //{
        //    var result = new ResultData();
        //    bool success = false;
        //    string message = "";

        //    try
        //    {
        //        var user = HttpContext.GetUser();

        //        RunningNumber model;
        //        if (modelData.mode == Constants.FORM_MODE_CREATE)
        //        {
        //            var isExist = db.RunningNumbers.Where(r => r.Company_Id == modelData.Company_Id).ToArray();
        //            if (isExist != null)
        //                throw new Exception("Company is already exist on Running Number.");

        //            foreach(var detail in modelData.Detail)
        //            {
        //                model = new RunningNumber();
        //                model.Company_Id = modelData.Company_Id;
        //                model.Trans_Type = detail.Trans_Type;
        //                model.RangeFromNumber = detail.RangeFromNumber;
        //                model.RangeToNumber = detail.RangeToNumber;
        //                model.Format = detail.Format;
        //                model.Create_Date = DateTime.Now;
        //                model.Create_By = user;
        //                db.Entry(model).State = System.Data.Entity.EntityState.Added;
        //            }
        //        }
        //        else
        //        {
        //            if(modelData.mode == Constants.FORM_MODE_EDIT)
        //            {
        //                foreach(var detail in modelData.Detail)
        //                {
        //                    if(detail.mode == Constants.FORM_MODE_CREATE)
        //                    {
        //                        model = new RunningNumber();
        //                        model.Create_Date = DateTime.Now;
        //                        model.Create_By = user;
        //                        db.Entry(model).State = System.Data.Entity.EntityState.Added;
        //                    }
        //                    else
        //                    {
        //                        model = db.RunningNumbers.Find(detail.Id);

        //                        if (detail.mode == Constants.FORM_MODE_EDIT)
        //                        {
        //                            model.Edit_Date = DateTime.Now;
        //                            model.Edit_By = user;
        //                            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
        //                        }

        //                        else if(detail.mode == Constants.FORM_MODE_DELETE)
        //                        {
        //                            db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
        //                        }
        //                    }

        //                    if(detail.mode != Constants.FORM_MODE_DELETE)
        //                    {
        //                        model.Company_Id = modelData.Company_Id;
        //                        model.Trans_Type = detail.Trans_Type;
        //                        model.RangeFromNumber = detail.RangeFromNumber;
        //                        model.RangeToNumber = detail.RangeToNumber;
        //                        model.Format = detail.Format;
        //                    }

        //                }
        //            }
        //        }

        //        using (var transaction = db.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                db.SaveChanges();
        //                transaction.Commit();

        //                message = "OK";
        //                success = true;

        //            }
        //            catch (Exception exc)
        //            {
        //                message = exc.Message;
        //                transaction.Rollback();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        success = false;
        //        message = ex.Message;
        //    }

        //    result.success = success;
        //    result.message = message;

        //    return result;
        //}

        public class EditorHelper
        {
            public RunningNumberModel header { get; set; }
            public RunningNumberDetailModel detail { get; set; }
        }
    }
}
