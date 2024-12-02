using FAMS_Models.Base;

namespace FAMS_Models
{
    public class CertificateTypeModel : BaseModel
    {
        public string mode { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
