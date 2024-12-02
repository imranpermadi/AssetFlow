using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Utilities;

namespace FAMS_ViewModels
{
    public class UomViewModel : UomModel
    {
        public UomViewModel() { }

        public UomViewModel(DataEntities db, long id)
        {
            UoM model = db.UoM.Find(id);

            if (model == null) return;
            BaseProgram.CopyProperties(typeof(UoM), model, typeof(UomViewModel), this);
        }

        public UomViewModel(UoM model)
        {
            BaseProgram.CopyProperties(typeof(UoM), model, typeof(UomViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
    }
}
