using FAMS_Data;
using FAMS_Models.Resources;
using FAMS_Models.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;

namespace FAMS_ViewModels.Utilities
{
//    public class UMail
//    {
//        static string cellStyle = "padding: 5px; border: 1px solid black;width:150px";
//        static string basicStyle = "padding: 6px; border: 1px solid black";
//        static string headerStyle = "padding: 6px; border: 1px solid black;background-color:#b8b6b6";
//        static string cellStyleData = "padding: 5px; border: 1px solid black;width:300px";
//        static string dividerStyle = "border-top:none;border-bottom:none;width:30px";
//        static string divider = "";
//        static string dividerCol = $"<td style='{dividerStyle}'>{divider}</td>";
//        static string newLines = "<br /><br />";

//        public static string SendMail(DMSEntities db, string refNo, string action, string subject, string body, List<string> to = null, List<string> cc = null, List<string> bcc = null, List<Attachment> attachments = null)
//        {
//            string result = "OK";
//            string host = UUtils.GetSetting("EMAIL_HOST"); //ConfigurationManager.AppSettings["EMAIL_HOST"].ToString();
//            string user = UUtils.GetSetting("EMAIL_USER"); //ConfigurationManager.AppSettings["EMAIL_USER"].ToString();
//            string pass = UUtils.GetSetting("EMAIL_PASS"); //ConfigurationManager.AppSettings["EMAIL_PASS"].ToString();

//            var logTo = new List<string>();
//            var logCc = new List<string>();

//            try
//            {
//                using (SmtpClient client = new SmtpClient(host))
//                {
//                    client.Credentials = new NetworkCredential(user, pass);
//                    client.EnableSsl = true;
//                    client.Port = 587;
//                    //client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
//                    //client.PickupDirectoryLocation = "D:/TESTMAIL/";
//                    using (MailMessage message = new MailMessage())
//                    {
//                        message.Subject = subject;
//                        message.From = new MailAddress(user, "FAMS");
//                        //message.To.Add(new MailAddress("stenley.teh@id.company-intl.com"));

//                        //to = new List<string>();
//                        //cc = new List<string>();
//                        //to.Add("stenley.teh@id.company-intl.com");

//                        //QA
//                        if(cc == null)
//                        {
//                            cc = new List<string>();
//                        }
//                        cc.Add("stenley.teh@id.company-intl.com");


//                        if (to != null)
//                        {
//                            foreach (string s in to)
//                            {
//                                if (!string.IsNullOrEmpty(s))
//                                    message.To.Add(new MailAddress(s.TrimStart().TrimEnd()));
//                            }
//                        }
//                        if (cc != null)
//                        {
//                            foreach (string s in cc)
//                            {
//                                if (!string.IsNullOrEmpty(s))
//                                    message.CC.Add(new MailAddress(s.TrimStart().TrimEnd()));
//                            }
//                        }
//                        if (bcc != null)
//                        {
//                            foreach (string s in bcc)
//                                message.Bcc.Add(new MailAddress(s.TrimStart().TrimEnd()));
//                        }

//                        if (attachments != null && attachments.Count > 0)
//                        {
//                            foreach (Attachment attachment in attachments)
//                                message.Attachments.Add(attachment);
//                        }

//                        logTo = message.To.Select(r => r.Address).ToList();
//                        logCc = message.CC.Select(r => r.Address).ToList();

//                        string messageBody = @"<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'><title></title></head>";
//                        messageBody += "<div style=\"font-family:Verdana, Geneva, sans-serif;font-size:11px\">";
//                        messageBody += body;
//                        messageBody += "</div>";
//                        messageBody += "</html>";
//                        message.Body = messageBody;
//                        message.IsBodyHtml = true;
//                        client.Send(message);

//                        result = "OK";
//                    }
//                }
//            }
//            catch (Exception exc)
//            {
//                result = exc.Message.Replace(Environment.NewLine, " ");
//            }

//            UUtils.CreateMailLog(db, refNo, action, logTo, logCc, bcc, subject, result);

//            return result;
//        }
//        public static string SendFCM(string title, string content, List<string> fcmTokens)
//        {
//            string result = "OK";
//            try
//            {
//                FirebaseUtils.initFirebase();
//                FirebaseUtils.seAsync(fcmTokens, title, content);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }

//            return result;
//        }

//        public static string SendCreateOrder(DMSEntities db, SAP_Order header, List<string> firstApprovers)
//        {
//            var result = "";
//            var body = "";
//            var url = UUtils.GetApplicationLink();
//            try
//            {
//                StringBuilder sb = new StringBuilder();
//                string newLines = "<br /><br />";

//                var to = new List<string>();
//                var approversName = new List<string>();
//                foreach (var username in firstApprovers)
//                {
//                    var userData = db.Users.Where(r => r.Employee_Number == username && !r.Is_Deleted).FirstOrDefault();
//                    if (userData != null)
//                    {
//                        if (!to.Contains(userData.Email)) to.Add(userData.Email);
//                        if (!approversName.Contains(userData.Full_Name)) approversName.Add(userData.Full_Name);
//                    }
//                }

//                var creatorUsername = header.Created_By.Split('[')[1]
//                                                      .Split(']')[0]
//                                                      .Trim();
//                var user = db.Users.Where(r => r.Employee_Number == creatorUsername).FirstOrDefault();
//                var creatorName = user != null ? user.Full_Name : "";

//                sb.Append($"Dear {string.Join(" / ", approversName)},");
//                sb.Append(newLines);
//                sb.Append($"<b>Please <span style='color:green'>Approve</span> or <span style='color:red'>Reject</span> the order by logging in to FAMS ({url})</b>");
//                sb.Append(newLines);
//                sb.Append("Below is the Order details for Order Number: <b>" + header.Document_No + "</b>");
//                sb.Append(newLines);

//                var orderNo = header.Document_No;
//                //var sapNo = header.SAP_No;
//                var productCategoryName = header.Product_Category; //Business_Segment_Name;
//                var plantName = header.Plant; //header.Plant_Name;
//                var warehouse = header.Warehouse_Code; //header.Warehouse_Name;
//                var distribution = header.Distribution;
//                var incoterm = header.Incoterm;
//                var docType = header.Document_Type;
//                var orderType = header.Order_Type;
//                var poNo = header.PO_No;
//                //var soNo = header.SO_No;
//                //var doNo = header.DO_BL_No;
//                //var grCustomNo = header.GR_Custom_No;
//                //var documentDate = header.Order_Date;
//                var documentDate = header.Document_Date;
//                var deliveryAddress = header.Delivery_Address;

//                var orderTypeData = db.Order_Type.Where(r => r.Code == header.Order_Type && !r.Is_Deleted).FirstOrDefault();
//                if (orderTypeData == null)
//                    throw new Exception(Resources.ORDER_TYPE_INVALID);

//                var details = new List<SAPOrderDetailViewModel>();
//                var orderDetails = db.SAP_Order_Detail.Where(r => r.Document_No == header.Document_No).ToArray();
//                foreach (var orderDetail in orderDetails)
//                {
//                    details.Add(new SAPOrderDetailViewModel(orderDetail));
//                }

//                string relationType = "";
//                var isRelation = UUtils.IsRelationOrder(db, header.Order_Type, header.Incoterm, out relationType);

//                sb.Append(isRelation ? GetMailRelationOrderDetail(db, header, details) : GetMailOrderDetail(db, header, details));
//                sb.Append(newLines);
//                sb.Append("<strong>Thanks and Regards, </strong>");
//                sb.Append(newLines);
//                sb.Append($"<b>{creatorName}</b>");
//                sb.Append(newLines);
//                sb.Append("<i>Note: This is an auto-generated E-Mail</i>");

//                body = sb.ToString();

//                string refNo = header.Document_No; //header.SAP_No + " / " + header.Order_No;
//                var subject = string.Format(BaseProgram.GetResourceValue("EMAIL_SUBJECT_CREATE_ORDER_MANUAL"), refNo);
//                result = SendMail(db, refNo, "CREATE ORDER", subject, body, to);
//                //SendFCM("FAMS Order", "", to);
//            }
//            catch (Exception exc)
//            {

//                result = exc.Message.Replace(Environment.NewLine, " ");
//            }
//            return result;
//        }

//        public static string SendCreateAppointment(DMSEntities db, Shipment header, List<string> users, bool editAppointment)
//        {
//            var result = "";
//            var body = "";
//            var url = UUtils.GetApplicationLink(); //HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.Port != 443 ? ":" + HttpContext.Current.Request.Url.Port : "");
//            //to = new List<String>();
//            //cc = new List<String>();
//            try
//            {
//                var approversName = new List<string>();
//                foreach (var username in users)
//                {
//                    var userData = db.Users.Where(r => r.Employee_Number == username && !r.Is_Deleted).FirstOrDefault();
//                    if (userData != null)
//                    {
//                        if (!approversName.Contains(userData.Full_Name)) approversName.Add(userData.Full_Name);
//                    }
//                }

//                //var orders = header.Shipment_Order_Detail.ToArray();

//                var orders = db.Shipment_Order_Detail.Where(r => r.Shipment_Id == header.Id && !r.Is_Deleted).ToArray();
                
//                var shipmentNo = header.Shipment_No;
//                var ordersNo = string.Join(", ", orders.Select(r => r.Order_No).ToList());

//                ShipmentViewModel shipment = new ShipmentViewModel(header);

//                var details = db.Shipment_Order_Detail.Where(r => r.Shipment_Id == shipment.Id && !r.Is_Deleted).ToList();
//                var drivers = db.Shipment_Driver.Where(r => r.Shipment_Id == shipment.Id && !r.Is_Deleted).ToList();

//                var loadingData = db.Locations.Where(r => r.Id == shipment.Loading_Destination_From_Id).FirstOrDefault();
//                var destinationData = db.Locations.Where(r => r.Id == shipment.Loading_Destination_To_Id).FirstOrDefault();

//                var appointmentDate = shipment.Appointment_Date.Value.ToString("dd/MM/yyyy");
//                var appointmentTime = shipment.Appointment_Time;
//                var productCategory = shipment.Product_Category;
//                var loading = $"[{loadingData.Code}] {loadingData.Description}";
//                var destination = $"[{destinationData.Code}] {destinationData.Description}";
//                var transporterName = shipment.Transporter_Name;
//                var truckTypeName = shipment.Truck_Type_Name;

//                StringBuilder sb = new StringBuilder();
//                string newLines = "<br /><br />";
//                string cellStyle = "padding: 8px; border: 1px solid black";
//                sb.Append($"Dear {string.Join(" / ", approversName)},");
//                sb.Append(newLines);

//                sb.Append($"<b>Please <span style='color:green'>Approve</span> or <span style='color:red'>Reject</span> the appointment by logging in to FAMS</b> ({url})");
//                sb.Append(newLines);
//                sb.Append($"Shipment with Number: {shipmentNo} ; Order with number: {ordersNo} , {(editAppointment ? "edit an" : "have a new")} appointment");
//                //sb.Append("Here is a new appointment with reference number: <b>" + header.SAP_No + "/" + header.Shipment_No + "</b>");
//                sb.Append(newLines);

//                sb.Append(GetMailAppointment(db, header, details, drivers));

//                sb.Append(newLines);
//                sb.Append("<strong>Thanks and Regards, </strong>");
//                sb.Append(newLines);
//                sb.Append($"<b>{shipment.Transporter_Name}</b>");
//                sb.Append(newLines);
//                sb.Append("<i>Note: This is an auto-generated E-Mail</i>");

//                body = sb.ToString();

//                //EMAIL TO
//                var to = new List<string>();
//                var cc = new List<string>();

//                var shipmentDriver = db.Shipment_Driver.Where(r => r.Shipment_Id == shipment.Id && !r.Is_Deleted).ToList();
//                var fcmTokens = new List<String>();
//                foreach (var sd in shipmentDriver)
//                {
//                    var driver = db.Users.Where(r => r.Driver_Id == sd.Driver_Id && !r.Is_Deleted).FirstOrDefault();
//                    if (driver != null)
//                    {
//                        if (driver.fcm_token != null)
//                        {
//                            fcmTokens.Add(driver.fcm_token);
//                        }
//                        if (!string.IsNullOrEmpty(driver.Email) && !cc.Contains(driver.Email))
//                            cc.Add(driver.Email);
//                    }

//                }

//                var warehouseLocation = "";
//                var warehouse = db.Warehouses.Where(r => r.Code == shipment.Warehouse_Code && !r.Is_Deleted).FirstOrDefault();
//                if (warehouse != null) warehouseLocation = warehouse.Location;

//                //Transporters + Locations
//                var transporters = db.Users.Where(r => r.Transporter_Code == header.Transporter_Code &&
//                                                        r.Role == Constants.Roles.TRANSPORTER &&
//                                                        !r.Is_Deleted &&
//                                                        (r.Location_All || (!r.Location_All && db.User_Location.Where(x => x.User_Id == r.Employee_Number && x.Location_Code == warehouseLocation && !r.Is_Deleted).Any()))
//                                                ).ToArray();
//                if (transporters != null)
//                {
//                    foreach (var transporter in transporters)
//                    {
//                        var transporterToken = transporter.fcm_token;

//                        fcmTokens.Add(transporterToken);
//                        if (!string.IsNullOrEmpty(transporter.Email) && !cc.Contains(transporter.Email))
//                            cc.Add(transporter.Email);
//                    }
//                }

//                foreach (var user in users)
//                {
//                    var userData = db.Users.Where(r => r.Employee_Number == user && !r.Is_Deleted).FirstOrDefault();
//                    if (userData != null)
//                    {
//                        if (!to.Contains(userData.Email)) to.Add(userData.Email);
//                    }
//                }

//                string refNo = header.Shipment_No; // header.SAP_No + " / " + header.Shipment_No;
//                string subject = string.Format(BaseProgram.GetResourceValue("EMAIL_SUBJECT_TRANSPORTER_CREATE_APPOINTMENT"), refNo, editAppointment ? "EDIT" : "CREATE");
//                result = SendMail(db, refNo, "CREATE APPOINTMENT", subject, body, to);

//                SendFCM("FAMS Appointment", "Appointment for Shipment Number: " + header.Shipment_No + " created", fcmTokens);
//            }
//            catch (Exception exc)
//            {

//                result = exc.Message.Replace(Environment.NewLine, " ");
//            }
//            return result;
//        }

//        public static string SendAssignTransporter(DMSEntities db, Order header, string username, List<string> firstApprovers, string nextMatrix)
//        {
//            var result = "";
//            var body = "";
//            var url = UUtils.GetApplicationLink(); //HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.Port != 443 ? ":" + HttpContext.Current.Request.Url.Port : "");
//            //to = new List<String>();
//            //cc = new List<String>();
//            try
//            {
//                StringBuilder sb = new StringBuilder();
//                string newLines = "<br /><br />";
//                string cellStyle = "padding: 8px; border: 1px solid black";

//                var to = new List<String>();
//                var approversName = new List<string>();
//                foreach (var approverUser in firstApprovers)
//                {
//                    var userData = db.Users.Where(r => r.Employee_Number == approverUser && !r.Is_Deleted).FirstOrDefault();
//                    if (userData != null)
//                    {
//                        if (!to.Contains(userData.Email)) to.Add(userData.Email);
//                        if (!approversName.Contains(userData.Full_Name)) approversName.Add(userData.Full_Name);
//                    }
//                }

//                var creatorUsername = username.Split('[')[1]
//                                                      .Split(']')[0]
//                                                      .Trim();
//                var user = db.Users.Where(r => r.Employee_Number == creatorUsername).FirstOrDefault();
//                var creatorName = user != null ? user.Full_Name : "";

//                sb.Append($"Dear {string.Join(" / ", approversName)},");


//                //sb.Append("Dear Approver,");
//                sb.Append(newLines);
//                string refNo = header.Order_No; //header.SAP_No + " / " + header.Order_No;

//                sb.Append($"<b>Please <span style='color:green'>Approve</span> or <span style='color:red'>Reject</span> the Order by logging in to FAMS ({url})</b>");
//                sb.Append(newLines);
//                sb.Append($"Order with number : <b>{header.Order_No}</b> has been assigned to Transporter : <b>[{header.Transporter_Code}] {header.Transporter_Name}</b>");
//                sb.Append(newLines);
//                //sb.Append(newLines);
//                //sb.Append(DisplayNames.FREIGHT_COST_SELECTED + " : " + header.Freight_Cost);
//                //sb.Append(newLines);

//                //List<Order_DetailViewModel> orderDetail = new List<Order_DetailViewModel>();
//                //var orderDetails = db.Order_Detail.Where(r => r.Order_Id == header.Id && !r.Is_Deleted).ToArray();
//                //for (int i = 0; i < orderDetails.Length; i++)
//                //{
//                //    orderDetail.Add(new Order_DetailViewModel(orderDetails[i], false));
//                //}
//                //sb.Append(GetMailOrderDetail(db, new OrderViewModel(header), orderDetail));
//                //sb.Append(newLines);

//                string approvalMatrix = nextMatrix;
//                //foreach(var matrix in approvalMatrixList)
//                //{
//                //    var matrixLookup = db.Lookups.Where(r => r.Lookup_Group == Constants.LookupGroup.ASSIGN_TRANSPORTER_MATRIX && r.Lookup_Key == matrix).FirstOrDefault();
//                //    if(matrixLookup != null)
//                //    {
//                //        approvalMatrix += matrixLookup.Lookup_Value + ", ";
//                //    }
//                //}

//                //approvalMatrix = approvalMatrix.TrimEnd(' ').TrimEnd(',');

//                var orderFreightLog = db.Order_Freight_Log.Where(r => r.Order_Id == header.Id && !r.Is_Deleted).ToList();


//                var approvalMatrixLookup = db.Lookups.Where(r => r.Lookup_Group == Constants.LookupGroup.ASSIGN_TRANSPORTER_MATRIX && r.Lookup_Key == approvalMatrix).FirstOrDefault();
//                var approvalMatrixName = approvalMatrixLookup == null ? "" : approvalMatrixLookup.Lookup_Value;

//                sb.Append(GetMailAssignTransporter(db, header, approvalMatrixName, orderFreightLog));

//                //if (header.Assign_Reason != null && header.Assign_Reason != "")
//                //{
//                //    var reason = "";
//                //    if (header.Assign_Reason_Others != null && header.Assign_Reason_Others != "")
//                //    {
//                //        reason = header.Assign_Reason_Others;
//                //    }
//                //    else
//                //    {
//                //        reason = db.Lookups.Where(r => r.Lookup_Key == header.Assign_Reason).Select(r => r.Lookup_Value).FirstOrDefault();
//                //    }

//                //    sb.Append(DisplayNames.REASON + " : " + reason);
//                //    sb.Append(newLines);
//                //}

//                //sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//                //sb.Append("<thead>");
//                //sb.Append("<tr>");
//                //sb.Append($"<th style='{headerStyle}'>No</th>");
//                //sb.Append($"<th style='{headerStyle}'>Transporter</th>");
//                //sb.Append("</tr>");
//                //sb.Append("</thead>");
//                //sb.Append("<tbody>");
//                //var index = 1;
//                //foreach (var detail in transporterCode)
//                //{
//                //    sb.Append("<tr>");
//                //    sb.Append($"<td style='{basicStyle}'>{index}</td>");
//                //    sb.Append($"<td style='{cellStyleData}'>{detail}</td>");
//                //    sb.Append("</tr>");
//                //    index++;
//                //}
//                //sb.Append("</tbody>");
//                //sb.Append("</table>");

//                //sb.Append(newLines);
//                //sb.Append(newLines);
//                //foreach (var trans in transporterCode)
//                //{
//                //    to.AddRange(db.Users.Where(r => r.Transporter_Code == trans && r.Employee_Number != trans).Select(r => r.Email).ToList());
//                //}
//                //cc.Add(db.Users.Where(r => header.Created_By.Contains(r.Employee_Number)).Select(r => r.Email).FirstOrDefault());

//                //var to = new List<string>();
//                //foreach(var approver in approvers)
//                //{
//                //    var userData = db.Users.Where(r => r.Employee_Number == approver && !r.Is_Deleted).FirstOrDefault();
//                //    if (!String.IsNullOrEmpty(userData.Email))
//                //    {
//                //        if (!to.Contains(userData.Email)) to.Add(userData.Email);
//                //    }
//                //}

//                sb.Append(newLines);
//                sb.Append("<strong>Thanks and Regards, </strong>");
//                sb.Append(newLines);
//                sb.Append($"<b>{creatorName}</b>");
//                sb.Append(newLines);
//                sb.Append("<i>Note: This is an auto-generated E-Mail</i>");

//                body = sb.ToString();



//                string subject = string.Format(BaseProgram.GetResourceValue("EMAIL_SUBJECT_WAITING_APPROVAL_ASSIGN_TRANSPORTER"), refNo, approvalMatrixName);

//                result = SendMail(db, refNo, "ASSIGN TRANSPORTER (WILMAR)", subject, body, to);

//                string warehouseLocation = "";
//                var transporterTokens = db.Users.Where(r => r.Employee_Number == header.Transporter_Code && r.Role == Constants.Roles.TRANSPORTER && !r.Is_Deleted).Select(r => r.fcm_token).ToList();
//                var fcmTokens = new List<String>();
//                var transporters = db.Users.Where(r => r.Transporter_Code == header.Transporter_Code &&
//                                                        r.Role == Constants.Roles.TRANSPORTER &&
//                                                        !r.Is_Deleted &&
//                                                        (r.Location_All || (!r.Location_All && db.User_Location.Where(x => x.User_Id == r.Employee_Number && x.Location_Code == warehouseLocation && !r.Is_Deleted).Any()))
//                                                ).ToArray();
//                if (transporters != null)
//                {
//                    foreach (var transporter in transporters)
//                    {
//                        var transporterToken = transporter.fcm_token;

//                        fcmTokens.Add(transporterToken);
//                        //if (!string.IsNullOrEmpty(transporter.Email) && !cc.Contains(transporter.Email))
//                        //    cc.Add(transporter.Email);
//                    }
//                }
//                foreach (var tkn in transporterTokens)
//                {
//                    fcmTokens.Add(tkn);
//                }
//                SendFCM("FAMS Assign Transporter", "Order with number : " + header.Order_No + " has been assigned to you", fcmTokens);
//            }
//            catch (Exception exc)
//            {
//                result = exc.Message.Replace(Environment.NewLine, " ");
//            }
//            return result;
//        }

//        public static string SendAssignTransporterApprovalMail(DMSEntities db, Order order, Approval header, List<Approval_Detail> details, string status, string approvalRemarks)
//        {
//            var result = "";
//            var body = "";
//            var to = new List<String>();
//            var cc = new List<String>();
//            try
//            {
//                StringBuilder sb = new StringBuilder();

//                var creatorUsername = header.Created_By.Split('[')[1]
//                                                    .Split(']')[0]
//                                                    .Trim();
//                var user = db.Users.Where(r => r.Employee_Number == creatorUsername).FirstOrDefault();
//                var creatorName = user != null ? user.Full_Name : "";

//                var approverUsername = header.Last_Approval_By;
//                var approver = db.Users.Where(r => r.Employee_Number == approverUsername).FirstOrDefault();
//                var approverName = approver != null ? approver.Full_Name : "";

//                //var to = new List<string>();
//                //var approversName = new List<string>();
//                //foreach (var username in firstApprovers)
//                //{
//                //    var userData = db.Users.Where(r => r.Employee_Number == username && !r.Is_Deleted).FirstOrDefault();
//                //    if (userData != null)
//                //    {
//                //        if (!to.Contains(userData.Email)) to.Add(userData.Email);
//                //        if (!approversName.Contains(userData.Full_Name)) approversName.Add(userData.Full_Name);
//                //    }
//                //}

//                string color = status == Constants.ApprovalStatus.APPROVED ? "green" : "red";

//                sb.Append($"Dear {creatorName},");
//                sb.Append(newLines);
//                sb.Append("Order with Reference Number: <b>" + header.Reference_No + $"</b> has been <b><span style='color:{color}'>" + status + "</span></b>");
//                sb.Append(newLines);
//                //if (!string.IsNullOrEmpty(approvalRemarks))
//                //{
//                //    sb.Append($"<b>REMARKS: {approvalRemarks}</b>");
//                //    sb.Append(newLines);
//                //}

//                /*Show reason if approver reject*/
//                if (!string.IsNullOrEmpty(approvalRemarks))
//                {
//                    sb.Append("<b>" + DisplayNames.REASON + " : " + header.Remarks + "</b>");
//                    sb.Append(newLines);
//                }

//                var orderDetailsModel = new List<OrderDetailViewModel>();
//                var orderDetails = db.Order_Detail.Where(r => r.Order_Id == header.Order_Id && !r.Is_Deleted).ToArray();
//                foreach (var orderDetail in orderDetails)
//                {
//                    orderDetailsModel.Add(new OrderDetailViewModel(orderDetail));
//                }

//                string relationType = "";
//                //var isRelation = UUtils.IsRelationOrder(db, order.Order_Type.Code, order.Incoterm.Incoterms, out relationType);

//                order.Id = header.Order_Id ?? 0;
//                sb.Append(GetMailAssignTransporter(db, order, header.Info1, null, false));
//                //sb.Append(isRelation ? GetMailRelationOrderDetail(db, order, orderDetailsModel) : GetMailOrderDetail(db, order, orderDetailsModel));
//                sb.Append(newLines);

//                sb.Append(newLines);
//                sb.Append("<strong>Thanks and Regards, </strong>");
//                sb.Append(newLines);
//                sb.Append($"<b>{approverName}</b>");
//                sb.Append(newLines);
//                sb.Append("<i>Note: This is an auto-generated E-Mail</i>");

//                body = sb.ToString();
//                to = new List<String>();
//                var creator = db.Users.Where(r => header.Created_By.Contains(r.Employee_Number)).FirstOrDefault();
//                if (details.Count > 0)
//                {
//                    var z = details[0].Username;
//                    var nextApproval = db.Users.Where(r => z.Contains(r.Employee_Number)).FirstOrDefault();
//                    cc.Add(creator.Email);
//                    to.Add(nextApproval.Email);
//                }
//                else
//                {
//                    to.Add(creator.Email);
//                }

//                string refNo = header.Reference_No;
//                var subject = string.Format(BaseProgram.GetResourceValue("EMAIL_SUBJECT_APPROVED_ASSIGN_TRANSPORTER"), refNo, header.Info1, status.ToUpper());


//                result = SendMail(db, refNo, "APPROVAL", subject, body, to, cc);
//                //SendFCM("FAMS Approval", "",to);
//            }
//            catch (Exception exc)
//            {

//                result = exc.Message.Replace(Environment.NewLine, " ");
//            }
//            return result;
//        }

//        public static string SendAssignDriver(DMSEntities db, long shipmentId)
//        {
//            var result = "";
//            var body = "";
//            //var order = db.Orders.Where(r => r.Id == orderId).FirstOrDefault();

//            var to = new List<String>();
//            var cc = new List<String>();
//            try
//            {
//                var shipment = db.Shipments.Where(r => r.Id == shipmentId).FirstOrDefault();
//                var shipmentOrders = db.Shipment_Order_Detail.Where(r => r.Shipment_Id == shipmentId && !r.Is_Deleted).ToArray();
//                var orders = shipmentOrders.Select(r => r.Order).ToList();
//                var drivers = db.Shipment_Driver.Where(r => r.Shipment_Id == shipmentId && !r.Is_Deleted).ToArray();

//                StringBuilder sb = new StringBuilder();
//                //string newLines = "<br /><br />";
//                //string cellStyle = "padding: 8px; border: 1px solid black";
//                //sb.Append("Dear Logistic Planner / Warehouse,");
//                //sb.Append(newLines);
//                //if (orders.Count > 1)
//                //{
//                //    sb.Append("Order Number:");
//                //    foreach (var order in orders)
//                //    {
//                //        sb.Append(newLines);
//                //        sb.Append("<b>" + order.Order_No + "</b> ");
//                //    }
//                //    sb.Append(newLines);
//                //}
//                //else
//                //{
//                //    sb.Append("Order Number: <b>" + orders[0].Order_No + "</b> ");
//                //}
//                //sb.Append("Shipment with number : <b>" + shipment.Shipment_No + "</b> has been assigned to Driver(s)");
//                //sb.Append(newLines);
//                //sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");

//                ////header
//                //sb.Append("<tr>");
//                //sb.Append($"<th style='{cellStyle}'>Driver</th>");
//                //sb.Append($"<th style='{cellStyle}'>Plate No</th>");
//                //sb.Append("</tr>");
//                //foreach (var driver in drivers)
//                //{
//                //    var tempDriver = db.Drivers.Where(r => r.Id == driver.Driver_Id && !r.Is_Deleted).FirstOrDefault();
//                //    var tempVehiclie = db.Vehicles.Where(r => r.Id == driver.Vehicle_Id && !r.Is_Deleted).FirstOrDefault();
//                //    sb.Append("<tr>");
//                //    sb.Append($"<td style='{cellStyle}'>" + tempDriver.Name + "</td>");
//                //    sb.Append($"<td style='{cellStyle}'>" + tempVehiclie.Vehicle_No + "</td>");
//                //    sb.Append("</tr>");
//                //}
//                //sb.Append("</table>");

//                //to.Add(db.Users.Where(r => shipment.Created_By.Contains(r.Employee_Number)).Select(r => r.Email).FirstOrDefault());

//                //body = sb.ToString();
//                //string refNo = shipment.SAP_No + " / " + shipment.Shipment_No;
//                //var subject = string.Format(BaseProgram.GetResourceValue("EMAIL_SUBJECT_TRANSPORTER_REQUEST_ACTION"), refNo);
//                //result = SendMail(db, refNo, "ASSIGN DRIVER", subject, body, to, cc);

//                var shipmentDriver = db.Shipment_Driver.Where(r => r.Shipment_Id == shipment.Id && !r.Is_Deleted).ToList();
//                var driverTokens = new List<String>();
//                foreach (var sd in drivers)
//                {
//                    var driver = db.Users.Where(r => r.Driver_Id == sd.Driver_Id && !r.Is_Deleted).FirstOrDefault();
//                    if (driver != null)
//                    {
//                        if (driver.fcm_token != null)
//                        {
//                            driverTokens.Add(driver.fcm_token);
//                        }
//                    }

//                }
//                SendFCM("FAMS Assign Driver", "Shipment with number: " + shipment.Shipment_No + " has been assigned to you", driverTokens);
//            }
//            catch (Exception exc)
//            {
//                result = exc.Message.Replace(Environment.NewLine, " ");
//            }
//            return result;
//        }
//        public static string SendTransporterAcceptReject(DMSEntities db, Order header, string transporterCode, string status, Approval approval)
//        {
//            var result = "";
//            var body = "";
//            var url = UUtils.GetApplicationLink(); //HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.Port != 443 ? ":" + HttpContext.Current.Request.Url.Port : "");
//            var to = new List<String>();
//            var cc = new List<String>();
//            try
//            {
//                var creatorUsername = header.Created_By.Split('[')[1]
//                                                    .Split(']')[0]
//                                                    .Trim();
//                var user = db.Users.Where(r => r.Employee_Number == creatorUsername).FirstOrDefault();
//                var creatorName = user != null ? user.Full_Name : "";

//                StringBuilder sb = new StringBuilder();
//                string newLines = "<br /><br />";
//                sb.Append($"Dear {header.Transporter_Name},");
//                sb.Append(newLines);
//                sb.Append("Order with Number: <b>" + header.Order_No + "</b> has been assigned to you");
//                sb.Append(newLines);
//                sb.Append($"<b>Please <span style='color:green'>Accept</span> or <span style='color:red'>Reject</span> the Order by logging in to FAMS</b> ({url})");
//                sb.Append(newLines);

//                List<OrderDetailViewModel> details = new List<OrderDetailViewModel>();
//                var orderDetails = db.Order_Detail.Where(r => r.Order_Id == header.Id && !r.Is_Deleted).ToArray();
//                for (int i = 0; i < orderDetails.Length; i++)
//                {
//                    details.Add(new OrderDetailViewModel(orderDetails[i], false));
//                }

//                sb.Append(GetMailOrderDetailAssignTransporter(db, header, details));
//                sb.Append(newLines);

//                sb.Append(newLines);
//                sb.Append("<strong>Thanks and Regards, </strong>");
//                sb.Append(newLines);
//                sb.Append($"<b>{creatorName}</b>");
//                sb.Append(newLines);
//                sb.Append("<i>Note: This is an auto-generated E-Mail</i>");


//                //cc.Add(db.Users.Where(r => header.Created_By.Contains(r.Employee_Number)).Select(r => r.Email).FirstOrDefault());

//                var warehouseLocation = "";
//                var warehouse = db.Warehouses.Where(r => r.Code == header.Warehouse_Code && !r.Is_Deleted).FirstOrDefault();
//                if (warehouse != null) warehouseLocation = warehouse.Location;


//                //STENLEY [24/12/2021]: Does not check the location of the transporters
//                //var listTransporterEmail = db.Users.Where(r => r.Transporter_Code == transporterCode && !r.Is_Deleted && !header.Created_By.Contains(r.Employee_Number) && !string.IsNullOrEmpty(r.Email) && r.Role == Constants.Roles.TRANSPORTER).Select(r => r.Email).ToList();
//                //to.AddRange(listTransporterEmail);

//                body = sb.ToString();
//                string refNo = header.Order_No; //header.SAP_No + " / " + header.Order_No;
//                var subject = string.Format(BaseProgram.GetResourceValue("EMAIL_SUBJECT_TRANSPORTER_REQUEST_ACTION"), refNo);

//                //var transporters = db.Users.Where(r => r.Transporter_Code == transporterCode && r.Role == Constants.Roles.TRANSPORTER).Select(r => r.fcm_token).ToList();

//                //Transporters + Locations
//                var fcmTokens = new List<string>();
//                var transporters = db.Users.Where(r => r.Transporter_Code == header.Transporter_Code &&
//                                                        r.Role == Constants.Roles.TRANSPORTER &&
//                                                        !r.Is_Deleted &&
//                                                        (r.Location_All || (!r.Location_All && db.User_Location.Where(x => x.User_Id == r.Employee_Number && x.Location_Code == warehouseLocation && !r.Is_Deleted).Any()))
//                                                ).ToArray();
//                if (transporters != null)
//                {
//                    foreach (var transporter in transporters)
//                    {
//                        var transporterToken = transporter.fcm_token;

//                        fcmTokens.Add(transporterToken);
//                        if (!string.IsNullOrEmpty(transporter.Email) && !to.Contains(transporter.Email))
//                            to.Add(transporter.Email);
//                    }
//                }

//                result = SendMail(db, refNo, "ASSIGN TRANSPORTER", subject, body, to, cc);

//                SendFCM("FAMS Assign Transporter", "Order " + header.Order_No + " has been assigned to you. Please ACCEPT / REJECT the order", fcmTokens);
//            }
//            catch (Exception exc)
//            {
//                result = exc.Message.Replace(Environment.NewLine, " ");
//            }
//            return result;
//        }

//        public static string SendTransporterDispatched(DMSEntities db, Order header, List<OrderDetailViewModel> details, string transporterCode, string status, string approvalRemarks)
//        {
//            var result = "";
//            var body = "";
//            var url = UUtils.GetApplicationLink(); //HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.Port != 443 ? ":" + HttpContext.Current.Request.Url.Port : "");
//            var to = new List<String>();
//            var cc = new List<String>();
//            try
//            {
//                StringBuilder sb = new StringBuilder();
//                string newLines = "<br /><br />";

//                var creatorUsername = header.Created_By.Split('[')[1]
//                                                    .Split(']')[0]
//                                                    .Trim();
//                var user = db.Users.Where(r => r.Employee_Number == creatorUsername).FirstOrDefault();
//                var creatorName = user != null ? user.Full_Name : "";

//                var color = status == Constants.OrderStatus.ACCEPTED ? "green" : "red";
//                sb.Append($"Dear Logistic Planner/{creatorName},");
//                sb.Append(newLines);
//                sb.Append($"Order with Number: <b>{header.Order_No}</b> has been <b><span style='color:{color}'>{status}</span></b>");
//                sb.Append(newLines);
//                if (status == Constants.OrderStatus.REJECTED)
//                {
//                    sb.Append("<b>" + DisplayNames.REASON + " : " + approvalRemarks + "</b>");
//                    sb.Append(newLines);
//                }

//                //var details = new List<OrderDetailViewModel>();
//                //var orderDetails = db.Order_Detail.Where(r => r.Order_Id == header.Id && !r.Is_Deleted).ToArray();
//                //for(int i = 0; i< orderDetails.Length; i++)
//                //{
//                //    details.Add(new OrderDetailViewModel(orderDetails[i]));
//                //}

//                sb.Append(GetMailOrderDetailAssignTransporter(db, header, details));
//                sb.Append(newLines);
//                sb.Append("<strong>Thanks and Regards, </strong>");
//                sb.Append(newLines);
//                sb.Append($"<b>{header.Transporter_Name}</b>");
//                sb.Append(newLines);
//                sb.Append("<i>Note: This is an auto-generated E-Mail</i>");

//                cc.Add(db.Users.Where(r => header.Created_By.Contains(r.Employee_Number)).Select(r => r.Email).FirstOrDefault());

//                var warehouseLocation = "";
//                var warehouse = db.Warehouses.Where(r => r.Code == header.Warehouse_Code && !r.Is_Deleted).FirstOrDefault();
//                if (warehouse != null) warehouseLocation = warehouse.Location;

//                var transporters = db.Users.Where(r => r.Transporter_Code == transporterCode &&
//                                                        r.Role == Constants.Roles.TRANSPORTER &&
//                                                        !r.Is_Deleted &&
//                                                        (r.Location_All || (!r.Location_All && db.User_Location.Where(x => x.User_Id == r.Employee_Number && x.Location_Code == warehouseLocation && !r.Is_Deleted).Any()))
//                                                ).ToArray();

//                var toList = transporters.Where(r => !string.IsNullOrEmpty(r.Email)).Select(r => r.Email).Distinct().ToList();

//                to.AddRange(toList);
//                body = sb.ToString();

//                string refNo = header.Order_No; // header.SAP_No + " / " + header.Order_No;
//                var subject = string.Format(BaseProgram.GetResourceValue("EMAIL_SUBJECT_TRANSPORTER_ACTION"), refNo, status + " ORDER");

//                result = SendMail(db, refNo, "ASSIGN TRANSPORTER (FEEDBACK)", subject, body, to, cc);

//                //var transporters = db.Users.Where(r => r.Transporter_Code == transporterCode && r.Role == Constants.Roles.TRANSPORTER).Select(r => r.fcm_token).ToList();
//                //SendFCM("FAMS Order", "Order " + header.Order_No + " has been " + header.Dispatch_Status, transporters);
//            }
//            catch (Exception exc)
//            {
//                result = exc.Message.Replace(Environment.NewLine, " ");
//            }
//            return result;
//        }
//        public static string SendApprovalMail(DMSEntities db, Approval header, List<Approval_Detail> details, string status, List<string> to = null, List<string> cc = null, List<string> bcc = null)
//        {
//            var result = "";
//            var body = "";
//            to = new List<String>();
//            cc = new List<String>();
//            try
//            {
//                StringBuilder sb = new StringBuilder();
//                sb.Append("Dear Requester,");
//                sb.Append(newLines);
//                sb.Append("Approval with Reference Number: <b>" + header.Reference_No + "</b> has been <b>" + status + "</b>");
//                sb.Append(newLines);

//                /*Show reason if approver reject*/
//                if (status == Constants.ApprovalStatus.REJECTED)
//                {
//                    sb.Append("<b>" + DisplayNames.REASON + " : " + header.Remarks + "</b>");
//                    sb.Append(newLines);
//                }

//                sb.Append(newLines);
//                sb.Append("<strong>Thanks and Regards, </strong>");
//                sb.Append(newLines);
//                sb.Append("<b>FAMS</b>");
//                sb.Append(newLines);
//                sb.Append("<i>Note: This is an auto-generated E-Mail</i>");

//                body = sb.ToString();
//                to = new List<String>();
//                var creator = db.Users.Where(r => header.Created_By.Contains(r.Employee_Number)).FirstOrDefault();
//                if (details.Count > 0)
//                {
//                    var z = details[0].Username;
//                    var nextApproval = db.Users.Where(r => z.Contains(r.Employee_Number)).FirstOrDefault();
//                    cc.Add(creator.Email);
//                    to.Add(nextApproval.Email);
//                }
//                else
//                {
//                    to.Add(creator.Email);
//                }

//                string refNo = header.Reference_No;
//                var subject = string.Format(BaseProgram.GetResourceValue("EMAIL_SUBJECT_APPROVE_ORDER_MANUAL"), refNo);
//                result = SendMail(db, refNo, "APPROVAL", subject, body, to, cc);
//                //SendFCM("FAMS Approval", "",to);
//            }
//            catch (Exception exc)
//            {

//                result = exc.Message.Replace(Environment.NewLine, " ");
//            }
//            return result;
//        }



//        public static string SendOrderApprovalMail(DMSEntities db, Approval header, List<Approval_Detail> details, string status, string approvalRemarks)
//        {
//            var result = "";
//            var body = "";
//            var to = new List<String>();
//            var cc = new List<String>();
//            try
//            {
//                StringBuilder sb = new StringBuilder();

//                var creatorUsername = header.Created_By.Split('[')[1]
//                                                    .Split(']')[0]
//                                                    .Trim();
//                var user = db.Users.Where(r => r.Employee_Number == creatorUsername).FirstOrDefault();
//                var creatorName = user != null ? user.Full_Name : "";

//                var approverUsername = header.Last_Approval_By;
//                var approver = db.Users.Where(r => r.Employee_Number == approverUsername).FirstOrDefault();
//                var approverName = approver != null ? approver.Full_Name : "";

//                //var to = new List<string>();
//                //var approversName = new List<string>();
//                //foreach (var username in firstApprovers)
//                //{
//                //    var userData = db.Users.Where(r => r.Employee_Number == username && !r.Is_Deleted).FirstOrDefault();
//                //    if (userData != null)
//                //    {
//                //        if (!to.Contains(userData.Email)) to.Add(userData.Email);
//                //        if (!approversName.Contains(userData.Full_Name)) approversName.Add(userData.Full_Name);
//                //    }
//                //}

//                string color = status == Constants.ApprovalStatus.APPROVED ? "green" : "red";

//                sb.Append($"Dear {creatorName},");
//                sb.Append(newLines);
//                sb.Append("Order with Reference Number: <b>" + header.Reference_No + $"</b> has been <b><span style='color:{color}'>" + status + "</span></b>");
//                sb.Append(newLines);
//                //if (!string.IsNullOrEmpty(approvalRemarks))
//                //{
//                //    sb.Append($"<b>REMARKS: {approvalRemarks}</b>");
//                //    sb.Append(newLines);
//                //}

//                /*Show reason if approver reject*/
//                if (!string.IsNullOrEmpty(approvalRemarks))
//                {
//                    sb.Append("<b>" + DisplayNames.REASON + " : " + header.Remarks + "</b>");
//                    sb.Append(newLines);
//                }

//                var order = header.SAP_Order;
//                var orderDetailsModel = new List<SAPOrderDetailViewModel>();
//                var sapNo = order.Document_No;
//                var orderDetails = db.SAP_Order_Detail.Where(r => r.Document_No == sapNo).ToArray();
//                foreach (var orderDetail in orderDetails)
//                {
//                    orderDetailsModel.Add(new SAPOrderDetailViewModel(orderDetail));
//                }

//                string relationType = "";
//                var isRelation = UUtils.IsRelationOrder(db, order.Order_Type, order.Incoterm, out relationType);

//                sb.Append(isRelation ? GetMailRelationOrderDetail(db, order, orderDetailsModel) : GetMailOrderDetail(db, order, orderDetailsModel));
//                sb.Append(newLines);

//                sb.Append(newLines);
//                sb.Append("<strong>Thanks and Regards, </strong>");
//                sb.Append(newLines);
//                sb.Append($"<b>{approverName}</b>");
//                sb.Append(newLines);
//                sb.Append("<i>Note: This is an auto-generated E-Mail</i>");

//                body = sb.ToString();
//                to = new List<String>();
//                var creator = db.Users.Where(r => header.Created_By.Contains(r.Employee_Number)).FirstOrDefault();
//                if (details.Count > 0)
//                {
//                    var z = details[0].Username;
//                    var nextApproval = db.Users.Where(r => z.Contains(r.Employee_Number)).FirstOrDefault();
//                    cc.Add(creator.Email);
//                    to.Add(nextApproval.Email);
//                }
//                else
//                {
//                    to.Add(creator.Email);
//                }

//                string refNo = header.Reference_No;
//                var subject = string.Format(BaseProgram.GetResourceValue("EMAIL_SUBJECT_APPROVE_ORDER_MANUAL"), refNo, status.ToUpper());


//                result = SendMail(db, refNo, "APPROVAL", subject, body, to, cc);
//                //SendFCM("FAMS Approval", "",to);
//            }
//            catch (Exception exc)
//            {

//                result = exc.Message.Replace(Environment.NewLine, " ");
//            }
//            return result;
//        }

//        public static string SendApprovalAppointmentMail(DMSEntities db, Approval approval, Shipment header, List<Approval_Detail> details, string status, string approvalRemarks)
//        {
//            var result = "";
//            var body = "";
//            var to = new List<String>();
//            var cc = new List<String>();
//            try
//            {
//                StringBuilder sb = new StringBuilder();
//                Shipment shipment = header;

//                var transporterCode = header.Transporter_Code;
//                var transporterName = header.Transporter_Name;
//                var shipmentNo = shipment.Shipment_No;
//                var orderNo = shipment.Order_No;
//                var sapNo = shipment.SAP_No;

//                var approverUsername = approval.Last_Approval_By;
//                var approver = db.Users.Where(r => r.Employee_Number == approverUsername).FirstOrDefault();
//                var approverName = approver != null ? approver.Full_Name : "";

//                var color = status == Constants.ApprovalStatus.APPROVED ? "green" : "red";

//                sb.Append($"Dear {transporterName} / Driver,");
//                sb.Append(newLines);
//                sb.Append($"Shipment appointment with Reference Number: <span style='font-weight:bold'>{shipmentNo} / {orderNo}</span> has been <b><span style='color:{color}'>{status}</span></b>");
//                sb.Append(newLines);
//                if (!string.IsNullOrEmpty(approvalRemarks))
//                {
//                    sb.Append($"<span style='font-weight:bold'>REMARKS: {approvalRemarks}</span>");
//                    sb.Append(newLines);
//                }

//                //sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//                //sb.Append("<tr>");
//                //sb.Append($"<td style='{cellStyle}'><b>{DisplayNames.APPOINTMENT_DATE}</b></td><td style='{cellStyleData}'>{shipment.Appointment_Date}</td>");
//                //sb.Append("</tr>");
//                //sb.Append("<tr>");
//                //sb.Append($"<td style='{cellStyle}'><b>{DisplayNames.APPOINTMENT_TIME}</b></td><td style='{cellStyleData}'>{shipment.Appointment_Time}</td>");
//                //sb.Append("</tr>");
//                //sb.Append("<tr>");
//                //sb.Append($"<td style='{cellStyle}'><b>{DisplayNames.WAREHOUSE}</b></td><td style='{cellStyleData}'>{shipment.Warehouse_Name}</td>");
//                //sb.Append("</tr>");
//                //sb.Append($"<td style='{cellStyle}'><b>{DisplayNames.BUSINESS_SEGMENT}</b></td><td style='{cellStyleData}'>{shipment.Business_Segment.Description}</td>");
//                //sb.Append("</tr>");
//                //sb.Append("</table>");
//                //sb.Append(newLines);

//                //if (status == Constants.ApprovalStatus.REJECTED)
//                //{
//                //    sb.Append(DisplayNames.REASON + " : " + shipment.Appointment_Status_Remarks);
//                //    sb.Append(newLines);
//                //}

//                var shipmentDetails = db.Shipment_Order_Detail.Where(r => r.Shipment_Id == shipment.Id && !r.Is_Deleted).ToList();
//                var shipmentDrivers = db.Shipment_Driver.Where(r => r.Shipment_Id == shipment.Id && !r.Is_Deleted).ToList();

//                sb.Append(GetMailAppointment(db, header, shipmentDetails, shipmentDrivers));

//                sb.Append(newLines);
//                sb.Append("<strong>Thanks and Regards, </strong>");
//                sb.Append(newLines);
//                sb.Append($"<b>{approverName}</b>");
//                sb.Append(newLines);
//                sb.Append("<i>Note: This is an auto-generated E-Mail</i>");

//                body = sb.ToString();
//                to = new List<String>();
//                var creator = db.Users.Where(r => shipment.Created_By.Contains(r.Employee_Number)).FirstOrDefault();
//                if (details.Count > 0)
//                {
//                    var z = details[0].Username;
//                    var nextApproval = db.Users.Where(r => z.Contains(r.Employee_Number)).FirstOrDefault();
//                    cc.Add(creator.Email);
//                    to.Add(nextApproval.Email);
//                }
//                else
//                {
//                    to.Add(creator.Email);
//                }

//                var warehouseLocation = "";
//                var warehouse = db.Warehouses.Where(r => r.Code == header.Warehouse_Code && !r.Is_Deleted).FirstOrDefault();
//                if (warehouse != null) warehouseLocation = warehouse.Location;

//                var transporters = db.Users.Where(r => r.Transporter_Code == transporterCode &&
//                                                        r.Role == Constants.Roles.TRANSPORTER &&
//                                                        !r.Is_Deleted &&
//                                                        (r.Location_All || (!r.Location_All && db.User_Location.Where(x => x.User_Id == r.Employee_Number && x.Location_Code == warehouseLocation && !r.Is_Deleted).Any()))
//                                                ).ToArray();

//                var transporterTokenList = transporters.Where(r => !string.IsNullOrEmpty(r.fcm_token)).Select(r => r.fcm_token).Distinct().ToList();
//                var toList = transporters.Where(r => !string.IsNullOrEmpty(r.Email)).Select(r => r.Email).Distinct().ToList();

//                string refNo = shipment.Shipment_No; //shipment.SAP_No + " / " + shipment.Shipment_No;
//                string subject = string.Format(BaseProgram.GetResourceValue("EMAIL_SUBJECT_APPROVED_CREATE_APPOINTMENT"), refNo, status);
//                result = SendMail(db, refNo, "APPROVAL - APPOINTMENT", subject, body, to, cc);

//                var shipmentDriver = db.Shipment_Driver.Where(r => r.Shipment_Id == shipment.Id && !r.Is_Deleted).ToList();
//                var fcmTokens = new List<String>();
//                foreach (var sd in shipmentDriver)
//                {
//                    var driver = db.Users.Where(r => r.Driver_Id == sd.Driver_Id && !r.Is_Deleted).FirstOrDefault();
//                    if (driver != null)
//                    {
//                        if (driver.fcm_token != null)
//                        {
//                            fcmTokens.Add(driver.fcm_token);
//                        }
//                    }
//                }
//                //var transporterToken = db.Users.Where(r => r.Employee_Number == header.Transporter_Code && r.Role == Constants.Roles.TRANSPORTER && !r.Is_Deleted).Select(r => r.fcm_token).FirstOrDefault();
//                //if (transporterToken != null)
//                //{
//                //    fcmTokens.Add(transporterToken);
//                //}

//                fcmTokens.AddRange(transporterTokenList);
//                SendFCM("FAMS Appointment", "Appointment with Reference Number: " + shipment.SAP_No + " / " + shipment.Shipment_No + " has been " + status, fcmTokens);
//            }
//            catch (Exception exc)
//            {

//                result = exc.Message.Replace(Environment.NewLine, " ");
//            }
//            return result;
//        }

//        public static string GetMailOrderDetail(DMSEntities db, OrderViewModel order, List<OrderDetailViewModel> details)
//        {
//            StringBuilder sb = new StringBuilder();
//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<tr>");
//            sb.Append($"<td style='{cellStyle}'><b>Order No</b></td><td style='{cellStyleData}'>{order.Order_No + " / " + order.SAP_No}</td><td style='{dividerStyle}'>{divider}</td><td style='{cellStyle}'><b>Incoterm</b></td><td style='{cellStyleData}'>{order.Incoterm_Name}</td>");
//            sb.Append("</tr>");

//            if (order.Status == Constants.OrderStatus.WAITING_FOR_ORDER_APPROVAL)
//            {
//                sb.Append("<tr>");
//                sb.Append($"<td style='{cellStyle}'><b>Product Category</b></td><td style='{cellStyleData}'>{order.Business_Segment_Name}</td><td style='{dividerStyle}'>{divider}</td><td style='{cellStyle}'><b>Incoterm2</b></td><td style='{cellStyleData}'>{order.Incoterm2}</td>");
//                sb.Append("</tr>");

//                sb.Append("<tr>");
//                sb.Append($"<td style='{cellStyle}'><b>Plant</b></td><td style='{cellStyleData}'>{order.Plant_Name}</td><td style='{dividerStyle}'>{divider}</td><td style='{cellStyle}'><b>Document Type</b></td><td style='{cellStyleData}'>{order.Document_Type}</td>");
//                sb.Append("</tr>");

//                sb.Append("<tr>");
//                sb.Append($"<td style='{cellStyle}'><b>Distribution</b></td><td style='{cellStyleData}'>{order.Distribution}</td><td style='{dividerStyle}'>{divider}</td><td style='{cellStyle}'><b>Customer</b></td><td style='{cellStyleData}'>{order.Customer_Vendor_Name}</td>");
//                sb.Append("</tr>");

//                sb.Append("<tr>");
//                sb.Append($"<td style='{cellStyle}'><b>WB Trans Type</b></td><td style='{cellStyleData}'>{order.WB_Trans_Type}</td><td style='{dividerStyle}'>{divider}</td><td style='{cellStyle}'><b>Address</b></td><td style='{cellStyleData}'>{order.Delivery_Address ?? order.Customer_Vendor_Address}</td>");
//                sb.Append("</tr>");

//                sb.Append("<tr>");
//                sb.Append($"<td style='{cellStyle}'><b>Order Type</b></td><td style='{cellStyleData}'>{order.Order_Type_Name}</td><td style='{dividerStyle}'>{divider}</td><td style='{cellStyle}'><b>Warehouse</b></td><td style='{cellStyleData}'>{order.Warehouse_Name}</td>");
//                sb.Append("</tr>");
//            }

//            if (order.Status == Constants.OrderStatus.DISPATCHED || order.Status == Constants.OrderStatus.WAITING_FOR_DISPATCH_APPROVAL)
//            {
//                sb.Append("<tr>");
//                sb.Append($"<td style='{cellStyle}'><b>Loading</b></td><td style='{cellStyleData}'>{order.Loading_Destination_From_Name}</td><td style='{dividerStyle}'>{divider}</td><td style='{cellStyle}'><b>Stuffing Date</b></td><td style='{cellStyleData}'>{order.Stuffing_Date_From.Value.ToString("dd /MM/yyyy") + " - " + order.Stuffing_Date_To.Value.ToString("dd/MM/yyyy")}</td>");
//                sb.Append("</tr>");

//                sb.Append("<tr>");
//                sb.Append($"<td style='{cellStyle}'><b>Destination</b></td><td style='{cellStyleData}'>{order.Loading_Destination_To_Name}</td><td style='{dividerStyle}'>{divider}</td><td style='{cellStyle}'><b>Truck Type</b></td><td style='{cellStyleData}'>{order.Truck_Type_Name}</td>");
//                sb.Append("</tr>");

//                sb.Append("<tr>");
//                sb.Append($"<td style='{cellStyle}'><b>Shipment Type</b></td><td style='{cellStyleData}'>{order.shipment_type_name}</td><td style='{dividerStyle}'>{divider}</td><td style='{cellStyle}'><b>Notes to Transporter</b></td><td style='{cellStyleData}'>{order.Transporter_Notes}</td>");
//                sb.Append("</tr>");
//            }

//            sb.Append("</table>");

//            sb.Append(newLines);

//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<thead>");
//            sb.Append("<tr>");
//            sb.Append($"<th style='{headerStyle}'>Item No</th>");
//            sb.Append($"<th style='{headerStyle}'>Material</th>");
//            sb.Append($"<th style='{headerStyle}'>Qty</th>");
//            sb.Append($"<th style='{headerStyle}'>UOM</th>");
//            sb.Append("</tr>");
//            sb.Append("</thead>");
//            sb.Append("<tbody>");
//            foreach (var detail in details)
//            {
//                sb.Append("<tr>");
//                sb.Append($"<td style='{basicStyle}'>{detail.Item_No}</td>");
//                sb.Append($"<td style='{cellStyleData}'>{detail.Material_Name}</td>");
//                sb.Append($"<td style='{cellStyle};text-align:right'>{detail.Qty.ToString("N3", new CultureInfo("en-GB"))}</td>");
//                sb.Append($"<td style='{basicStyle}'>{detail.UOM}</td>");
//                sb.Append("</tr>");
//            }
//            sb.Append("</tbody>");
//            sb.Append("</table>");

//            return sb.ToString();
//        }

//        public static string GetMailAppointment(DMSEntities db, Shipment shipment, List<Shipment_Order_Detail> details, List<Shipment_Driver> drivers)
//        {
//            StringBuilder sb = new StringBuilder();

//            var loadingData = db.Locations.Where(r => r.Id == shipment.Loading_Destination_From_Id).FirstOrDefault();
//            var destinationData = db.Locations.Where(r => r.Id == shipment.Loading_Destination_To_Id).FirstOrDefault();

//            var appointmentDate = shipment.Appointment_Date.Value.ToString("dd/MM/yyyy");
//            var appointmentTime = shipment.Appointment_Time;
//            var productCategory = shipment.Product_Category;
//            var loading = $"[{loadingData.Code}] {loadingData.Description}";
//            var destination = $"[{destinationData.Code}] {destinationData.Description}";
//            var transporterName = shipment.Transporter_Name;
//            var truckTypeName = shipment.Truck_Type_Name;
//            var warehouseCode = shipment.Warehouse_Code;
//            var warehouseName = shipment.Warehouse_Name;

//            string newLines = "<br /><br />";
//            string cellStyle = "padding: 8px; border: 1px solid black";

//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<tr>");
//            sb.Append(
//                    $"<td style='{cellStyle}'><b>{DisplayNames.APPOINTMENT_DATE}</b></td><td style='{cellStyleData}'>{appointmentDate}</td>" +
//                    $"<td style='{dividerStyle}'>{divider}</td>" +
//                    $"<td style='{cellStyle}'><b>{DisplayNames.SAP_NO}</b></td><td style='{cellStyleData}'>{shipment.SAP_No}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//                    $"<td style='{cellStyle}'><b>{DisplayNames.APPOINTMENT_TIME}</b></td><td style='{cellStyleData}'>{appointmentTime}</td>" +
//                    $"<td style='{dividerStyle}'>{divider}</td>" +
//                    $"<td style='{cellStyle}'><b>{DisplayNames.LOADING}</b></td><td style='{cellStyleData}'>{loading}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//                    $"<td style='{cellStyle}'><b>{DisplayNames.BUSINESS_SEGMENT}</b></td><td style='{cellStyleData}'>{productCategory}</td>" +
//                    $"<td style='{dividerStyle}'>{divider}</td>" +
//                    $"<td style='{cellStyle}'><b>{DisplayNames.DESTINATION}</b></td><td style='{cellStyleData}'>{destination}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//                    $"<td style='{cellStyle}'><b>{DisplayNames.TRANSPORTER}</b></td><td style='{cellStyleData}'>{transporterName}</td>" +
//                    $"<td style='{dividerStyle}'>{divider}</td>" +
//                    $"<td style='{cellStyle}'><b>{DisplayNames.TRUCK_TYPE}</b></td><td style='{cellStyleData}'>{truckTypeName}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//                    $"<td style='{cellStyle}'><b>{DisplayNames.WAREHOUSE}</b></td><td style='{cellStyleData}'>{"[" + warehouseCode + "]" + warehouseName}</td>");
//            sb.Append("</tr>");
//            sb.Append("</table>");
//            sb.Append(newLines);


//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<thead>");
//            sb.Append("<tr>");
//            sb.Append($"<th style='{headerStyle}'>No</th>");
//            sb.Append($"<th style='{headerStyle}'>Driver</th>");
//            sb.Append($"<th style='{headerStyle}'>License No.</th>");
//            sb.Append($"<th style='{headerStyle}'>Vehicle No.</th>");
//            sb.Append($"<th style='{headerStyle}'>Truck Type</th>");
//            sb.Append("</tr>");
//            sb.Append("</thead>");
//            sb.Append("<tbody>");
//            for (int i = 0; i < drivers.Count; i++)
//            {
//                var driver = drivers[i];

//                sb.Append("<tr>");
//                sb.Append($"<td style='{basicStyle}'>{(i + 1)}</td>");
//                sb.Append($"<td style='{basicStyle}'>{driver.Driver_Name}</td>");
//                sb.Append($"<td style='{basicStyle}'>{driver.License_No}</td>");
//                sb.Append($"<td style='{basicStyle}'>{driver.Vehicle_Name}</td>");
//                sb.Append($"<td style='{basicStyle}'>{driver.Truck_Type_Name}</td>");
//                sb.Append("</tr>");
//            }
//            sb.Append("</tbody>");
//            sb.Append("</table>");
//            sb.Append(newLines);

//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<thead>");
//            sb.Append("<tr>");
//            sb.Append($"<th style='{headerStyle}'>Item No</th>");
//            sb.Append($"<th style='{headerStyle}'>SAP No</th>");
//            sb.Append($"<th style='{headerStyle}'>Order No</th>");
//            sb.Append($"<th style='{headerStyle}'>Material</th>");
//            sb.Append($"<th style='{headerStyle}'>Qty</th>");
//            sb.Append($"<th style='{headerStyle}'>UOM</th>");
//            sb.Append("</tr>");
//            sb.Append("</thead>");
//            sb.Append("<tbody>");
//            foreach (var detail in details)
//            {
//                sb.Append("<tr>");
//                sb.Append($"<td style='{basicStyle}'>{detail.Item_No}</td>");
//                sb.Append($"<td style='{cellStyleData}'>{detail.SAP_No}</td>");
//                sb.Append($"<td style='{cellStyleData}'>{detail.Order_No}</td>");
//                sb.Append($"<td style='{cellStyleData}'>{detail.Material_Name}</td>");
//                sb.Append($"<td style='{cellStyle};text-align:right'>{detail.Qty.ToString("N3", new CultureInfo("en-GB"))}</td>");
//                sb.Append($"<td style='{basicStyle}'>{detail.UOM}</td>");
//                sb.Append("</tr>");
//            }
//            sb.Append("</tbody>");
//            sb.Append("</table>");

//            return sb.ToString();
//        }


//        public static string GetMailShipmentOrderDetail(DMSEntities db, Shipment shipment, List<Shipment_Order_Detail> details, List<Shipment_Driver> drivers)
//        {
//            StringBuilder sb = new StringBuilder();

//            var loadingData = db.Locations.Where(r => r.Id == shipment.Loading_Destination_From_Id).FirstOrDefault();
//            var destinationData = db.Locations.Where(r => r.Id == shipment.Loading_Destination_To_Id).FirstOrDefault();
//            var shipmentTypeData = db.Lookups.Where(r => r.Lookup_Group == Constants.LookupGroup.SHIPMENT_TYPE && r.Lookup_Key == shipment.Shipment_Type).FirstOrDefault();

//            var sapNo = shipment.SAP_No.Replace("\n", "<br/>");
//            var company = shipment.Company.Code + " - " + shipment.Company.Name;

//            var companyName = "";
//            if(shipment.Company != null)
//            {
//                companyName = shipment.Company.Code + " - " + shipment.Company.Name;
//            }
//            else
//            {
//                var companyData = db.Companies.Where(r => r.Code == shipment.Company_Code).FirstOrDefault();
//                if(companyData != null)
//                {
//                    companyName = shipment.Company_Code + " - " + companyData.Name;
//                }
//                else
//                {
//                    companyName = shipment.Company_Code;
//                }
//            }

//            var productCategory = shipment.Product_Category;
//            var customerVendor = shipment.Customer_Name;
//            var transporterName = shipment.Transporter_Name;
//            var deliveryAddress = string.IsNullOrEmpty(shipment.Delivery_Address) ? shipment.Delivery_Address : shipment.Customer_Vendor_Address;
//            var truckTypeName = shipment.Truck_Type_Name;
//            var stuffingDateFrom = shipment.Stuffing_Date_From.HasValue ? shipment.Stuffing_Date_From.Value.ToString("dd/MM/yyyy") : "";
//            var stuffingDateTo = shipment.Stuffing_Date_To.HasValue ? shipment.Stuffing_Date_To.Value.ToString("dd/MM/yyyy") : "";
//            var stuffingDate = stuffingDateFrom + " - " + stuffingDateTo;
//            var loading = $"[{loadingData.Code}] {loadingData.Description}";
//            var destination = $"[{destinationData.Code}] {destinationData.Description}";
//            var shipmentType = shipmentTypeData.Lookup_Value.ToUpper();

//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<thead>");
//            sb.Append("<tr>");
//            sb.Append($"<th style='{headerStyle}'>No</th>");
//            sb.Append($"<th style='{headerStyle}'>Driver</th>");
//            sb.Append($"<th style='{headerStyle}'>License No.</th>");
//            sb.Append($"<th style='{headerStyle}'>Vehicle No.</th>");
//            sb.Append($"<th style='{headerStyle}'>Truck Type</th>");
//            sb.Append("</tr>");
//            sb.Append("</thead>");
//            sb.Append("<tbody>");
//            for (int i = 0; i < drivers.Count; i++)
//            {
//                var driver = drivers[i];

//                sb.Append("<tr>");
//                sb.Append($"<td style='{basicStyle}'>{(i + 1)}</td>");
//                sb.Append($"<td style='{basicStyle}'>{driver.Driver_Name}</td>");
//                sb.Append($"<td style='{basicStyle}'>{driver.License_No}</td>");
//                sb.Append($"<td style='{basicStyle}'>{driver.Vehicle_Name}</td>");
//                sb.Append($"<td style='{basicStyle}'>{driver.Truck_Type_Name}</td>");
//                sb.Append("</tr>");
//            }
//            sb.Append("</tbody>");
//            sb.Append("</table>");

//            sb.Append(newLines);

//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<tr>");
//            sb.Append(
//                $"<td style='{cellStyle}'><b>{DisplayNames.SAP_NO}</b></td><td style='{cellStyleData}'>{sapNo}</td>" +
//                $"{dividerCol}" +
//                $"<td style='{cellStyle}'><b>{DisplayNames.CUSTOMER_VENDOR}</b></td><td style='{cellStyleData}'>{customerVendor}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//                $"<td style='{cellStyle}'><b>{DisplayNames.COMPANY}</b></td><td style='{cellStyleData}'>{companyName}</td>" +
//               $"{dividerCol}" +
//                $"<td style='{cellStyle}'><b>{DisplayNames.STUFFING_DATE}</b></td><td style='{cellStyleData}'>{stuffingDate}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.BUSINESS_SEGMENT}</b></td><td style='{cellStyleData}'>{productCategory}</td>" +
//              $"<td style='{dividerStyle}'>{divider}</td>" +
//              $"<td style='{cellStyle}' rowspan='2'><b>{DisplayNames.DELIVERY_ADDRESS}</b></td><td style='{cellStyleData}' rowspan='2'>{deliveryAddress}</td>");
//            sb.Append("</tr>");
//            sb.Append(
//             $"<td style='{cellStyle}'><b>{DisplayNames.SHIPMENT_TYPE}</b></td><td style='{cellStyleData}'>{shipmentType}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.LOADING}</b></td><td style='{cellStyleData}'>{loading}</td>" +
//              $"{dividerCol}" +
//              $"<td style='{cellStyle}'><b>{DisplayNames.DESTINATION}</b></td><td style='{cellStyleData}'>{destination}</td>"
//              );
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.TRUCK_TYPE}</b></td><td style='{cellStyleData}'>{truckTypeName}</td>");
//            sb.Append("</tr>");
//            sb.Append("</table>");

