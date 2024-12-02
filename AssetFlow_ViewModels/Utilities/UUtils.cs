using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Resources;
using FAMS_Models.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FAMS_ViewModels.Utilities
{
    public class UUtils
    {

        public static string GetCulture(string language)
        {
            string culture = "en-US";
            if (language == "id") culture = "id-ID";
            else if (language == "en") culture = "en-US";
            return culture;
        }

        public static string GetSetting(string code)
        {
            var db = new DataEntities();
            var setting = db.Setting.Where(r => r.Code == code).FirstOrDefault();
            return setting != null ? setting.Value : "";
        }


        //public static string CreateApproval(DMSEntities db,
        //                                    string type,
        //                                    string mode,
        //                                    string companyCode,
        //                                    string businessSegment,
        //                                    string orderType,
        //                                    string location,
        //                                    string purchaseOrg,
        //                                    string referenceNo,
        //                                    string username,
        //                                    string empNumber,
        //                                    decimal amount,
        //                                    out List<string> firstApproval,
        //                                    string docType = null,
        //                                    string distribution = null,
        //                                    string matrixType = null,
        //                                    string truckType = null,
        //                                    List<long> orderDocuments = null,
        //                                    long? shipmentId = null,
        //                                    long? forceRejectId = null,
        //                                    bool exceedQuota = false,
        //                                    bool higherPrice = false,
        //                                    bool lowerPrice = false,
        //                                    bool differentNoT = false,
        //                                    string referenceNo1 = "",
        //                                    string referenceNo2 = "")
        //{
        //    string result = "";
        //    firstApproval = new List<string>();

        //    try
        //    {
        //        var companyList = new List<string>() { "ALL" };
        //        var businessSegmentList = new List<string>() { "ALL" };
        //        var orderTypeList = new List<string>() { "ALL" };
        //        var locationList = new List<string>() { "ALL" };
        //        var purOrgList = new List<string>() { "ALL" };
        //        var distributionList = new List<string>() { "ALL" };
        //        var docTypeList = new List<string>() { "ALL" };

        //        if (!string.IsNullOrEmpty(companyCode)) companyList.Add(companyCode);
        //        if (!string.IsNullOrEmpty(businessSegment)) businessSegmentList.Add(businessSegment);
        //        if (!string.IsNullOrEmpty(orderType)) orderTypeList.Add(orderType);
        //        if (!string.IsNullOrEmpty(location)) locationList.Add(location);
        //        if (!string.IsNullOrEmpty(purchaseOrg)) purOrgList.Add(purchaseOrg);
        //        if (!string.IsNullOrEmpty(distribution)) distributionList.Add(distribution);
        //        if (!string.IsNullOrEmpty(docType)) docTypeList.Add(docType);

        //        //matrixType = "HIGHER";

        //        #region Get Matrix
        //        Approval_Matrix matrix = null;
        //        var getMatrix = db.Approval_Matrix.Where(r => !r.Is_Deleted
        //                                               && r.Mode == mode.ToUpper()
        //                                               && r.Type == type
        //                                               && companyList.Contains(r.Company_Code)
        //                                               && businessSegmentList.Contains(r.Business_Segment_Code)
        //                                               && purOrgList.Contains(r.Purchase_Organization_Code)
        //                                               && locationList.Contains(r.Location)
        //                                               //&& (r.Truck_Type == truckType)
        //                                               && distributionList.Contains(r.Distribution)
        //                                               && orderTypeList.Contains(r.Order_Type)
        //                                               && docTypeList.Contains(r.Document_Type)
        //                                               && r.Info1 == matrixType)
        //                                            .ToArray();

        //        if (getMatrix.Length > 0)
        //        {
        //            if (getMatrix.Length > 1)
        //            {
        //                var specificMatrix = getMatrix.Where(r =>
        //                                                        r.Business_Segment_Code != "ALL" ||
        //                                                        r.Purchase_Organization_Code != "ALL" ||
        //                                                        r.Location != "ALL" ||
        //                                                        r.Distribution != "ALL" ||
        //                                                        r.Order_Type != "ALL" ||
        //                                                        r.Document_Type != "ALL"
        //                                                    ).FirstOrDefault();

        //                if (specificMatrix != null)
        //                    matrix = specificMatrix;
        //                else
        //                    matrix = getMatrix[0];

        //            }
        //            else matrix = getMatrix[0];
        //        }


        //        if (matrix == null)
        //            throw new Exception("Approval matrix (" + mode + " " + type + ") not found");
        //        #endregion

        //        List<Approval_Matrix_Detail> exceedDetails = new List<Approval_Matrix_Detail>();
        //        if (exceedQuota)
        //        {
        //            Approval_Matrix matrixExceed = null;

        //            #region Get Matrix Exceed 
        //            var getMatrixExceed = db.Approval_Matrix.Where(r => !r.Is_Deleted
        //                                              && r.Mode == mode.ToUpper()
        //                                              && r.Type == type
        //                                              && companyList.Contains(r.Company_Code)
        //                                              && businessSegmentList.Contains(r.Business_Segment_Code)
        //                                              && purOrgList.Contains(r.Purchase_Organization_Code)
        //                                              && locationList.Contains(r.Location)
        //                                              //&& (r.Truck_Type == truckType)
        //                                              && distributionList.Contains(r.Distribution)
        //                                              && orderTypeList.Contains(r.Order_Type)
        //                                              && docTypeList.Contains(r.Document_Type)
        //                                              && r.Info1 == Constants.AssignedMatrixType.EXCEED)
        //                                           .ToArray();

        //            if (getMatrixExceed.Length > 0)
        //            {
        //                if (getMatrixExceed.Length > 1)
        //                {
        //                    var specificMatrix = getMatrixExceed.Where(r =>
        //                                                            r.Business_Segment_Code != "ALL" ||
        //                                                            r.Purchase_Organization_Code != "ALL" ||
        //                                                            r.Location != "ALL" ||
        //                                                            r.Distribution != "ALL" ||
        //                                                            r.Order_Type != "ALL" ||
        //                                                            r.Document_Type != "ALL"
        //                                                        ).FirstOrDefault();

        //                    if (specificMatrix != null)
        //                        matrixExceed = specificMatrix;
        //                    else
        //                        matrixExceed = getMatrixExceed[0];

        //                }
        //                else matrixExceed = getMatrixExceed[0];
        //            }
        //            #endregion

        //            if (matrixExceed == null)
        //                throw new Exception("Approval matrix (" + mode + " " + type + " EXCEED) not found");


        //            exceedDetails = db.Approval_Matrix_Detail.Where(r => r.Approval_Matrix_Id == matrixExceed.Id && r.Is_Deleted != "Y").OrderBy(r => r.Level).ToList();
        //            if (exceedDetails.Count <= 0)
        //                throw new Exception("No approvers has been set up for this matrix (" + mode + " " + type + " EXCEED)");
        //        }

        //        //Not = Number of trips
        //        List<Approval_Matrix_Detail> differentNotDetails = new List<Approval_Matrix_Detail>();
        //        if (differentNoT)
        //        {
        //            Approval_Matrix matrixNoT = null;
        //            #region Get Matrix Different NoT
        //            var getMatrixNoT = db.Approval_Matrix.Where(r => !r.Is_Deleted
        //                                              && r.Mode == mode.ToUpper()
        //                                              && r.Type == type
        //                                              && companyList.Contains(r.Company_Code)
        //                                              && businessSegmentList.Contains(r.Business_Segment_Code)
        //                                              && purOrgList.Contains(r.Purchase_Organization_Code)
        //                                              && locationList.Contains(r.Location)
        //                                              //&& (r.Truck_Type == truckType)
        //                                              && distributionList.Contains(r.Distribution)
        //                                              && orderTypeList.Contains(r.Order_Type)
        //                                              && docTypeList.Contains(r.Document_Type)
        //                                              && r.Info1 == Constants.AssignedMatrixType.DIFFERENT_NOT)
        //                                           .ToArray();

        //            if (getMatrixNoT.Length > 0)
        //            {
        //                if (getMatrixNoT.Length > 1)
        //                {
        //                    var specificMatrix = getMatrixNoT.Where(r =>
        //                                                            r.Business_Segment_Code != "ALL" ||
        //                                                            r.Purchase_Organization_Code != "ALL" ||
        //                                                            r.Location != "ALL" ||
        //                                                            r.Distribution != "ALL" ||
        //                                                            r.Order_Type != "ALL" ||
        //                                                            r.Document_Type != "ALL"
        //                                                        ).FirstOrDefault();

        //                    if (specificMatrix != null)
        //                        matrixNoT = specificMatrix;
        //                    else
        //                        matrixNoT = getMatrixNoT[0];

        //                }
        //                else matrixNoT = getMatrixNoT[0];


        //                if (matrixNoT == null)
        //                    throw new Exception("Approval matrix (" + mode + " " + type + " DIFFERENT NoT) not found");

        //                differentNotDetails = db.Approval_Matrix_Detail.Where(r => r.Approval_Matrix_Id == matrixNoT.Id && !r.Is_Deleted).OrderBy(r => r.Level).ToList();
        //                if (differentNotDetails.Count <= 0)
        //                    throw new Exception("No approvers has been set up for this matrix (" + mode + " " + type + " DIFFERENT NoT)");
        //            }
        //            #endregion
        //        }

        //        List<Approval_Matrix_Detail> higherDetails = new List<Approval_Matrix_Detail>();
        //        if (higherPrice)
        //        {
        //            Approval_Matrix matrixHigher = null;
        //            #region Get Matrix Higher
        //            var getMatrixHigher = db.Approval_Matrix.Where(r => !r.Is_Deleted
        //                                              && r.Mode == mode.ToUpper()
        //                                              && r.Type == type
        //                                              && companyList.Contains(r.Company_Code)
        //                                              && businessSegmentList.Contains(r.Business_Segment_Code)
        //                                              && purOrgList.Contains(r.Purchase_Organization_Code)
        //                                              && locationList.Contains(r.Location)
        //                                              //&& (r.Truck_Type == truckType)
        //                                              && distributionList.Contains(r.Distribution)
        //                                              && orderTypeList.Contains(r.Order_Type)
        //                                              && docTypeList.Contains(r.Document_Type)
        //                                              && r.Info1 == Constants.AssignedMatrixType.HIGHER)
        //                                           .ToArray();

        //            if (getMatrixHigher.Length > 0)
        //            {
        //                if (getMatrixHigher.Length > 1)
        //                {
        //                    var specificMatrix = getMatrixHigher.Where(r =>
        //                                                            r.Business_Segment_Code != "ALL" ||
        //                                                            r.Purchase_Organization_Code != "ALL" ||
        //                                                            r.Location != "ALL" ||
        //                                                            r.Distribution != "ALL" ||
        //                                                            r.Order_Type != "ALL" ||
        //                                                            r.Document_Type != "ALL"
        //                                                        ).FirstOrDefault();

        //                    if (specificMatrix != null)
        //                        matrixHigher = specificMatrix;
        //                    else
        //                        matrixHigher = getMatrixHigher[0];

        //                }
        //                else matrixHigher = getMatrixHigher[0];
        //            }
        //            #endregion

        //            if (matrixHigher == null)
        //                throw new Exception("Approval matrix (" + mode + " " + type + " EXCEED) not found");

        //            higherDetails = db.Approval_Matrix_Detail.Where(r => r.Approval_Matrix_Id == matrixHigher.Id && r.Is_Deleted != "Y").OrderBy(r => r.Level).ToList();
        //            if (higherDetails.Count <= 0)
        //                throw new Exception("No approvers has been set up for this matrix (" + mode + " " + type + " NOT INCLUDE)");
        //        }


        //        var details = db.Approval_Matrix_Detail.Where(r => r.Approval_Matrix_Id == matrix.Id && r.Value <= amount && r.Is_Deleted != "Y").OrderBy(r => r.Level).ToArray();
        //        if (details.Length > 0)
        //        {
        //            List<string> mType = new List<string>();

        //            var approval = new Approval();
        //            //approval.SAP_Order = sapOrder;
        //            //approval.Order = orderId;
        //            approval.Shipment_Id = shipmentId;
        //            approval.Force_Reject_Id = forceRejectId;
        //            approval.Mode = mode.ToUpper();
        //            approval.Category = type;
        //            approval.Reference_No = referenceNo;
        //            approval.Active = true;
        //            approval.Approval_Matrix = matrix;
        //            approval.Status = Constants.ApprovalStatus.WAITING_FOR_APPROVAL;
        //            approval.Created_By = username;
        //            approval.Created_Date = DateTime.Now;
        //            approval.Reference_No1 = referenceNo1;
        //            approval.Reference_No2 = referenceNo2;
        //            db.Entry(approval).State = System.Data.Entity.EntityState.Added;

        //            //var firstLevel = details.OrderBy(r => r.Level).Select(r => r.Level).FirstOrDefault();
        //            var firstLevel = 1; //TYPE IS UNKNOWN (EXCEED/DIFFERENT/LOWER/HIGHER)
        //            var lastLevel = details.OrderByDescending(r => r.Level).Select(r => r.Level).FirstOrDefault();
        //            //firstApproval = details[0].Email;

        //            List<Approval_Detail> approvalDetails = new List<Approval_Detail>();

        //            var assignMatrixLookup = db.Lookups.Where(r => r.Lookup_Group == Constants.LookupGroup.ASSIGN_TRANSPORTER_MATRIX).ToArray();

        //            if (exceedQuota)
        //            {
        //                if (assignMatrixLookup.Length > 0)
        //                {
        //                    var mLookup = assignMatrixLookup.Where(r => r.Lookup_Key == Constants.AssignedMatrixType.EXCEED).FirstOrDefault();
        //                    if (mLookup != null)
        //                        mType.Add(mLookup.Lookup_Value);
        //                }

        //                for (int i = 0; i < exceedDetails.Count; i++)
        //                {
        //                    var detail = exceedDetails[i];
        //                    var approvalDetail = new Approval_Detail();
        //                    approvalDetail.Approval = approval;
        //                    approvalDetail.Level = detail.Level;

        //                    if (matrix.Validity == Constants.ApprovalValidity.ALL)
        //                    {
        //                        if (approvalDetail.Level == firstLevel) approvalDetail.Active = true;
        //                        else approvalDetail.Active = false;

        //                        if (approvalDetail.Level == lastLevel) approvalDetail.Last_Approval = true;
        //                        else approvalDetail.Last_Approval = false;
        //                    }
        //                    else
        //                    {
        //                        approvalDetail.Active = true;
        //                        approvalDetail.Last_Approval = true;
        //                    }

        //                    approvalDetail.Username = detail.Username;
        //                    approvalDetail.Info1 = Constants.AssignedMatrixType.EXCEED;
        //                    approvalDetail.Status = Constants.ApprovalStatus.WAITING_FOR_APPROVAL;
        //                    db.Entry(approvalDetail).State = System.Data.Entity.EntityState.Added;
        //                    approvalDetails.Add(approvalDetail);

        //                }
        //            }

        //            if (differentNoT)
        //            {
        //                if (assignMatrixLookup.Length > 0)
        //                {
        //                    var mLookup = assignMatrixLookup.Where(r => r.Lookup_Key == Constants.AssignedMatrixType.DIFFERENT_NOT).FirstOrDefault();
        //                    if (mLookup != null)
        //                        mType.Add(mLookup.Lookup_Value);
        //                }

        //                for (int i = 0; i < differentNotDetails.Count; i++)
        //                {
        //                    var detail = differentNotDetails[i];
        //                    var approvalDetail = new Approval_Detail();
        //                    approvalDetail.Level = detail.Level + (exceedQuota ? 1 : 0);

        //                    if (matrix.Validity == Constants.ApprovalValidity.ALL)
        //                    {
        //                        if (approvalDetail.Level == firstLevel) approvalDetail.Active = true;
        //                        else approvalDetail.Active = false;

        //                        if (approvalDetail.Level == lastLevel) approvalDetail.Last_Approval = true;
        //                        else approvalDetail.Last_Approval = false;
        //                    }
        //                    else
        //                    {
        //                        approvalDetail.Active = true;
        //                        approvalDetail.Last_Approval = true;
        //                    }

        //                    approvalDetail.Username = detail.Username;
        //                    approvalDetail.Info1 = Constants.AssignedMatrixType.DIFFERENT_NOT;
        //                    approvalDetail.Status = Constants.ApprovalStatus.WAITING_FOR_APPROVAL;
        //                    db.Entry(approvalDetail).State = System.Data.Entity.EntityState.Added;
        //                    approvalDetails.Add(approvalDetail);
        //                }
        //            }

        //            if (higherPrice)
        //            {
        //                if (assignMatrixLookup.Length > 0)
        //                {
        //                    var mLookup = assignMatrixLookup.Where(r => r.Lookup_Key == Constants.AssignedMatrixType.HIGHER).FirstOrDefault();
        //                    if (mLookup != null)
        //                        mType.Add(mLookup.Lookup_Value);
        //                }

        //                for (int i = 0; i < higherDetails.Count; i++)
        //                {
        //                    var detail = higherDetails[i];
        //                    var approvalDetail = new Approval_Detail();
        //                    approvalDetail.Approval = approval;
        //                    approvalDetail.Level = detail.Level + (differentNoT ? 1 : 0) + (exceedQuota ? 1 : 0);

        //                    if (matrix.Validity == Constants.ApprovalValidity.ALL)
        //                    {
        //                        if (approvalDetail.Level == firstLevel) approvalDetail.Active = true;
        //                        else approvalDetail.Active = false;

        //                        if (approvalDetail.Level == lastLevel) approvalDetail.Last_Approval = true;
        //                        else approvalDetail.Last_Approval = false;
        //                    }
        //                    else
        //                    {
        //                        approvalDetail.Active = true;
        //                        approvalDetail.Last_Approval = true;
        //                    }

        //                    approvalDetail.Username = detail.Username;
        //                    approvalDetail.Info1 = Constants.AssignedMatrixType.HIGHER;
        //                    approvalDetail.Status = Constants.ApprovalStatus.WAITING_FOR_APPROVAL;
        //                    db.Entry(approvalDetail).State = System.Data.Entity.EntityState.Added;
        //                    approvalDetails.Add(approvalDetail);
        //                }
        //            }


        //            if (lowerPrice)
        //            {
        //                if (assignMatrixLookup.Length > 0)
        //                {
        //                    var mLookup = assignMatrixLookup.Where(r => r.Lookup_Key == Constants.AssignedMatrixType.LOWER).FirstOrDefault();
        //                    if (mLookup != null)
        //                        mType.Add(mLookup.Lookup_Value);
        //                }

        //                var approvalDetail = new Approval_Detail();
        //                approvalDetail.Approval = approval;
        //                approvalDetail.Level = 1 + (exceedQuota ? 1 : 0);

        //                if (matrix.Validity == Constants.ApprovalValidity.ALL)
        //                {
        //                    if (approvalDetail.Level == firstLevel) approvalDetail.Active = true;
        //                    else approvalDetail.Active = false;

        //                    if (approvalDetail.Level == lastLevel) approvalDetail.Last_Approval = true;
        //                    else approvalDetail.Last_Approval = false;
        //                }
        //                else
        //                {
        //                    approvalDetail.Active = true;
        //                    approvalDetail.Last_Approval = true;
        //                }

        //                approvalDetail.Username = empNumber;
        //                approvalDetail.Status = Constants.ApprovalStatus.WAITING_FOR_APPROVAL;
        //                approvalDetail.Info1 = Constants.AssignedMatrixType.LOWER;
        //                db.Entry(approvalDetail).State = System.Data.Entity.EntityState.Added;
        //                approvalDetails.Add(approvalDetail);
        //            }
        //            else
        //            {
        //                if (assignMatrixLookup.Length > 0)
        //                {
        //                    var mLookup = assignMatrixLookup.Where(r => r.Lookup_Key == matrixType).FirstOrDefault();
        //                    if (mLookup != null)
        //                        mType.Add(mLookup.Lookup_Value);
        //                }

        //                for (int i = 0; i < details.Length; i++)
        //                {
        //                    var detail = details[i];
        //                    var approvalDetail = new Approval_Detail();
        //                    approvalDetail.Approval = approval;
        //                    approvalDetail.Level = detail.Level + (differentNoT ? 1 : 0) + (exceedQuota ? 1 : 0) + (higherPrice ? 1 : 0);

        //                    if (matrix.Validity == Constants.ApprovalValidity.ALL)
        //                    {
        //                        if (approvalDetail.Level == firstLevel) approvalDetail.Active = true;
        //                        else approvalDetail.Active = false;

        //                        if (approvalDetail.Level == lastLevel) approvalDetail.Last_Approval = true;
        //                        else approvalDetail.Last_Approval = false;
        //                    }
        //                    else
        //                    {
        //                        approvalDetail.Active = true;
        //                        approvalDetail.Last_Approval = true;
        //                    }

        //                    approvalDetail.Username = detail.Username;
        //                    approvalDetail.Info1 = matrixType;
        //                    approvalDetail.Status = Constants.ApprovalStatus.WAITING_FOR_APPROVAL;
        //                    db.Entry(approvalDetail).State = System.Data.Entity.EntityState.Added;
        //                    approvalDetails.Add(approvalDetail);
        //                }
        //            }

        //            approval.Info1 = string.Join(", ", mType);
        //            firstApproval = approvalDetails.Where(r => r.Level == firstLevel).Select(r => r.Username).ToList();

        //            if (orderDocuments == null)
        //            {
        //                var approvalDocument = new Approval_Document();
        //                approvalDocument.Order = orderId;
        //                approvalDocument.SAP_Order = sapOrder;
        //                approvalDocument.Approval = approval;
        //                db.Entry(approvalDocument).State = System.Data.Entity.EntityState.Added;
        //            }
        //            else
        //            {
        //                for (int i = 0; i < orderDocuments.Count; i++)
        //                {
        //                    var approvalDocument = new Approval_Document();
        //                    approvalDocument.Order_Id = orderDocuments[i];
        //                    approvalDocument.Approval = approval;
        //                    db.Entry(approvalDocument).State = System.Data.Entity.EntityState.Added;
        //                }
        //            }

        //            result = "OK";
        //        }
        //        else
        //        {
        //            throw new Exception("No approvers has been set up for this matrix (" + mode + " " + type + ")");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        firstApproval = null;
        //        result = ex.Message;
        //    }
        //    return result;
        //}

        //public static bool CreateApprovalV2(DataEntities db,
        //                                       string type,
        //                                       string category,
        //                                       string referenceNo,
        //                                       string referenceNo1,
        //                                       UserData user,
        //                                       List<ApprovalDetail> approvalDetails,
        //                                       out string message,
        //                                       out List<string> firstApprovers,
        //                                       out Approval approval,
        //                                       out string firstMatrix,
        //                                       long packing_id = 0,
        //                                       long document_id = 0,
        //                                       Document_Transfer docTrans = null,
        //                                       Document_Deactivation docDeactivate = null,
        //                                       Approval_Matrix getmatrix = null
        //                                   )
        //{
        //    bool result = false;
        //    message = "";
        //    firstApprovers = new List<string>();
        //    firstMatrix = "";
        //    approval = new Approval();

        //    try
        //    {
        //        if (approvalDetails == null || approvalDetails.Count <= 0)
        //            throw new Exception("No Approval Detail");

        //        var distinctDetails = approvalDetails.GroupBy(r => new {
        //            Level = r.Level,
        //            Username = r.Username,
        //            Info1 = r.Info1,
        //            Info2 = r.Info2,
        //            Approval_As = r.Approval_As
        //        }
        //        )
        //        .Select(g => new Approval_Detail
        //        {
        //            Level = g.Key.Level,
        //            Username = g.Key.Username,
        //            Status = Constants.ApprovalStatus.WAITING_FOR_APPROVAL,
        //            Info1 = g.Key.Info1,
        //            Info2 = g.Key.Info2,
        //            Approval_As = g.Key.Approval_As
        //        }).Distinct().OrderBy(r => r.Level).ToList();

        //        //approval.Order = order;
        //        approval.Mode = category.ToUpper();
        //        approval.Category = type;
        //        approval.Reference_No = referenceNo;
        //        approval.Reference_No1 = referenceNo1;

        //        if (type == Constants.ReleaseMatrixType.TRANSFER)
        //              approval.Document_Transfer = docTrans;
        //        else if (type == Constants.ReleaseMatrixType.DEACTIVATION)
        //              approval.Document_Deactivation = docDeactivate;
        //        approval.Approval_Matrix = getmatrix;

        //        //approval.Reference_No1 = order.SAP_No;
        //        approval.Active = true;
        //        approval.Status = Constants.ApprovalStatus.WAITING_FOR_APPROVAL;
        //        approval.Created_By = user.GetDisplayName();
        //        approval.Created_Date = DateTime.Now;

        //        db.Entry(approval).State = EntityState.Added;

        //        var matrixes = new List<string>();

        //        var firstLevel = distinctDetails.Select(r => r.Level).First();

        //        for (int i = 0; i < distinctDetails.Count; i++)
        //        {
        //            var detail = distinctDetails[i];

        //            var newDetail = new Approval_Detail();
        //            newDetail.Approval = approval;
        //            newDetail.Username = detail.Username;
        //            newDetail.Approval_As = detail.Approval_As;
        //            newDetail.Level = detail.Level;
        //            newDetail.Status = Constants.ApprovalStatus.WAITING_FOR_APPROVAL;
        //            newDetail.Info1 = detail.Info1;
        //            newDetail.Info2 = detail.Info2;

        //            if (newDetail.Level == firstLevel)
        //            {
        //                firstMatrix = detail.Info1;
        //                newDetail.Active = true;
        //            }

        //            db.Entry(newDetail).State = EntityState.Added;

        //            //var lookup = db.Lookups.Where(r => r.Lookup_Group == Constants.LookupGroup.ASSIGN_TRANSPORTER_MATRIX && !r.Is_Deleted && r.Lookup_Key == detail.Info1).FirstOrDefault();
        //            //if (lookup != null)
        //            //{
        //            //    if (!matrixes.Contains(lookup.Lookup_Value))
        //            //        matrixes.Add(lookup.Lookup_Value);
        //            //}

        //            var lookup = db.Lookups.Where(r => r.Lookup_Group == Constants.LookupGroup.DOCUMENT_TRANSFER_TYPE && r.Is_Deleted != "Y" && r.Lookup_Key == detail.Info1).FirstOrDefault();
        //            if (lookup != null)
        //            {
        //                if (!matrixes.Contains(lookup.Lookup_Value))
        //                    matrixes.Add(lookup.Lookup_Value);
        //            }
        //        }

        //        approval.Info1 = String.Join(", ", matrixes);

        //        //create Approval Document
        //        var approvalDocument = new Approval_Document();

        //        if (type == Constants.ReleaseMatrixType.TRANSFER)
        //            approvalDocument.Document_Transfer = docTrans;
        //        else if (type == Constants.ReleaseMatrixType.DEACTIVATION)
        //            approvalDocument.Document_Deactivation = docDeactivate;

        //        approvalDocument.Approval = approval;
        //        db.Entry(approvalDocument).State = EntityState.Added;

        //        firstApprovers = distinctDetails.Where(r => r.Level == firstLevel).Select(r => r.Username).ToList();

        //        if(packing_id != 0)
        //        {
        //            if(document_id == 0)
        //            {
        //                //All documents in the packing



        //            }
        //            else
        //            {

        //            }


        //        }

        //        if(document_id != 0 && packing_id == 0)
        //        {


        //        }

        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        message = ex.Message;
        //    }

        //    return result;
        //}

        public static bool GetApprovalLevel(DataEntities db,
                                                    string type,
                                                    string category,
                                                    string company,
                                                    string department,
                                                    string location,
                                                    string storageLocation,
                                                    string username,
                                                    out List<ApprovalDetail> approvalDetails,
                                                    out ReleaseMatrix getmatrix,
                                                    out string message,
                                                    string matrixType = null)
        {
            bool result = false;
            approvalDetails = new List<ApprovalDetail>();
            getmatrix = new ReleaseMatrix();
            message = "";

            try
            {


                var companyList = new List<string>() { "ALL" };
                var departmentList = new List<string>() { "ALL" };
                var locationList = new List<string>() { "ALL" };
                var storageLocationList = new List<string>() { "ALL" };

                if (!string.IsNullOrEmpty(company)) companyList.Add(company);
                if (!string.IsNullOrEmpty(department)) departmentList.Add(department);
                if (!string.IsNullOrEmpty(location)) locationList.Add(location);
                if (!string.IsNullOrEmpty(storageLocation)) storageLocationList.Add(storageLocation);

                #region Get Matrix
                ReleaseMatrix matrix = null;
                var getMatrix = db.ReleaseMatrix.Where(r => r.IsDeleted != "Y"
                                                        //&& r.Category.ToLower() == category.ToLower()
                                                        && r.Type.ToLower() == type.ToLower()
                                                        //&& companyList.Contains(r.CompanyCode)
                                                        //&& r.Info1 == matrixType
                                                     ).ToArray();

                if (getMatrix.Length > 0)
                {
                    if (getMatrix.Length > 1)
                    {
                        //var specificMatrix = getMatrix.Where(r =>
                        //                                        //r.CompanyCode == "ALL"
                        //                                        //r.Department == "ALL" ||
                        //                                        //r.Storage_Location == "ALL" ||
                        //                                        //r.Location == "ALL"
                        //                                    ).FirstOrDefault();

                        //if (specificMatrix != null)
                        //    matrix = specificMatrix;
                        //else
                        //    matrix = getMatrix[0];

                    }
                    else matrix = getMatrix[0];
                }

                getmatrix = matrix;
                if (matrix == null)
                    throw new Exception("Approval matrix (" + category + " " + type + ") not found");
               
                #endregion

                //var details = db.Approval_Matrix_Detail.Where(r => r.Approval_Matrix_Id == matrix.Id && r.Is_Deleted != "Y").OrderBy(r => r.Level).ToArray();
                //var details = db.ReleaseMatrixDetail.Where(r => r.ReleaseMatrixId == matrix.Id && r.IsDeleted != "Y" && r.ApprovalAs != Constants.ApprovalAs.REQUESTER).OrderBy(r => r.Level).ToArray();
                //if (details.Length > 0)
                //{
                //    List<string> mType = new List<string>();

                //    //var firstLevel = details.OrderBy(r => r.Level).Select(r => r.Level).FirstOrDefault();
                //    var firstLevel = 1; //TYPE IS UNKNOWN (EXCEED/DIFFERENT/LOWER/HIGHER)
                //    var lastLevel = details.OrderByDescending(r => r.Level).Select(r => r.Level).FirstOrDefault();
                //    //firstApproval = details[0].Email;

                //    //var assignMatrixLookup = db.Lookups.Where(r => r.Lookup_Group == Constants.LookupGroup.ASSIGN_TRANSPORTER_MATRIX).ToArray();
                //    //if (assignMatrixLookup.Length > 0)
                //    //{
                //    //    var mLookup = assignMatrixLookup.Where(r => r.Lookup_Key == matrixType).FirstOrDefault();
                //    //    if (mLookup != null)
                //    //        mType.Add(mLookup.Lookup_Value);
                //    //}

                //    for (int i = 0; i < details.Length; i++)
                //    {
                //        var detail = details[i];
                //        var approvalDetail = new ApprovalDetail();

                //        if (matrix.Validity == Constants.ApprovalValidity.ALL)
                //        {
                //            if (approvalDetail.Level == firstLevel) approvalDetail.Active = "Y";
                //            else approvalDetail.Active = "N";

                //            if (approvalDetail.Level == lastLevel) approvalDetail.LastApproval = "Y";
                //            else approvalDetail.LastApproval = "N";
                //        }
                //        else
                //        {
                //            approvalDetail.Active = "Y";
                //            approvalDetail.LastApproval = "Y";
                //        }

                //        approvalDetail.Username = detail.Username;
                //        approvalDetail.ApprovalAs = detail.ApprovalAs;
                //        approvalDetail.Info1 = matrixType;
                //        approvalDetail.Status = Constants.ApprovalStatus.WAITING_FOR_APPROVAL;
                //        approvalDetail.Level = detail.Level;
                //        approvalDetails.Add(approvalDetail);
                //    }


                //    //if (lowerPrice) //KALAU ADA APPROVAL KE SENDIRI 
                //    //{
                //    //    if (assignMatrixLookup.Length > 0)
                //    //    {
                //    //        var mLookup = assignMatrixLookup.Where(r => r.Lookup_Key == Constants.AssignedMatrixType.LOWER).FirstOrDefault();
                //    //        if (mLookup != null)
                //    //            mType.Add(mLookup.Lookup_Value);
                //    //    }

                //    //    var approvalDetail = new Approval_Detail();
                //    //    approvalDetail.Level = 1 + (exceedQuota ? 1 : 0);

                //    //    if (matrix.Validity == Constants.ApprovalValidity.ALL)
                //    //    {
                //    //        if (approvalDetail.Level == firstLevel) approvalDetail.Active = true;
                //    //        else approvalDetail.Active = false;

                //    //        if (approvalDetail.Level == lastLevel) approvalDetail.Last_Approval = true;
                //    //        else approvalDetail.Last_Approval = false;
                //    //    }
                //    //    else
                //    //    {
                //    //        approvalDetail.Active = true;
                //    //        approvalDetail.Last_Approval = true;
                //    //    }

                //    //    approvalDetail.Username = username;
                //    //    approvalDetail.Status = Constants.ApprovalStatus.WAITING_FOR_APPROVAL;
                //    //    approvalDetail.Info1 = Constants.AssignedMatrixType.LOWER;
                //    //    approvalDetails.Add(approvalDetail);
                //    //}
                //    //else
                //    //{
                //    //    if (assignMatrixLookup.Length > 0)
                //    //    {
                //    //        var mLookup = assignMatrixLookup.Where(r => r.Lookup_Key == matrixType).FirstOrDefault();
                //    //        if (mLookup != null)
                //    //            mType.Add(mLookup.Lookup_Value);
                //    //    }

                //    //    for (int i = 0; i < details.Length; i++)
                //    //    {
                //    //        var detail = details[i];
                //    //        var approvalDetail = new Approval_Detail();
                //    //        approvalDetail.Level = detail.Level + (differentNoT ? 1 : 0) + (exceedQuota ? 1 : 0) + (higherPrice ? 1 : 0);

                //    //        if (matrix.Validity == Constants.ApprovalValidity.ALL)
                //    //        {
                //    //            if (approvalDetail.Level == firstLevel) approvalDetail.Active = true;
                //    //            else approvalDetail.Active = false;

                //    //            if (approvalDetail.Level == lastLevel) approvalDetail.Last_Approval = true;
                //    //            else approvalDetail.Last_Approval = false;
                //    //        }
                //    //        else
                //    //        {
                //    //            approvalDetail.Active = true;
                //    //            approvalDetail.Last_Approval = true;
                //    //        }

                //    //        approvalDetail.Username = detail.Username;
                //    //        approvalDetail.Info1 = matrixType;
                //    //        approvalDetail.Status = Constants.ApprovalStatus.WAITING_FOR_APPROVAL;
                //    //        approvalDetails.Add(approvalDetail);
                //    //    }
                //    //}

                //    result = true;
                //}
                //else
                //{
                //    throw new Exception("No approvers has been set up for this matrix (" + category + " " + type + ")");
                //}
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return result;
        }


       

    }
}
