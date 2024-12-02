using FAMS_Data;
using FAMS_Models.Resources;
using FAMS_Models.Utilities;
using FAMS_ViewModels.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FAMS_Service
{
    public class ApprovalHandler
    {
        public static bool Process(string jsonData, UserData userData, out string message, out ApprovalHelperModel resultModel)
        {
            var result = false;
            message = "";
            resultModel = new ApprovalHelperModel();
            

            try 
            {
                ApprovalHelperModel model = JsonConvert.DeserializeObject<ApprovalHelperModel>(jsonData);

                foreach (var modelData in model.data)
                {
                    Approval approval = new Approval();
                    List<ApprovalDetail> nextApprovers = new List<ApprovalDetail>();
                    DataEntities db = new DataEntities();
                    //string gprocess = ProcessApproval(db, UUtils.GetApplicationLink(), modelData.id, model.status, userData.Employee_Number, modelData.remarks, out approval, out nextApprovers);
                    string gprocess = ProcessApproval(db, modelData.id, model.status, userData.Username, modelData.remarks, out approval, out nextApprovers);

                    modelData.success = gprocess == "OK";
                    modelData.message = gprocess;

                }

                result = true;
                message = "OK";
                resultModel = model;

            }
            catch(Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            return result;
        }


        public static string ProcessApproval(DataEntities db, long Id, string status, string username, string remarks, out Approval approval, out List<ApprovalDetail> nextApprovers)
        {
            string result = "";
            approval = null;
            nextApprovers = new List<ApprovalDetail>();

            using (var trans = db.Database.BeginTransaction())
            {
                var approvalUser = db.User.Where(r => r.Username == username).FirstOrDefault();
                if (approvalUser == null)
                    throw new Exception(Resources.APPROVAL_NOT_ALLOWED); //("Approver is not allowed to access this application");

                try
                {
                    var detail = db.ApprovalDetail.Where(r => r.Id == Id).FirstOrDefault();

                    if (detail != null)
                    {
                        if (detail.Active != "Y")
                        {
                            var user = db.User.Where(r => r.Username == detail.ApprovalBy).FirstOrDefault();
                            throw new Exception(String.Format(Resources.APPROVAL_PROCESSED, detail.Status, user.Fullname, detail.ApprovalDate.Value.ToString("dd/MM/yyyy HH:mm:ss")));
                            //("Document has been " + detail.Status + " by " + user.Full_Name + " on " + detail.Approval_Date.Value.ToString("dd/MM/yyyy HH:mm:ss"));
                        }

                        var approvalId = detail.ApprovalId;

                        approval = db.Approval.Where(r => r.Id == approvalId).FirstOrDefault();
                        if (approval == null)
                            throw new Exception(Resources.APPROVAL_INVALID); // ("Approval data not found");

                        var allLevels = db.ApprovalDetail.Where(r => r.ApprovalId == approvalId).ToArray();
                        if (allLevels.Length <= 0)
                            throw new Exception(Resources.APPROVAL_NEXT_INVALID); //("Next approval level not found");

                        //var allDocuments = db.ApprovalDocument.Where(r => r.Approval_Id == approvalId).ToArray();
                        //if (allDocuments.Length <= 0)
                        //    throw new Exception(Resources.APPROVAL_DOC_INVALID); //("Documents for this approval not found");

                        string mode = approval.Mode;
                        string category = approval.Category;
                        var sendApprovalNotification = false;
                        //var sendOrderApprovalNotification = false;
                        //var sendAppointmentNotification = false;
                        //Shipment shipmentAppointment = new Shipment();
                        //var sendTransporterDispatch = false;
                        //var sendAssignTransporterApproval = false;
                        //var sendOrderAssignTransporterNotification = false;
                        //var headerMailOrder = new Order();
                        //var sendApprovalNotification = false;

                        bool fullyApproved = false;

                        if (status == Constants.ApprovalStatus.REJECTED)
                        {
                            approval.Status = status;
                            approval.Active = "N";
                            approval.Remarks = remarks;

                            for (int i = 0; i < allLevels.Length; i++)
                            {
                                allLevels[i].Active = "N";
                                allLevels[i].Status = status;
                                allLevels[i].Remarks = remarks;
                                allLevels[i].ApprovalBy = username;
                                allLevels[i].ApprovalDate = DateTime.Now;
                                
                                if (allLevels[i].Username == approvalUser.Username)
                                    allLevels[i].LastApproval = "Y";

                                db.Entry(allLevels[i]).State = EntityState.Modified;
                            }

                            if (category == Constants.TransactionType.TRANSFER)
                            {
                                //var documentTrans = allDocuments;
                                //var dataTrans = allDocuments.Select(a => a.Document_Transfer).FirstOrDefault();
                                //for (int i = 0; i< documentTrans.Length; i++)
                                //{
                                  
                                    //dataTrans.Status = Constants.ApprovalStatus.REJECTED;

                                    //string role = UUtils.Release_As(approvalUser.Username, approvalUser.ApprovalAs);
                                    //var log = UUtils.MappingLog(dataTrans.Request_No, role, status, remarks, approvalUser.Employee_Number, dataTrans, null, status);
                                    //db.Entry(dataTrans).State = System.Data.Entity.EntityState.Modified;
                                //}
                                       
                            }

                        }
                        else if (status == Constants.ApprovalStatus.APPROVED)
                        {
                            var matrix = approval.ReleaseMatrix == null ? "ALL" : approval.ReleaseMatrix.Validity;
                            var approveOrder = false;
                            var approveAppointment = false;
                            var approveDispatch = false;
                            var approveForceReject = false;

                            if (matrix == Constants.ApprovalValidity.EITHER)
                            {
                                approval.Status = status;

                                for (int i = 0; i < allLevels.Length; i++)
                                {
                                    allLevels[i].Active = "";
                                    allLevels[i].ApprovalBy = username;
                                    allLevels[i].ApprovalDate = DateTime.Now;
                                    db.Entry(allLevels[i]).State = EntityState.Modified;
                                    if (allLevels[i].Username == approvalUser.Username)
                                        allLevels[i].LastApproval = "Y";
                                }

                                fullyApproved = true;
                            }
                            else if (matrix == Constants.ApprovalValidity.ALL)
                            {
                                int currentLevel = detail.Level.Value;
                                var currentLevelApprovers = allLevels.Where(r => r.Level == currentLevel).ToArray();

                                for (int x = 0; x < currentLevelApprovers.Length; x++)
                                {
                                    currentLevelApprovers[x].Active = "N";
                                    currentLevelApprovers[x].Status = status;
                                    currentLevelApprovers[x].ApprovalBy = username;
                                    currentLevelApprovers[x].ApprovalDate = DateTime.Now;
                                    if (currentLevelApprovers[x].Username == approvalUser.Username)
                                        currentLevelApprovers[x].LastApproval = "Y";
                                    db.Entry(currentLevelApprovers[x]).State = EntityState.Modified;
                                }

                                int nextLevel = currentLevel + 1;
                                var nextLevelApprovers = allLevels.Where(r => r.Level == nextLevel).ToArray();

                                if (nextLevelApprovers.Length > 0)
                                {
                                    for (int x = 0; x < nextLevelApprovers.Length; x++)
                                    {
                                        if (!nextApprovers.Contains(nextLevelApprovers[x])) nextApprovers.Add(nextLevelApprovers[x]);

                                        nextLevelApprovers[x].Active = "Y";
                                        db.Entry(nextLevelApprovers[x]).State = EntityState.Modified;
                                        sendApprovalNotification = true;
                                    }


                                }
                                else
                                {
                                    approval.Status = status;

                                    if (category == Constants.TransactionType.TRANSFER)
                                    {
                                        sendApprovalNotification = true; //SEND EMAIL IF FULLY APPROVED;
                                        approveOrder = true;
                                    }
                                   
                                    fullyApproved = true;
                                }
                            }

                            if(category == Constants.TransactionType.TRANSFER)
                            {
                                //var dataTrans = allDocuments.Select(a => a.Document_Transfer).FirstOrDefault();
                                //if (fullyApproved)
                                //{
                                //    dataTrans.Status = Constants.ApprovalStatus.APPROVED;
                                //    /* for (int i = 0; i < allDocuments.Length; i++)
                                //     {
                                //         var dataTrans = allDocuments[i].Document_Transfer;

                                //     }*/
                                //    db.Entry(dataTrans).State = System.Data.Entity.EntityState.Modified;
                                //}
                               


                            }

                        }

                        approval.LastApprovalBy = username;
                        approval.LastApprovalDate = DateTime.Now;
                        db.Entry(approval).State = EntityState.Modified;

                        db.SaveChanges();
                        trans.Commit();

                        var listNextApprovers = nextApprovers;
                        var approvalData = approval;

                        /*Task.Run(() =>
                        {
                            if (sendOrderApprovalNotification)
                            {
                                UMail.SendOrderApprovalMail(db, approvalData, listNextApprovers, status, remarks);
                            }

                            //if (sendApprovalNotification)
                            //{

                            //    //send level selanjutnya
                            //}
                            ////UMail.SendApprovalMail();
                            if (sendTransporterDispatch)
                            {

                                UMail.SendTransporterAcceptReject(db, headerMailOrder, headerMailOrder.Transporter_Code, status, approvalData);
                            }
                            if (sendAppointmentNotification)
                            {
                                UMail.SendApprovalAppointmentMail(db, approvalData, shipmentAppointment, listNextApprovers, status, remarks);
                            }

                            //Send to next approvers
                            if (sendOrderAssignTransporterNotification)
                            {
                                var nextReleaseMatrix = listNextApprovers.Select(r => r.Info1).FirstOrDefault();
                                var nextApproversName = listNextApprovers.Select(r => r.Username).ToList();
                                UMail.SendAssignTransporter(db, headerMailOrder, approvalData.Created_By, nextApproversName, nextReleaseMatrix);
                            }

                            if (sendApprovalNotification)
                            {
                                //send level selanjutnya
                            }
                            //UMail.SendApprovalMail();


                            if (sendAssignTransporterApproval)
                            {
                                var approvalMatrixLower = db.Lookups.Where(r => r.Lookup_Group == Constants.LookupGroup.ASSIGN_TRANSPORTER_MATRIX && r.Lookup_Key == Constants.AssignedMatrixType.LOWER && !r.Is_Deleted).FirstOrDefault();
                                if (approvalData.Info1 != approvalMatrixLower.Lookup_Value)
                                {
                                    UMail.SendAssignTransporterApprovalMail(db, headerMailOrder, approvalData, listNextApprovers, status, remarks);
                                }
                            }

                            //allLevels = db.FLApproval_Detail.Where(r => r.FLApproval_Id == approvalId).ToArray();
                            //if (allLevels.Length > 0)
                            //    SendRequesterNotification(db, approval.Reference_No, status, approvalUser.Email, allLevels.ToList(), fullyApproved);
                        });*/

                        //allLevels = db.FLApproval_Detail.Where(r => r.FLApproval_Id == approvalId).ToArray();
                        //if (allLevels.Length > 0)
                        //    SendRequesterNotification(db, approval.Reference_No, status, approvalUser.Email, allLevels.ToList(), fullyApproved);
                    }
                    else
                    {
                        throw new Exception(Resources.APPROVAL_LEVEL_INVALID); //("Approval level not found");
                    }

                    result = "OK";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    result = ex.Message;
                }
            }

            return result;
        }

        //public static string ProcessApproval(DMSEntities db, string url, long Id, string status, string username, string remarks, out Approval approval, out List<Approval_Detail> nextApprovers)
        //{
        //    string result = "";
        //    approval = null;
        //    nextApprovers = new List<Approval_Detail>();

        //    using (var trans = db.Database.BeginTransaction())
        //    {
        //        var approvalUser = db.Users.Where(r => r.Employee_Number == username).FirstOrDefault();
        //        if (approvalUser == null)
        //            throw new Exception(Resources.APPROVAL_NOT_ALLOWED); //("Approver is not allowed to access this application");

        //        try
        //        {
        //            var detail = db.Approval_Detail.Where(r => r.Id == Id).FirstOrDefault();

        //            if (detail != null)
        //            {
        //                if (!detail.Active)
        //                {
        //                    var user = db.Users.Where(r => r.Employee_Number == detail.Approval_By).FirstOrDefault();
        //                    throw new Exception(String.Format(Resources.APPROVAL_PROCESSED, detail.Status, user.Full_Name, detail.Approval_Date.Value.ToString("dd/MM/yyyy HH:mm:ss")));
        //                        //("Document has been " + detail.Status + " by " + user.Full_Name + " on " + detail.Approval_Date.Value.ToString("dd/MM/yyyy HH:mm:ss"));
        //                }

        //                var approvalId = detail.Approval_Id;

        //                approval = db.Approvals.Where(r => r.Id == approvalId).FirstOrDefault();
        //                if (approval == null)
        //                    throw new Exception(Resources.APPROVAL_INVALID); // ("Approval data not found");

        //                var allLevels = db.Approval_Detail.Where(r => r.Approval_Id == approvalId).ToArray();
        //                if (allLevels.Length <= 0)
        //                    throw new Exception(Resources.APPROVAL_NEXT_INVALID); //("Next approval level not found");

        //                var allDocuments = db.Approval_Document.Where(r => r.Approval_Id == approvalId).ToArray();
        //                if (allDocuments.Length <= 0)
        //                    throw new Exception(Resources.APPROVAL_DOC_INVALID); //("Documents for this approval not found");

        //                string mode = approval.Mode;
        //                string category = approval.Category;
        //                var sendApprovalNotification = false;
        //                var sendOrderApprovalNotification = false;
        //                var sendAppointmentNotification = false;
        //                Shipment shipmentAppointment = new Shipment();
        //                var sendTransporterDispatch = false;
        //                var sendAssignTransporterApproval = false;
        //                var sendOrderAssignTransporterNotification = false;
        //                var headerMailOrder = new Order();
        //                //var sendApprovalNotification = false;

        //                bool fullyApproved = false;

        //                if (status == Constants.ApprovalStatus.REJECTED)
        //                {
        //                    approval.Status = status;
        //                    approval.Active = false;
        //                    approval.Remarks = remarks;

        //                    for (int i = 0; i < allLevels.Length; i++)
        //                    {
        //                        allLevels[i].Active = false;
        //                        allLevels[i].Status = status;
        //                        allLevels[i].Remarks = remarks;
        //                        allLevels[i].Approval_By = username;
        //                        allLevels[i].Approval_Date = DateTime.Now;
        //                        db.Entry(allLevels[i]).State = EntityState.Modified;
        //                    }

        //                    if (category == Constants.TransactionType.ORDER)
        //                    {
        //                        for (int i = 0; i < allDocuments.Length; i++)
        //                        {


        //                            //document.Is_Deleted = true;
        //                            //document.Deleted_Date = DateTime.Now;
        //                            //document.Deleted_By = username;
        //                            if (mode == Constants.FORM_MODE_EDIT.ToUpper())
        //                            {
        //                                //var docModif = db.FLDocumentModified.Where(r => r.FLRealDocument_Id == document.Id && r.Status == Constants.ApprovalStatus.WAIT && !r.Is_Deleted).FirstOrDefault();
        //                                //if (docModif != null)
        //                                //{
        //                                //    docModif.Status = status;
        //                                //    document.Status = Constants.ApprovalStatus.APPROVED;
        //                                //    db.Entry(docModif).State = EntityState.Modified;
        //                                //}
        //                            }
        //                            else if (mode == Constants.FORM_MODE_DELETE.ToUpper())
        //                            {
        //                                //document.Status = Constants.ApprovalStatus.REJECTED;
        //                            }
        //                            else if (mode == Constants.FORM_MODE_DISPATCH.ToUpper())
        //                            {
        //                                var document = allDocuments[i].Order;
        //                                sendTransporterDispatch = false; //true;
        //                                sendAssignTransporterApproval = true;

        //                                UUtils.CopyOrder(headerMailOrder, document);

        //                                document.Status = Constants.OrderStatus.REJECTED;
        //                                //document.Transporter_Id = null;
        //                                string shipmentType = document.Shipment_Type;
        //                                ////document.Is_Deleted = false;

        //                                //if (!document.SAP_Transporter.HasValue || (document.SAP_Transporter.HasValue && !document.SAP_Transporter.Value))
        //                                //{
        //                                //    document.Transporter_Code = null;
        //                                //    document.Transporter_Name = null;
        //                                //    document.Shipment_Type = null;
        //                                //    document.Dispatch_Status = null;
        //                                //    document.Loading_Destination_From_Id = null;
        //                                //    document.Loading_Destination_From_Name = null;
        //                                //    document.Loading_Destination_To_Id = null;
        //                                //    document.Loading_Destination_To_Name = null;
        //                                //    document.Freight_Cost = null;
        //                                //    document.Freight_Cost_Suggested = null;
        //                                //    document.Assign_Reason = null;
        //                                //    document.Assign_Reason_Others = null;
        //                                //}

        //                                document.Dispatch_Status_Remarks = remarks;
        //                                string revertMessage = "", newOrderNo = "";

        //                                //if(shipmentType != Constants.ShipmentType.COLLECTIVE)
        //                                //UUtils.RevertOrderToUnassigned(db, document, Constants.OrderStatus.PENDING, username, shipmentType, out revertMessage, out newOrderNo);

        //                                UUtils.CreateOrderLog(db, document, Constants.OrderStatus.WAITING_FOR_DISPATCH_APPROVAL, Constants.ApprovalStatus.REJECTED, username);
        //                                db.Entry(document).State = EntityState.Modified;
        //                            }
        //                            else
        //                            {
        //                                //Create Order
        //                                var document = allDocuments[i].SAP_Order;
        //                                document.Status = Constants.ApprovalStatus.REJECTED;
        //                                sendOrderApprovalNotification = true;
        //                                db.Entry(document).State = EntityState.Modified;
        //                            }

        //                        }
        //                    }
        //                    else if (category == Constants.TransactionType.APPOINTMENT)
        //                    {
        //                        sendAppointmentNotification = true;
        //                        var shipment = approval.Shipment;

        //                        UUtils.CopyShipment(shipmentAppointment, shipment);
        //                        shipmentAppointment.Id = shipment.Id;
        //                        shipmentAppointment.Shipment_No = shipment.Shipment_No;

        //                        shipment.Appointment_Status = status;
        //                        shipment.Appointment_Status_Remarks = remarks;
        //                        //shipmentAppointment.Shipment_No = shipment.Shipment_No;
        //                        ////shipmentAppointment.Business_Segment = shipment.Business_Segment;
        //                        //UUtils.CopyShipment(shipmentAppointment, shipment);

        //                        shipment.Appointment_Slot = null;
        //                        shipment.Appointment_Matrix_Schedule_Id = null;
        //                        shipment.Appointment_Time_From = null;
        //                        shipment.Appointment_Time_To = null;
        //                        shipment.Appointment_Time = null;
        //                        shipment.Appointment_Date = null;

        //                        //transferDoc.Is_Deleted = true;
        //                        //transferDoc.Deleted_Date = DateTime.Now;
        //                        //transferDoc.Deleted_By = username;
        //                        db.Entry(shipment).State = EntityState.Modified;
        //                    }
        //                    else if (category == Constants.TransactionType.FORCE_REJECT)
        //                    {
        //                        var forceReject = approval.Force_Reject;

        //                        forceReject.Status = Constants.ApprovalStatus.REJECTED;
        //                        db.Entry(forceReject).State = EntityState.Modified;
        //                    }
        //                }
        //                else if (status == Constants.ApprovalStatus.APPROVED)
        //                {
        //                    var matrix = approval.Approval_Matrix == null ? "ALL" : approval.Approval_Matrix.Validity;
        //                    var approveOrder = false;
        //                    var approveAppointment = false;
        //                    var approveDispatch = false;
        //                    var approveForceReject = false;

        //                    if (matrix == Constants.ApprovalValidity.EITHER)
        //                    {
        //                        approval.Status = status;

        //                        for (int i = 0; i < allLevels.Length; i++)
        //                        {
        //                            allLevels[i].Active = false;
        //                            allLevels[i].Approval_By = username;
        //                            allLevels[i].Approval_Date = DateTime.Now;
        //                            db.Entry(allLevels[i]).State = EntityState.Modified;
        //                        }

        //                        if (category == Constants.TransactionType.ORDER)
        //                        {
        //                            approveOrder = true;
        //                        }
        //                        else if (category == Constants.TransactionType.APPOINTMENT)
        //                        {
        //                            sendAppointmentNotification = true;
        //                            approveAppointment = true;
        //                        }
        //                        else if (category == Constants.TransactionType.FORCE_REJECT)
        //                        {
        //                            approveForceReject = true;
        //                        }

        //                        fullyApproved = true;
        //                    }
        //                    else if (matrix == Constants.ApprovalValidity.ALL)
        //                    {
        //                        int currentLevel = detail.Level.Value;
        //                        var currentLevelApprovers = allLevels.Where(r => r.Level == currentLevel).ToArray();

        //                        for (int x = 0; x < currentLevelApprovers.Length; x++)
        //                        {
        //                            currentLevelApprovers[x].Active = false;
        //                            currentLevelApprovers[x].Status = status;
        //                            currentLevelApprovers[x].Approval_By = username;
        //                            currentLevelApprovers[x].Approval_Date = DateTime.Now;
        //                            db.Entry(currentLevelApprovers[x]).State = EntityState.Modified;
        //                        }

        //                        int nextLevel = currentLevel + 1;
        //                        var nextLevelApprovers = allLevels.Where(r => r.Level == nextLevel).ToArray();

        //                        if (nextLevelApprovers.Length > 0)
        //                        {
        //                            for (int x = 0; x < nextLevelApprovers.Length; x++)
        //                            {
        //                                if (!nextApprovers.Contains(nextLevelApprovers[x])) nextApprovers.Add(nextLevelApprovers[x]);

        //                                nextLevelApprovers[x].Active = true;
        //                                db.Entry(nextLevelApprovers[x]).State = EntityState.Modified;
        //                                sendApprovalNotification = true;
        //                            }


        //                            if (category == Constants.TransactionType.ORDER && mode == Constants.FORM_MODE_DISPATCH.ToUpper())
        //                            {
        //                                var document = approval.Order;
        //                                UUtils.CopyOrder(headerMailOrder, document);

        //                                headerMailOrder.Id = document.Id;
        //                                headerMailOrder.Order_No = document.Order_No;
        //                                headerMailOrder.Created_By = document.Created_By;
        //                                headerMailOrder.Created_Date = document.Created_Date;
        //                                sendOrderAssignTransporterNotification = true;
        //                            }

        //                        }
        //                        else
        //                        {
        //                            approval.Status = status;

        //                            if (category == Constants.TransactionType.ORDER)
        //                            {
        //                                sendOrderApprovalNotification = true; //SEND EMAIL IF FULLY APPROVED;
        //                                approveOrder = true;
        //                            }
        //                            else if (category == Constants.TransactionType.APPOINTMENT)
        //                            {
        //                                sendAppointmentNotification = true;
        //                                approveAppointment = true;
        //                            }
        //                            else if (category == Constants.TransactionType.FORCE_REJECT)
        //                            {
        //                                approveForceReject = true;
        //                            }

        //                            fullyApproved = true;
        //                        }
        //                    }

        //                    if (category == Constants.TransactionType.ORDER)
        //                    {
        //                        for (int i = 0; i < allDocuments.Length; i++)
        //                        {


        //                            if (approveOrder)
        //                            {
        //                                string currentStatus = "";

        //                                if (mode == Constants.FORM_MODE_CREATE.ToUpper())
        //                                {
        //                                    var order = allDocuments[i].SAP_Order;
        //                                    currentStatus = "";

        //                                    order.Status = Constants.OrderStatus.APPROVED;

        //                                    string relationType = "";
        //                                    //var isRelation = UUtils.IsRelationOrder(db, order.Order_Type.Code, order.Incoterm.Incoterms, out relationType);

        //                                    //if (isRelation)
        //                                    //{
        //                                    //    //order.Status = Constants.OrderStatus.DISPATCHED;
        //                                    //    //order.Dispatch_Status_Date = DateTime.Now;
        //                                    //    //order.Dispatch_Status = Constants.OrderStatus.ACCEPTED;
        //                                    //    //order.Dispatch_Approval_Date = DateTime.Now;

        //                                    //}
        //                                    //else
        //                                    //{
        //                                    //    order.Status = Constants.OrderStatus.PENDING;
        //                                    //}

        //                                    //UUtils.CreateOrderLog(db, order, currentStatus, Constants.ApprovalStatus.APPROVED, username);

        //                                    db.Entry(order).State = EntityState.Modified;
        //                                }
        //                                else if (mode == Constants.FORM_MODE_DISPATCH.ToUpper())
        //                                {
        //                                    var order = allDocuments[i].Order;
        //                                    currentStatus = order.Status;

        //                                    sendOrderApprovalNotification = false;
        //                                    sendTransporterDispatch = true;
        //                                    sendAssignTransporterApproval = true;
        //                                    headerMailOrder = order;
        //                                    order.Status = Constants.OrderStatus.DISPATCHED;
        //                                    order.Dispatch_Status = Constants.OrderStatus.WAITING_FOR_APPROVAL;
        //                                    order.Dispatch_Approval_Date = DateTime.Now;
        //                                    order.Mark_Complete = null;
        //                                    order.Complete_Order = null;

        //                                    var companyCode = "";
        //                                    var businessSegmentCode = "";

        //                                    if (order.Company_Id.HasValue) companyCode = order.Company.Code;
        //                                    else companyCode = order.Company_Code;

        //                                    if (order.Business_Segment_Id.HasValue) businessSegmentCode = order.Business_Segment.Code;
        //                                    else businessSegmentCode = order.Business_Segment_Code;


        //                                    string messageAutoReject = "";
        //                                    DateTime autoRejectDate = DateTime.Now;
        //                                    bool getAutoReject = UUtils.GetAutoRejectDate(db, companyCode, businessSegmentCode, order.Status, null, DateTime.Now, out messageAutoReject, out autoRejectDate);
        //                                    if (getAutoReject)
        //                                    {
        //                                        order.Auto_Reject_Date = autoRejectDate;
        //                                    }
        //                                    else
        //                                    {

        //                                    }

        //                                    //string message = "";
        //                                    //bool copied = UUtils.CopyOrder(db, order, username, out message);
        //                                    //if (!copied)
        //                                    //{

        //                                    //}


        //                                    UUtils.CreateOrderLog(db, order, currentStatus, Constants.ApprovalStatus.APPROVED, username);
        //                                    UUtils.CreateOrderLog(db, order, Constants.ApprovalStatus.APPROVED, order.Status, username);

        //                                    db.Entry(order).State = EntityState.Modified;
        //                                }



        //                            }
        //                        }
        //                    }
        //                    else if (category == Constants.TransactionType.APPOINTMENT)
        //                    {
        //                        var shipment = approval.Shipment;

        //                        if (approveAppointment)
        //                        {
        //                            sendAppointmentNotification = true;
        //                            var appointmentDate = shipment.Appointment_Date.Value;
        //                            var appointmentTimeTo = shipment.Appointment_Time_To.Value;

        //                            var validationDate = new DateTime(appointmentDate.Year, appointmentDate.Month, appointmentDate.Day, appointmentTimeTo.Hour, appointmentTimeTo.Minute, appointmentTimeTo.Second);

        //                            if (DateTime.Now > validationDate)
        //                                throw new Exception(Resources.APPOINTMENT_APPROVAL_EXPIRED);

        //                            string currentShipmentStatus = shipment.Status;
        //                            string currentShipmentSubStatus = shipment.Progress_Status;

        //                            shipment.Appointment_Status = Constants.ApprovalStatus.APPROVED;
        //                            shipment.Appointment_Status_Date = DateTime.Now;
        //                            shipment.Status = Constants.OrderStatus.IN_PROGRESS;

        //                            if (shipment.Progress_Status != Constants.ShipmentSubStatus.REGISTRATION_IN && shipment.Progress_Status != Constants.ShipmentSubStatus.REGISTRATION_OUT)
        //                            {
        //                                shipment.Progress_Status = Constants.ShipmentSubStatus.APPOINTMENT;
        //                            }

        //                            shipment.Appointment_Status_Remarks = remarks;

        //                            //var shipmentDetails = db.Shipment_Detail.Where(r => r.Shipment_Id == shipment.Id).ToArray();
        //                            //for (int x = 0; x < shipmentDetails.Length; x++)
        //                            //{
        //                            //    var shipmentOrder = shipmentDetails[x];
        //                            //    string currentStatus = shipmentOrder.Order.Status;
        //                            //    //shipmentOrder.Order.Status = Constants.OrderStatus.IN_PROGRESS;
        //                            //    //db.Entry(shipmentOrder.Order).State = EntityState.Modified;
        //                            //    //UUtils.CreateOrderLog(db, shipmentOrder.Order, currentStatus, Constants.OrderStatus.IN_PROGRESS, username);
        //                            //}

        //                            UUtils.CreateShipmentLog(db, shipment, currentShipmentStatus, currentShipmentSubStatus, shipment.Status, shipment.Progress_Status, username);
        //                            shipmentAppointment.Shipment_No = shipment.Shipment_No;
        //                            UUtils.CopyShipment(shipmentAppointment, shipment);
        //                            shipmentAppointment.Id = shipment.Id;
        //                            db.Entry(shipment).State = EntityState.Modified;
        //                        }
        //                    }
        //                    else if (category == Constants.TransactionType.FORCE_REJECT)
        //                    {
        //                        //var forceReject = approval.Force_Reject;

        //                        //if (approveForceReject)
        //                        //{
        //                        //    string approvalMessage = "";
        //                        //    bool approve = AutoRejectUtils.RejectOrderApproved(db, approval, forceReject, out approvalMessage);
        //                        //    if (!approve)
        //                        //    {
        //                        //        throw new Exception(approvalMessage);
        //                        //    }
        //                        //}
        //                    }
        //                }

        //                approval.Last_Approval_By = username;
        //                approval.Last_Approval_Date = DateTime.Now;
        //                db.Entry(approval).State = EntityState.Modified;

        //                db.SaveChanges();
        //                trans.Commit();

        //                var listNextApprovers = nextApprovers;
        //                var approvalData = approval;

        //                Task.Run(() =>
        //                {
        //                    if (sendOrderApprovalNotification)
        //                    {
        //                        UMail.SendOrderApprovalMail(db, approvalData, listNextApprovers, status, remarks);
        //                    }

        //                    //if (sendApprovalNotification)
        //                    //{

        //                    //    //send level selanjutnya
        //                    //}
        //                    ////UMail.SendApprovalMail();
        //                    if (sendTransporterDispatch)
        //                    {

        //                        UMail.SendTransporterAcceptReject(db, headerMailOrder, headerMailOrder.Transporter_Code, status, approvalData);
        //                    }
        //                    if (sendAppointmentNotification)
        //                    {
        //                        UMail.SendApprovalAppointmentMail(db, approvalData, shipmentAppointment, listNextApprovers, status, remarks);
        //                    }

        //                    //Send to next approvers
        //                    if (sendOrderAssignTransporterNotification)
        //                    {
        //                        var nextReleaseMatrix = listNextApprovers.Select(r => r.Info1).FirstOrDefault();
        //                        var nextApproversName = listNextApprovers.Select(r => r.Username).ToList();
        //                        UMail.SendAssignTransporter(db, headerMailOrder, approvalData.Created_By, nextApproversName, nextReleaseMatrix);
        //                    }

        //                    if (sendApprovalNotification)
        //                    {
        //                        //send level selanjutnya
        //                    }
        //                    //UMail.SendApprovalMail();


        //                    if (sendAssignTransporterApproval)
        //                    {
        //                        var approvalMatrixLower = db.Lookups.Where(r => r.Lookup_Group == Constants.LookupGroup.ASSIGN_TRANSPORTER_MATRIX && r.Lookup_Key == Constants.AssignedMatrixType.LOWER && !r.Is_Deleted).FirstOrDefault();
        //                        if (approvalData.Info1 != approvalMatrixLower.Lookup_Value)
        //                        {
        //                            UMail.SendAssignTransporterApprovalMail(db, headerMailOrder, approvalData, listNextApprovers, status, remarks);
        //                        }
        //                    }

        //                    //allLevels = db.FLApproval_Detail.Where(r => r.FLApproval_Id == approvalId).ToArray();
        //                    //if (allLevels.Length > 0)
        //                    //    SendRequesterNotification(db, approval.Reference_No, status, approvalUser.Email, allLevels.ToList(), fullyApproved);
        //                });

        //                //allLevels = db.FLApproval_Detail.Where(r => r.FLApproval_Id == approvalId).ToArray();
        //                //if (allLevels.Length > 0)
        //                //    SendRequesterNotification(db, approval.Reference_No, status, approvalUser.Email, allLevels.ToList(), fullyApproved);
        //            }
        //            else
        //            {
        //                throw new Exception(Resources.APPROVAL_LEVEL_INVALID); //("Approval level not found");
        //            }

        //            result = "OK";
        //        }
        //        catch (Exception ex)
        //        {
        //            trans.Rollback();
        //            result = ex.Message;
        //        }
        //    }

        //    return result;
        //}

    }
}