//            sb.Append(newLines);

//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<thead>");
//            sb.Append("<tr>");
//            sb.Append($"<th style='{headerStyle}'>Item No</th>");
//            sb.Append($"<th style='{headerStyle}'>Material</th>");
//            sb.Append($"<th style='{headerStyle}'>Qty</th>");
//            sb.Append($"<th style='{headerStyle}'>UOM</th>");
//            sb.Append("</tr>");
//            sb.Append("</thead>");
//            sb.Append("<tbody>");
//            foreach (var detail in details)
//            {
//                sb.Append("<tr>");
//                sb.Append($"<td style='{basicStyle}'>{detail.Item_No}</td>");
//                sb.Append($"<td style='{cellStyleData}'>{detail.Material_Name}</td>");
//                sb.Append($"<td style='{cellStyle};text-align:right'>{detail.Qty.ToString("N3", new CultureInfo("en-GB"))}</td>");
//                sb.Append($"<td style='{basicStyle}'>{detail.UOM}</td>");
//                sb.Append("</tr>");
//            }
//            sb.Append("</tbody>");
//            sb.Append("</table>");

//            return sb.ToString();
//        }

//        public static string GetMailOrderDetailAssignTransporter(DMSEntities db, Order order, List<OrderDetailViewModel> details)
//        {
//            StringBuilder sb = new StringBuilder();

