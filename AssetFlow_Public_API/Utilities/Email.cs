//using MeetingRoomVueDB;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Net;
//using System.Net.Mail;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data.Entity;
//using System.Globalization;

//namespace MeetingRoomVue.Utilities
//{
//    public class Email
//    {
//        public static bool isProductionServer = Convert.ToBoolean(ConfigurationManager.AppSettings["isProductionServer"]);
//        public static string helpDeskEmail = ConfigurationManager.AppSettings["helpDeskEmail"];

//        public static string emailHost = isProductionServer ? "10.1.1.3" : "10.1.1.3";
//        public static string emailID = isProductionServer ? "no-reply.meetingroom@company.co.id" : "no-reply.meetingroom@company.co.id";
//        public static string emailPass = isProductionServer ? "M3n4bl3D" : "M3n4bl3D";

//        public static string SendApproval(long id, string userEmail, int currentSequence)
//        {
//            MeetingRoomEntities db = new MeetingRoomEntities();
//            string result = Constants.OK;
//            Booking booking = db.Bookings.Include(r => r.Booking_Approvals).Include(r => r.Room).Include(r => r.Room.Floor).Include(r => r.Room.Floor.Location).Where(r => r.Id == id).SingleOrDefault();
//            string url = UUtils.GetAppLink();
//            if (booking == null)
//                return "Booking ID not found!";

//            if (booking.Canceled.Value == true)
//                return "Canceled Meeting Room Booking request";

//            using (SmtpClient client = new SmtpClient(emailHost))
//            {
//                client.Credentials = new NetworkCredential(emailID, emailPass);

//                using (MailMessage message = new MailMessage())
//                {
//                    try
//                    {
//                        StringBuilder body = new StringBuilder();
//                        message.IsBodyHtml = true;
//                        message.From = new MailAddress(emailID, "Wilmar Office System");
//                        message.Subject = "[Meeting Room Booking] - #" + booking.Id + " ";
//                        if (booking.Approved == null)
//                        {
//                            message.Subject += " Request for Approval";
//                            var pendingApprovals_ = db.Booking_Approvals.Where(r => r.Booking_Id == booking.Id && r.Approver_Level == currentSequence).ToList();
//                            List<string> names = new List<string>();
//                            foreach (var pendingApproval in pendingApprovals_)
//                            {
//                                if (message.To.Where(r => r.Address.ToUpper() == pendingApproval.Approver_Email.ToUpper()).ToArray().Length == 0)
//                                {
//                                    message.To.Add(new MailAddress(pendingApproval.Approver_Email, pendingApproval.Approver_Name));
//                                    names.Add(pendingApproval.Approver_Name);
//                                }
//                            }
//                            body.AppendLine("<div style=\"font-size : 80%; font-family : 'Myriad Web',Verdana,Helvetica,Arial,sans-serif;\" width='100%'>");
//                            body.AppendLine("<p>Dear " + (string.Join(",", names)) + ",</p>");
//                            body.AppendLine(@"
//                            <p>
//                            There's meeting room request that needs your approval.
//                            </p>");
//                            body.AppendLine(GetDetailTable(booking, db));
//                            body.AppendLine("<br/>");
//                            body.AppendLine("<p>Click here to view the open room booking request approval : <a href='" + url + "/Approval'>Meeting Room Booking</a></p>");
//                            body.AppendLine("<br/>");
//                            if (message.CC.Where(r => r.Address.ToUpper() == userEmail.ToUpper()).ToArray().Length == 0)
//                                message.CC.Add(new MailAddress(userEmail));
//                        }
//                        else
//                        {
//                            if (booking.Approved.Value)
//                            {
//                                message.Subject += " Request Approved";
//                                if (message.To.Where(r => r.Address.ToUpper() == userEmail.ToUpper()).ToArray().Length == 0)
//                                    message.To.Add(new MailAddress(userEmail));
//                                body.AppendLine("<div style=\"font-size : 80%; font-family : 'Myriad Web',Verdana,Helvetica,Arial,sans-serif;\" width='100%'>");
//                                body.AppendLine("<p>Dear " + booking.Create_By + ",</p>");
//                                body.AppendLine(@"
//                                <p>
//                                Your meeting room request has been fully approved by <b>" + booking.Approved_By + @"</b>.
//                                </p>");
//                                body.AppendLine("<br/>");
//                                body.AppendLine("<p>Click here to view the meeting room booking request : <a href='" + url + "/Booking/View/" + booking.Id.ToString() + "'>Meeting Room Booking</a></p>");
//                                body.AppendLine("<br/>");
//                            }
//                            else if (!booking.Approved.Value)
//                            {
//                                message.Subject += " Request Rejected";
//                                if (message.To.Where(r => r.Address.ToUpper() == userEmail.ToUpper()).ToArray().Length == 0)
//                                    message.To.Add(new MailAddress(userEmail));
//                                body.AppendLine("<div style=\"font-size : 80%; font-family : 'Myriad Web',Verdana,Helvetica,Arial,sans-serif;\" width='100%'>");
//                                body.AppendLine("<p>Dear " + booking.Create_By + ",</p>");
//                                body.AppendLine(@"
//                                <p>
//                                Your meeting room request has been rejected by <b>" + booking.Approved_By + @"</b>.
//                                </p>");
//                                body.AppendLine("<br/>");
//                                body.AppendLine("<p>Click here to view the meeting room booking request : <a href='" + url + "/Booking/View/" + booking.Id.ToString() + "'>Meeting Room Booking</a></p>");
//                                body.AppendLine("<br/>");
//                            }
//                        }
//                        body.AppendLine("<br/>");
//                        body.AppendLine("<p><b>Thanks and Regards,</b></p>");
//                        body.AppendLine("<br/>");
//                        body.AppendLine("<p><b>Meeting Room Booking System</b></p>");
//                        body.AppendLine("</div>");
//                        message.Body = body.ToString();
//                        client.Send(message);
//                    }
//                    catch (Exception exc)
//                    {
//                        result = exc.Message;
//                    }
//                    GenerateLog(booking, db, message, result);
//                }
//            }
//            return result;
//        }

