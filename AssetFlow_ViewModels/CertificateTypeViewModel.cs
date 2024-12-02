using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Utilities;

namespace FAMS_ViewModels
{
    public class CertificateTypeViewModel : CertificateTypeModel
    {
        public CertificateTypeViewModel() { }

        public CertificateTypeViewModel(DataEntities db, long id)
        {
            CertificateType model = db.CertificateType.Find(id);

            if (model == null) return;
            BaseProgram.CopyProperties(typeof(CertificateType), model, typeof(CertificateTypeViewModel), this);
        }

        public CertificateTypeViewModel(CertificateType model)
        {
            BaseProgram.CopyProperties(typeof(CertificateType), model, typeof(CertificateTypeViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
    }
}
