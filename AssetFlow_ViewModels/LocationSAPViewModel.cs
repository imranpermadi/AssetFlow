using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Utilities;

namespace FAMS_ViewModels
{
    public class LocationSAPViewModel : LocationSAPModel
    {
        public LocationSAPViewModel() { }

        public LocationSAPViewModel(DataEntities db, long id)
        {
            LocationSAP model = db.LocationSAP.Find(id);

            if (model == null) return;
            BaseProgram.CopyProperties(typeof(LocationSAP), model, typeof(LocationSAPViewModel), this);
        }

        public LocationSAPViewModel(LocationSAP model)
        {
            BaseProgram.CopyProperties(typeof(LocationSAP), model, typeof(LocationSAPViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
    }
}