//        public static string SendEmail(long id, string userEmail)
//        {
//            MeetingRoomEntities db = new MeetingRoomEntities();
//            string result = Constants.OK;
//            Booking booking = db.Bookings.Include(r => r.Room).Include(r => r.Booking_Participants).Include(r => r.Room.Floor).Include(r => r.Room.Floor.Location).Include(r => r.Room.Room_Admins).Where(r => r.Id == id).SingleOrDefault();
//            if (booking == null)
//                return "Booking ID not found!";
//            string url = UUtils.GetAppLink();
//            using (SmtpClient client = new SmtpClient(emailHost))
//            {
//                client.Credentials = new NetworkCredential(emailID, emailPass);
//                //string[] adminEmailTo = db.Administrators.Where(r => r.Send_As == "to" && !string.IsNullOrEmpty(r.Email)).Select(r => r.Email).ToArray();
//                //string[] adminEmailCC = db.Administrators.Where(r => r.Send_As == "cc" && !string.IsNullOrEmpty(r.Email)).Select(r => r.Email).ToArray();
//                var a = booking.Booking_Equipments;
//                string[] adminEmailTo = booking.Room.Room_Admins.Select(r => r.Email).ToArray();
//                List<string> adminEquipments = new List<string>();
//                string[] participants = booking.Booking_Participants.Select(r => r.Email).ToArray();
//                if (booking.Booking_Equipments.Count > 0)
//                {
//                    //var listEquipment = booking.Booking_Equipments.Where(r=> r.Checked).Select(r => r.Equipment.Id).ToList();
//                    adminEquipments = db.Room_Admin_Equipment.Where(r => /*listEquipment.Contains(r.Equipment_Id) &&*/ r.Location_Id == booking.Room.Floor.Location.Id).Select(r => r.Email).ToList();
//                }