//            var loadingData = db.Locations.Where(r => r.Id == order.Loading_Destination_From_Id).FirstOrDefault();
//            var destinationData = db.Locations.Where(r => r.Id == order.Loading_Destination_To_Id).FirstOrDefault();

//            var sapNo = order.SAP_No;

//            var company = order.Company_Name;
//            var productCategory = order.Business_Segment_Name;
//            var customerVendor = order.Customer_Vendor_Name;
//            var transporterName = order.Transporter_Name;
//            var transporterNotes = order.Transporter_Notes;
//            var deliveryAddress = string.IsNullOrEmpty(order.Delivery_Address) ? order.Customer_Vendor_Address : order.Delivery_Address;
//            var truckTypeName = order.Truck_Type_Name;
//            var stuffingDateFrom = order.Stuffing_Date_From.HasValue ? order.Stuffing_Date_From.Value.ToString("dd/MM/yyyy") : "";
//            var stuffingDateTo = order.Stuffing_Date_To.HasValue ? order.Stuffing_Date_To.Value.ToString("dd/MM/yyyy") : "";
//            var stuffingDate = stuffingDateFrom + " - " + stuffingDateTo;
//            var loading = $"[{loadingData.Code}] {loadingData.Description}";
//            var destination = $"[{destinationData.Code}] {destinationData.Description}";

