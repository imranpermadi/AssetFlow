using FAMS_Models.Base;

namespace FAMS_Models
{
    public class LocationSAPModel : BaseModel
    {
        public string mode { get; set; }
        public long Id { get; set; }
        public string SAPCompanyCode { get; set; }
        public string Location { get; set; }
        public string Plant { get; set; }
        public string BA { get; set; }
    }
}
