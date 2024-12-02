using FAMS_API.Utilities;
using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Resources;
using FAMS_Models.Utilities;
using FAMS_ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace FAMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuthorize]
    public class LookupController : ControllerBase
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
            int skip = request != null ? request.start : 0;
            string orderBy = "A.Id DESC";
            List<SqlParameter> parameters = new List<SqlParameter>(), parametert = new List<SqlParameter>();

            try
            {
                string query = @"SELECT * 
                                    FROM [Lookup] A 
                                    WHERE 1=1 {0}
                                    ORDER BY {3} {2} {1}";
                string whereQuery = "";
                string totalQuery = "SELECT COUNT(A.Id) From [Lookup] A WHERE 1=1  {0}";

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

                                //if (columnName.Contains("Is_Deleted"))
                                //{
                                //    whereQuery += $" AND ISNULL({tableAlias}{columnName},0) = {(filterValue == "Y" ? "1" : "0")}";
                                //}
                                //else
                                //{
                                //    if (columnName.Contains("Date"))
                                //    {
                                //        whereQuery += " AND FORMAT(" + tableAlias + columnName + ", 'dd/MM/yyyy HH:mm') LIKE @" + colName;
                                //    }
                                //    else
                                //    {
                                //        whereQuery += " AND " + tableAlias + columnName + " LIKE @" + colName;
                                //    }

                                //    parameters.Add(new SqlParameter("@" + colName, "%" + filter.value + "%"));
                                //}

                                if (columnName.Contains("Date"))
                                {
                                    whereQuery += " AND FORMAT(" + tableAlias + columnName + ", 'dd/MM/yyyy HH:mm') LIKE @" + colName;
                                }
                                else
                                {
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

                            sortList.Add(tableAlias + columnName + " " + sortBy);
                        }
                        orderBy = String.Join(", ", sortList);
                    }
                }

                string fQuery = string.Format(query,
                                        whereQuery,
                                        (pageSize > 0 ? "FETCH NEXT " + pageSize.ToString() + " ROWS ONLY" : ""),
                                        (skip > -1 ? "OFFSET " + skip.ToString() + " ROWS" : ""),
                                        (string.IsNullOrEmpty(orderBy) ? "Id DESC" : orderBy)
                                    );

                var data = db.Database.SqlQuery<LookupModel>(fQuery, parameters.ToArray()).ToList();

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

            try
            {
                var header = new LookupModel();

                if (id != 0)
                {
                    header = new LookupViewModel(db, id);
                }

                result.data = new EditorHelper()
                {
                    header = header,
                };
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        [HttpPost]
        public ResultData Save(LookupModel modelData)
        {
            var result = new ResultData();
            bool success = false;
            string message = "";

            try
            {
                var user = HttpContext.GetUser();

                Lookup model;
                if (modelData.mode == Constants.FORM_MODE_CREATE)
                {
                    //var existedData = db.Materials.Where(r =>
                    //                        r.Material_Code == modelData.Material_Code
                    //                    ).ToArray();

                    //if (existedData != null && existedData.Length > 0)
                    //{
                    //    throw new Exception("Company code already exists!");
                    //}

                    model = new Lookup();
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = user;
                    db.Entry(model).State = System.Data.Entity.EntityState.Added;
                }
                else
                {
                    model = db.Lookup.Find(modelData.Id);

                    if (modelData.mode == Constants.FORM_MODE_DELETE)
                    {
                        model.DeletedDate = DateTime.Now;
                        model.DeletedBy = user;
                        model.IsDeleted = "Y";
                    }
                    else if (modelData.mode == Constants.FORM_MODE_EDIT)
                    {
                        model.EditedDate = DateTime.Now;
                        model.EditedBy = user;
                    }
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                }

                if (modelData.mode != Constants.FORM_MODE_DELETE)
                {

                    model.LookupGroup = modelData.LookupGroup;
                    model.LookupKey = modelData.LookupKey;
                    model.LookupValue = modelData.LookupValue;
                    model.LookupInfo1 = modelData.LookupInfo1;
                    model.LookupInfo2 = modelData.LookupInfo2;
                    model.IsDeleted = "N";
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();

                        message = string.Format(Resources.MD_LOOKUP_SAVE_SUCCESS, model.LookupKey);
                        success = true;

                    }
                    catch (Exception exc)
                    {
                        message = exc.Message;
                        //result = "Failed to save data.<br/>Error Message : " + exc_.Message.Replace(Environment.NewLine, " ").Replace("'", "");
                        transaction.Rollback();
                    }
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
            public LookupModel header { get; set; }
        }
    }
}