//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<tr>");
//            sb.Append(
//                $"<td style='{cellStyle}'><b>{DisplayNames.SAP_NO}</b></td><td style='{cellStyleData}'>{sapNo}</td>" +
//                $"{dividerCol}" +
//                $"<td style='{cellStyle}'><b>{DisplayNames.CUSTOMER_VENDOR}</b></td><td style='{cellStyleData}'>{customerVendor}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//                $"<td style='{cellStyle}'><b>{DisplayNames.COMPANY}</b></td><td style='{cellStyleData}'>{company}</td>" +
//               $"{dividerCol}" +
//                $"<td style='{cellStyle}'><b>{DisplayNames.STUFFING_DATE}</b></td><td style='{cellStyleData}'>{stuffingDate}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.BUSINESS_SEGMENT}</b></td><td style='{cellStyleData}'>{productCategory}</td>" +
//              $"<td style='{dividerStyle}'>{divider}</td>" +
//              $"<td style='{cellStyle}'><b>{DisplayNames.TRANSPORTER}</b></td><td style='{cellStyleData}'>{transporterName}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.LOADING}</b></td><td style='{cellStyleData}'>{loading}</td>" +
//              $"{dividerCol}" +
//              $"<td style='{cellStyle}' rowspan='2'><b>{DisplayNames.DELIVERY_ADDRESS}</b></td><td style='{cellStyleData}' rowspan='2'>{deliveryAddress}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.DESTINATION}</b></td><td style='{cellStyleData}'>{destination}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.TRUCK_TYPE}</b></td><td style='{cellStyleData}'>{truckTypeName}</td>" +
//              $"{dividerCol}" +
//              $"<td style='{cellStyle}' rowspan='2'><b>{DisplayNames.TRANSPORTER_NOTES}</b></td><td style='{cellStyleData}' rowspan='2'>{transporterNotes}</td>");
//            sb.Append("</tr>");
//            sb.Append("</table>");

