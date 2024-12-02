using FAMS_Models.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models
{
    public class CompanyModel
    {
        public CompanyModel() 
        {
            Details = new List<CompanyDetailModel>();
        }

        public long Id { get; set; }
        public string Code { get; set; }

        public string CodeSAP { get; set; }
        public string Description { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public string EditedBy { get; set; }
        public string IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; }
      

        #region CustomAttributes
        public string mode { get; set; }

        public List<CompanyDetailModel> Details { get; set; }
        #endregion
    }

    public class CompanyDetailModel
    {
        public CompanyDetailModel() { }

        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string LocationCode { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public string EditedBy { get; set; }
        public string IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public string Company_Code { get; set; }
        public string Company_Description { get; set; }

        #region CustomAttributes
        public string mode { get; set; }
        #endregion
    }


}
