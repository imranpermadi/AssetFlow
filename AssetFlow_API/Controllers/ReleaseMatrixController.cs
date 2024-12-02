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
using System.Threading.Tasks;

namespace FAMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuthorize]
    public class ReleaseMatrixController : ControllerBase
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
                                    FROM ReleaseMatrix A 
                                    WHERE 1=1 {0}
                                    ORDER BY {3} {2} {1}";
                string whereQuery = "";
                string totalQuery = "SELECT COUNT(A.Id) From ReleaseMatrix A WHERE 1=1  {0}";

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
                                        (string.IsNullOrEmpty(orderBy) ? "Created_Date DESC" : orderBy)
                                    );

                var data = db.Database.SqlQuery<ReleaseMatrixModel>(fQuery, parameters.ToArray()).ToList();

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
                var header = new ReleaseMatrixModel();

                if (id != 0)
                {
                    header = new ReleaseMatrixViewModel(db, id);
                }

                result.data = new EditorHelper()
                {
                    header = header,
                    detail = new ReleaseMatrixDetailModel()
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
        public ResultData Save(ReleaseMatrixModel modelData)
        {
            var result = new ResultData();
            bool success = false;
            string message = "";

            try
            {
                var user = HttpContext.GetUserData();

                ReleaseMatrix model;
                if (modelData.mode == Constants.FORM_MODE_CREATE)
                {
                    //var existedData = db.Materials.Where(r =>
                    //                        r.Material_Code == modelData.Material_Code
                    //                    ).ToArray();

                    //if (existedData != null && existedData.Length > 0)
                    //{
                    //    throw new Exception("Company code already exists!");
                    //}

                    model = new ReleaseMatrix();
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = user.GetDisplayName();
                    model.IsDeleted = "N";
                    db.Entry(model).State = System.Data.Entity.EntityState.Added;
                }
                else
                {
                    model = db.ReleaseMatrix.Find(modelData.Id);

                    if (modelData.mode == Constants.FORM_MODE_DELETE)
                    {
                        model.DeletedDate = DateTime.Now;
                        model.DeletedBy = user.GetDisplayName();
                        model.IsDeleted = "Y";
                    }
                    else if (modelData.mode == Constants.FORM_MODE_EDIT)
                    {
                        model.EditedDate = DateTime.Now;
                        model.EditedBy = user.GetDisplayName();
                    }
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                }

                if (modelData.mode != Constants.FORM_MODE_DELETE)
                {
                    //model.Mode = modelData.Mode;
                    //model.Type = modelData.Type;
                    //model.Company_Code = modelData.Company_Code;
                    //model.Business_Segment_Code = modelData.Business_Segment_Code;
                    //model.Location = modelData.Location;
                    //model.Order_Type = modelData.Order_Type;
                    //model.Purchase_Organization_Code = modelData.Purchase_Organization_Code;
                    //model.Truck_Type = modelData.Truck_Type;
                    //model.Validity = modelData.Validity;
                    //model.Distribution = modelData.Distribution;
                    //model.Document_Type = modelData.Document_Type;
                    //model.Location = modelData.Location;
                    //model.Company = modelData.Company;
                    //model.Storage_Location = modelData.Storage_Location;
                    //model.Department= modelData.Department;
                    modelData.Validity = modelData.Validity;
                    model.Type = modelData.Type;
                    //model.Category= modelData.Category;

                    model.Description = modelData.Description;
                    model.Validity = modelData.Validity;
                    //model.Info1 = modelData.Info1;
                    //model.Info2 = modelData.Info2;
                    model.IsDeleted = "N";
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (modelData.Details != null)
                        {
                            if (modelData.Details.Count == 0)
                                throw new Exception("At least one or more approver required");

                            var deleted = modelData.Details.Where(r => r.mode == Constants.FORM_MODE_DELETE).ToArray();
                            if (deleted.Length == modelData.Details.Count)
                                throw new Exception("At least one or more approver required");

                            foreach (var Detail in modelData.Details)
                            {
                                if (modelData.mode == Constants.FORM_MODE_DELETE)
                                {
                                    Detail.mode = modelData.mode;
                                }

                                if (Detail.mode == Constants.FORM_MODE_UNCHANGED)
                                {
                                    Detail.mode = Constants.FORM_MODE_EDIT;
                                }

                                if (Detail.mode != Constants.FORM_MODE_UNCHANGED)
                                {
                                    //bool saved = SaveDetail(db, Detail, modelData.Details, model, user, out message);
                                    //if (!saved)
                                    //    throw new Exception(message);
                                }

                            }
                        }

                        db.SaveChanges();
                        transaction.Commit();

                        message = string.Format(Resources.MD_APPROVAL_MATRIX_SAVE_SUCCESS, model);
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

        #region HelperFunctions
        //private bool SaveDetail(DataEntities db, ReleaseMatrixDetailModel currentDetail, List<ReleaseMatrixDetailModel> allDetails, ReleaseMatrix header, User user, out string message)
        //{
        //    bool result = false;
        //    message = "";
        //    ReleaseMatrixDetail model;

        //    try
        //    {
        //        if (currentDetail.mode == Constants.FORM_MODE_CREATE)
        //        {
        //            model = new ReleaseMatrixDetail();
        //            model.CreatedBy = user.GetDisplayName();
        //            model.CreatedDate = DateTime.Now;
        //        }
        //        else
        //        {
        //            model = db.ReleaseMatrixDetail.Find(currentDetail.Id);

        //            if (currentDetail.mode == Constants.FORM_MODE_DELETE)
        //            {
        //                model.IsDeleted = "Y";
        //                model.DeletedDate = DateTime.Now;
        //                model.DeletedBy = user.GetDisplayName();
        //            }
        //            else if (currentDetail.mode == Constants.FORM_MODE_EDIT)
        //            {
        //                model.EditedDate = DateTime.Now;
        //                model.EditedBy = user.GetDisplayName();
        //            }
        //        }

        //        if (currentDetail.mode != Constants.FORM_MODE_DELETE)
        //        {

        //            model.ReleaseMatrix = header;
        //            model.ApprovalAs = currentDetail.ApprovalAs;
        //            model.Username = currentDetail.Username;
        //            model.Level = currentDetail.Level;
        //            model.Value = currentDetail.Value;

        //            var userApprover = db.User.Where(r => r.Username == currentDetail.Username && r.IsDeleted != "Y").FirstOrDefault();
        //            if(userApprover != null)
        //            {
        //                model.Email = userApprover.Email;
        //                model.Fullname = userApprover.Fullname;
        //            }

        //            model.IsDeleted = "N";
        //        }

        //        if (currentDetail.mode == Constants.FORM_MODE_CREATE)
        //            db.Entry(model).State = System.Data.Entity.EntityState.Added;
        //        else if (currentDetail.mode == Constants.FORM_MODE_EDIT)
        //            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
        //        else if (currentDetail.mode == Constants.FORM_MODE_DELETE)
        //            db.Entry(model).State = System.Data.Entity.EntityState.Modified;

        //        result = true;
        //        message = "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        message = ex.Message;
        //    }

        //    return result;
        //}
        #endregion



        public class EditorHelper
        {
            public ReleaseMatrixModel header { get; set; }

            public ReleaseMatrixDetailModel detail { get; set; }
        }

    }
}