//            sb.Append(newLines);

//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<thead>");
//            sb.Append("<tr>");
//            sb.Append($"<th style='{headerStyle}'>Item No</th>");
//            sb.Append($"<th style='{headerStyle}'>SAP No</th>");
//            sb.Append($"<th style='{headerStyle}'>Material</th>");
//            sb.Append($"<th style='{headerStyle}'>Qty</th>");
//            sb.Append($"<th style='{headerStyle}'>UOM</th>");
//            sb.Append("</tr>");
//            sb.Append("</thead>");
//            sb.Append("<tbody>");
//            foreach (var detail in details)
//            {

//                sb.Append("<tr>");
//                sb.Append($"<td style='{basicStyle}'>{detail.Item_No}</td>");
//                sb.Append($"<td style='{basicStyle}'>{detail.sap_no}</td>");
//                sb.Append($"<td style='{cellStyleData}'>{detail.Material_Name}</td>");
//                sb.Append($"<td style='{cellStyle};text-align:right'>{detail.Qty.ToString("N3", new CultureInfo("en-GB"))}</td>");
//                sb.Append($"<td style='{basicStyle}'>{detail.UOM}</td>");
//                sb.Append("</tr>");
//            }
//            sb.Append("</tbody>");
//            sb.Append("</table>");

