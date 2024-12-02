using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FAMS_ViewModels
{
    public class GroupViewModel:GroupModel
    {
        public GroupViewModel() { }

        public GroupViewModel(DataEntities db, long id)
        {
            Group model = db.Group.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(Group), model, typeof(GroupViewModel), this);

            Details = new List<GroupMenuModel>();
            var details = db.GroupMenu.Where(r => r.GroupId == id).ToArray();
            foreach (var detail in details)
            {
                Details.Add(new GroupMenuViewModel(detail));
            }
        }

        public GroupViewModel(Group model)
        {
            BaseProgram.CopyProperties(typeof(Group), model, typeof(GroupViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
    }

    public class GroupMenuViewModel:GroupMenuModel
    {
        public GroupMenuViewModel() { }

        public GroupMenuViewModel(DataEntities db, long id)
        {
            GroupMenu model = db.GroupMenu.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(GroupMenu), model, typeof(GroupMenuModel), this);
        }

        public GroupMenuViewModel(GroupMenu model)
        {
            BaseProgram.CopyProperties(typeof(GroupMenu), model, typeof(GroupMenuModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
    }

}
