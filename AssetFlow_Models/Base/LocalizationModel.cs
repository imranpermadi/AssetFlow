using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models.Base
{
    public class LocalizationModel
    {
        public long Id { get; set; }
        public string LanguageCode { get; set; }
        public string Code { get; set; }
        public string Label { get; set; }
        public string Category { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public string EditedBy { get; set; }
        public string IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }

        #region CustomAttributes
        public string mode { get; set; }
        public string language_name { get; set; }
        #endregion
    }

    public class LanguageLocalizationModel
    {
        public string Code { get; set; }
        public string Category { get; set; }

        public List<LocalizationModel> Details { get; set; }
        
        public string mode { get; set; }

    }


}