//                using (MailMessage message = new MailMessage())
//                {
//                    try
//                    {
//                        message.IsBodyHtml = true;
//                        message.From = new MailAddress(emailID, "Wilmar Office System");
//                        message.Subject = GetSubject(booking);
//                        if (!string.IsNullOrEmpty(helpDeskEmail))
//                            message.To.Add(new MailAddress(helpDeskEmail));
//                        foreach (string s in adminEmailTo)
//                        {
//                            if (message.To.Where(r => r.Address.ToUpper() == s.ToUpper()).ToArray().Length == 0)
//                                message.To.Add(new MailAddress(s));
//                        }
//                        foreach (string s in adminEquipments)
//                        {
//                            if (message.CC.Where(r => r.Address.ToUpper() == s.ToUpper()).ToArray().Length == 0)
//                                message.CC.Add(new MailAddress(s));
//                        }
//                        foreach (string s in participants)
//                        {
//                            if (message.CC.Where(r => r.Address.ToUpper() == s.ToUpper()).ToArray().Length == 0)
//                                message.CC.Add(new MailAddress(s));
//                        }

//                        //foreach (string s in adminEmailCC)
//                        //{
//                        //    if (message.CC.Where(r => r.Address.ToUpper() == s.ToUpper()).ToArray().Length == 0)
//                        //        message.CC.Add(new MailAddress(s));
//                        //}
//                        if (message.CC.Where(r => r.Address.ToUpper() == userEmail.ToUpper()).ToArray().Length == 0 && message.To.Where(r => r.Address.ToUpper() == userEmail.ToUpper()).ToArray().Length == 0)
//                            message.To.Add(new MailAddress(userEmail));
//                        message.Body = GetBody(booking, db, url);
//                        client.Send(message);
//                    }
//                    catch (Exception exc)
//                    {
//                        result = exc.Message;
//                    }
//                    GenerateLog(booking, db, message, result);
//                }
//            }
//            return result;
//        }

//        public static string SendParticipant(long id, string userEmail)
//        {
//            MeetingRoomEntities db = new MeetingRoomEntities();
//            string result = Constants.OK;
//            Booking booking = db.Bookings.Include(r => r.Room).Include(r => r.Room.Floor).Include(r => r.Room.Floor.Location).Include(r => r.Booking_Participants).Where(r => r.Id == id).SingleOrDefault();
//            if (booking == null)
//                return "Booking ID not found!";

//            if (booking.Booking_Participants == null || booking.Booking_Participants.Count <= 0)
//                return "No participant";

//            string url = UUtils.GetAppLink();

//            using (SmtpClient client = new SmtpClient(emailHost))
//            {
//                client.Credentials = new NetworkCredential(emailID, emailPass);

//                Dictionary<string, string> listParticipants = new Dictionary<string, string>();

//                if (booking.Booking_Participants != null && booking.Booking_Participants.Count > 0)
//                    listParticipants = booking.Booking_Participants.ToDictionary(r => r.Name, r => r.Email);

//                using (MailMessage message = new MailMessage())
//                {
//                    try
//                    {
//                        message.From = new MailAddress(emailID, "Wilmar Office System");

//                        //add participant
//                        foreach (KeyValuePair<string, string> participant in listParticipants)
//                        {
//                            // do something with entry.Value or entry.Key
//                            if (message.To.Where(r => r.Address.ToUpper() == participant.Value.ToUpper()).ToArray().Length == 0)
//                                if (!message.To.Contains(new MailAddress(participant.Value, participant.Key)))
//                                    message.To.Add(new MailAddress(participant.Value, participant.Key));
//                        }

//                        if (message.To.Where(r => r.Address.ToUpper() == userEmail.ToUpper()).ToArray().Length == 0)
//                            if (!message.To.Contains(new MailAddress(userEmail, userEmail)))
//                                message.To.Add(new MailAddress(userEmail, userEmail));

//                        if (booking.Canceled.Value == true)
//                            message.Subject = "Canceled : " + booking.Description;
//                        else
//                            message.Subject = booking.Description;

//                        message.IsBodyHtml = true;

//                        message.Body = GetBody(booking, db, url, true);

//                        StringBuilder str = new StringBuilder();
//                        str.AppendLine("BEGIN:VCALENDAR");
//                        str.AppendLine("PRODID:-//Microsoft Corporation//Outlook 16.0 MIMEDIR//EN");
//                        str.AppendLine("VERSION:2.0");
//                        if (booking.Canceled.Value == true)
//                            str.AppendLine("METHOD:CANCEL");
//                        else
//                            str.AppendLine("METHOD:REQUEST");
//                        str.AppendLine("BEGIN:VEVENT");

