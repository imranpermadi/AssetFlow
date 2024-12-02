using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_ViewModels
{
    public class ReleaseMatrixViewModel: ReleaseMatrixModel
    {
        public ReleaseMatrixViewModel() { }

        public ReleaseMatrixViewModel(DataEntities db, long id)
        {
            ReleaseMatrix model = db.ReleaseMatrix.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(ReleaseMatrix), model, typeof(ReleaseMatrixModel), this);

            Details = new List<ReleaseMatrixDetailModel>();
            var details = db.ReleaseMatrixDetail.Where(r => r.ReleaseId == id && r.IsDeleted != "Y").ToArray();
            foreach (var detail in details)
            {
                Details.Add(new ReleaseMatrixDetailViewModel(detail));
            }
        }

        public ReleaseMatrixViewModel(ReleaseMatrix model)
        {
            BaseProgram.CopyProperties(typeof(ReleaseMatrix), model, typeof(ReleaseMatrixModel), this);
            //mode = Constants.FORM_MODE_UNCHANGED;
        }

    }

    public class ReleaseMatrixDetailViewModel: ReleaseMatrixDetailModel
    {
        public ReleaseMatrixDetailViewModel() { }

        public ReleaseMatrixDetailViewModel(DataEntities db, long id)
        {
            ReleaseMatrixDetail model = db.ReleaseMatrixDetail.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(ReleaseMatrixDetail), model, typeof(ReleaseMatrixDetailModel), this);
        }

        public ReleaseMatrixDetailViewModel(ReleaseMatrixDetail model)
        {
            BaseProgram.CopyProperties(typeof(ReleaseMatrixDetail), model, typeof(ReleaseMatrixDetailModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }



    }


}
