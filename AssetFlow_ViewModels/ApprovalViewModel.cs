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
    public class ApprovalViewModel : ApprovalModel
    {
        public ApprovalViewModel() { }

        public ApprovalViewModel(DataEntities db, long id)
        {
            Approval model = db.Approval.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(Approval), model, typeof(ApprovalModel), this);
        }

        public ApprovalViewModel(Approval model)
        {
            BaseProgram.CopyProperties(typeof(Approval), model, typeof(ApprovalModel), this);
            //mode = Constants.FORM_MODE_UNCHANGED;
        }
    }

    public class ApprovalDetailViewModel : ApprovalDetailModel
    {
        public ApprovalDetailViewModel() { }

        public ApprovalDetailViewModel(DataEntities db, long id)
        {
            ApprovalDetail model = db.ApprovalDetail.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(ApprovalDetail), model, typeof(ApprovalDetailModel), this);
        }

        public ApprovalDetailViewModel(ApprovalDetail model)
        {
            BaseProgram.CopyProperties(typeof(ApprovalDetail), model, typeof(ApprovalDetailModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;


        }
    }

    //public class ApprovalDocumentViewModel : ApprovalDocumentModel
    //{
    //    public ApprovalDocumentViewModel() { }

    //    public ApprovalDocumentViewModel(DMSEntities db, long id)
    //    {
    //        Approval_Document model = db.Approval_Document.Find(id);
    //        if (model == null) return;
    //        BaseProgram.CopyProperties(typeof(Approval_Document), model, typeof(ApprovalDocumentModel), this);
    //    }

    //    public ApprovalDocumentViewModel(Approval_Detail model)
    //    {
    //        BaseProgram.CopyProperties(typeof(Approval_Document), model, typeof(ApprovalDocumentModel), this);
    //        mode = Constants.FORM_MODE_UNCHANGED;
    //    }
    //}


}
