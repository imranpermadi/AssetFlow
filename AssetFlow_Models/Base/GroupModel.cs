using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models
{
    public class GroupModel
    {
        public GroupModel()
        {
            Details = new List<GroupMenuModel>();
        }

        public string mode { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public string EditedBy { get; set; }

        public string IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }

        public List<GroupMenuModel> Details { get; set; }
    }

    public class GroupMenuModel
    {
        public string mode { get; set; }
        public long Id { get; set; }
        public long GroupId { get; set; }
        public long MenuId { get; set; }
        public bool View { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Create { get; set; }
        public bool Print { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public string EditedBy { get; set; }
        public string IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }


    }
}
