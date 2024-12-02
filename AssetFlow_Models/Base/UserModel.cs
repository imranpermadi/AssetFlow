using FAMS_Models.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models
{
    public class UserModel
    {
        public UserModel() {
            Groups = new List<UserGroupModel>();
            Companies = new List<UserCompanyModel>();
        }
        [DefaultValue(Constants.FORM_MODE_UNCHANGED)]
        public string mode { get; set; }

        public long Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string LocationCode { get; set; }
        public string IsAdmin { get; set; }
        public string UseAD { get; set; }
   
        public Nullable<System.DateTime> LastAccessDate { get; set; }

        public bool CompanyAll { get; set; }
        public bool DepartmentAll { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
    
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public string EditedBy { get; set; }
        public string IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; }


        public List<UserGroupModel> Groups { get; set; }
        public List<UserCompanyModel> Companies { get; set; }
    }

    public class UserGroupModel
    {
        public string mode { get; set; }
        public long Id { get; set; }
        public long UserId { get; set; }
        public long GroupId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public string EditedBy { get; set; }
        public string IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; }
    }

    public class UserCompanyModel
    {
        public string mode { get; set; }
        public long Id { get; set; }
        public long UserId { get; set; }
        public long CompanyId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public string EditedBy { get; set; }
        public string IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; }
    }

    //public class UserDepartmentModel
    //{
    //    public string mode { get; set; }
    //    public long Id { get; set; }
    //    public long UserId { get; set; }
    //    public long DepartmentId { get; set; }
    //    public Nullable<System.DateTime> CreatedDate { get; set; }
    //    public string CreatedBy { get; set; }
    //    public Nullable<System.DateTime> EditedDate { get; set; }
    //    public string EditedBy { get; set; }
    //    public string IsDeleted { get; set; }
    //    public Nullable<System.DateTime> DeletedDate { get; set; }
    //    public string DeletedBy { get; set; }
    //}

    //USER LOGIN LOG TOKEN

}
