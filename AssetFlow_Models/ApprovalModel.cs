using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models
{
    public class ApprovalModel
    {
        public long Id { get; set; }
        public Nullable<long> Document_Transfer_Id { get; set; }
        public Nullable<long> Document_Deactivation_Id { get; set; }
        public string Category { get; set; }
        public string Mode { get; set; }
        public string Reference_No { get; set; }
        public string Reference_No1 { get; set; }
        public string Reference_No2 { get; set; }
        public string Remarks { get; set; }
        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public string Status { get; set; }
        public long Approval_Matrix_Id { get; set; }
        public Nullable<long> Appointment_Matrix_Id { get; set; }
        public string Last_Approval_By { get; set; }
        public Nullable<System.DateTime> Last_Approval_Date { get; set; }
        public bool Active { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Created_By { get; set; }



        #region CustomAttributes
        public long Approval_Id { get; set; }

        public string Username { get; set; }
        public string Approval_By { get; set; }
        public DateTime? Approval_Date { get; set; }
        public string Document_Desc { get; set; }
        public string Storage_Location { get; set; }
        public string Storage_From { get; set; }
        public string Destination { get; set; }
        public string Packing_No { get; set; }
        public string Transfer_Type { get; set; }
        public List<ApprovalDocumentDetail> Details { get; set; }
        #endregion


    }

    public class ApprovalDetailModel
    {
        public long Id { get; set; }
        public long Approval_Id { get; set; }

        public string Approval_As { get; set; }
        public string Username { get; set; }
        public Nullable<int> Level { get; set; }
        public string Status { get; set; }
        public bool Active { get; set; }
        public bool Last_Approval { get; set; }
        public Nullable<System.DateTime> Approval_Date { get; set; }
        public string Approval_By { get; set; }
        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public string Remarks { get; set; }



        #region CustomAttributes
        public string mode { get; set; }

        public string fullname { get; set; }

        public bool is_current { get; set; }

        public int level_increment { get; set; }
        #endregion
    }

    public class ApprovalDocumentModel
    {
        public long Id { get; set; }
        public long Approval_Id { get; set; }

        #region CustomAttributes
        public string mode { get; set; }
        #endregion
    }

    public class ApprovalDocumentDetail
    {
        public string Doc_No { get; set; }
        public string Doc_Description { get; set; }
        public string Doc_Category_Code { get; set; }
        public string Doc_Validity { get; set; }
        public Nullable<System.DateTime> Period_From { get; set; }
        public Nullable<System.DateTime> Period_To { get; set; }
        public string Attachment { get; set; }
        public string Status { get; set; }

        public string mode { get; set; }
        public string Ref_No { get; set; }
        public string Doc_Category { get; set; }
        public string Storage_Location { get; set; }
        public string Packing { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public long Doc_Id { get; set; }
        public string Packaging { get; set; }

    }

    public class Approval_Log
    {
        public int? Level { get; set; }
        public string Action_By { get; set; }
        public string Validity { get; set; }
        public string Status { get; set; }
        public string Action_Role { get; set; }
    }
}
