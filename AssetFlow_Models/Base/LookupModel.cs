using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models
{
    public class LookupModel
    {
        public string mode { get; set; }

        public long Id { get; set; }
        public string LookupGroup { get; set; }
        public string LookupKey { get; set; }
        public string LookupValue { get; set; }
        public string LookupInfo1 { get; set; }
        public string LookupInfo2 { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public string EditedBy { get; set; }
        public string IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; }
    }
}
