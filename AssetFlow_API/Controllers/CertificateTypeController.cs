using FAMS_API.Utilities;
using FAMS_Data;
using FAMS_Models.Utilities;
using FAMS_Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Linq;
using FAMS_ViewModels;
using FAMS_Models.Resources;

namespace FAMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuthorize]
    public class CertificateTypeController : Controller
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
            int pageSize = request.pageSize;
            int skip = request != null ? request.start : 0;
            string orderBy = "A.CreatedDate DESC";
            List<SqlParameter> parameters = new List<SqlParameter>(), parametert = new List<SqlParameter>();

            try
            {
                string query = @"SELECT A.*
                                    FROM [CertificateType] A 
                                    WHERE 1=1 {0}
                                    ORDER BY {3} {2} {1}";
                string whereQuery = "";
                string totalQuery = @"SELECT COUNT(A.Id) From [CertificateType] A 
                                    WHERE 1=1 {0}";

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

                                if (columnName.Contains("Date") || columnName.Contains("date"))
                                {
                                    whereQuery += " AND FORMAT(" + tableAlias + columnName + ", 'yyyy-MM-dd') LIKE @" + colName;
                                    DateTime dt = Convert.ToDateTime(filter.value);
                                    parameters.Add(new SqlParameter("@" + colName, "%" + dt.ToString("yyyy-MM-dd") + "%"));
                                }
                                else
                                {
                                    whereQuery += " AND " + tableAlias + columnName + " LIKE @" + colName;
                                    parameters.Add(new SqlParameter("@" + colName, "%" + filter.value + "%"));
                                }
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
                                        (string.IsNullOrEmpty(orderBy) ? "CreatedDate DESC" : orderBy)
                                    );

                var data = db.Database.SqlQuery<CertificateTypeModel>(fQuery, parameters.ToArray()).ToList();

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
                var header = new CertificateTypeModel();

                if (id != 0)
                    header = new CertificateTypeViewModel(db, id);

                result.data = new EditorHelper()
                {
                    header = header
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
        public ResultData Save(CertificateTypeModel modelData)
        {
            var result = new ResultData();
            bool success = false;
            string message = "";

            try
            {
                var user = HttpContext.GetUserData();

                CertificateType model = new CertificateType();

                if (modelData.mode == Constants.FORM_MODE_CREATE)
                {
                    var existedData = db.CertificateType.Where(r => r.Code == modelData.Code && r.IsDeleted == "N").ToArray();

                    if (existedData != null && existedData.Length > 0 && modelData.mode == Constants.FORM_MODE_CREATE)
                        throw new Exception(string.Format(Resources.EXIST_DATA, "Certificate Type " + modelData.Code));

                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = user.GetDisplayName();
                    model.IsDeleted = "N";
                    db.Entry(model).State = System.Data.Entity.EntityState.Added;
                }
                else
                {
                    model = db.CertificateType.Find(modelData.Id);

                    if (modelData.mode == Constants.FORM_MODE_EDIT)
                    {
                        model.EditedDate = DateTime.Now;
                        model.EditedBy = user.GetDisplayName();
                    }
                    else if (modelData.mode == Constants.FORM_MODE_DELETE)
                    {
                        if (UUtilsApi.HasTransaction(typeof(CertificateType), model.Id, out message))
                            throw new Exception(string.Format(Resources.USED_ON_TRANSACTION, "Certificate Type", "code", message));

                        model.DeletedDate = DateTime.Now;
                        model.DeletedBy = user.GetDisplayName();
                        model.IsDeleted = "Y";
                    }

                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                }

                if (modelData.mode != Constants.FORM_MODE_DELETE)
                {
                    model.Description = modelData.Description;
                    model.Code = modelData.Code;
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();

                        message = "OK";
                        success = true;
                    }
                    catch (Exception exc)
                    {
                        message = exc.Message;
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
            public CertificateTypeModel header { get; set; }
        }
    }
}
