using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models
{
    public class ReportModel
    {
        public ReportModel() {
            Location_Id = new List<int>();
            Company_Id= new List<int>();
        }    
        public string Report_Type { get; set; }
        public List<int> Company_Id { get; set; }
        public List<int> Location_Id { get; set; }
        public int? Storage_Location_Id { get; set; }
        public int? Department_Id { get; set; }
        public string Document_Category { get; set; }
        public int? Packing_No { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Period_From { get; set; }
        public Nullable<System.DateTime> Period_To { get; set; }

        public int? From_Packing_No { get; set; }
        public int? To_Packing_No { get; set; }
        public string Relocation_No { get; set; }
        public string Remarks { get; set; }
    }

    public class ReportResult
    {
        public string title { get; set; }
        public string header { get; set; }
        public string content { get; set; }
    }
}