//                        DateTime timeStart = (DateTime.ParseExact(
//                                                    booking.Book_Start.ToString("dd/MM/yyyy HH:mm:ss"),
//                                                    "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));

//                        DateTime timeEnd = (DateTime.ParseExact(
//                                                    booking.Book_End.ToString("dd/MM/yyyy HH:mm:ss"),
//                                                    "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
//                        str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", timeStart.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
//                        str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
//                        str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", timeEnd.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
//                        str.AppendLine(string.Format("LAST-MODIFIED:{0:yyyyMMddTHHmmssZ}", DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));

//                        str.AppendLine("LOCATION:" + booking.Room.Floor.Location.City + "-" + booking.Room.Floor.Location.Location_Name + " - " + booking.Room.Room_Name + " " + booking.Room.Floor.Floor1);
//                        str.AppendLine(string.Format("DESCRIPTION:{0}", booking.Description));
//                        //str.AppendLine("UID:" + System.Guid.NewGuid());
//                        string bookBy = "";
//                        try
//                        {
//                            bookBy = booking.Booked_By.ToString().Split('-')[0].ToString().TrimEnd();
//                        }
//                        catch
//                        {
//                            bookBy = "999";
//                        }
//                        str.AppendLine("UID:" + id + bookBy);
//                        str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", booking.Description));

//                        str.AppendLine(string.Format("SUMMARY:{0}", booking.Description));
//                        str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", userEmail));
//                        str.AppendLine("SEQUENCE:0");

//                        foreach (MailAddress email in message.To)
//                        {
//                            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", email.DisplayName, email.Address));
//                        }

//                        str.AppendLine("CREATED:" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
//                        str.AppendLine("BEGIN:VALARM");
//                        str.AppendLine("TRIGGER:-PT15M");
//                        str.AppendLine("ACTION:DISPLAY");
//                        str.AppendLine("DESCRIPTION:Reminder");
//                        str.AppendLine("END:VALARM");
//                        str.AppendLine("END:VEVENT");
//                        str.AppendLine("END:VCALENDAR");

//                        var htmlContentType = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Html);
//                        var avHtmlBody = AlternateView.CreateAlternateViewFromString(message.Body, htmlContentType);
//                        message.AlternateViews.Add(avHtmlBody);

//                        System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
//                        if (booking.Canceled.Value == true)
//                            ct.Parameters.Add("method", "CANCEL");
//                        else
//                            ct.Parameters.Add("method", "REQUEST");

//                        AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);
//                        message.AlternateViews.Add(avCal);

//                        client.Send(message);
//                    }
//                    catch (Exception exc)
//                    {
//                        result = exc.Message;
//                    }
//                    GenerateLog(booking, db, message, "Send invitation : " + result);
//                }
//            }
//            return result;
//        }

//        private static string GetSubject(Booking booking)
//        {
//            StringBuilder result = new StringBuilder();
//            result.Append("[Meeting Room Booking Request] - #" + booking.Id + " ");
//            if (booking.Canceled == null || booking.Canceled.Value == false)
//                result.Append("New Meeting Room Booking Request");
//            else
//                result.Append("Canceled Meeting Room Booking request");
//            return result.ToString();
//        }

//        private static string GetBody(Booking booking, MeetingRoomEntities db, string url, bool invitation = false)
//        {
//            StringBuilder result = new StringBuilder();
//            result.AppendLine("<div style=\"font-size : 80%; font-family : 'Myriad Web',Verdana,Helvetica,Arial,sans-serif;\" width='100%'>");
//            result.AppendLine("<p>Dear All,</p>");
//            if (invitation)
//            {
//                result.AppendLine(GetDetailTable(booking, db, invitation));

//                result.AppendLine(@"
//                <p>
//                <i>This invitation has been updated to your calendar automatically.</i>
//                </p>");