//            return sb.ToString();
//        }

//        public static string GetMailOrderDetail(DMSEntities db, SAP_Order order, List<SAPOrderDetailViewModel> details, bool isRelation = false, string approvalRemarks = "")
//        {
//            StringBuilder sb = new StringBuilder();

//            var sapNo = order.Document_No; //order.SAP_No;
//            var company = order.Company_Code; //order.Company.Code + " - " + order.Company.Name;
//            var incoterm = order.Incoterm; //order.Incoterm_Name;
//            var incoterm2 = order.Incoterm2;
//            var productCategory = order.Product_Category; // order.Business_Segment_Name;
//            var distribution = order.Distribution;
//            var plant = order.Plant; //order.Plant_Code + " - " + order.Plant_Name;
//            var customerVendor = order.Customer_Vendor_Code; //order.Customer_Vendor_Name;
//            var orderType = order.Order_Type; //order.Order_Type.Code + " - " + order.Order_Type.Name;
//            //var stuffingDate = order.Stuffing_Date (order.Stuffing_Date_From.HasValue ? order.Stuffing_Date_From.Value.ToString("dd/MM/yyyy") : "") + " - " + (order.Stuffing_Date_To.HasValue ? order.Stuffing_Date_To.Value.ToString("dd/MM/yyyy") : "");
//            string warehouseCode = order.Warehouse_Code; //order.Warehouse_Name;
//            var docType = order.Document_Type;
//            var remarks = ""; // order.Remarks;
//            //var deliveryAddress = order.Delivery_Address;
//            var salesOrg = order.Sales_Organization;
//            var purOrg = order.Purchase_Organization;
//            var customerVendorCode = order.Customer_Vendor_Code;

//            var incoterm2Name = incoterm2;
//            var companyName = company;
//            var incotermName = incoterm;
//            var productCategoryName = productCategory;
//            var plantName = plant;
//            var orderTypeName = orderType;
//            var warehouseName = warehouseCode;
//            var customerVendorName = customerVendorCode;

//            var incoterm2Data = db.Locations.Where(r => r.Code == incoterm2 && !r.Is_Deleted).FirstOrDefault();
//            if (incoterm2Data != null)
//            {
//                incoterm2Name = incoterm2 + " - " + incoterm2Data.Description;
//            }

//            var companyData = db.Companies.Where(r => r.Code == company).FirstOrDefault();
//            if (companyData != null)
//            {
//                companyName = company + " - " + companyData.Name;
//            }

//            var incotermData = db.Incoterms.Where(r => r.Incoterms == incoterm).FirstOrDefault();
//            if (incotermData != null)
//            {
//                incotermName = incoterm + " - " + incotermData.Description;
//            }

//            var productCategoryData = db.Business_Segment.Where(r => r.Code == productCategory).FirstOrDefault();
//            if (productCategoryData != null)
//            {
//                productCategoryName = productCategory + " - " + productCategoryData.Description;
//            }

//            var plantData = db.Plants.Where(r => r.Code == plant).FirstOrDefault();
//            if (plantData != null)
//            {
//                plantName = plant + " - " + plantData.Description;
//            }

//            var orderTypeData = db.Order_Type.Where(r => r.Code == orderType).FirstOrDefault();
//            if (orderTypeData != null)
//            {
//                orderTypeName = orderType + " - " + orderTypeData.Name;
//            }

//            var warehouseData = db.Warehouses.Where(r => r.Code == warehouseCode).FirstOrDefault();
//            if(warehouseData != null)
//            {
//                warehouseName = warehouseCode + " - " + warehouseData.Description;
//            }

//            var useCustomer = orderTypeData.Use_Customer;
//            var purSalesOrg = "";
//            var customerVendorAddress = "";

//            if (useCustomer)
//            {
//                var salesOrgData = db.Sales_Organization.Where(r => r.Code == salesOrg).FirstOrDefault();
//                if (salesOrgData != null)
//                    purSalesOrg = salesOrg + " - " + salesOrgData.Description;

//                var customerData = db.Customers.Where(r => r.Customer_Code == customerVendorCode).FirstOrDefault();
//                if(customerData != null)
//                {
//                    customerVendorName = customerVendorCode + " - " + customerData.Customer_Name1;
//                    customerVendorAddress = UUtils.GetCustomerAddress(customerData);
//                }
//            }
//            else
//            {
//                var purOrgData = db.Purchase_Organization.Where(r => r.Code == purOrg).FirstOrDefault();
//                if (purOrgData != null)
//                    purSalesOrg = purOrg + " - " + purOrgData.Description;

//                var vendorData = db.Vendors.Where(r => r.Code == customerVendorCode).FirstOrDefault();
//                if(vendorData != null)
//                {
//                    customerVendorName = customerVendorCode + " - " + vendorData.Name;
//                    customerVendorAddress = UUtils.GetVendorAddress(vendorData);
//                }
//            }
            
//            var deliveryAddress = string.IsNullOrEmpty(order.Delivery_Address) ? customerVendorAddress : order.Delivery_Address;

//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<tr>");
//            sb.Append(
//                $"<td style='{cellStyle}'><b>{DisplayNames.MANUAL_ORDER}</b></td><td style='{cellStyleData}'>{sapNo}</td>" +
//                $"{dividerCol}" +
//                $"<td style='{cellStyle}'><b>{DisplayNames.INCOTERM}</b></td><td style='{cellStyleData}'>{incotermName}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//                $"<td style='{cellStyle}'><b>{DisplayNames.COMPANY}</b></td><td style='{cellStyleData}'>{companyName}</td>" +
//               $"{dividerCol}" +
//                $"<td style='{cellStyle}'><b>{DisplayNames.INCOTERM2}</b></td><td style='{cellStyleData}'>{incoterm2Name}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.BUSINESS_SEGMENT}</b></td><td style='{cellStyleData}'>{productCategoryName}</td>" +
//              $"<td style='{dividerStyle}'>{divider}</td>" +
//              $"<td style='{cellStyle}'><b>{(useCustomer ? DisplayNames.SALES_ORG : DisplayNames.PUR_ORG)}</b></td><td style='{cellStyleData}'>{purSalesOrg}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.PLANT}</b></td><td style='{cellStyleData}'>{plantName}</td>" +
//              $"{dividerCol}" +
//              $"<td style='{cellStyle}'><b>{DisplayNames.CUSTOMER_VENDOR}</b></td><td style='{cellStyleData}'>{customerVendorName}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.ORDER_TYPE}</b></td><td style='{cellStyleData}'>{orderTypeName}</td>" +
//              $"{dividerCol}" +
//               $"<td style='{cellStyle}' rowspan='3'><b>{DisplayNames.DELIVERY_ADDRESS}</b></td><td style='{cellStyleData}' rowspan='3'>{deliveryAddress}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.DISTRIBUTION}</b></td><td style='{cellStyleData}'>{distribution}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.DOC_TYPE}</b></td><td style='{cellStyleData}'>{docType}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.WAREHOUSE}</b></td><td style='{cellStyleData}'>{warehouseName}</td>" +
//               $"{dividerCol}" +
//               $"<td style='{cellStyle}'><b>{DisplayNames.REMARKS}</b></td><td style='{cellStyleData}'>{remarks}</td>"
//              );
//            sb.Append("</tr>");

//            sb.Append("</table>");

//            sb.Append(newLines);

//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<thead>");
//            sb.Append("<tr>");
//            sb.Append($"<th style='{headerStyle}'>Item No</th>");
//            //sb.Append($"<th style='{headerStyle}'>SAP No</th>");
//            sb.Append($"<th style='{headerStyle}'>Material</th>");
//            sb.Append($"<th style='{headerStyle}'>Qty</th>");
//            sb.Append($"<th style='{headerStyle}'>UOM</th>");
//            sb.Append("</tr>");
//            sb.Append("</thead>");
//            sb.Append("<tbody>");
//            foreach (var detail in details)
//            {
//                sb.Append("<tr>");
//                sb.Append($"<td style='{basicStyle}'>{detail.Item_No}</td>"); 
//                //sb.Append($"<td style='{basicStyle}'>{detail.sap_no}</td>");
//                sb.Append($"<td style='{cellStyleData}'>{detail.material_name}</td>");
//                sb.Append($"<td style='{cellStyle};text-align:right'>{detail.Qty.Value.ToString("N3", new CultureInfo("en-GB"))}</td>");
//                sb.Append($"<td style='{basicStyle}'>{detail.UOM}</td>");
//                sb.Append("</tr>");
//            }
//            sb.Append("</tbody>");
//            sb.Append("</table>");

//            return sb.ToString();
//        }

//        public static string GetMailAssignTransporter(DMSEntities db, Order order, string approvalMatrix, List<Order_Freight_Log> transporterLists, bool showLogs = true)
//        {
//            StringBuilder sb = new StringBuilder();

//            var loadingData = db.Locations.Where(r => r.Id == order.Loading_Destination_From_Id).FirstOrDefault();
//            var destinationData = db.Locations.Where(r => r.Id == order.Loading_Destination_To_Id).FirstOrDefault();
//            var shipmentTypeData = db.Lookups.Where(r => r.Lookup_Group == Constants.LookupGroup.SHIPMENT_TYPE && r.Lookup_Key == order.Shipment_Type).FirstOrDefault();

//            var details = db.Order_Detail.Where(r => r.Order_Id == order.Id).ToArray();

//            var freightMaster = "";
//            if (details.Length > 0)
//            {
//                for (int i = 0; i < details.Length; i++)
//                {
//                    var detail = details[i];

//                    if (detail.Freight_Master.HasValue)
//                    {
//                        freightMaster += "IDR - " + detail.Freight_Master.Value.ToString("N0", new CultureInfo("en-GB")) + " |";
//                    }
//                }
//            }


//            freightMaster = freightMaster.TrimEnd('|');

//            var sapNo = order.SAP_No.Replace("\n", "<br/>");

//            var companyName = "";
//            var businessSegmentName = "";

//            if(order.Company != null)
//            {
//                companyName = order.Company.Code + " - " + order.Company.Name;
//            }
//            else
//            {
//                var companyData = db.Companies.Where(r => r.Code == order.Company_Code).FirstOrDefault();
//                if (companyData != null)
//                {
//                    companyName = order.Company_Code + " - " + companyData.Name;
//                }
//                else companyName = order.Company_Code;
//            }

//            if(order.Business_Segment != null)
//            {
//                businessSegmentName = order.Business_Segment.Code + " - " + order.Business_Segment.Description;
//            }
//            else
//            {
//                var businessSegmentData = db.Business_Segment.Where(r => r.Code == order.Business_Segment_Code).FirstOrDefault();
//                if(businessSegmentData != null)
//                {
//                    businessSegmentName = order.Business_Segment_Code + " - " + businessSegmentData.Description;
//                }
//            }


//            //var company = order.Company.Code + " - " + order.Company.Name;
//            //var productCategory = order.Business_Segment_Name;
//            var customerVendor = order.Customer_Vendor_Name;
//            var transporterName = order.Transporter_Name;
//            var transporterNotes = order.Transporter_Notes;
//            //var deliveryAddress = order.Delivery_Address ?? order.Customer_Vendor_Address;
//            var truckTypeName = order.Truck_Type_Name;
//            var stuffingDateFrom = order.Stuffing_Date_From.HasValue ? order.Stuffing_Date_From.Value.ToString("dd/MM/yyyy") : "";
//            var stuffingDateTo = order.Stuffing_Date_To.HasValue ? order.Stuffing_Date_To.Value.ToString("dd/MM/yyyy") : "";
//            var stuffingDate = stuffingDateFrom + " - " + stuffingDateTo;
//            var loading = $"[{loadingData.Code}] {loadingData.Description}";
//            var destination = $"[{destinationData.Code}] {destinationData.Description}";

//            var numTrip = order.Freight_Cost_Value.HasValue && order.Freight_Cost_Value.Value ? order.Freight_Container.Value.ToString() : "";
//            var freightCost = order.Freight_Cost.Value.ToString("N0", new CultureInfo("en-GB"));
//            var freightCostTotal = order.Freight_Total_Cost_User_NoT.HasValue ? order.Freight_Total_Cost_User_NoT.Value.ToString("N0", new CultureInfo("en-GB")) :
//                                    (order.Freight_Total_Cost.HasValue ? order.Freight_Total_Cost.Value.ToString("N0", new CultureInfo("en-GB")) : "0");
//            var freightUom = order.Freight_Cost_UOM;
//            var shipmentType = shipmentTypeData.Lookup_Value;

//            string color = "#ffffff";
//            string textColor = "#de8444";

//            var differentOAMatrix = db.Lookups.Where(r => r.Lookup_Key == Constants.AssignedMatrixType.DIFFERENT_OA).FirstOrDefault();
//            bool useDifferentFormat = false;

