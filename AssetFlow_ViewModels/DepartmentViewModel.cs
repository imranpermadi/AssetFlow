using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Utilities;

namespace FAMS_ViewModels
{
    public class DepartmentViewModel : DepartmentModel
    {
        public DepartmentViewModel() { }

        public DepartmentViewModel(DataEntities db, long id)
        {
            Department model = db.Department.Find(id);

            if (model == null) return;
            BaseProgram.CopyProperties(typeof(Department), model, typeof(DepartmentViewModel), this);
        }

        public DepartmentViewModel(Department model)
        {
            BaseProgram.CopyProperties(typeof(Department), model, typeof(DepartmentViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
    }
}