//                result.AppendLine("<br/><br/>");
//                result.AppendLine("<b>Thanks and Regards,</b><br/>");
//                result.AppendLine("<b>Meeting Room Booking System</b><br/>");
//                result.AppendLine("To book a meeting room, you can acccess to <a href='https://meetingroom.company.co.id'> https://meetingroom.company.co.id </a>");
//                result.AppendLine("</div>");
//            }
//            else
//            {
//                if (booking.Canceled == null || booking.Canceled.Value == false)
//                {
//                    result.AppendLine(@"
//                    <p>
//                    There is a new meeting room booking request.
//                    </p>");
//                    result.AppendLine(GetDetailTable(booking, db));
//                }
//                else
//                {
//                    result.AppendLine(@"
//                    <p>
//                    This meeting room booking request has been cancelled by " + booking.Canceled_By + " on " + booking.Canceled_Date + @".<br/>
//                    Reason: " + booking.Canceled_Reason + @". </br>
//                    Kindly proceed to ignore this request.
//                    </p>");
//                    result.AppendLine(GetDetailTable(booking, db));
//                }
//                result.AppendLine("<br/>");
//                result.AppendLine("<p>Click here to view the meeting room booking request : <a href='" + url + "/Booking/View/" + booking.Id.ToString() + "'>Meeting Room Booking</a></p>");
//                result.AppendLine("This E-Mail is sent to Room Admins, Requester and Equipment Admins");
//                result.AppendLine("<br/><br/>");
//                result.AppendLine("<br/>");
//                result.AppendLine("<p><b>Thanks and Regards,</b></p>");
//                result.AppendLine("<br/>");
//                result.AppendLine("<p><b>Meeting Room Booking System</b></p>");
//                result.AppendLine("</div>");
//            }

//            return result.ToString();
//        }

//        private static string GetDetailTable(Booking booking, MeetingRoomEntities db, bool invitation = false)
//        {
//            StringBuilder result = new StringBuilder();
//            var participants = booking.Booking_Participants.ToArray();
//            if (invitation)
//            {
//                string bookBy = "";
//                try
//                {
//                    bookBy = booking.Booked_By.ToString().Split('-')[1].ToString().TrimStart();
//                }
//                catch
//                {
//                    bookBy = booking.Booked_By.ToString();
//                }

//                if (booking.Canceled == true)
//                    result.AppendLine("Below Request had been <b>Cancelled</b> by the Organizer (<b>" + bookBy + "</b>) :");
//                else
//                {
//                    result.AppendLine(@"
//                                        <p>
//                                        <b>" + bookBy + @"</b> is inviting you to join a meeting with below detail :
//                                        </p>");
//                }

//                result.AppendLine("<table style=\"font-size : 70%; font-family : 'Myriad Web',Verdana,Helvetica,Arial,sans-serif;\" cellspacing='2' cellpadding='2' border='0'>");
//                result.AppendLine("<tbody>");

//                result.AppendLine("<tr style=\"background-color: #f2f2f2;\">");
//                result.AppendLine("<td><b>Title</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Description + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr>");
//                result.AppendLine("<td><b>Start</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Book_Start.ToString("yyyy-MM-dd HH:mm") + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr style=\"background-color: #f2f2f2;\">");
//                result.AppendLine("<td><b>End</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Book_End.ToString("yyyy-MM-dd HH:mm") + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr>");
//                result.AppendLine("<td><b>Location</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Room.Floor.Location.City + "-" + booking.Room.Floor.Location.Location_Name + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr style=\"background-color: #f2f2f2;\">");
//                result.AppendLine("<td><b>Floor</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Room.Floor.Floor1 + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr>");
//                result.AppendLine("<td><b>Room</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Room.Room_Name + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr style=\"background-color: #f2f2f2;\">");
//                result.AppendLine("<td><b>Booked by</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Booked_By.ToString() + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr>");
//                result.AppendLine("<td><b>Department</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Department + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("</tbody>");
//                result.AppendLine("</table>");
//                result.AppendLine("</p>");
//            }
//            else
//            {
//                result.AppendLine("<p>");
//                result.AppendLine("Detail for this request can be seen below.");
//                result.AppendLine("</p>");
//                result.AppendLine("<p>");
//                result.AppendLine("<table style=\"font-size : 70%; font-family : 'Myriad Web',Verdana,Helvetica,Arial,sans-serif;\" cellspacing='2' cellpadding='2' border='0'>");
//                result.AppendLine("<tbody>");

