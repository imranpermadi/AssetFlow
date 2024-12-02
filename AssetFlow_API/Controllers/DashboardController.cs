using FAMS_API.Utilities;
using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Text;
using FAMS_ViewModels.Utilities;

namespace FAMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuthorize]
    public class DashboardController : ControllerBase
    {
        DataEntities db = new DataEntities();


        //[HttpGet]
        //[Route("status_count")]
        //public ResultData GetOrderStatusCount()
        //{
        //    var result = new ResultData();
        //    bool success = false;
        //    string message = "";

        //    try
        //    {
        //        var isAdmin = HttpContext.GetUserIsAdmin();
        //        var role = HttpContext.GetUserRole();
        //        var department = HttpContext.GetUserDepartment();

        //        List<SAPOrderModel> sapOrders = new List<SAPOrderModel>();
        //        List<OrderModel> orders = new List<OrderModel>();
        //        List<ShipmentModel> shipments = new List<ShipmentModel>();

        //        var sapOrderQuery = @"SELECT SAP.Id, SAP.Status
        //                                FROM SAP_Order SAP
        //                                INNER JOIN Order_Type F ON F.Code = SAP.Order_Type AND (F.Info1 IS NULL OR F.Info1 = SAP.Transfer_Type) AND F.Is_Deleted = 0
        //                                WHERE 1=1 AND ISNULL(SAP.Mark_Deleted,'') != 'Y' {0} {1}";

        //        var orderQuery = @"SELECT A.Id, A.Status
        //                            FROM [Order] A 
        //                            LEFT JOIN Order_Type F ON F.Id = A.Order_Type_Id
        //                            {1}
        //                            WHERE 1=1 AND A.Status NOT IN ('CANCELLED', 'REJECTED', 'UNASSIGNED TRANSPORTER', 'WAITING FOR ORDER APPROVAL') AND A.Is_Deleted = 0 {0}";

        //        var shipmentQuery = @"SELECT A.Id, A.Status, A.Progress_Status
        //                             FROM [Shipment] A
        //                                {1}
        //                                WHERE 1=1 AND A.Is_Deleted = 0 {0}";

        //        var sapAuthQuery = "";
        //        var sapCommodityQuery = "";
        //        var orderSAPAuthQuery = @" OUTER APPLY (
        //                                            SELECT TOP 1 OD.ORDER_ID
        //                                                FROM Order_Detail OD
        //                                                INNER JOIN SAP_ORDER SAP ON SAP.ID = OD.SAP_Order_Id
        //                                                WHERE OD.Is_Deleted = 0 AND OD.SAP_Order_Id IS NOT NULL and OD.ORDER_ID = A.Id
        //                                                    {0}
        //                                        ) X";
        //        var orderAuthQuery = "";

        //        var shipmentAuthQuery = @"INNER JOIN
        //                      (SELECT DISTINCT SOD.Shipment_Id
        //                       FROM Shipment_Order_Detail SOD
        //                       INNER JOIN [SAP_Order] SAP ON SAP.Document_No = SOD.SAP_No --AND O.Is_Deleted = 0
        //                       INNER JOIN Order_Type F ON F.Code = SAP.Order_Type AND F.Is_Deleted = 0
        //                                LEFT JOIN Material M ON M.Material_Code = SOD.Material_Code AND ISNULL(M.Deletion_Flag, '') != 'X'
        //                       WHERE SOD.Is_Deleted = 0 {0} {1}) X ON X.Shipment_Id = A.Id ";
        //        var shipmentWhereQuery = "";

        //        var fOrderAuthQuery = "";

        //        if (!isAdmin)
        //        {
        //            if(role == Constants.Roles.USER)
        //            {
        //                bool allCompany = false, allSegment = false, allPlant = false, allPOrg = false, allSOrg = false, allDistribution = false, allCommodity = false, allLocation = false;

        //                var listCompany = HttpContext.GetUserCompany(out allCompany);
        //                var listCompanyCodes = HttpContext.GetUserCompanyCode(out allCompany);
        //                var listBusinessSegment = HttpContext.GetUserBusinessSegment(out allSegment);
        //                var listBusinessSegmentCodes = HttpContext.GetUserBusinessSegmentCode(out allSegment);
        //                var listPlants = HttpContext.GetUserPlants(out allPlant);
        //                var listPurOrg = HttpContext.GetUserPurchaseOrg(out allPOrg);
        //                var listSalesOrgCodes = HttpContext.GetUserSalesOrgCode(out allSOrg);
        //                var listSalesOrg = HttpContext.GetUserSalesOrg(out allSOrg);
        //                var listDistributions = HttpContext.GetUserDistribution(out allDistribution);
        //                var listCommodities = HttpContext.GetUserCommodity(out allCommodity);
        //                var listLocations = HttpContext.GetUserLocation(out allLocation); //UAuth.GetUserLocation(out allLocation);

        //                if (!allCompany)
        //                {
        //                    var companies = listCompany.Count > 0 ? string.Join(", ", listCompany.Select(r => r.ToString()).ToList()) : "''";
        //                    var companiesCode = listCompanyCodes.Count > 0 ? string.Join(", ", listCompanyCodes.Select(r => "'" + r.ToString() + "'").ToList()) : "''";

        //                    orderAuthQuery += " AND A.Company_Id IN (" + companies + ")";
        //                    sapAuthQuery += " AND SAP.Company_Code IN (" + companiesCode + ")";
        //                }

        //                if (!allSegment)
        //                {
        //                    var businessSegment = listBusinessSegment.Count > 0 ? string.Join(", ", listBusinessSegment.Select(r => r.ToString()).ToList()) : "''";
        //                    var businessSegemntCodes = listBusinessSegmentCodes.Count > 0 ? string.Join(", ", listBusinessSegmentCodes.Select(r => "'" + r.ToString() + "'").ToList()) : "''";

        //                    orderAuthQuery += " AND A.Business_Segment_Id IN (" + businessSegment + ")";
        //                    sapAuthQuery += " AND SAP.Product_Category IN (" + businessSegemntCodes + ")";
        //                }

        //                if (!allPlant)
        //                {
        //                    var plants = listPlants.Count > 0 ? string.Join(", ", listPlants.Select(r => "'" + r + "'").ToList()) : "''";

        //                    orderAuthQuery += " AND A.Plant_Code IN (" + plants + ") ";
        //                    sapAuthQuery += " AND SAP.Plant IN (" + plants + ") ";
        //                }

        //                if (!allPOrg)
        //                {
        //                    var purOrg = listPurOrg.Count > 0 ? string.Join(", ", listPurOrg.Select(r => "'" + r + "'").ToList()) : "''";

        //                    orderAuthQuery += " AND ((F.Use_POrg = 1 AND A.Purchase_Organization_Code IN (" + purOrg + ")) ";
        //                    sapAuthQuery += " AND ((F.Use_POrg = 1 AND SAP.Purchase_Organization IN (" + purOrg + ")) ";
        //                }

        //                if (!allSOrg)
        //                {
        //                    var salesOrg = listSalesOrg.Count > 0 ? string.Join(", ", listSalesOrg.Select(r => r.ToString()).ToList()) : "''";
        //                    var salesOrgCodes = listSalesOrgCodes.Count > 0 ? string.Join(", ", listSalesOrgCodes.Select(r => "'" + r.ToString() + "'").ToList()) : "''";

        //                    orderAuthQuery += (!allPOrg ? " OR " : " AND (") + " (F.Use_SOrg = 1 AND A.Sales_Organization_Id IN (" + salesOrg + "))) ";
        //                    sapAuthQuery += (!allPOrg ? " OR " : " AND (") + " (F.Use_SOrg = 1 AND SAP.Sales_Organization IN (" + salesOrgCodes + "))) ";
        //                }
        //                else 
        //                {
        //                    if (!allPOrg)
        //                    {
        //                        orderAuthQuery += " OR (F.Use_SOrg = 1 AND 1=1)) ";
        //                        sapAuthQuery += " OR (F.Use_SOrg = 1 AND 1=1)) ";
        //                    }
        //                }

        //                if (!allDistribution)
        //                {
        //                    var distributions = listDistributions.Count > 0 ? string.Join(", ", listDistributions.Select(r => "'" + r + "'").ToList()) : "''";

        //                    orderAuthQuery += " AND (F.Use_Distribution = 0 OR (F.Use_Distribution = 1 AND A.Distribution IN (" + distributions + ")))";
        //                    sapAuthQuery += " AND (F.Use_Distribution = 0 OR (F.Use_Distribution = 1 AND SAP.Distribution IN (" + distributions + ")))";
        //                }

        //                fOrderAuthQuery = " AND (X.Order_Id IS NOT NULL OR (1=1 " + orderAuthQuery + "))";

        //                if (!allCommodity && department != Constants.Departments.WAREHOUSE)
        //                {
        //                    var commodities = listCommodities.Count > 0 ? string.Join(", ", listCommodities.Select(r => "'" + r + "'").ToList()) : "''";

        //                    sapCommodityQuery += @" AND EXISTS (
        //                               SELECT TOP 1 SOD.Document_No
        //                                FROM SAP_Order_Detail SOD
        //                                LEFT JOIN Material M ON M.Material_Code = SOD.Material_Code AND ISNULL(M.Deletion_Flag,'') != 'X'
        //                                WHERE SOD.Document_No = SAP.Document_No AND M.Commodity_Group IN (" + commodities + @")
        //                              )";

        //                    fOrderAuthQuery += @" AND EXISTS (
        //                               SELECT TOP 1 SOD.Order_Id
        //                                FROM Order_Detail SOD
        //                                LEFT JOIN Material M ON M.Material_Code = SOD.Material_Code AND ISNULL(M.Deletion_Flag,'') != 'X'
        //                                WHERE SOD.Order_Id = A.Id AND M.Commodity_Group IN (" + commodities + @")
        //                              ) ";
        //                }

        //                if(department == Constants.Departments.WAREHOUSE)
        //                {
        //                    if (!allLocation)
        //                    {
        //                        string locations = string.Join(", ", listLocations.Select(r => "'" + r + "'").ToList());
        //                        locations = string.IsNullOrEmpty(locations) ? "''" : locations;

        //                        shipmentWhereQuery += " AND D.Location IN (" + locations + ")";
        //                    }

        //                    if (!allSegment)
        //                    {
        //                        var businessSegment = listBusinessSegment.Count > 0 ? string.Join(", ", listBusinessSegment.Select(r => "'" + r.ToString() + "'").ToList()) : "''";
        //                        var businessSegmentCodes = listBusinessSegmentCodes.Count > 0 ? string.Join(", ", listBusinessSegmentCodes.Select(r => "'" + r.ToString() + "'").ToList()) : "''";
        //                        shipmentWhereQuery += $" AND (A.Business_Segment_Id IN ({businessSegment}) OR A.Product_Category IN ({businessSegmentCodes})) ";
        //                    }

        //                    if (!allCompany)
        //                    {
        //                        var companies = listCompany.Count > 0 ? string.Join(", ", listCompany.Select(r => "'" + r.ToString() + "'").ToList()) : "''";
        //                        var companyCodes = listCompanyCodes.Count > 0 ? string.Join(", ", listCompanyCodes.Select(r => "'" + r.ToString() + "'").ToList()) : "''";
        //                        shipmentWhereQuery += $" AND (A.Company_Id IN ({companies}) OR A.Company_Code IN ({companyCodes}))";
        //                    }

        //                    shipmentAuthQuery = "";
        //                    shipmentAuthQuery += @" INNER JOIN WAREHOUSE D ON D.CODE = A.Warehouse_Code AND D.Is_Deleted = 0 ";

        //                    sapAuthQuery = " ";
        //                    orderSAPAuthQuery = " ";

        //                    fOrderAuthQuery = " ";
        //                }

        //            }
        //            else if(role == Constants.Roles.TRANSPORTER)
        //            {

        //            }
        //        }

        //        var fSAPAuthQuery = " AND ( 1=1 " + sapAuthQuery + ")";

        //        var fOrderSAPAuthQuery = string.Format(orderSAPAuthQuery, sapAuthQuery);
        //        var fShipmentAuthQuery = string.Format(shipmentAuthQuery, sapAuthQuery, sapCommodityQuery);


        //        var fSAPOrderQuery = string.Format(sapOrderQuery, fSAPAuthQuery, sapCommodityQuery);
        //        var fOrderQuery = string.Format(orderQuery, fOrderAuthQuery, fOrderSAPAuthQuery);
        //        var fShipmentQuery = string.Format(shipmentQuery, shipmentWhereQuery, fShipmentAuthQuery);

        //        sapOrders = db.Database.SqlQuery<SAPOrderModel>(fSAPOrderQuery).ToList();
        //        orders = db.Database.SqlQuery<OrderModel>(fOrderQuery).ToList();
        //        shipments = db.Database.SqlQuery<ShipmentModel>(fShipmentQuery).ToList();


        //        var totalSAPOrder = sapOrders.Count;
        //        var totalSAPOrderApproved = sapOrders.Where(r => r.Status == Constants.OrderStatus.APPROVED).Count();
        //        var totalSAPOrderWaitingApproval = sapOrders.Where(r => r.Status == Constants.OrderStatus.WAITING_FOR_APPROVAL).Count();

        //        var totalOrders = orders.Count;
        //        var totalOrderWaitingApproval = orders.Where(r => r.Status == Constants.OrderStatus.WAITING_FOR_DISPATCH_APPROVAL).Count();
        //        var totalOrderAssigned = orders.Where(r => r.Status == Constants.OrderStatus.DISPATCHED).Count();
        //        //var totalOrderCancelled = orders.Where(r => r.Status == Constants.OrderStatus.CANCELLED).Count();
        //        //var totalOrderRejected = orders.Where(r => r.Status == Constants.OrderStatus.REJECTED).Count();
        //        var totalOrderCompleted = orders.Where(r => r.Status == Constants.OrderStatus.COMPLETED).Count();

        //        var totalShipments = shipments.Count;
        //        var totalShipmentNew = shipments.Where(r=> r.Status == Constants.ShipmentStatus.NEW).Count();
        //        var totalShipmentAppointment = shipments.Where(r => r.Status == Constants.ShipmentStatus.IN_PROGRESS && r.Progress_Status == Constants.ShipmentSubStatus.APPOINTMENT).Count();
        //        var totalShipmentRegisIn = shipments.Where(r => r.Status == Constants.ShipmentStatus.IN_PROGRESS && r.Progress_Status == Constants.ShipmentSubStatus.REGISTRATION_IN).Count();
        //        var totalShipmentRegisOut = shipments.Where(r => r.Status == Constants.ShipmentStatus.IN_PROGRESS && r.Progress_Status == Constants.ShipmentSubStatus.REGISTRATION_OUT).Count();
        //        var totalShipmentCompleted = shipments.Where(r => r.Status == Constants.ShipmentStatus.COMPLETED).Count();

        //        result.data = new
        //        {
        //            sap_order_total = totalSAPOrder,
        //            sap_order_approved = totalSAPOrderApproved,
        //            sap_order_waiting_approval = totalSAPOrderWaitingApproval,
        //            order_total = totalOrders,
        //            order_waiting_approval = totalOrderWaitingApproval,
        //            order_assigned = totalOrderAssigned,
        //            //order_cancelled = totalOrderCancelled,
        //            //order_rejected = totalOrderRejected,
        //            order_completed = totalOrderCompleted,
        //            shipment_total = totalShipments,
        //            shipment_new = totalShipmentNew,
        //            shipment_appointment = totalShipmentAppointment,
        //            shipment_regis_in = totalShipmentRegisIn,
        //            shipment_regis_out = totalShipmentRegisOut,
        //            shipment_completed = totalShipmentCompleted,
        //        };


        //        ////var status = db.Orders.Where(r => !r.Is_Deleted).Select(r => r.Status).ToArray();
        //        //var status = db.Orders.Where(r => !r.Is_Deleted).ToArray();
        //        //if (!isAdmin)
        //        //{
        //        //    if (role == Constants.Roles.USER)
        //        //    {
        //        //        bool allCompany = false, allSegment = false, allPlant = false, allPOrg = false, allSOrg = false, allDistribution = false;

        //        //        var listCompany = UAuth.GetUserCompany(out allCompany);
        //        //        var listBusinessSegment = UAuth.GetUserBusinessSegment(out allSegment);
        //        //        var listPlants = UAuth.GetUserPlants(out allPlant);
        //        //        var listPurOrg = UAuth.GetUserPurchaseOrg(out allPOrg);
        //        //        var listSalesOrg = UAuth.GetUserSalesOrg(out allSOrg);
        //        //        var listDistributions = UAuth.GetUserDistribution(out allDistribution);

        //        //        var userQuery = "";

        //        //        var tempData = status.Where(r =>
        //        //                                    (allCompany || (!allCompany && listCompany.Contains(r.Company_Id))) &&
        //        //                                    (allSegment || (!allSegment && listBusinessSegment.Contains(r.Business_Segment_Id))) &&
        //        //                                    (allPlant || (!allPlant && listPlants.Contains(r.Plant_Code))) &&
        //        //                                    (
        //        //                                        (r.Order_Type.Use_POrg && (allPOrg || (!allPOrg && listPurOrg.Contains(r.Purchase_Organization_Code)))) ||
        //        //                                        (r.Order_Type.Use_SOrg && (allSOrg || (!allSOrg && listSalesOrg.Contains(r.Sales_Organization_Id.Value))))
        //        //                                    ) &&
        //        //                                    (!r.Order_Type.Use_Distribution || (r.Order_Type.Use_Distribution && (allDistribution || (!allDistribution && listDistributions.Contains(r.Distribution)))))
        //        //                            ).ToArray();

        //        //        status = tempData;
        //        //    }


        //            //else if (role == Constants.Roles.TRANSPORTER)
        //            //{
        //            //    var transporterId = User.Identity.GetUserDataByKey(Constants.CLAIM_TRANSPORTER_ID);

        //            //    bool allLocation = false;
        //            //    var transporterLocations = UAuth.GetUserLocation(out allLocation);

        //            //    status = status.Where(r => r.Transporter_Code == transporterId).ToArray();

        //            //    if (!allLocation)
        //            //    {
        //            //        string locations = string.Join(", ", transporterLocations.Select(r => "'" + r + "'").ToList());

        //            //        locations = string.IsNullOrEmpty(locations) ? "''" : locations;
        //            //        if (locations.Length > 0)
        //            //        {
        //            //            status = (from ord in status from w in db.Warehouses where ord.Warehouse_Code == w.Code where locations.Contains(w.Location) select ord).ToArray();
        //            //        }
        //            //    }

        //            //}
        //            //else if (role == Constants.Roles.DRIVER)
        //            //{
        //            //    var driverId = User.Identity.GetUserDataByKey(Constants.CLAIM_DRIVER_ID);


        //            //    bool allLocation = false;
        //            //    var driverLocations = UAuth.GetUserLocation(out allLocation);
        //            //    string locations = string.Join(", ", driverLocations.Select(r => "'" + r + "'").ToList());
        //            //    //status = status.Where(r => locations.Contains(r.Location))).ToArray();
        //            //    var sd = (from i in db.Shipment_Detail from d in db.Shipment_Driver where i.Shipment_Id.Equals(d.Shipment_Id) where d.Driver_Id.ToString() == driverId select i.Order_Id).ToList();
        //            //    status = status.Where(r => sd.Contains(r.Id)).ToArray();
        //            //    if (driverLocations.Count > 0)
        //            //    {
        //            //        status = (from ord in status from w in db.Warehouses where ord.Warehouse_Code == w.Code where locations.Contains(w.Location) select ord).ToArray();
        //            //    }

        //            //}
        //        //}

        //        success = true;
        //        message = "OK";
        //    }
        //    catch(Exception ex)
        //    {
        //        success = false;
        //        message = ex.Message;
        //    }

        //    result.success = success;
        //    result.message = message;

        //    return result;
        //}


        //[HttpGet]
        //[Route("completed")]
        //public ResultData CompletedChart()
        //{
        //    var result = new ResultData();
        //    var message = "";
        //    bool success = false;

        //    try
        //    {
        //        List<object> dataList = new List<object>();
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        string queryAuth = "";
        //        string query = @"SELECT Order_Date, Count(*) AS Order_Count 
        //                        FROM [ORDER] 
        //                        WHERE Status = '" + Constants.OrderStatus.COMPLETED + @"' AND Is_Deleted = 0 AND Order_Date >= @startDate AND Order_Date <= @endDate 
        //                        GROUP BY Order_Date ";

        //        DateTime startDate = DateTime.Now.AddDays(-6);
        //        DateTime endDate = DateTime.Now;

        //        parameters.Add(new SqlParameter("@startDate", startDate));
        //        parameters.Add(new SqlParameter("@endDate", endDate));

        //        string fQuery = string.Format(query, queryAuth);

        //        List<OrderCompletedHelper> data = db.Database.SqlQuery<OrderCompletedHelper>(fQuery, parameters.ToArray()).ToList();

        //        var dates = Enumerable.Range(0, 7).Select(offset => startDate.AddDays(offset)).ToArray();

        //        var groupPerDay = dates.Select(r => new
        //        {
        //            Date = r.Date,
        //            Count = data.Where(x => x.Order_Date.Date == r.Date).Select(x => x.Order_Count).FirstOrDefault()
        //        }).ToArray();

        //        List<string> labelList = groupPerDay.OrderBy(r => r.Date).Select(r => r.Date.ToString("dd/MM/yyyy")).ToList();
        //        List<string> backgroundList = new List<string>()
        //        {
        //            "rgba(0,166,90,0.7)",
        //            //"rgba(60,141,188,0.7)",
        //            //"rgba(0,192,239,0.7)",
        //            //"rgba(0,166,91,0.7)",
        //        };

        //        var d = new
        //        {
        //            label = "Completed",
        //            backgroundColor = "rgba(0,166,90,0.7)",
        //            data = groupPerDay.OrderBy(r => r.Date).Select(r => r.Count).ToArray()
        //        };

        //        dataList.Add(d);

        //        success = true;
        //        message = "OK";
        //        result.data = new
        //        {
        //            labels = labelList,
        //            datasets = new List<Object>(dataList)
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        success = false;
        //        message = ex.Message;
        //    }

        //    result.message = message;
        //    result.success = success;

        //    return result;
        //}



        [HttpGet]
        [Route("test")]
        public ResultData test()
        {
            var result = new ResultData();

            //new CloudStorage().Upload();


            return result;
        }

    }

    public class OrderCompletedHelper
    {
        public DateTime Order_Date { get; set; }
        public int Order_Count { get; set; }
    }
}
