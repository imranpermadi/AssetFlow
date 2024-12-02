using FAMS_API.Utilities;
using FAMS_Data;
using FAMS_Models;
using FAMS_Models.MobileModels;
using FAMS_Models.Utilities;
using FAMS_ViewModels;
using FAMS_ViewModels.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using RabbitTester.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FAMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuthorize]
    public class ApprovalController : ControllerBase
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
                var user = HttpContext.GetUserData();
                List<string> orderCols = new List<string>()
                {
                    "Reference_No", "Reference_No1", "Reference_No2", "Order_Type_Name", "Order_Date", "Company_Name", "Business_Segment_Name", "Customer_Vendor_Name", "Plant_Name", "Pur_Sales_Org", "Incoterm_Name", "Warehouse_Name"
                };
                List<string> assignCols = new List<string>()
                {
                    "Transporter_Name", "Loading_Destination_From_Name", "Loading_Destination_To_Name","Freight_Cost_Suggested", "System_NoT", "User_NoT", "Freight_Total_Different", "Freight_Total_Cost_User_NoT", "Freight_Best_Price", "Freight_Best_Price_UOM", "Freight_Different_Per_UOM", "Shipment_Type_Name", "Freight_Different_Percentage", "Stuffing_Date_From", "Stuffing_Date_To", "Freight_Cost", "Freight_Container", "Freight_Total_Cost", "Freight_Cost_UOM", "Freight_Cost_Per", "Truck_Type_Name", "Assign_Reason"
                };
                List<string> appointmentCols = new List<string>()
                {
                    "Appointment_Transporter_Name", "Appointment_Loading", "Appointment_Destination", "Appointment_Date", "Appointment_Slot", "Category", "Appointment_Time", "Vehicle_Name", "Driver_Name", "Appointment_Business_Segment", "Appointment_Warehouse", "Created_By", "Created_Date", "Last_Approval_By", "Last_Approval_Date"
                };

                string query = "", queryCount = "";

                query = @"SELECT A.* FROM (SELECT A.Id, O.Id as Document_Transfer_Id, P.Id as Document_Deactivation_Id, B.Id AS Approval_Id, B.Reference_No, B.Status, O.Transfer_Type, O.Document_Desc,
								CASE 
									WHEN B.Category = 'TRANSFER' OR B.Category = 'RETURN' 
										THEN (Select CONCAT(Parent_Name,' - ', Description) from Storage_Location 
											where Id = O.Storage_Location_Id)
									WHEN B.Category = 'DEACTIVATION' 
										THEN (Select CONCAT(Parent_Name,' - ', Description) from Storage_Location 
											where Id = P.Storage_Location_Id)
								end as Storage_From,
								
								CASE 
									WHEN B.Category = 'TRANSFER' THEN 'DOCUMENT TRANSFER'
									WHEN B.Category = 'DEACTIVATION' THEN 'DOCUMENT DEACTIVATION'
									WHEN B.Category = 'RETURN' THEN 'DOCUMENT RETURN'
								end as Category,

								CASE 
									WHEN B.Category = 'TRANSFER' OR B.Category = 'RETURN' 
										THEN 
										CASE 
											WHEN O.Transfer_Type = 'WITHIN STORAGE LOCATION' or  O.Transfer_Type = 'EXTERNAL' 
													THEN (Select CONCAT(Parent_Name,' - ', Description) from Storage_Location 
															where Id = O.Destination)
											WHEN O.Transfer_Type = 'ANOTHER DEPT' 
												THEN (Select Description from Department 
															where Id = O.Destination)
										end 
									WHEN B.Category = 'DEACTIVATION' 
										THEN 'REMOVAL'
								end as Destination,

								B.Remarks, B.Created_By, B.Created_Date, B.Last_Approval_By, B.Last_Approval_Date,
								CASE 
									WHEN B.Category = 'TRANSFER' or B.Category = 'RETURN'
										THEN 
											 CASE
												when O.Packing_Detail_Id is not null 
													then (select Ref_No from Packing_Detail where Id = O.Packing_Detail_Id)
												else (select A.Ref_No from Packing_Detail A 
														join Document B on B.Packing_Detail_Id = A.Id
														where B.Id = O.Document_Id)
											end
									WHEN B.Category = 'DEACTIVATION' 
										THEN
											 CASE
												when O.Packing_Detail_Id is not null 
													then (select Ref_No from Packing_Detail where Id = O.Packing_Detail_Id)
												else (select A.Ref_No from Packing_Detail A 
														join Document B on B.Packing_Detail_Id = A.Id
														where B.Id = O.Document_Id)
											end
								end as Packing_No
								from Approval_Detail A
		                        INNER JOIN Approval B ON B.Id = A.Approval_Id AND B.Active = 1
		                        INNER JOIN Approval_Matrix R on R.Id = B.Approval_Matrix_Id
		                        LEFT JOIN Location L on L.Code = R.Location
		                        LEFT JOIN [User] U on U.Employee_Number = B.Created_By
		                        LEFT JOIN Document_Transfer O ON O.Id = B.Document_Transfer_Id AND O.Is_Deleted = 'N'
								LEFT JOIN Document_Deactivation P ON P.Id = B.Document_Deactivation_Id AND P.Is_Deleted = 'N'
                                WHERE 1=1 $user AND $status $category) A where 1=1 {0}
                                ORDER BY {3} {2} {1}";

                queryCount = @"SELECT COUNT(A.Id) FROM (SELECT A.Id, B.Id AS Approval_Id, B.Reference_No, B.Status, O.Transfer_Type, O.Document_Desc,
								CASE 
									WHEN B.Category = 'TRANSFER' OR B.Category = 'RETURN' 
										THEN (Select CONCAT(Parent_Name,' - ', Description) from Storage_Location 
											where Id = O.Storage_Location_Id)
									WHEN B.Category = 'DEACTIVATION' 
										THEN (Select CONCAT(Parent_Name,' - ', Description) from Storage_Location 
											where Id = P.Storage_Location_Id)
								end as Storage_From,
								
								CASE 
									WHEN B.Category = 'TRANSFER' THEN 'DOCUMENT TRANSFER'
									WHEN B.Category = 'DEACTIVATION' THEN 'DOCUMENT DEACTIVATION'
									WHEN B.Category = 'RETURN' THEN 'DOCUMENT RETURN'
								end as Category,

								CASE 
									WHEN B.Category = 'TRANSFER' OR B.Category = 'RETURN' 
										THEN 
										CASE 
											WHEN O.Transfer_Type = 'WITHIN STORAGE LOCATION' or  O.Transfer_Type = 'EXTERNAL' 
													THEN (Select CONCAT(Parent_Name,' - ', Description) from Storage_Location 
															where Id = O.Destination)
											WHEN O.Transfer_Type = 'ANOTHER DEPT' 
												THEN (Select Description from Department 
															where Id = O.Destination)
										end 
									WHEN B.Category = 'DEACTIVATION' 
										THEN 'REMOVAL'
								end as Destination,

								B.Remarks, B.Created_By, B.Created_Date, B.Last_Approval_By, B.Last_Approval_Date,
								CASE 
									WHEN B.Category = 'TRANSFER' or B.Category = 'RETURN'
										THEN 
											 CASE
												when O.Packing_Detail_Id is not null 
													then (select Ref_No from Packing_Detail where Id = O.Packing_Detail_Id)
												else (select A.Ref_No from Packing_Detail A 
														join Document B on B.Packing_Detail_Id = A.Id
														where B.Id = O.Document_Id)
											end
									WHEN B.Category = 'DEACTIVATION' 
										THEN
											 CASE
												when O.Packing_Detail_Id is not null 
													then (select Ref_No from Packing_Detail where Id = O.Packing_Detail_Id)
												else (select A.Ref_No from Packing_Detail A 
														join Document B on B.Packing_Detail_Id = A.Id
														where B.Id = O.Document_Id)
											end
								end as Packing_No
								from Approval_Detail A
		                        INNER JOIN Approval B ON B.Id = A.Approval_Id AND B.Active = 1
		                        INNER JOIN Approval_Matrix R on R.Id = B.Approval_Matrix_Id
		                        LEFT JOIN Location L on L.Code = R.Location
		                        LEFT JOIN [User] U on U.Employee_Number = B.Created_By
		                        LEFT JOIN Document_Transfer O ON O.Id = B.Document_Transfer_Id AND O.Is_Deleted = 'N'
								LEFT JOIN Document_Deactivation P ON P.Id = B.Document_Deactivation_Id AND P.Is_Deleted = 'N'
                                WHERE 1=1 $user AND $status $category ) A where 1=1 {0}";

                string whereQuery = "";

                string queryStatus = "";
                bool getApproved = false;
                if (getApproved)
                {
                    queryStatus = "A.Active = 0 AND (A.Status = '" + Constants.ApprovalStatus.REJECTED + "' OR A.Status = '" + Constants.ApprovalStatus.APPROVED + "') ";
                }
                else
                {
                    queryStatus = "A.Active = 1 AND A.Status = '" + Constants.ApprovalStatus.WAITING_FOR_APPROVAL + "' ";
                }

                query = query.Replace("$status", queryStatus);
                queryCount = queryCount.Replace("$status", queryStatus);

                if (user.IsAdmin != "Y")
                {
                    query = query.Replace("$user", "AND A.Username = @username");
                    queryCount = queryCount.Replace("$user", "AND A.Username = @username");

                    parameters.Add(new SqlParameter("@username", user.Username));
                }
                else
                {
                    query = query.Replace("$user", "");
                    queryCount = queryCount.Replace("$user", "");
                }


                if (!string.IsNullOrEmpty(request.category))
                {
                    query = query.Replace("$category", "AND B.Category = @category");
                    queryCount = queryCount.Replace("$category", "AND B.Category = @category");
                    parameters.Add(new SqlParameter("@Category", request.category.ToUpper()));
                }

                /*if (!string.IsNullOrEmpty(request.category))
                {
                    var category = "";
                    if (request.category == "Order Create")
                    {
                        category = "Approval Order Create";
                    }
                    else if (request.category == "Order Assign to Transporter")
                    {
                        category = "Approval OA";
                    }
                    else
                    {
                        category = "Approval Appointment";
                    }

                    whereQuery += @" AND CASE
                                    WHEN B.Category = 'APPOINTMENT' AND B.Mode = 'CREATE' THEN 'APPROVAL APPOINTMENT'
                                    WHEN B.Category = 'ORDER' AND B.Mode = 'CREATE' THEN 'APPROVAL ORDER CREATE'
                                    ELSE 'APPROVAL OA'
                                END = @categories";

                    parameters.Add(new SqlParameter("@categories", category));
                }*/


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
                                string filterColumnName = columnName;

                                if (columnName.Contains("Date"))
                                {
                                    whereQuery += " AND FORMAT(" + tableAlias + columnName + ", 'yyyy-MM-dd') LIKE @" + colName;
                                    DateTime dt = Convert.ToDateTime(filter.value);
                                    parameters.Add(new SqlParameter("@" + colName, "%" + dt.ToString("yyyy-MM-dd") + "%"));
                                }
                                else
                                {
                                    whereQuery += (" AND " + tableAlias + filterColumnName + " LIKE @" + columnName + " ESCAPE '\\'");
                                    filterValue = BaseProgram.EscapeSymbols(filterValue);
                                    parameters.Add(new SqlParameter("@" + columnName, "%" + filterValue + "%"));
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

                var listOfData_ = db.Database.SqlQuery<ApprovalModel>(fQuery, parameters.ToArray()).ToList();

                foreach (SqlParameter prm in parameters)
                    parametert.Add(new SqlParameter(prm.ParameterName, prm.Value));

                var qt = db.Database.SqlQuery<Int32>(string.Format(queryCount, whereQuery), parametert.ToArray()).ToArray();
                totalRecords = qt.Length > 0 ? qt[0] : 0;



                #region
                //totalRecords = 0;

                //foreach (var data in listOfData_)
                //{
                //    data.Details = new List<OrderDetailModel>();

                //    if (data.SAP_Order_Id.HasValue)
                //    {
                //        var sapOrder = db.SAP_Order.Where(r=> r.Id == data.SAP_Order_Id.Value).FirstOrDefault();
                //        if(sapOrder != null)
                //        {
                //            SAP_Order_Detail[] details = null;
                //            if (sapOrder.Order_Type == "STO" && sapOrder.Type == Constants.OrderCreateType.SYNC_SAP)
                //            {
                //                details = db.SAP_Order_Detail.Where(r => r.Document_No == sapOrder.Document_No && r.Item_No == sapOrder.Item_No.ToString() && r.Transporter_Code == sapOrder.Transporter).ToArray();
                //            }
                //            else
                //            {
                //                details = db.SAP_Order_Detail.Where(r => r.Document_No == sapOrder.Document_No && r.Transporter_Code == sapOrder.Transporter).ToArray();
                //            }

                //            if(details.Length > 0)
                //            {
                //                for(int i=0; i< details.Length; i++)
                //                {
                //                    var detailModel = new OrderDetailModel();
                //                    var det = details[i];

                //                    detailModel.Material_Code = det.Material_Code;
                //                    detailModel.Qty = det.Qty.HasValue ? det.Qty.Value : 0;
                //                    detailModel.UOM = det.UOM;
                //                    detailModel.Item_No = string.IsNullOrEmpty(det.Item_No) ? 0 : int.Parse(det.Item_No);
                //                    detailModel.Material_Name = UUtils.GetMaterialName(det.Material_Code);

                //                    data.Details.Add(detailModel);
                //                }
                //            }
                //        }

                //    }

                //    if (data.Order_Id.HasValue)
                //    {
                //        var details = db.Order_Detail.Where(r => r.Order_Id == data.Order_Id.Value && !r.Is_Deleted).ToArray();

                //        if (details.Length > 0)
                //        {
                //            for (int i = 0; i < details.Length; i++)
                //            {
                //                data.Details.Add(new OrderDetailViewModel(details[i]));
                //            }
                //        }
                //    }
                //    else if (data.Shipment_Id.HasValue)
                //    {
                //        var shipmentDetails = db.Shipment_Order_Detail.Where(r => r.Shipment_Id == data.Shipment_Id.Value && r.Order_Id.HasValue && !r.Is_Deleted).ToArray();
                //        foreach (var detail in shipmentDetails)
                //        {
                //            var sId = detail.Order_Id;
                //            var details = db.Order_Detail.Where(r => r.Order_Id == sId && !r.Is_Deleted).ToArray();

                //            if (details.Length > 0)
                //            {
                //                for (int i = 0; i < details.Length; i++)
                //                {
                //                    var model = new OrderDetailViewModel(details[i]);
                //                    model.Qty = detail.Qty;
                //                    data.Details.Add(model);

                //                }
                //            }
                //        }
                //    }
                //}
                #endregion

                success = true;
                response.data = listOfData_;
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


        [Route("status")]
        [HttpGet]
        public ResultData GetStatus()
        {
            var result = new ResultData();

            try
            {
                var user = HttpContext.GetUserData();
                Dictionary<string, string> statusList = new Dictionary<string, string>();

                statusList.Add(Constants.TransactionType.TRANSFER, "TRANSFER");
                statusList.Add(Constants.TransactionType.RETURN, "RETURN");
                statusList.Add(Constants.TransactionType.DEACTIVATION, "DEACTIVATION");
               
                result.data = statusList;
                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Id to show current level, Approval Id for data checking
        /// </summary>
        /// <param name="id"></param>
        /// <param name="approval_id"></param>
        /// <returns></returns>
        [Route("approval_details/{id}/{approval_id}")]
        [HttpGet]
        public ResultData GetApprovalDetails(long id, long approval_id)
        {
            var result = new ResultData();
            var success = false;
            var message = "";

            try
            {
                var approval = db.Approval.Where(r => r.Id == approval_id).FirstOrDefault();

                if (approval != null)
                {
                    List<ApprovalDetailViewModel> newDetails = new List<ApprovalDetailViewModel>();
                    ApprovalViewModel approvalModel = new ApprovalViewModel(approval);
                   // var order_details = new OrderViewModel();

                    var assignMatrixLookup = db.Lookup.Where(r => r.LookupGroup == Constants.LookupGroup.DOCUMENT_TRANSFER_TYPE).ToArray();
                    var details = db.ApprovalDetail.Where(r => r.ApprovalId == approval_id).ToArray();
                    foreach (var detail in details)
                    {
                        var vDetail = new ApprovalDetailViewModel(detail);
                        var user = db.User.Where(r => r.Username == detail.Username).FirstOrDefault();
                        if (user != null) vDetail.fullname = user.Fullname;

                        var mLookup = assignMatrixLookup.Where(r => r.LookupKey == vDetail.Info1).FirstOrDefault();
                        if (mLookup != null)
                        {
                            vDetail.Info1 = mLookup.LookupValue;
                        }

                        vDetail.is_current = vDetail.Id == id;

                        newDetails.Add(vDetail);
                    }


                    result.data = newDetails;
                    //result.data = new {
                    //    approval = approvalModel,
                    //    details = ,
                    //    order_details = order_details
                    //};

                    success = true;
                    message = "OK";
                }
            }
            catch(Exception ex)
            {
                message = ex.Message;
            }

            result.success = success;
            result.message = message;  

            return result;
        }

        [HttpPut]
        [Route("process")]
        public ResultData Process(ApprovalHelperModel model)
        {
            var result = new ResultData();

            try
            {
                var user = HttpContext.GetUserData();
                var rpc = new RpcClient();
                var res = rpc.Call($"{Constants.BackendModule.APPROVAL}.", user, model);

                result = JsonConvert.DeserializeObject<ResultData>(res);
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