//                result.AppendLine("<tr>");
//                result.AppendLine("<td><b>ID</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Id + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr style=\"background-color: #f2f2f2;\">");
//                result.AppendLine("<td><b>Start</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Book_Start.ToString("yyyy-MM-dd HH:mm") + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr>");
//                result.AppendLine("<td><b>End</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Book_End.ToString("yyyy-MM-dd HH:mm") + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr style=\"background-color: #f2f2f2;\">");
//                result.AppendLine("<td><b>Location</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Room.Floor.Location.City + "-" + booking.Room.Floor.Location.Location_Name + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr>");
//                result.AppendLine("<td><b>Floor</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Room.Floor.Floor1 + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr style=\"background-color: #f2f2f2;\">");
//                result.AppendLine("<td><b>Room</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Room.Room_Name + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr>");
//                result.AppendLine("<td><b>Booked by</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Booked_By.ToString() + "</td>");
//                result.AppendLine("</tr>");

//                if (booking.Canceled.HasValue && booking.Canceled.Value)
//                {
//                    result.AppendLine("<tr style=\"background-color: #f2f2f2;\">");
//                    result.AppendLine("<td><b>Cancelled by</b></td>");
//                    result.AppendLine("<td>:</td>");
//                    result.AppendLine("<td>" + booking.Canceled_By.ToString() + "</td>");
//                    result.AppendLine("</tr>");

//                }

//                result.AppendLine("<tr style=\"background-color: #f2f2f2;\">");
//                result.AppendLine("<td><b>Department</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Department + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr>");
//                result.AppendLine("<td><b>Meeting Purpose</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.Description + "</td>");
//                result.AppendLine("</tr>");

//                result.AppendLine("<tr style=\"background-color: #f2f2f2;\">");
//                result.AppendLine("<td><b>Participant</b></td>");
//                result.AppendLine("<td>:</td>");
//                result.AppendLine("<td>" + booking.capacity + "</td>");
//                result.AppendLine("</tr>");

//                if (booking.Room.Room_Type == "EXECUTIVE")
//                {
//                    result.AppendLine("<tr>");
//                    result.AppendLine("<td><b>Zoom Meeting Id</b></td>");
//                    result.AppendLine("<td>:</td>");
//                    result.AppendLine("<td>" + booking.Meeting_Id + "</td>");
//                    result.AppendLine("</tr>");
//                }

//                result.AppendLine("</tbody>");
//                result.AppendLine("</table>");
//                result.AppendLine("</p>");

//                //Participant Name
//                if (participants.Count() != 0)
//                {
//                    result.AppendLine("<br/>");
//                    result.AppendLine("<table style=\"font-size : 70%; font-family : 'Myriad Web',Verdana,Helvetica,Arial,sans-serif;\" cellspacing='2' cellpadding='2' border='0'>");
//                    result.AppendLine("<tbody>");

//                    result.AppendLine("<tr>");
//                    result.AppendLine("<td><b>Participant List</b></td>");
//                    result.AppendLine("<td colspan='1'>:</td>");
//                    foreach (var s in participants)
//                    {
//                        result.AppendLine("<td>" + s.Name + "</td>");
//                        result.AppendLine("<td>|</td>");
//                        result.AppendLine("<td>" + s.Email + "</td>");
//                        result.AppendLine("<tr/>");
//                        result.AppendLine("<td colspan='2'/>");
//                    }
//                    result.AppendLine("</tr>");

//                    result.AppendLine("</tbody>");
//                    result.AppendLine("</table>");
//                }

//                //Consumables
//                var consumables = booking.Booking_Consumables.ToArray();
//                if (consumables.Count() != 0)
//                {
//                    result.AppendLine("<br/>");
//                    result.AppendLine("<table style=\"font-size : 70%; font-family : 'Myriad Web',Verdana,Helvetica,Arial,sans-serif;\" cellspacing='2' cellpadding='2' border='0'>");
//                    result.AppendLine("<tbody>");

