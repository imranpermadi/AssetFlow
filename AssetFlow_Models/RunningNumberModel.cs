using FAMS_Models.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models
{
    public class RunningNumberModel
    {
        public RunningNumberModel()
        {
            Detail = new List<RunningNumberDetailModel>();
        }

        public string mode { get; set; }
        public long Company_Id { get; set; }
        public string Company_Code { get; set; }
        public string Company_Name { get; set; }
        public List<RunningNumberDetailModel> Detail { get; set; }
        //public long Id { get; set; }
        //public string Trans_Type { get; set; }
        //public Nullable<int> RangeFromNumber { get; set; }
        //public Nullable<int> RangeToNumber { get; set; }
        //public string Format { get; set; }
        //public Nullable<System.DateTime> Create_Date { get; set; }
        //public string Create_By { get; set; }
        //public Nullable<System.DateTime> Edit_Date { get; set; }
        //public string Edit_By { get; set; }

    }

    public class RunningNumberDetailModel
    {
        public RunningNumberDetailModel() { }

        public string mode { get; set; }
        public long Id { get; set; }
        public long Company_Id { get; set; }
        public string Trans_Type { get; set; }
        public Nullable<int> RangeFromNumber { get; set; }
        public Nullable<int> RangeToNumber { get; set; }
        public string Format { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Edit_Date { get; set; }
        public string Edit_By { get; set; }

    }
}
