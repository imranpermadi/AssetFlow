using FAMS_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models
{
    public class RequestLogModel
    {
        public long Id { get; set; }
        public string Ref_No { get; set; }
        public Nullable<long> Document_Transfer_Id { get; set; }
        public Nullable<long> Document_Deactivation_Id { get; set; }
        public string Action_Role { get; set; }
        public string Action { get; set; }
        public string Notes { get; set; }
        public string Action_By { get; set; }
        public Nullable<System.DateTime> Action_Date { get; set; }
    }
}
