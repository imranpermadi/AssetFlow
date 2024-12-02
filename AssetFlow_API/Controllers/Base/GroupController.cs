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
    public class GroupController : ControllerBase
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
            int pageSize = 15;
            int skip = request == null ? request.start : 0;
            string orderBy = "A.Id DESC";
            List<SqlParameter> parameters = new List<SqlParameter>(), parametert = new List<SqlParameter>();

            try
            {
                string query = @"SELECT * 
                                    FROM [Group] A 
                                    WHERE 1=1 {0}
                                    ORDER BY {3} {2} {1}";
                string whereQuery = "";
                string totalQuery = "SELECT COUNT(A.Id) From [Group] A WHERE 1=1  {0}";

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
                                        (string.IsNullOrEmpty(orderBy) ? "CreatedDate DESC" : orderBy)
                                    );

                var data = db.Database.SqlQuery<GroupModel>(fQuery, parameters.ToArray()).ToList();

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
                var header = new GroupViewModel();

                if (id != 0)
                {
                    header = new GroupViewModel(db, id);
                }

                result.data = new EditorHelper()
                {
                    header = header,
                    detail = new GroupMenuModel()
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
        public ResultData Save(GroupModel modelData)
        {
            var result = new ResultData();
            bool success = false;
            string message = "";

            try
            {
                var user = HttpContext.GetUserData().UserData();

                Group model;
                if (modelData.mode == Constants.FORM_MODE_CREATE)
                {
                    var existedData = db.Group.Where(r =>
                                            r.Code == modelData.Code
                                        ).ToArray();

                    if (existedData != null && existedData.Length > 0)
                    {
                        throw new Exception("Group already exists!");
                    }

                    model = new Group();
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = user.GetDisplayName();
                    db.Entry(model).State = System.Data.Entity.EntityState.Added;
                }
                else
                {
                    model = db.Group.Find(modelData.Id);

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
                    model.Code = modelData.Code;
                    model.Description = modelData.Description;
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (modelData.Details != null)
                        {
                            foreach (var Detail in modelData.Details)
                            {
                                if (modelData.mode == Constants.FORM_MODE_DELETE)
                                {
                                    Detail.mode = modelData.mode;
                                }

                                if (Detail.mode != Constants.FORM_MODE_UNCHANGED)
                                {
                                    bool saved = SaveMenu(db, Detail, modelData.Details, model, user, out message);
                                    if (!saved)
                                        throw new Exception(message);
                                }

                            }
                        }

                        db.SaveChanges();
                        transaction.Commit();

                        message = string.Format(Resources.MD_GROUP_SAVE_SUCCESS, modelData.Code);
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


        [NonAction]
        public bool SaveMenu(DataEntities db, GroupMenuModel currentDetail, List<GroupMenuModel> allDetails, Group group, UserData currentUser, out string message)
        {
            bool result = false;
            message = "";
            GroupMenu model;

            try
            {
                if (currentDetail.mode == Constants.FORM_MODE_CREATE)
                {
                    model = new GroupMenu();
                    model.CreatedBy = currentUser.GetDisplayName();
                    model.CreatedDate = DateTime.Now;
                    model.IsDeleted = "N";
                }
                else
                {
                    model = db.GroupMenu.Find(currentDetail.Id);

                    if (currentDetail.mode == Constants.FORM_MODE_DELETE)
                    {
                        model.IsDeleted = "Y";
                        model.DeletedDate = DateTime.Now;
                        model.DeletedBy = currentUser.GetDisplayName();
                        db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
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
                    model.Group = group;

                    var menu = db.Menu.Where(r => r.Id == currentDetail.MenuId).FirstOrDefault();


                    if (menu == null)
                        throw new Exception(Resources.MENU_INVALID);

                    var conflictingTime = allDetails.Where(r =>
                                                            r.Id != currentDetail.Id &&
                                                            r.MenuId == currentDetail.MenuId
                                                            && r.mode != Constants.FORM_MODE_DELETE
                                                        ).FirstOrDefault();

                    if (conflictingTime != null)
                        throw new Exception("Duplicate Menu " + menu.MenuName + " found");

                    //model.Time_From = this.Time_From;
                    //model.Time_To = this.Time_To;
                    //model.Slot_Name = this.Slot_Name;
                    //model.Max_Slot = this.Max_Slot;

                    if(currentDetail.mode == Constants.FORM_MODE_CREATE)
                    {
                        var existing = db.GroupMenu.Where(r =>
                                            r.GroupId == model.GroupId &&
                                            r.MenuId == currentDetail.MenuId
                                        //&& !r.Is_Deleted
                                        ).FirstOrDefault();
                        if (existing != null)
                            throw new Exception("Menu " + menu.MenuName + " has been added previously for this group");
                    }

                    model.MenuId = menu.Id;
                    model.View = currentDetail.View;
                    model.Create = currentDetail.Create;
                    model.Edit = currentDetail.Edit;
                    model.Delete = currentDetail.Delete;
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


        public class EditorHelper
        {
            public GroupModel header { get; set; }
            public GroupMenuModel detail { get; set; }
        }

    }
}
