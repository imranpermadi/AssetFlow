using FAMS_API.Utilities;
using FAMS_Models;
using FAMS_Models.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FAMS_ViewModels.Utilities;
using FAMS_Data;
using FAMS_Models.Resources;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Configuration;

namespace FAMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuthorize]
    public class ReportController : ControllerBase
    {
        //[HttpPost]
        //[Route("freight_report")]
        //public ResultData FreightReport(FreightCostReportModel model)
        //{
        //    var response = new ResultData();
        //    response.message = Constants.OK;
        //    response.success = true;
        //    try
        //    {
        //        Dictionary<string, object> sqlParams = new Dictionary<string, object>();
        //        string query = @"SELECT '' AS Row_Num,
        //                                    ROW_NUMBER() OVER (PARTITION BY X.Order_No, X.SAP_No ORDER BY X.Order_No ASC) AS IDX,
        //                              COUNT(*) OVER  (PARTITION BY X.Order_No, X.SAP_No ORDER BY X.Order_No ASC) AS IDX1,
        //                                    *
        //                                    FROM (
        //                                        SELECT DISTINCT 

        //                                      A.Order_Type_Name, 
        //                                            A.Company_Code,
        //                                            A.Order_Date,
        //                                      A.SAP_No,
        //                                      A.Transporter_Name,
        //                                      A.Loading_Destination_From_Name,
        //                                      A.Loading_Destination_To_Name,
        //                                      A.Truck_Type_Name,
        //                                            'IDR' AS Ccy,
        //                                            B.Freight_Master,
        //                                      B.Freight_Cost,
        //                                      ISNULL(A.Freight_Container, 1) AS Freight_Container,
        //                                            ISNULL(A.Freight_Cost_UOM, S.Freight_Cost_UOM) AS Freight_Cost_UOM,
        //                                      B.Freight_Total_Cost,
        //                                      A.Business_Segment_Name,
        //                                            A.Order_No,
        //                                            B.Item_No,
        //                                      B.Material_Code,
        //                                      M.Material_Description,
        //                                      B.Qty,
        //                                      B.UOM,
        //                                            A.Warehouse_Name,
        //                                            A.Distribution,
        //                                      A.Document_Type,
        //                                            ISNULL(A.SAP_Transporter, 0) AS SAP_Transporter,
        //                                      CASE
        //                                       WHEN G.Header_Approval_Status = @approvedStatus THEN @releaseStatusFull
        //                                       WHEN G.Count > 0 THEN @releaseStatusPartial
        //                                       ELSE @releaseStatusUnreleased
        //                                      END AS Release_Status,
        //                                      UPPER(L1.Lookup_Value) AS Shipment_Type_Name,
        //                                      A.Created_Date,
        //                                      A.Created_By
        //                                        FROM [Order] A 
        //                                     INNER JOIN Order_Detail B ON B.Order_Id = A.Id AND B.Is_Deleted = 0
        //                                     INNER JOIN Material M ON M.Material_Code = B.Material_Code AND ISNULL(M.Deletion_Flag, '') != 'X'
        //                                     LEFT JOIN Lookup L1 ON L1.Lookup_Group = @lookupShipment AND L1.Lookup_Key = A.Shipment_Type
        //                                     LEFT JOIN (
        //                                      SELECT D.Approval_Id, E.Order_Id, F.Status AS Header_Approval_Status, COUNT(*) AS Count
        //                                       FROM Approval_Detail D
        //                                       INNER JOIN Approval_Document E ON E.Approval_Id = D.Approval_Id
        //                                       INNER JOIN Approval F ON F.Id = D.Approval_Id
        //                                       WHERE D.Status = @approvedStatus 
        //                                       GROUP BY D.Approval_Id, E.Order_Id, F.Status
        //                                     ) G ON G.Order_Id = A.Id
        //                                     LEFT JOIN Incoterm D ON D.Id= A.Incoterm_Id AND D.Is_Deleted = 0
        //                                        LEFT JOIN Order_Type F ON F.Id = A.Order_Type_Id
        //                                     LEFT JOIN WBTransType E ON E.Type = A.WB_Trans_Type AND E.Is_Deleted = 0
        //                                        LEFT JOIN SAP_Order S ON S.Document_No = A.SAP_No AND S.Transporter = A.Transporter_Code AND A.SAP_Transporter = 1 AND A.Type = @sapSyncType
        //                                        {2}
        //                                     WHERE 1=1 
        //                                      AND A.Is_Deleted = 0 
        //                                      AND (A.Status = @assignedStatus OR A.Status = @assignedStatus1)
        //                                      AND NOT EXISTS (SELECT LOOKUP_INFO1 FROM [LOOKUP] WHERE LOOKUP_GROUP = @lookupRelation AND Lookup_Info1 = E.TYPE AND Lookup_Info2 = D.INCOTERMS AND IS_DELETED = 0) 
        //                                            AND ISNULL(A.Transporter_Code,'') != ''
        //                                            {0}

        //                                    ) X 
        //                                    WHERE 1=1 {1} 
        //                                    ORDER BY X.Order_No, X.Item_No ASC
        //                   ";
        //        string whereQuery = "";
        //        string releaseQuery = "";
        //        sqlParams.Add("lookupShipment", Constants.LookupGroup.SHIPMENT_TYPE);
        //        sqlParams.Add("lookupRelation", Constants.LookupGroup.RELATION_SETTING);
        //        sqlParams.Add("assignedStatus", Constants.OrderStatus.DISPATCHED);
        //        sqlParams.Add("assignedStatus1", Constants.OrderStatus.COMPLETED);
        //        sqlParams.Add("approvedStatus", Constants.ApprovalStatus.APPROVED);
        //        sqlParams.Add("releaseStatusFull", Constants.ReleaseStatus.FULL);
        //        sqlParams.Add("releaseStatusPartial", Constants.ReleaseStatus.PARTIAL);
        //        sqlParams.Add("releaseStatusUnreleased", Constants.ReleaseStatus.UNRELEASED);
        //        sqlParams.Add("sapSyncType", Constants.OrderCreateType.SYNC_SAP);

        //        string reportDocDate = "";

        //        var sapOrderQuery = "";
        //        var sapWhereQuery = "";
        //        var userQuery = "";

        //        #region Set Filter

        //        if (model.Order_Date_From.HasValue)
        //        {
        //            reportDocDate = model.Order_Date_From.Value.ToString("dd/MM/yyyy");

        //            //FORMAT(lp.Registration_Date, 'dd/MM/yyyy')
        //            if (model.Order_Date_To.HasValue)
        //            {
        //                whereQuery += " AND (A.Order_Date >= @orderDateFrom AND A.Order_Date <= @orderDateTo)";
        //                sqlParams.Add("orderDateFrom", model.Order_Date_From.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        //                sqlParams.Add("orderDateTo", model.Order_Date_To.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

        //                reportDocDate += " - " + model.Order_Date_To.Value.ToString("dd/MM/yyyy");
        //            }
        //            else
        //            {
        //                whereQuery += " AND A.Order_Date = @orderDateFrom";
        //                sqlParams.Add("orderDateFrom", model.Order_Date_From.Value.ToString("yyyy-MM-dd 00:00:00"));
        //            }
        //        }

        //        if (model.Created_Date.HasValue)
        //        {
        //            whereQuery += " AND A.Created_Date >= @createdDateFrom AND A.Created_Date <= @createdDateTo";
        //            sqlParams.Add("createdDateFrom", model.Created_Date.Value.ToString("yyyy-MM-dd") + " 00:00:00");

        //            if (model.Created_Date_To.HasValue)
        //            {
        //                sqlParams.Add("createdDateTo", model.Created_Date_To.Value.ToString("yyyy-MM-dd") + " 23:59:59");
        //            }
        //            else
        //            {
        //                sqlParams.Add("createdDateTo", model.Created_Date.Value.ToString("yyyy-MM-dd") + " 23:59:59");
        //            }
        //        }

        //        if (model.Order_Type_Id.HasValue)
        //        {
        //            whereQuery += " AND A.Order_Type_Id = @orderTypeId";
        //            sqlParams.Add("orderTypeId", model.Order_Type_Id.Value);
        //        }

        //        if (!string.IsNullOrEmpty(model.SAP_No))
        //        {
        //            whereQuery += $" AND A.SAP_No LIKE '%{model.SAP_No}%'";
        //            //sqlParams.Add("sapNo", model.SAP_No);
        //        }

        //        if (!String.IsNullOrEmpty(model.Order_No))
        //        {
        //            whereQuery += $" AND A.Order_No LIKE '%{model.Order_No}%'";
        //            //sqlParams.Add("orderNo", model.Order_No);
        //        }

        //        if (model.Loading_Destination_From_Id.HasValue)
        //        {
        //            whereQuery += " AND A.Loading_Destination_From_Id = @loading";
        //            sqlParams.Add("loading", model.Loading_Destination_From_Id.Value);
        //        }

        //        if (model.Loading_Destination_To_Id.HasValue)
        //        {
        //            whereQuery += " AND A.Loading_Destination_To_Id = @destination";
        //            sqlParams.Add("destination", model.Loading_Destination_To_Id.Value);
        //        }

        //        if (!string.IsNullOrEmpty(model.Transporter_Code))
        //        {
        //            whereQuery += " AND A.Transporter_Code = @transporterCode";
        //            sqlParams.Add("transporterCode", model.Transporter_Code);
        //        }

        //        if (model.Business_Segment_Id.HasValue)
        //        {
        //            whereQuery += " AND A.Business_Segment_Id = @businessSegmentId";
        //            sqlParams.Add("businessSegmentId", model.Business_Segment_Id.Value);
        //        }

        //        if (!string.IsNullOrEmpty(model.Material_Code))
        //        {
        //            whereQuery += " AND B.Material_Code = @materialCode";
        //            sqlParams.Add("materialCode", model.Material_Code);
        //        }

        //        if (!string.IsNullOrEmpty(model.Distribution))
        //        {
        //            whereQuery += " AND A.Distribution = @distribution";
        //            sqlParams.Add("distribution", model.Distribution);
        //        }

        //        if (!string.IsNullOrEmpty(model.Document_Type))
        //        {
        //            whereQuery += " AND A.Document_Type = @documentType";
        //            sqlParams.Add("documentType", model.Document_Type);
        //        }

        //        if (!string.IsNullOrEmpty(model.Shipment_Type))
        //        {
        //            whereQuery += " AND A.Shipment_Type = @shipmentType";
        //            sqlParams.Add("shipmentType", model.Shipment_Type);
        //        }

        //        if (!string.IsNullOrEmpty(model.Release_Status))
        //        {
        //            releaseQuery += " AND X.Release_Status = @releaseStatus";
        //            sqlParams.Add("releaseStatus", model.Release_Status);
        //        }

        //        if (!string.IsNullOrEmpty(model.SAP_Freight_Cost))
        //        {
        //            whereQuery += " AND ISNULL(A.SAP_Transporter, 0) = @sapTransporter";
        //            sqlParams.Add("sapTransporter", model.SAP_Freight_Cost == "Y" ? "1" : "0");
        //        }

        //        if (!string.IsNullOrEmpty(model.Warehouse_Code))
        //        {
        //            whereQuery += " AND A.Warehouse_Code = @warehouseCode";
        //            sqlParams.Add("warehouseCode", model.Warehouse_Code);
        //        }
        //        bool isAdmin = Extensions.GetUserIsAdmin(HttpContext);
        //        if (!isAdmin)
        //        {
        //            bool allCompany = false, allSegment = false, allPlant = false, allDistribution = false, allSalesOrg = false, allPurOrg = false, allLocation = false, allCommodity = false;

        //            var listCompany = HttpContext.GetUserCompany(out allCompany);
        //            var listCompanyCodes = HttpContext.GetUserCompanyCode(out allCompany);
        //            var listSegment = HttpContext.GetUserBusinessSegment(out allSegment);
        //            var listSegmentCodes = HttpContext.GetUserBusinessSegmentCode(out allSegment);
        //            var listPlant = HttpContext.GetUserPlants(out allPlant);
        //            var listPurOrg = HttpContext.GetUserPurchaseOrg(out allPurOrg);
        //            var listSalesOrgCodes = HttpContext.GetUserSalesOrgCode(out allSalesOrg);
        //            var listSalesOrg = HttpContext.GetUserSalesOrg(out allSalesOrg);
        //            var listDistributions = HttpContext.GetUserDistribution(out allDistribution);
        //            var listCommodities = HttpContext.GetUserCommodity(out allCommodity);
        //            var listLocations = HttpContext.GetUserLocation(out allLocation); //UAuth.GetUserLocation(out allLocation);

        //            sapOrderQuery = @" OUTER APPLY (
        //                                            SELECT TOP 1 OD.ORDER_ID
        //                                                FROM Order_Detail OD
        //                                                INNER JOIN SAP_ORDER SAP ON SAP.ID = OD.SAP_Order_Id
        //                                                WHERE OD.Is_Deleted = 0 AND OD.SAP_Order_Id IS NOT NULL and OD.ORDER_ID = A.Id
        //                                                    {0}
        //                                        ) X";

        //            if (!allCompany)
        //            {
        //                var companies = listCompany.Count > 0 ? string.Join(", ", listCompany.Select(r => r.ToString()).ToList()) : "''";
        //                userQuery += " AND A.Company_Id IN (" + companies + ") ";

        //                var companiesCode = listCompanyCodes.Count > 0 ? string.Join(", ", listCompanyCodes.Select(r => "'" + r.ToString() + "'").ToList()) : "''";
        //                sapWhereQuery += " AND SAP.Company_Code IN (" + companiesCode + ")";
        //            }

        //            if (!allSegment)
        //            {
        //                var businessSegment = listSegment.Count > 0 ? string.Join(", ", listSegment.Select(r => r.ToString()).ToList()) : "''";
        //                userQuery += " AND A.Business_Segment_Id IN (" + businessSegment + ") ";

        //                var businessSegemntCodes = listSegmentCodes.Count > 0 ? string.Join(", ", listSegmentCodes.Select(r => "'" + r.ToString() + "'").ToList()) : "''";
        //                sapWhereQuery += " AND SAP.Product_Category IN (" + businessSegemntCodes + ")";
        //            }

        //            if (!allPlant)
        //            {
        //                var plants = listPlant.Count > 0 ? string.Join(", ", listPlant.Select(r => "'" + r + "'").ToList()) : "''";
        //                userQuery += " AND A.Plant_Code IN (" + plants + ") ";

        //                sapWhereQuery += " AND SAP.Plant IN (" + plants + ") ";
        //            }

        //            if (!allPurOrg)
        //            {
        //                var purOrg = listPurOrg.Count > 0 ? string.Join(", ", listPurOrg.Select(r => "'" + r + "'").ToList()) : "''";
        //                userQuery += " AND ((F.Use_POrg = 1 AND A.Purchase_Organization_Code IN (" + purOrg + ")) ";

        //                sapWhereQuery += " AND ((F.Use_POrg = 1 AND SAP.Purchase_Organization IN (" + purOrg + ")) ";
        //            }

        //            if (!allSalesOrg)
        //            {
        //                var salesOrg = listSalesOrg.Count > 0 ? string.Join(", ", listSalesOrg.Select(r => r.ToString()).ToList()) : "''";
        //                userQuery += (!allPurOrg ? " OR " : " AND (") + " (F.Use_SOrg = 1 AND A.Sales_Organization_Id IN (" + salesOrg + "))) ";

        //                var salesOrgCodes = listSalesOrgCodes.Count > 0 ? string.Join(", ", listSalesOrgCodes.Select(r => "'" + r.ToString() + "'").ToList()) : "''";
        //                sapWhereQuery += (!allPurOrg ? " OR " : " AND (") + "(F.Use_SOrg = 1 AND SAP.Sales_Organization IN (" + salesOrgCodes + "))) ";
        //            }
        //            else
        //            {
        //                if (!allPurOrg)
        //                {
        //                    userQuery += " OR (F.Use_SOrg = 1 AND 1=1)) ";
        //                    sapWhereQuery += "OR (F.Use_SOrg = 1 AND 1=1)) ";
        //                }
        //            }

        //            if (!allDistribution)
        //            {
        //                var distributions = listDistributions.Count > 0 ? string.Join(", ", listDistributions.Select(r => "'" + r + "'").ToList()) : "''";
        //                userQuery += " AND (F.Use_Distribution = 0  OR (F.Use_Distribution = 1 AND A.Distribution IN (" + distributions + ")))";

        //                sapWhereQuery += " AND (F.Use_Distribution = 0 OR (F.Use_Distribution = 1 AND SAP.Distribution IN (" + distributions + ")))";
        //            }

        //            sapOrderQuery = string.Format(sapOrderQuery, sapWhereQuery);

        //            var aQuery = "(X.Order_Id IS NOT NULL OR (1=1 AND X.Order_Id IS NULL " + userQuery + "))";

        //            if (!allCommodity)
        //            {
        //                var commodities = listCommodities.Count > 0 ? string.Join(", ", listCommodities.Select(r => "'" + r + "'").ToList()) : "''";

        //                aQuery += @" AND EXISTS (
        //                               SELECT TOP 1 SOD.Order_Id
        //                                FROM Order_Detail SOD
        //                                LEFT JOIN Material M ON M.Material_Code = SOD.Material_Code AND ISNULL(M.Deletion_Flag,'') != 'X'
        //                                WHERE SOD.Order_Id = A.Id AND M.Commodity_Group IN (" + commodities + @")
        //                              ) ";
        //            }

        //            whereQuery += $" AND ({aQuery})";
        //        }
        //        #endregion

        //        string finalQuery = String.Format(query, whereQuery, releaseQuery, sapOrderQuery);

        //        string strJSON = "", strHead = "", strFoot = "";
        //        DataTable dt = new DataTable();
        //        string result = SQL.GetDataTable("", finalQuery, sqlParams, out dt);
        //        if (result != "OK")
        //        {
        //            throw new Exception(result);
        //        }

        //        var reportName = "FAMS FREIGHT COST REPORT";
        //        UReport.printFreightCostReport(dt, reportDocDate, reportName, model.Show_Merged, out strHead, out strJSON, out strFoot);
        //        string headerLabel=UReport.printHeaderLabel(reportName, reportDocDate,false, true);

        //        var data = new ReportResult();
        //        data.title = reportName;
        //        data.header = headerLabel;
        //        data.content = string.Concat("<table>", strHead, strJSON, strFoot, "</table>");
        //        response.data = data;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.message = ex.Message;
        //        response.success = false;
        //    }
        //    return response;
        //}



       // [HttpPost]
       // [Route("generate_document_report")]
       // public ResultData DocumentReport(ReportModel model)
       // {
       //     var response = new ResultData();
       //     bool success = false;
       //     string message = "";
       //     string modul = "";
       //     try
       //     {
       //        string query = "";


       //         if(model.Report_Type == Constants.Report_Type.DOCUMENT_PER_LOCATION)
       //         {
       //             query = @"SELECT A.* FROM (SELECT 0 Row_Num, D.Code Company,D.Description [Company Name],  CONCAT(C.Parent_Name,' - ', C.Description) [Storage Location], 
       //                     E.Description Location,F.Description Department, CONCAT(G.Description,' (',B.Ref_No,')') Packing, H.Description [Document Category],
       //                     A.Doc_No as [Document No], A.Doc_Description [Document Description], A.Period_From  [Period From], A.Period_To [Period To],
       //                     A.Created_By [Created By], A.Created_Date [Created Date], A.Edited_By [Edited By], A.Edited_Date [Edited Date]
       //                     FROM Document A 
       //                     LEFT JOIN Packing_Detail B on B.Id = A.Packing_Detail_Id
       //                     LEFT JOIN Storage_Location C on C.Id = A.Storage_Location_Id
       //                     LEFT JOIN Company D on D.Id = A.Company_Id
       //                     LEFT JOIN Location E on E.Id = A.Location_Id
       //                     LEFT JOIN Department F on F.Id = A.Department_Id
       //                     LEFT JOIN Document_Category H on H.Code = A.Doc_Category_Code
       //                     LEFT JOIN Packing_Type G on G.Code = B.Type_Code
       //                     WHERE 1 = 1 {0}) A";
       //         }

       //         string whereQuery = "";
       //         Dictionary<string, object> sqlParams = new Dictionary<string, object>();

       //         string reportDocDate = "";

       //         DateTime from = new DateTime();
       //         DateTime to = new DateTime();

       //         if (!string.IsNullOrEmpty(model.Period_From.ToString()))
       //         {
       //             from = Convert.ToDateTime(model.Period_From);

       //             whereQuery += " AND A.Period_From >= @periodFrom AND A.Period_From <= @periodTo ";
       //             sqlParams.Add("@periodFrom", from.ToString("yyyy-MM-dd") + " 00:00:00");

       //             if (!string.IsNullOrEmpty(model.Period_To.ToString()))
       //             {
       //                 to = Convert.ToDateTime(model.Period_To);
       //                 sqlParams.Add("@periodTo", to.ToString("yyyy-MM-dd") + " 23:59:59");
       //             }
       //             else
       //             {
       //                 sqlParams.Add("@periodTo", from.ToString("yyyy-MM-dd") + " 23:59:59");
       //             }
                       
       //         }

       //         if (model.Location_Id.Count != 0)
       //         {
       //             var locationCode = string.Join(", ", model.Location_Id);
       //             whereQuery += " and E.Id in (select TRIM(value) from string_split(@loccode,',')) ";
       //             sqlParams.Add("@loccode", locationCode);
       //         }

       //         if (model.Company_Id.Count != 0)
       //         {   var company = string.Join(", ", model.Company_Id);
       //             whereQuery += " and D.Id in (select TRIM(value) from string_split(@company,',')) ";
       //             sqlParams.Add("@company", company);
       //         }

       //         if (model.Department_Id != null)
       //         {
       //             whereQuery += " and F.Id  = @department ";
       //             sqlParams.Add("@department", model.Department_Id);
       //         }

       //         if (model.Storage_Location_Id != null)
       //         {
       //             whereQuery += " and C.Id  = @Storage_Location_Id ";
       //             sqlParams.Add("@Storage_Location_Id", model.Storage_Location_Id);
       //         }

       //         if (model.Packing_No != null)
       //         {
       //             whereQuery += " and B.Id  = @Packing_No ";
       //             sqlParams.Add("@Packing_No", model.Packing_No);
       //         }

       //         if (model.Document_Category != null)
       //         {
       //             whereQuery += " and H.Code  = @Document_Category ";
       //             sqlParams.Add("@Document_Category", model.Document_Category);
       //         }

       //         if (model.Status != null)
       //         {
       //             whereQuery += " and A.Status  = @Status ";
       //             sqlParams.Add("@Status", model.Status);
       //         }

       //         string fullQuery = string.Format(query, whereQuery);
       //         DataTable q = new DataTable();
       //         string sError = SQL.GetDataTable("", fullQuery, sqlParams, out q, true);
            
       //         if (sError != "OK") throw new Exception(sError);
                
       //         var reportName = "DOCUMENT REPORT";
       //         reportDocDate = model.Period_From.ToString();
       //         string toDate = model.Period_To.ToString();

       //         string strJSON = "", strHead = "", strFoot = "";
       //         string headerLabel = UReport.printHeaderLabel(reportName, reportDocDate, toDate, true, true);

       //         var data = new ReportResult();
       //         data.title = reportName;
       //         data.header = headerLabel;

       //         UReport.DocumentReport(q, out strHead, out strJSON, out strFoot, modul);

       //         data.content = string.Concat("<table>", strHead, strJSON, "</table>");
       //         response.success = true;
       //         response.data = data;
       //     }
       //     catch (Exception ex)
       //     {
       //         message = ex.Message;
       //     }

       //     response.success = success;
       //     response.message = message;

       //     return response;
       // }
       // [HttpPost]
       // [Route("generate_movement_report")]
       // public ResultData MovementReport(ReportModel model)
       // {
       //     var response = new ResultData();
       //     bool success = false;
       //     string message = "";
       //     string modul = "";
       //     try
       //     {
       //         string query = "";


       //         if (model.Report_Type == Constants.Report_Type.PACKING_RELOCATION)
       //         {
       //             query = @"SELECT A.* FROM (SELECT 0 Row_Num, D.Code Company,D.Description [Company Name],	E.Description Location, 
							//PL.Ref_No [Relocation No],CONCAT(G1.Description,' (',P1.Ref_No,')') [From Packing No.], 
							//CONCAT(G2.Description,' (',P2.Ref_No,')') [Relocate To Packing No.],
       //                     A.Doc_No as [Document No], A.Doc_Description [Document Description], 
							//H.Description [Document Category], A.Period_From  [Period From], A.Period_To [Period To],
							//CASE 
							//	WHEN PL.New_Packing = 'Y' THEN 'Yes'
							//	WHEN PL.New_Packing = 'N' THEN 'No'
							//end as [New Packing], '' [Type], '' [Packing Desc], PL.Created_By [Rellocate By], 
							//PL.Created_Date [Rellocate Date]
       //                     FROM Document A 
							//JOIN Packing_Relocation PL on PL.Document_Id = A.Id 
							//LEFT JOIN Packing_Detail P1 on P1.Id = PL.Packing_Detail_Id_From
							//LEFT JOIN Packing_Detail P2 on P2.Id = PL.Packing_Detail_Id_To
       //                     LEFT JOIN Company D on D.Id = A.Company_Id
       //                     LEFT JOIN Location E on E.Id = A.Location_Id
       //                     LEFT JOIN Document_Category H on H.Code = A.Doc_Category_Code
       //                     LEFT JOIN Packing_Type G1 on G1.Code = P1.Type_Code
							//LEFT JOIN Packing_Type G2 on G2.Code = P2.Type_Code
							//WHERE 1 = 1 {0} ) A";
       //         }

       //         string whereQuery = "";
       //         Dictionary<string, object> sqlParams = new Dictionary<string, object>();

       //         string reportDocDate = "";

       //         DateTime from = new DateTime();
       //         DateTime to = new DateTime();

       //         if (!string.IsNullOrEmpty(model.Period_From.ToString()))
       //         {
       //             from = Convert.ToDateTime(model.Period_From);

       //             whereQuery += " AND A.Period_From >= @periodFrom AND A.Period_From <= @periodTo ";
       //             sqlParams.Add("@periodFrom", from.ToString("yyyy-MM-dd") + " 00:00:00");

       //             if (!string.IsNullOrEmpty(model.Period_To.ToString()))
       //             {
       //                 to = Convert.ToDateTime(model.Period_To);
       //                 sqlParams.Add("@periodTo", to.ToString("yyyy-MM-dd") + " 23:59:59");
       //             }
       //             else
       //             {
       //                 sqlParams.Add("@periodTo", from.ToString("yyyy-MM-dd") + " 23:59:59");
       //             }

       //         }

       //         if (model.Location_Id.Count != 0)
       //         {
       //             var locationCode = string.Join(", ", model.Location_Id);
       //             whereQuery += " and E.Id in (select TRIM(value) from string_split(@loccode,',')) ";
       //             sqlParams.Add("@loccode", locationCode);
       //         }

       //         if (model.Company_Id.Count != 0)
       //         {
       //             var company = string.Join(", ", model.Company_Id);
       //             whereQuery += " and D.Id in (select TRIM(value) from string_split(@company,',')) ";
       //             sqlParams.Add("@company", company);
       //         }

       //     /*    if (model.Department_Id != null)
       //         {
       //             whereQuery += " and F.Id  = @department ";
       //             sqlParams.Add("@department", model.Department_Id);
       //         }*/
 
       //         if (model.Relocation_No != null)
       //         {
       //             whereQuery += " and PL.Id  = @Relocation_No ";
       //             sqlParams.Add("@Relocation_No", model.Relocation_No);
       //         }

       //         if (model.From_Packing_No != null)
       //         {
       //             whereQuery += " and PL.Packing_Detail_Id_From  = @From_Packing_No ";
       //             sqlParams.Add("@From_Packing_No", model.From_Packing_No);
       //         }

       //         if (model.To_Packing_No != null)
       //         {
       //             whereQuery += " and PL.Packing_Detail_Id_To  = @To_Packing_No ";
       //             sqlParams.Add("@To_Packing_No", model.To_Packing_No);
       //         }


       //         if (model.Document_Category != null)
       //         {
       //             whereQuery += " and H.Code  = @Document_Category ";
       //             sqlParams.Add("@Document_Category", model.Document_Category);
       //         }

       //         if (model.Remarks != null)
       //         {
       //             whereQuery += " and PL.Reason  = @Remarks ";
       //             sqlParams.Add("@Remarks", model.Remarks);
       //         }

       //         string fullQuery = string.Format(query, whereQuery);
       //         DataTable q = new DataTable();
       //         string sError = SQL.GetDataTable(fullQuery, sqlParams, out q, true);

       //         if (sError != "OK") throw new Exception(sError);

       //         var reportName = "MOVEMENT REPORT";
       //         reportDocDate = model.Period_From.ToString();
       //         string toDate = model.Period_To.ToString();

       //         string strJSON = "", strHead = "", strFoot = "";
       //         string headerLabel = UReport.printHeaderLabel(reportName, reportDocDate, toDate, true, true);

       //         var data = new ReportResult();
       //         data.title = reportName;
       //         data.header = headerLabel;

       //         UReport.MovementReport(q, out strHead, out strJSON, out strFoot, modul);

       //         data.content = string.Concat("<table>", strHead, strJSON, "</table>");
       //         response.success = true;
       //         response.data = data;
       //     }
       //     catch (Exception ex)
       //     {
       //         message = ex.Message;
       //     }

       //     response.success = success;
       //     response.message = message;

       //     return response;
       // }
    }
}