//                    result.AppendLine("<tr>");
//                    result.AppendLine("<td><b>Consumable List</b></td>");
//                    result.AppendLine("<td colspan='1'>:</td>");
//                    result.AppendLine("<td><b>Food/Beverage</b></td>");
//                    result.AppendLine("<td>|</td>");
//                    result.AppendLine("<td><b>Qty Planned</b></td>");
//                    result.AppendLine("<td>|</td>");
//                    //result.AppendLine("<td><b>Qty Consumed</b></td>");
//                    //result.AppendLine("<td>|</td>");
//                    result.AppendLine("<td><b>UOM</b></td>");
//                    result.AppendLine("<tr/>");
//                    result.AppendLine("<td colspan='2'/>");

//                    foreach (var s in consumables)
//                    {
//                        result.AppendLine("<td>" + s.Consumable.Name + "</td>");
//                        result.AppendLine("<td>|</td>");
//                        result.AppendLine("<td>" + s.Qty_Planned + "</td>");
//                        result.AppendLine("<td>|</td>");
//                        //result.AppendLine("<td>" + s.Qty_Consumed + "</td>");
//                        //result.AppendLine("<td>|</td>");
//                        result.AppendLine("<td>" + s.UOM + "</td>");
//                        result.AppendLine("<tr/>");
//                        result.AppendLine("<td colspan='2'/>");
//                    }
//                    result.AppendLine("</tr>");

//                    result.AppendLine("</tbody>");
//                    result.AppendLine("</table>");
//                }

//                if (!string.IsNullOrEmpty(booking.Consumable_Remarks))
//                {
//                    result.AppendLine("<br/>");
//                    result.AppendLine("<table style=\"font-size : 70%; font-family : 'Myriad Web',Verdana,Helvetica,Arial,sans-serif;\" cellspacing='2' cellpadding='2' border='0'>");
//                    result.AppendLine("<tbody>");

//                    result.AppendLine("<tr>");
//                    result.AppendLine("<td><b>Consumable Remarks</b></td>");
//                    result.AppendLine("<td colspan='1'>:</td>");
//                    result.AppendLine("<td>" + booking.Consumable_Remarks + "</td>");
//                    result.AppendLine("</tr>");

//                    result.AppendLine("</tbody>");
//                    result.AppendLine("</table>");
//                }


//                //Guests
//                var guests = booking.Booking_Guests.ToArray();
//                if (guests.Count() != 0)
//                {
//                    result.AppendLine("<br/>");
//                    result.AppendLine("<table style=\"font-size : 70%; font-family : 'Myriad Web',Verdana,Helvetica,Arial,sans-serif;\" cellspacing='2' cellpadding='2' border='0'>");
//                    result.AppendLine("<tbody>");

//                    result.AppendLine("<tr>");
//                    result.AppendLine("<td><b>Guest List</b></td>");
//                    result.AppendLine("<td colspan='1'>:</td>");
//                    result.AppendLine("<td><b>Name</b></td>");
//                    result.AppendLine("<td>|</td>");
//                    result.AppendLine("<td><b>Company</b></td>");
//                    result.AppendLine("<td>|</td>");
//                    result.AppendLine("<td><b>Position</b></td>");
//                    result.AppendLine("<tr/>");
//                    result.AppendLine("<td colspan='2'/>");

//                    foreach (var s in guests)
//                    {
//                        result.AppendLine("<td>" + s.Name + "</td>");
//                        result.AppendLine("<td>|</td>");
//                        result.AppendLine("<td>" + s.Position + "</td>"); //Memang terbalik
//                        result.AppendLine("<td>|</td>");
//                        result.AppendLine("<td>" + s.Company + "</td>");
//                        result.AppendLine("<tr/>");
//                        result.AppendLine("<td colspan='2'/>");
//                    }
//                    result.AppendLine("</tr>");

//                    result.AppendLine("</tbody>");
//                    result.AppendLine("</table>");
//                }

//                //List<Booking_Equipments> requiredEquipments = db.Booking_Equipments.Include(r => r.Equipment).Where(r => r.Booking_Id == booking.Id).ToList();
//                //if (requiredEquipments != null && requiredEquipments.Count > 0)
//                //{
//                //    result.AppendLine("<p>");
//                //    result.AppendLine("Required Equipments :");
//                //    result.AppendLine("</p>");
//                //    result.AppendLine("<p>");
//                //    result.AppendLine("<table style=\"font-size : 70%; font-family : 'Myriad Web',Verdana,Helvetica,Arial,sans-serif;\" cellspacing='2' cellpadding='2' border='1'>");
//                //    result.AppendLine("<thead>");
//                //    result.AppendLine("<tr>");
//                //    result.AppendLine("<th>Equipment Name</th>");
//                //    result.AppendLine("<th>Required</th>");
//                //    result.AppendLine("</tr>");
//                //    result.AppendLine("</thead>");

