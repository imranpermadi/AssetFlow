using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models
{
    public class ReleaseMatrixModel
    {
        public ReleaseMatrixModel() 
        {
            Details = new List<ReleaseMatrixDetailModel>();
        }

        public long Id { get; set; }
        public string Type { get; set; }
        public string Validity { get; set; }
        public string Description { get; set; }
        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public string EditedBy { get; set; }
        public string IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; }

        public List<ReleaseMatrixDetailModel> Details { get; set; }


        #region CustomAttributes
        public string mode { get; set; }

        #endregion
    }

    public class ReleaseMatrixDetailModel
    {
        public ReleaseMatrixDetailModel() { }

        public long Id { get; set; }
        public long ReleaseMatrixId { get; set; }
        public string ApprovalAs { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public int Level { get; set; }
        public decimal Value { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public string EditedBy { get; set; }
        public string IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; }


        #region CustomAttributes
        public string mode { get; set; }
        #endregion
    }



}