//            if (differentOAMatrix != null && differentOAMatrix.Lookup_Value == approvalMatrix)
//            {
//                color = "#de8444";
//                textColor = "#de8444";
//                useDifferentFormat = true;
//            }

//            var deliveryAddress = string.IsNullOrEmpty(order.Delivery_Address) ? order.Customer_Vendor_Address : order.Delivery_Address;

//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<tr>");
//            sb.Append(
//                $"<td style='{cellStyle}'><b>{DisplayNames.SAP_NO}</b></td><td style='{cellStyleData}'>{sapNo}</td>" +
//                $"{dividerCol}" +
//                $"<td style='{cellStyle}'><b>{DisplayNames.TRANSPORTER}</b></td><td style='{cellStyleData}'>{transporterName}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//                $"<td style='{cellStyle}'><b>{DisplayNames.COMPANY}</b></td><td style='{cellStyleData}'>{companyName}</td>" +
//               $"{dividerCol}" +
//                $"<td style='{cellStyle}'><b>{DisplayNames.STUFFING_DATE}</b></td><td style='{cellStyleData}'>{stuffingDate}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.BUSINESS_SEGMENT}</b></td><td style='{cellStyleData}'>{businessSegmentName}</td>" +
//              $"<td style='{dividerStyle}'>{divider}</td>" +
//              $"<td style='{cellStyle}'><b>{DisplayNames.FREIGHT_MASTER}</b></td><td style='{cellStyleData}'>{freightMaster}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.LOADING}</b></td><td style='{cellStyleData}'>{loading}</td>" +
//              $"{dividerCol}" +
//              $"<td style='{cellStyle}'><b>{DisplayNames.FREIGHT_COST_UOM}</b></td><td style='{cellStyleData}'>/{freightUom}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.DESTINATION}</b></td><td style='{cellStyleData}'>{destination}</td>" +
//              $"{dividerCol}" +
//              $"<td style='{cellStyle};background-color:{color}'><b>{DisplayNames.FREIGHT_COST}</b></td><td style='{cellStyleData};background-color:{color}'>IDR - {freightCost}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.SHIPMENT_TYPE}</b></td><td style='{cellStyleData}'>{shipmentType}</td>" +
//              $"{dividerCol}" +
//              $"<td style='{cellStyle};background-color:{color}'><b>{DisplayNames.FREIGHT_TOTAL_COST_USER}</b></td><td style='{cellStyleData};background-color:{color}'>IDR - {freightCostTotal}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//               $"<td style='{cellStyle}'><b>{DisplayNames.CUSTOMER_VENDOR}</b></td><td style='{cellStyleData}'>{customerVendor}</td>" +
//               $"{dividerCol}" +
//               $"<td style='{cellStyle}'><b>{DisplayNames.USER_NOT}</b></td><td style='{cellStyleData}'><span style='color:{textColor}'><b>{numTrip}</b></span></td>"
//              );
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//               $"<td>&nbsp;</td><td>&nbsp;</td>" +
//               $"{dividerCol}" +
//               $"<td style='{cellStyle}'><b>{DisplayNames.APPROVAL_MATRIX}</b></td><td style='{cellStyleData}'><span style='color:{textColor}'><b>{approvalMatrix}</b></span></td>"
//              );
//            sb.Append("</tr>");
//            sb.Append("</table>");

//            sb.Append(newLines);

//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<thead>");
//            sb.Append("<tr>");
//            sb.Append($"<th style='{headerStyle}'>Item Order</th>");
//            sb.Append($"<th style='{headerStyle}'>SAP No</th>");
//            sb.Append($"<th style='{headerStyle}'>Material</th>");
//            sb.Append($"<th style='{headerStyle}'>Qty</th>");
//            sb.Append($"<th style='{headerStyle}'>UOM</th>");
//            sb.Append("</tr>");
//            sb.Append("</thead>");
//            sb.Append("<tbody>");

//            foreach (var detail in details)
//            {
//                sb.Append("<tr>");
//                sb.Append($"<td style='{basicStyle}'>{detail.Item_No}</td>");
//                sb.Append($"<td style='{basicStyle}'>{(detail.SAP_Order_Id.HasValue ? detail.SAP_Order.Document_No : "")}</td>");
//                sb.Append($"<td style='{cellStyleData}'>{detail.Material_Name}</td>");
//                sb.Append($"<td style='{cellStyle};text-align:right'>{detail.Qty.ToString("N3", new CultureInfo("en-GB"))}</td>");
//                sb.Append($"<td style='{basicStyle}'>{detail.UOM}</td>");
//                sb.Append("</tr>");
//            }
//            sb.Append("</tbody>");
//            sb.Append("</table>");

//            if (showLogs)
//            {
//                if (useDifferentFormat && transporterLists.Count > 0)
//                {
//                    sb.Append(newLines);

//                    //sb.Append($"<span style='color:#1F497D'><b>{differentOAMatrix.Lookup_Value}</b></span>");
//                    sb.Append($"<span style='color:black;font-size:11px'><b>Freight Cost Comparison After Changes</b></span>");

//                    sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//                    sb.Append("<thead>");
//                    sb.Append("<tr>");
//                    sb.Append($"<th style='{headerStyle}'>Remark</th>");
//                    sb.Append($"<th style='{headerStyle}'>No</th>");
//                    sb.Append($"<th style='{headerStyle}'>{DisplayNames.TRANSPORTER}</th>");
//                    sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_MASTER}</th>");
//                    //sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_KG_BASED}</th>");
//                    sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_COST}</th>");
//                    sb.Append($"<th style='{headerStyle}'>{DisplayNames.USER_NOT}</th>");
//                    sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_TOTAL_COST_USER}</th>");
//                    //sb.Append($"<th style='{headerStyle}'>{DisplayNames.USER_NOT}</th>");
//                    //sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_TOTAL_COST_USER}</th>");
//                    sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_KG}</th>");
//                    sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_DIFFERENT_PER_UOM}</th>");
//                    sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_DIFFERENT_PERCENTAGE}</th>");
//                    sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_TOTAL_DIFFERENT}</th>");
//                    sb.Append("</tr>");
//                    sb.Append("</thead>");
//                    sb.Append("<tbody>");

//                    var recommendedTransporter = transporterLists[0];
//                    var transporterListRowStyle = basicStyle;
//                    var transporter = db.Transporters.Where(r => r.Code == recommendedTransporter.Transporter_Code).FirstOrDefault();
//                    var selectedTransporter = transporterLists.Where(r => r.Transporter_Code == order.Transporter_Code && r.Truck_Type_Id == order.Truck_Type_Id && order.Freight_Cost_Value == r.Freight_Value && (order.Freight_Cost_UOM == r.Per_UOM)).FirstOrDefault();

//                    sb.Append("<tr>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>Harga dari Master Data OA</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>1</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{order.Transporter_Name}</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{selectedTransporter.Freight_Master}</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{selectedTransporter.Freight_KG_Capacity.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{selectedTransporter.Freight_Cost.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{selectedTransporter.System_NoT.Value}</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{selectedTransporter.Freight_Cost_Total.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{selectedTransporter.User_NoT.Value}</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{selectedTransporter.Freight_Total_Cost_User_NoT.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{selectedTransporter.Freight_KG_Delivery_User.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>0</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>0.00%</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>0</td>");
//                    sb.Append("</tr>");

//                    //Old uses recommended
//                    //sb.Append("<tr>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>Harga dari Master Data OA</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>1</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{transporter.Name}</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{recommendedTransporter.Freight_Master}</td>");
//                    ////sb.Append($"<td style='{transporterListRowStyle}'>{recommendedTransporter.Freight_KG_Capacity.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{recommendedTransporter.Freight_Cost.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{recommendedTransporter.System_NoT.Value}</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{recommendedTransporter.Freight_Cost_Total.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    ////sb.Append($"<td style='{transporterListRowStyle}'>{recommendedTransporter.User_NoT.Value}</td>");
//                    ////sb.Append($"<td style='{transporterListRowStyle}'>{recommendedTransporter.Freight_Total_Cost_User_NoT.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{recommendedTransporter.Freight_KG_Delivery.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{recommendedTransporter.Freight_Different_Per_UOM.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{recommendedTransporter.Freight_Different_Percentage.Value.ToString("N2", new CultureInfo("en-GB"))}</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{recommendedTransporter.Freight_Total_Different.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    //sb.Append("</tr>");

//                    //Selected Transporter

//                    transporterListRowStyle += ";background-color:#ffe699";
//                    //var recommendedPriceKG = recommendedTransporter.Freight_KG_Delivery.Value;
//                    var recommendedPriceKG = selectedTransporter.Freight_KG_Delivery.Value;

//                    sb.Append("<tr>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>Perubahan pada Biaya Pengiriman</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>2</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{order.Transporter_Name}</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{selectedTransporter.Freight_Master}</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{selectedTransporter.Freight_KG_Capacity.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{order.Freight_Cost.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{selectedTransporter.System_NoT.Value}</td>");
//                    //sb.Append($"<td style='{transporterListRowStyle}'>{selectedTransporter.Freight_Total_Cost_User_NoT.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{order.User_NoT.Value}</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{order.Freight_Total_Cost_User_NoT.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{order.Freight_Best_Price.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{(order.Freight_Best_Price - selectedTransporter.Freight_KG_Delivery).Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{(recommendedPriceKG == 0 ? 0 : (order.Freight_Best_Price - selectedTransporter.Freight_KG_Delivery) / recommendedPriceKG).Value.ToString("N2", new CultureInfo("en-GB"))}%</td>");
//                    sb.Append($"<td style='{transporterListRowStyle}'>{(order.Freight_Total_Cost_User_NoT - selectedTransporter.Freight_Total_Cost_User_NoT).Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                    sb.Append("</tr>");

//                    sb.Append("</tbody>");
//                    sb.Append("</table>");
//                }

//                sb.Append(newLines);

//                //Freight Log Comparison
//                sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//                sb.Append("<thead>");

//                if (!useDifferentFormat)
//                {
//                    sb.Append("<tr>");
//                    sb.Append($"<th style='{cellStyleData};background-color:blue' colspan='2'><b>Number of Trips</b></th>");
//                    sb.Append($"<th style='{cellStyleData}'><b>{order.Freight_Container}</b></td>");
//                    sb.Append($"<th style='border-top:0px!important;border-right:0px!important' colspan='12'>&nbsp;</th>");
//                    sb.Append("</tr>");
//                    sb.Append("<tr>");
//                    sb.Append($"<th colspan='15' style='{headerStyle}'>AVAILABLE TRANSPORTER</th>");
//                    sb.Append("</tr>");
//                }

//                //sb.Append("<tr>");
//                //sb.Append($"<th style='{cellStyleData};background-color:blue' colspan='2'><b>Number of Trips</b></th>");
//                //sb.Append($"<th style='{cellStyleData}'><b>{order.Freight_Container}</b></td>");
//                //sb.Append($"<th style='border-top:0px!important;border-right:0px!important' colspan='12'>&nbsp;</th>");
//                //sb.Append("</tr>");
//                //sb.Append("<tr>");
//                //sb.Append($"<th colspan='15' style='{headerStyle}'>AVAILABLE TRANSPORTER</th>");
//                //sb.Append("</tr>");
//                sb.Append("<tr>");
//                sb.Append($"<th style='{headerStyle}'>No</th>");
//                sb.Append($"<th style='{headerStyle}'>{DisplayNames.TRANSPORTER}</th>");
//                sb.Append($"<th style='{headerStyle}'>{DisplayNames.TRUCK_TYPE}</th>");
//                sb.Append($"<th style='{headerStyle}'>{DisplayNames.UOM}</th>");
//                sb.Append($"<th style='{headerStyle}'>{DisplayNames.TRUCK_CAPACITY}</th>");
//                sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_MASTER}</th>");
//                sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_KG_BASED}</th>");

//                sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_COST}</th>");
//                sb.Append($"<th style='{headerStyle}'>{DisplayNames.SYSTEM_NOT}</th>");

//                sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_TOTAL_COST}</th>");
//                sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_KG}</th>");
//                sb.Append($"<th style='{headerStyle}'>{DisplayNames.USER_NOT}</th>");
//                sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_TOTAL_COST_USER}</th>");


//                if (!useDifferentFormat)
//                {
//                    sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_DIFFERENT_PER_UOM}</th>");
//                    sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_DIFFERENT_PERCENTAGE}</th>");
//                    sb.Append($"<th style='{headerStyle}'>{DisplayNames.FREIGHT_TOTAL_DIFFERENT}</th>");
//                }

//                sb.Append("</tr>");
//                sb.Append("</thead>");
//                sb.Append("<tbody>");

//                for (int x = 0; x < transporterLists.Count; x++)
//                {
//                    var list = transporterLists[x];
//                    var transporterListRowStyle = basicStyle;

//                    try 
//                    {
//                        if (order.Transporter_Code == list.Transporter_Code &&
//                           order.Truck_Type_Id == list.Truck_Type_Id &&
//                           order.Freight_Cost_Value == list.Freight_Value && (order.Freight_Cost_UOM == list.Per_UOM)
//                       )
//                        {
//                            transporterListRowStyle += ";background-color:#ffe699";
//                        }

//                        var transporter = db.Transporters.Where(r => r.Code == list.Transporter_Code).FirstOrDefault();
//                        var truckType = db.FleetTypes.Where(r => r.Id == list.Truck_Type_Id).FirstOrDefault();


//                        sb.Append("<tr>");
//                        sb.Append($"<td style='{transporterListRowStyle}'>{(x + 1)}</td>");
//                        sb.Append($"<td style='{transporterListRowStyle}'>{transporter.Name}</td>");
//                        sb.Append($"<td style='{transporterListRowStyle}'>{truckType.Description}</td>");
//                        sb.Append($"<td style='{transporterListRowStyle}'>{(list.Per_UOM)}</td>");
//                        sb.Append($"<td style='{transporterListRowStyle}'>{list.Truck_Type_Capacity.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                        sb.Append($"<td style='{transporterListRowStyle}'>{list.Freight_Master}</td>");
//                        sb.Append($"<td style='{transporterListRowStyle}'>{list.Freight_KG_Capacity.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                        sb.Append($"<td style='{transporterListRowStyle}'>{list.Freight_Cost.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                        sb.Append($"<td style='{transporterListRowStyle}'>{list.System_NoT.Value}</td>");
//                        sb.Append($"<td style='{transporterListRowStyle}'>{list.Freight_Cost_Total.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                        sb.Append($"<td style='{transporterListRowStyle};color:red'>{list.Freight_KG_Delivery.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                        sb.Append($"<td style='{transporterListRowStyle}'>{(order.Freight_Container.HasValue ? order.Freight_Container.Value : 1)}</td>");
//                        sb.Append($"<td style='{transporterListRowStyle}'>{list.Freight_Total_Cost_User_NoT.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");


