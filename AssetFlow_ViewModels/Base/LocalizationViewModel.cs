using FAMS_Data;
using FAMS_Models.Base;
using FAMS_Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_ViewModels.Base
{
    public class LocalizationViewModel : LocalizationModel
    {
        public LocalizationViewModel() { }

        public LocalizationViewModel(DataEntities db, long id)
        {
            Localization model = db.Localization.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(Localization), model, typeof(LocalizationModel), this);
        }

        public LocalizationViewModel(Localization model)
        {
            BaseProgram.CopyProperties(typeof(Localization), model, typeof(LocalizationModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
    }
}
