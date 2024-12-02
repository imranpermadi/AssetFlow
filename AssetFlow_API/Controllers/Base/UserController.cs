using FAMS_API.Utilities;
using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Resources;
using FAMS_Models.Utilities;
using FAMS_ViewModels;
using FAMS_ViewModels.Utilities;
using DocumentFormat.OpenXml.Bibliography;
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
    public class UserController : ControllerBase
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
                                    FROM [User] A 
                                    WHERE 1=1 {0}
                                    ORDER BY {3} {2} {1}";
                string whereQuery = "";
                string totalQuery = "SELECT COUNT(A.Id) From [User] A WHERE 1=1  {0}";

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
                                        (string.IsNullOrEmpty(orderBy) ? "Created_Date DESC" : orderBy)
                                    );

                var data = db.Database.SqlQuery<UserModel>(fQuery, parameters.ToArray()).ToList();

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
                var header = new UserModel();

                if (id != 0)
                {
                    header = new UserViewModel(db, id);
                }
                else
                {
                    header.Groups = new List<UserGroupModel>();
                }

                result.data = new EditorHelper()
                {
                    header = header,
                    group = new UserGroupModel(),
                    company = new UserCompanyModel(),
                    //department = new UserDepartmentModel(),
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
        public ResultData Save(UserModel modelData)
        {
            var result = new ResultData();
            bool success = false;
            string message = "";

            try
            {
                var user = HttpContext.GetUserData().UserData();

                User model;
                if (modelData.mode == Constants.FORM_MODE_CREATE)
                {
                    //var existedData = db.Materials.Where(r =>
                    //                        r.Material_Code == modelData.Material_Code
                    //                    ).ToArray();

                    //if (existedData != null && existedData.Length > 0)
                    //{
                    //    throw new Exception("Company code already exists!");
                    //}

                    model = new User();
                    model.FirstLogin = "Y";
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = user.GetDisplayName();
                    db.Entry(model).State = System.Data.Entity.EntityState.Added;
                }
                else
                {
                    model = db.User.Find(modelData.Id);

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
                    model.Username = modelData.Username;
                    model.Fullname = modelData.Fullname;
                    model.Email = modelData.Email;
                    model.UseAD = modelData.UseAD;
                    //model.CompanyAll = modelData.CompanyAll;
                    //model.Location_Code = modelData.Location_Code;
                    model.IsDeleted = "N";

                   
                    if (model.UseAD == "Y" && string.IsNullOrEmpty(modelData.Password))
                    {
                        string nPass = "";
                        string passMessage = "";
                        var complexity = UAuth.CheckPassword(model.Password, out passMessage, out nPass);
                        if (!complexity)
                        {
                            throw new Exception(passMessage);
                        }
                        model.Password = UEncryption.ComputeSha256Hash(nPass);
                    }
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (modelData.Groups != null)
                        {
                            foreach (var Detail in modelData.Groups)
                            {
                                if (modelData.mode == Constants.FORM_MODE_DELETE)
                                {
                                    Detail.mode = modelData.mode;
                                }

                                if (Detail.mode != Constants.FORM_MODE_UNCHANGED)
                                {
                                    bool saved = SaveGroups(db, Detail, modelData.Groups, model, user, out message);
                                    if (!saved)
                                        throw new Exception(message);
                                }

                            }
                        }

                        //if (modelData.Companies != null)
                        //{
                        //    foreach (var Detail in modelData.Companies)
                        //    {
                        //        if (modelData.mode == Constants.FORM_MODE_DELETE)
                        //        {
                        //            Detail.mode = modelData.mode;
                        //        }

                        //        if (Detail.mode != Constants.FORM_MODE_UNCHANGED)
                        //        {
                        //            bool saved = SaveCompany(db, Detail, modelData.Companies, model, user, out message);
                        //            if (!saved)
                        //                throw new Exception(message);
                        //        }
                        //    }
                        //}


                        //if (modelData.Departments != null)
                        //{
                        //    foreach (var Detail in modelData.Departments)
                        //    {
                        //        if (modelData.mode == Constants.FORM_MODE_DELETE)
                        //        {
                        //            Detail.mode = modelData.mode;
                        //        }

                        //        if (Detail.mode != Constants.FORM_MODE_UNCHANGED)
                        //        {
                        //            bool saved = SaveDepartment(db, Detail, modelData.Departments, model, user, out message);
                        //            if (!saved)
                        //                throw new Exception(message);
                        //        }
                        //    }
                        //}


                        db.SaveChanges();
                        transaction.Commit();

                        message = string.Format(Resources.MD_USER_SAVE_SUCCESS, model.Username);
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


        [HttpPost]
        [Route("reset_password")]
        public ResultData ResetPassword(UserModel modelData)
        {
            var result = new ResultData();
            bool success = false;
            string message = "";

            try
            {
                User model;
                string nPass = "";
                string passMessage = "";


                var complexity = UAuth.CheckPassword("", out passMessage, out nPass);
                if (!complexity)
                {
                    throw new Exception(passMessage);
                }

                model = db.User.Find(modelData.Id);
                model.Password = UEncryption.ComputeSha256Hash(nPass);
                model.FirstLogin = "Y";

                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                success = true;
                message = "Password has been reset.";
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

        [NonAction]
        private bool SaveGroups(DataEntities db, UserGroupModel currentDetail, List<UserGroupModel> allDetails, User user, UserData currentUser, out string message)
        {
            bool result = false;
            message = "";
            UserGroup model;

            try
            {
                if (currentDetail.mode == Constants.FORM_MODE_CREATE)
                {
                    model = new UserGroup();
                    model.CreatedBy = currentUser.GetDisplayName();
                    model.CreatedDate = DateTime.Now;
                    //model.Is_Deleted = false;
                }
                else
                {
                    model = db.UserGroup.Find(currentDetail.Id);

                    if (currentDetail.mode == Constants.FORM_MODE_DELETE)
                    {
                        //model.Is_Deleted = true;
                        //model.Delete_Date = DateTime.Now;
                        //model.Deleted_By = user.GetDisplayName();
                    }
                    else if (currentDetail.mode == Constants.FORM_MODE_EDIT)
                    {
                        //model.Is_Deleted = false;
                        model.EditedDate = DateTime.Now;
                        model.EditedBy = currentUser.GetDisplayName();
                    }
                }

                if (currentDetail.mode != Constants.FORM_MODE_DELETE)
                {   
                    model.User = user;

                    var group = db.Group.Where(r => r.Id == currentDetail.GroupId).FirstOrDefault();
                    if (group == null)
                        throw new Exception(Resources.GROUP_INVALID);


                    var conflictingTime = allDetails.Where(r =>
                                                            r.Id != currentDetail.Id &&
                                                            r.GroupId == currentDetail.GroupId
                                                            && r.mode != Constants.FORM_MODE_DELETE
                                                        ).FirstOrDefault();

                    if (conflictingTime != null)
                        throw new Exception("Duplicate Group " + group.Description + " found");

                    //model.Time_From = this.Time_From;
                    //model.Time_To = this.Time_To;
                    //model.Slot_Name = this.Slot_Name;
                    //model.Max_Slot = this.Max_Slot;

                    var existing = db.UserGroup.Where(r =>
                                            r.UserId == model.UserId &&
                                            r.GroupId == currentDetail.GroupId 
                                            //&& !r.Is_Deleted
                                        ).FirstOrDefault();
                    if (existing != null)
                        throw new Exception("Group " + group.Description + " has been added previously for this user");


                    model.GroupId = currentDetail.GroupId;
                    //businessSegmentName = businessSegment.Description;
                }

                if (currentDetail.mode == Constants.FORM_MODE_CREATE)
                    db.Entry(model).State = System.Data.Entity.EntityState.Added;
                else if (currentDetail.mode == Constants.FORM_MODE_EDIT)
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                else if (currentDetail.mode == Constants.FORM_MODE_DELETE)
                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;


                result = true;
                message = "OK";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return result;
        }

        //[NonAction]
        //public bool SaveCompany(DataEntities db, UserCompanyModel currentDetail, List<UserCompanyModel> allDetails, User user, UserData currentUser, out string message)
        //{
        //    bool result = false;
        //    message = "";
        //    UserCompany model;

        //    try
        //    {
        //        if (currentDetail.mode == Constants.FORM_MODE_CREATE)
        //        {
        //            model = new UserCompany();
        //            model.Created_By = currentUser.GetDisplayName();
        //            model.Created_Date = DateTime.Now;
        //            model.Is_Deleted = "N";
        //        }
        //        else
        //        {
        //            model = db.UserCompany.Find(currentDetail.Id);

        //            if (currentDetail.mode == Constants.FORM_MODE_DELETE)
        //            {
        //                model.Is_Deleted = "Y";
        //                model.Deleted_Date = DateTime.Now;
        //                model.Deleted_By = currentUser.GetDisplayName();
        //            }
        //            else if (currentDetail.mode == Constants.FORM_MODE_EDIT)
        //            {
        //                model.Is_Deleted = "N";
        //                model.Edited_Date = DateTime.Now;
        //                model.Edited_By = currentUser.GetDisplayName();
        //            }
        //        }

        //        if (currentDetail.mode != Constants.FORM_MODE_DELETE)
        //        {
        //            model.User = user;

        //            var company = db.Company.Where(r => r.Id == currentDetail.CompanyId).FirstOrDefault();
        //            if (company == null)
        //                throw new Exception(Resources.COMPANY_INVALID);


        //            var conflictingTime = allDetails.Where(r =>
        //                                                    r.Id != currentDetail.Id &&
        //                                                    r.CompanyId == currentDetail.CompanyId
        //                                                    && r.mode != Constants.FORM_MODE_DELETE
        //                                                ).FirstOrDefault();

        //            if (conflictingTime != null)
        //                throw new Exception("Duplicate Company " + company.Code + " found");

        //            //model.Time_From = this.Time_From;
        //            //model.Time_To = this.Time_To;
        //            //model.Slot_Name = this.Slot_Name;
        //            //model.Max_Slot = this.Max_Slot;

        //            var existing = db.UserCompany.Where(r =>
        //                                    r.UserId == model.User_Id &&
        //                                    r.CompanyId == currentDetail.CompanyId
        //                                    && r.Is_Deleted != "Y"
        //                                ).FirstOrDefault();
        //            if (existing != null)
        //                throw new Exception("Company " + company.Code + " has been added previously for this user");


        //            model.CompanyId = currentDetail.CompanyId;
        //            //businessSegmentName = businessSegment.Description;
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


        //[NonAction]
        //public bool SaveDepartment(DMSEntities db, UserDepartmentModel currentDetail, List<UserDepartmentModel> allDetails, User user, UserData currentUser, out string message)
        //{
        //    bool result = false;
        //    message = "";
        //    User_Department model;

        //    try
        //    {
        //        if (currentDetail.mode == Constants.FORM_MODE_CREATE)
        //        {
        //            model = new User_Department();
        //            model.Created_By = currentUser.GetDisplayName();
        //            model.Created_Date = DateTime.Now;
        //            model.Is_Deleted = "N";
        //        }
        //        else
        //        {
        //            model = db.User_Department.Find(currentDetail.Id);

        //            if (currentDetail.mode == Constants.FORM_MODE_DELETE)
        //            {
        //                model.Is_Deleted = "Y";
        //                model.Deleted_Date = DateTime.Now;
        //                model.Deleted_By = currentUser.GetDisplayName();
        //            }
        //            else if (currentDetail.mode == Constants.FORM_MODE_EDIT)
        //            {
        //                model.Is_Deleted = "N";
        //                model.Edited_Date = DateTime.Now;
        //                model.Edited_By = currentUser.GetDisplayName();
        //            }
        //        }

        //        if (currentDetail.mode != Constants.FORM_MODE_DELETE)
        //        {
        //            model.User = user;

        //            var department = db.Departments.Where(r => r.Id == currentDetail.Department_Id).FirstOrDefault();
        //            if (department == null)
        //                throw new Exception(Resources.COMPANY_INVALID);

                  
        //            var conflictingTime = allDetails.Where(r =>
        //                                                    r.Id != currentDetail.Id &&
        //                                                    r.Department_Id == currentDetail.Department_Id
        //                                                    && r.mode != Constants.FORM_MODE_DELETE
        //                                                ).FirstOrDefault();

        //            if (conflictingTime != null)
        //                throw new Exception("Duplicate Department " + department.Code + " found");

        //            //model.Time_From = this.Time_From;
        //            //model.Time_To = this.Time_To;
        //            //model.Slot_Name = this.Slot_Name;
        //            //model.Max_Slot = this.Max_Slot;

        //            var existing = db.User_Department.Where(r =>
        //                                    r.User_Id == model.User_Id &&
        //                                    r.Department_Id == currentDetail.Department_Id
        //                                    && r.Is_Deleted != "Y"
        //                                ).FirstOrDefault();
        //            if (existing != null)
        //                throw new Exception("Department " + department.Code + " has been added previously for this user");


        //            model.Department_Id = currentDetail.Department_Id;
        //            //businessSegmentName = businessSegment.Description;
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

        public class EditorHelper
        {
            public UserModel header { get; set; }
            public UserGroupModel group { get; set; }
            public UserCompanyModel company { get; set; }
            //public UserDepartmentModel department { get; set; }
        }
    }
}