//                        if (!useDifferentFormat)
//                        {
//                            sb.Append($"<td style='{transporterListRowStyle}'>{list.Freight_Different_Per_UOM.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                            sb.Append($"<td style='{transporterListRowStyle}'>{list.Freight_Different_Percentage.Value.ToString("N2", new CultureInfo("en-GB"))}%</td>");
//                            sb.Append($"<td style='{transporterListRowStyle}'>{list.Freight_Total_Different.Value.ToString("N0", new CultureInfo("en-GB"))}</td>");
//                        }

//                        //sb.Append($"<td style='{basicStyle}'>{detail.Item_No}</td>");
//                        //sb.Append($"<td style='{cellStyleData}'>{detail.Material_Name}</td>");
//                        //sb.Append($"<td style='{cellStyle};text-align:right'>{detail.Qty.ToString("N3", new CultureInfo("en-GB"))}</td>");
//                        //sb.Append($"<td style='{basicStyle}'>{detail.UOM}</td>");
//                        sb.Append("</tr>");
//                    }
//                    catch(Exception ex)
//                    {
//                        var a = "";
//                    }
//                }

//                sb.Append("<tr>");
//                sb.Append("<td style='border-right:0px!important;border-left:0px!important' colspan='15'>&nbsp;</td>");
//                sb.Append("</tr>");

//                //REASON
//                if (!string.IsNullOrEmpty(order.Assign_Reason))
//                {
//                    var reasonLookup = db.Lookups.Where(r => r.Lookup_Group == Constants.LookupGroup.ASSIGN_TRANSPORTER_REASON && r.Lookup_Key == order.Assign_Reason).FirstOrDefault();

//                    if (reasonLookup != null)
//                    {
//                        sb.Append("<tr>");
//                        sb.Append($"<td style='{headerStyle};background-color:yellow' rowspan='2' colspan='2'>{DisplayNames.REASON}</th>");
//                        sb.Append($"<td style='{cellStyleData}' colspan='13'>{reasonLookup.Lookup_Value}</td>");
//                        sb.Append("</tr>");
//                        sb.Append("<tr>");
//                        sb.Append($"<td style='{cellStyleData}' colspan='13'>{order.Assign_Reason_Others}</td>");
//                        sb.Append("</tr>");
//                    }
//                }

//                sb.Append("</tbody>");
//                sb.Append("</table>");
//            }

//            return sb.ToString();
//        }



//        public static string GetMailRelationOrderDetail(DMSEntities db, SAP_Order order, List<SAPOrderDetailViewModel> details, string approvalRemarks = "")
//        {
//            StringBuilder sb = new StringBuilder();

//            var sapNo = order.Document_No; //order.SAP_No;
//            var company = order.Company_Code; //order.Company.Code + " - " + order.Company.Name;
//            var incoterm = order.Incoterm; //order.Incoterm_Name;
//            var incoterm2 = order.Incoterm2;
//            var productCategory = order.Product_Category; // order.Business_Segment_Name;
//            var distribution = order.Distribution;
//            var plant = order.Plant; //order.Plant_Code + " - " + order.Plant_Name;
//            var customerVendor = order.Customer_Vendor_Code; //order.Customer_Vendor_Name;
//            var orderType = order.Order_Type; //order.Order_Type.Code + " - " + order.Order_Type.Name;
//            //var stuffingDate = order.Stuffing_Date (order.Stuffing_Date_From.HasValue ? order.Stuffing_Date_From.Value.ToString("dd/MM/yyyy") : "") + " - " + (order.Stuffing_Date_To.HasValue ? order.Stuffing_Date_To.Value.ToString("dd/MM/yyyy") : "");
//            string warehouseCode = order.Warehouse_Code; //order.Warehouse_Name;
//            var remarks = ""; // order.Remarks;

//            var incoterm2Name = incoterm2;
//            var companyName = company;
//            var incotermName = incoterm;
//            var productCategoryName = productCategory;
//            var plantName = plant;
//            var orderTypeName = orderType;
//            var warehouseName = warehouseCode;
//            string deliveryAddress = order.Delivery_Address;

//            var incoterm2Data = db.Locations.Where(r => r.Code == incoterm2).FirstOrDefault();
//            if (incoterm2Data != null)
//            {
//                incoterm2Name = incoterm2 + " - " + incoterm2Data.Description;
//            }

//            var companyData = db.Companies.Where(r => r.Code == company).FirstOrDefault();
//            if(companyData != null)
//            {
//                companyName = company + " - " + companyData.Name;
//            }

//            var incotermData = db.Incoterms.Where(r => r.Incoterms == incoterm).FirstOrDefault();
//            if(incotermData != null)
//            {
//                incotermName = incoterm + " - " + incotermData.Description;
//            }

//            var productCategoryData = db.Business_Segment.Where(r => r.Code == productCategory).FirstOrDefault();
//            if(productCategoryData != null)
//            {
//                productCategoryName = productCategory + " - " + productCategoryData.Description;
//            }

//            var plantData = db.Plants.Where(r => r.Code == plant).FirstOrDefault();
//            if(plantData != null)
//            {
//                plantName = plant + " - " + plantData.Description;
//            }

//            var orderTypeData = db.Order_Type.Where(r => r.Code == orderType).FirstOrDefault();
//            if(orderTypeData != null)
//            {
//                orderTypeName = orderType + " - " + orderTypeData.Name;
//            }


//            //string loading = "", destination = "";
//            //if (string.IsNullOrEmpty(order.Customer_Code)) //VENDOR
//            //{
//            //    loading = "[VENDOR] VENDOR";
//            //    destination = "[" + order.Location1.Code + "] " + order.Location1.Description;
//            //}
//            //else
//            //{
//            //    loading = "[" + order.Location.Code + "] " + order.Location.Description;
//            //    destination = "[CUSTOMER] CUSTOMER";
//            //}

//            //var deliveryAddress = string.IsNullOrEmpty(order.Delivery_Address) ? order.Customer_Vendor_Address : order.Delivery_Address;

//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<tr>");
//            sb.Append(
//                $"<td style='{cellStyle}'><b>{DisplayNames.MANUAL_ORDER}</b></td><td style='{cellStyleData}'>{sapNo}</td>" +
//                $"{dividerCol}" +
//                $"<td style='{cellStyle}'><b>{DisplayNames.INCOTERM}</b></td><td style='{cellStyleData}'>{incotermName}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//                $"<td style='{cellStyle}'><b>{DisplayNames.COMPANY}</b></td><td style='{cellStyleData}'>{companyName}</td>" +
//               $"{dividerCol}" +
//                $"<td style='{cellStyle}'><b>{DisplayNames.INCOTERM2}</b></td><td style='{cellStyleData}'>{incoterm2Name}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.BUSINESS_SEGMENT}</b></td><td style='{cellStyleData}'>{productCategoryName}</td>" +
//              $"<td style='{dividerStyle}'>{divider}</td>" +
//              $"<td style='{cellStyle}'><b>{DisplayNames.DISTRIBUTION}</b></td><td style='{cellStyleData}'>{distribution}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.PLANT}</b></td><td style='{cellStyleData}'>{plant}</td>" +
//              $"{dividerCol}" +
//              $"<td style='{cellStyle}'><b>{DisplayNames.CUSTOMER_VENDOR}</b></td><td style='{cellStyleData}'>{customerVendor}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//              $"<td style='{cellStyle}'><b>{DisplayNames.ORDER_TYPE}</b></td><td style='{cellStyleData}'>{orderType}</td>" +
//              $"{dividerCol}" +
//              $"<td style='{cellStyle}'><b>{DisplayNames.WAREHOUSE}</b></td><td style='{cellStyleData}'>{warehouseName}</td>");
//              //$"<td style='{cellStyle}'><b>{DisplayNames.STUFFING_DATE}</b></td><td style='{cellStyleData}'>{stuffingDate}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//            //  $"<td style='{cellStyle}'><b>{DisplayNames.WAREHOUSE}</b></td><td style='{cellStyleData}'>{warehouseName}</td>" +
//            //  $"{dividerCol}" +
//            $"<td style='{cellStyle}' rowspan='2'><b>{DisplayNames.DELIVERY_ADDRESS}</b></td><td style='{cellStyleData}' rowspan='2'>{deliveryAddress}</td>");
//            sb.Append("</tr>");
//            sb.Append("<tr>");
//            sb.Append(
//                $"<td style='{cellStyle}'><b>{DisplayNames.REMARKS}</b></td><td style='{cellStyleData}'{remarks}</td>");
//               //$"<td style='{cellStyle}'><b>{DisplayNames.LOADING}</b></td><td style='{cellStyleData}'>{loading}</td>");
//            sb.Append("</tr>");
//            //sb.Append("<tr>");
//            //sb.Append(
//            //   $"<td style='{cellStyle}'><b>{DisplayNames.DESTINATION}</b></td><td style='{cellStyleData}'>{destination}</td>" +
//            //   $"{dividerCol}" +
//            //   $"<td style='{cellStyle}'><b>{DisplayNames.REMARKS}</b></td><td style='{cellStyleData}'{remarks}</td>"
//            //  );
//            //sb.Append("</tr>");
//            sb.Append("</table>");

//            sb.Append(newLines);

//            sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//            sb.Append("<thead>");
//            sb.Append("<tr>");
//            sb.Append($"<th style='{headerStyle}'>Item No</th>");
//            sb.Append($"<th style='{headerStyle}'>Material</th>");
//            sb.Append($"<th style='{headerStyle}'>Qty</th>");
//            sb.Append($"<th style='{headerStyle}'>UOM</th>");
//            sb.Append("</tr>");
//            sb.Append("</thead>");
//            sb.Append("<tbody>");
//            foreach (var detail in details)
//            {
//                sb.Append("<tr>");
//                sb.Append($"<td style='{basicStyle}'>{detail.Item_No}</td>");
//                sb.Append($"<td style='{cellStyleData}'>{detail.material_name}</td>");
//                sb.Append($"<td style='{cellStyle};text-align:right'>{detail.Qty.Value.ToString("N3", new CultureInfo("en-GB"))}</td>");
//                sb.Append($"<td style='{basicStyle}'>{detail.UOM}</td>");
//                sb.Append("</tr>");
//            }
//            sb.Append("</tbody>");
//            sb.Append("</table>");

//            return sb.ToString();
//        }


//        public static string SendCreateShipment(DMSEntities db, Shipment header, List<string> to = null, List<string> cc = null, List<string> bcc = null)
//        {
//            var result = "";
//            var body = "";
//            try
//            {
//                StringBuilder sb = new StringBuilder();
//                string newLines = "<br /><br />";
//                string cellStyle = "padding: 8px; border: 1px solid black";
//                sb.Append("Dear Approver,");
//                sb.Append(newLines);
//                sb.Append("Below is the Order details for Order Number: <b>" + header.Order_No + "</b>");
//                sb.Append(newLines);

//                var shipmentNo = header.Shipment_No;
//                var sapNo = header.SAP_No;
//                var productCategoryName = header.Business_Segment.Code;
//                var deliveryAddress = header.Delivery_Address;
//                List<Shipment_Order_Detail> de = db.Shipment_Order_Detail.Where(r => r.Shipment_Id == header.Id).ToList();

//                //incoterm
//                //doc type
//                //order type
//                //wb trans type
//                //po no
//                //so no
//                //do no
//                //gr custom no
//                //document date
//                //delivery address


//                //Order data
//                sb.Append("<table style=\"border-collapse: collapse;font-family:Verdana, Geneva, sans-serif;font-size:11px\">");
//                sb.Append("<tr>");
//                sb.Append($"<td style='{cellStyle}'>Order No</td><td style='{cellStyle}'>{shipmentNo} / {sapNo}</td>");
//                sb.Append("</tr>");
//                sb.Append("<tr>");
//                sb.Append($"<td style='{cellStyle}'>Product Category</td><td style='{cellStyle}'>{productCategoryName}</td>");
//                sb.Append("</tr>");
//                sb.Append("<tr>");
//                sb.Append($"<td style='{cellStyle}'>Delivery Address</td><td style='{cellStyle}'>{deliveryAddress}</td>");
//                sb.Append("</tr>");
//                sb.Append("</table>");

//                sb.Append(newLines);
//                sb.Append("<strong>Thanks and Regards, </strong>");
//                sb.Append(newLines);
//                sb.Append("<b>FAMS</b>");
//                sb.Append(newLines);
//                sb.Append("<i>Note: This is an auto-generated E-Mail</i>");

//                body = sb.ToString();

//                string refNo = header.SAP_No + " / " + header.Shipment_No;
//                result = SendMail(db, refNo, "CREATE SHIPMENT", "FAMS Shipment " + refNo, body, to);
//                //SendFCM("FAMS Shipment", "", to);
//            }
//            catch (Exception exc)
//            {

//                result = exc.Message.Replace(Environment.NewLine, " ");
//            }
//            return result;
//        }
//        public static string SendAutoReject(DMSEntities db, Order header, List<string> to = null, List<string> cc = null, List<string> bcc = null)
//        {
//            var result = "";
//            var body = "";
//            to = new List<String>();
//            cc = new List<String>();
//            try
//            {
//                StringBuilder sb = new StringBuilder();

//                sb.Append("Dear Transporter,");
//                sb.Append(newLines);
//                sb.Append("Order with Reference Number: <b>" + /*header.SAP_No + " / " +*/ header.Order_No + "</b> has been <b>AUTOMATICALLY REJECTED</b>");
//                sb.Append(newLines);

//                sb.Append(newLines);
//                sb.Append("<strong>Thanks and Regards, </strong>");
//                sb.Append(newLines);
//                sb.Append("<b>FAMS</b>");
//                sb.Append(newLines);
//                sb.Append("<i>Note: This is an auto-generated E-Mail</i>");

//                body = sb.ToString();
//                to = new List<String>();

//                var creator = db.Users.Where(r => header.Created_By.Contains(r.Employee_Number)).FirstOrDefault();

//                string refNo = header.SAP_No + " / " + header.Order_No;
//                result = SendMail(db, refNo, "AUTO REJECT", "FAMS Auto Reject - " + refNo, body, to, cc);
//                //SendFCM("FAMS Approval", "",to);
//            }
//            catch (Exception exc)
//            {

//                result = exc.Message.Replace(Environment.NewLine, " ");
//            }
//            return result;
//        }

//    }

}
