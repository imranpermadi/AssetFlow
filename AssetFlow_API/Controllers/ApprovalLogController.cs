using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Utilities;
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
    public class ApprovalLogController : ControllerBase
    {

        DataEntities db = new DataEntities();

        //[HttpPost]
        //[Route("list")]
        //public ResultData List(IndexParams request = null)
        //{
        //    var response = new ResultData();
        //    bool success = false;
        //    string message = "";
        //    int totalRecords = 0;
        //    int pageSize = 15;
        //    int skip = request != null ? request.start : 0;
        //    string orderBy = "A.Id DESC";
        //    List<SqlParameter> parameters = new List<SqlParameter>(), parametert = new List<SqlParameter>();

        //    try
        //    {
        //        string query = @"SELECT A.Id,
        //                                B.Reference_No,
		      //                          B.Category,
		      //                          B.Mode,
		      //                          B.Info1,
		      //                          B.Info2,
		      //                          B.Status,
		      //                          B.Last_Approval_By,
		      //                          B.Last_Approval_Date,
		      //                          B.Active,
		      //                          B.Created_Date,
		      //                          B.Created_By,
		      //                          B.Remarks,
		      //                          B.Reference_No1,
		      //                          B.Reference_No2,
		      //                          A.Status AS Detail_Status,
		      //                          A.Username AS Detail_Username,
		      //                          A.Info1 AS Detail_Info1,
		      //                          A.Info2 AS Detail_Info2,
		      //                          A.Level AS Detail_Level,
		      //                          A.Approval_Date AS Detail_Approval_Date,
		      //                          A.Approval_By AS Detail_Approval_By
	       //                         FROM Approval_Detail A
	       //                         INNER JOIN Approval B ON B.Id = A.Approval_Id 
        //                            WHERE 1=1 {0}
        //                            ORDER BY {3} {2} {1}";
        //        string whereQuery = "";
        //        string totalQuery = "SELECT COUNT(A.Id) From [Approval_Detail] A INNER JOIN Approval B ON B.Id = A.Approval_Id WHERE 1=1  {0}";

        //        if (request != null)
        //        {
        //            if (request.filters != null && request.filters.Count > 0)
        //            {
        //                for (int i = 0; i < request.filters.Count; i++)
        //                {
        //                    var filter = request.filters[i];

        //                    if (!string.IsNullOrEmpty(filter.value))
        //                    {
        //                        string columnName = filter.field;
        //                        string colName = columnName;
        //                        string tableAlias = "B.";
        //                        string filterValue = filter.value;

        //                        if (columnName.Contains("Date"))
        //                        {
        //                            whereQuery += " AND FORMAT(" + tableAlias + columnName + ", 'dd/MM/yyyy HH:mm') LIKE @" + colName;
        //                        }
        //                        else
        //                        {
        //                            if (columnName.Contains("Detail_"))
        //                            {
        //                                tableAlias = "A.";
        //                                columnName = columnName.Replace("Detail_", "");
        //                            }

        //                            whereQuery += " AND " + tableAlias + columnName + " LIKE @" + colName;
        //                        }

        //                        parameters.Add(new SqlParameter("@" + colName, "%" + filter.value + "%"));
        //                    }
        //                }
        //            }

        //            if (request.sorts != null && request.sorts.Count > 0)
        //            {
        //                List<string> sortList = new List<string>();
        //                for (int i = 0; i < request.sorts.Count; i++)
        //                {
        //                    var sort = request.sorts[i];
        //                    string columnName = sort.field;
        //                    string tableAlias = "A.";
        //                    string sortBy = sort.order;

        //                    sortList.Add(tableAlias + columnName + " " + sortBy);
        //                }
        //                orderBy = String.Join(", ", sortList);
        //            }
        //        }

        //        string fQuery = string.Format(query,
        //                                whereQuery,
        //                                (pageSize > 0 ? "FETCH NEXT " + pageSize.ToString() + " ROWS ONLY" : ""),
        //                                (skip > -1 ? "OFFSET " + skip.ToString() + " ROWS" : ""),
        //                                (string.IsNullOrEmpty(orderBy) ? "Id DESC" : orderBy)
        //                            );

        //        var data = db.Database.SqlQuery<ApprovalLogModel>(fQuery, parameters.ToArray()).ToList();

        //        foreach (SqlParameter prm in parameters)
        //            parametert.Add(new SqlParameter(prm.ParameterName, prm.Value));

        //        var qt = db.Database.SqlQuery<Int32>(string.Format(totalQuery, whereQuery), parametert.ToArray()).ToArray();
        //        totalRecords = qt.Length > 0 ? qt[0] : 0;

        //        success = true;
        //        response.data = data;
        //    }
        //    catch (Exception ex)
        //    {
        //        message = ex.Message;
        //    }

        //    response.success = success;
        //    response.message = message;
        //    response.totalRecords = totalRecords;

        //    return response;

        //}
    }
}