//                //    result.AppendLine("<tbody>");
//                //    foreach (Booking_Equipments model in requiredEquipments)
//                //    {
//                //        result.AppendLine("<tr>");
//                //        result.AppendLine("<td>" + model.Equipment.Equipment_Name + "</td>");
//                //        result.AppendLine("<td>" + (model.Checked ? "Yes" : "No") + "</td>");
//                //        result.AppendLine("</tr>");
//                //    }
//                //    result.AppendLine("</tbody>");

//                //    result.AppendLine("</table>");
//                //    result.AppendLine("</p>");
//                //}


//                if (booking.Room.Room_Type.ToString() == "TRAINING")
//                {
//                    result.AppendLine("<p>");
//                    result.AppendLine("Additional Needs :");
//                    result.AppendLine("</p>");

//                    result.AppendLine("<p>");
//                    result.AppendLine("<table style=\"font-size : 70%; font-family : 'Myriad Web',Verdana,Helvetica,Arial,sans-serif;\" cellspacing='2' cellpadding='2' border='1'>");
//                    result.AppendLine("<thead>");
//                    result.AppendLine("<tr>");
//                    result.AppendLine("<th>Type of need</th>");
//                    result.AppendLine("<th>Required</th>");
//                    result.AppendLine("<th>Total per day</th>");
//                    result.AppendLine("<th>Remark</th>");
//                    result.AppendLine("</tr>");
//                    result.AppendLine("</thead>");

//                    result.AppendLine("<tbody>");

//                    result.AppendLine("<tr>");
//                    result.AppendLine("<td>Lunch</td>");
//                    result.AppendLine("<td>" + ((booking.Lunch == null || booking.Lunch == false) ? "No" : "Yes") + "</td>");
//                    result.AppendLine("<td>" + ((booking.Lunch == null || booking.Lunch == false) ? "-" : booking.Lunch_Per_Day.ToString()) + "</td>");
//                    result.AppendLine("<td>" + ((booking.Lunch == null || booking.Lunch == false) ? "-" : booking.Lunch_Remark.ToString()) + "</td>");
//                    result.AppendLine("</tr>");

//                    result.AppendLine("<tr>");
//                    result.AppendLine("<td>Coffee Break</td>");
//                    result.AppendLine("<td>" + ((booking.CoffeeBreak == null || booking.CoffeeBreak == false) ? "No" : "Yes") + "</td>");
//                    result.AppendLine("<td>" + ((booking.CoffeeBreak == null || booking.CoffeeBreak == false) ? "-" : booking.CoffeeBreak_Per_Day.ToString()) + "</td>");
//                    result.AppendLine("<td>" + ((booking.CoffeeBreak == null || booking.CoffeeBreak == false) ? "-" : booking.CoffeeBreak_Remark.ToString()) + "</td>");
//                    result.AppendLine("</tr>");

//                    result.AppendLine("</tbody>");

//                    result.AppendLine("</table>");
//                    result.AppendLine("</p>");
//                }
//            }

//            return result.ToString();
//        }

//        private static void GenerateLog(Booking booking, MeetingRoomEntities db, MailMessage message, string result)
//        {
//            Email_Logs log = new Email_Logs();
//            log.Booking_Id = booking.Id;
//            log.Email_To = message.To.Count > 0 ? string.Join(";", message.To.Select(r => r.Address).ToArray()) : "";
//            log.Email_CC = message.CC.Count > 0 ? string.Join(";", message.CC.Select(r => r.Address).ToArray()) : "";
//            log.Email_Subject = message.Subject;
//            log.Email_Body = message.Body;
//            log.Email_Result = result;
//            log.Triggered_By = "System";
//            log.Triggered_Date = DateTime.Now;
//            db.Entry(log).State = System.Data.Entity.EntityState.Added;
//            db.SaveChanges();
//        }
//    }
//}
