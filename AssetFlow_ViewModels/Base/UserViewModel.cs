using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FAMS_ViewModels
{
    public class UserViewModel :UserModel
    {
        public UserViewModel() {}

        public UserViewModel(DataEntities db, long id)
        {
            User model = db.User.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(User), model, typeof(UserViewModel), this);

            model.Password = "";

            Groups = new List<UserGroupModel>();
            var details = db.UserGroup.Where(r => r.UserId == id).ToArray();
            foreach (var detail in details)
                Groups.Add(new UserGroupViewModel(detail));

            //Companies = new List<UserCompanyModel>();
            //var companies = db.UserCompany.Where(r => r.User_Id == id && r.Is_Deleted != "Y").ToArray();
            //foreach (var company in companies)
            //    Companies.Add(new UserCompanyViewModel(company));

            //Departments = new List<UserDepartmentModel>();
            //var departments = db.User_Department.Where(r => r.User_Id == id && r.Is_Deleted != "Y").ToArray();
            //foreach (var department in departments)
            //    Departments.Add(new UserDepartmentViewModel(department));
        }

        public UserViewModel(User model)
        {
            BaseProgram.CopyProperties(typeof(User), model, typeof(UserViewModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;

            model.Password = "";
        }

    }


    public class UserGroupViewModel: UserGroupModel
    {
        public UserGroupViewModel() { }

        public UserGroupViewModel(DataEntities db, long id)
        {
            UserGroup model = db.UserGroup.Find(id);
            if (model == null) return;
            BaseProgram.CopyProperties(typeof(UserGroup), model, typeof(UserGroupModel), this);
        }

        public UserGroupViewModel(UserGroup model)
        {
            BaseProgram.CopyProperties(typeof(UserGroup), model, typeof(UserGroupModel), this);
            mode = Constants.FORM_MODE_UNCHANGED;
        }
    }

    //public class UserCompanyViewModel : UserCompanyModel
    //{
    //    public UserCompanyViewModel() { }

    //    public UserCompanyViewModel(DataEntities db, long id)
    //    {
    //        UserCompany model = db.UserCompany.Find(id);
    //        if (model == null) return;
    //        BaseProgram.CopyProperties(typeof(UserCompany), model, typeof(UserCompanyModel), this);
    //    }

    //    public UserCompanyViewModel(UserCompany model)
    //    {
    //        BaseProgram.CopyProperties(typeof(User_Company), model, typeof(UserCompanyModel), this);
    //        mode = Constants.FORM_MODE_UNCHANGED;
    //    }
    //}

    //public class UserDepartmentViewModel: UserDepartmentModel
    //{
    //    public UserDepartmentViewModel() { }

    //    public UserDepartmentViewModel(DMSEntities db, long id)
    //    {
    //        User_Department model = db.User_Department.Find(id);
    //        if (model == null) return;
    //        BaseProgram.CopyProperties(typeof(User_Department), model, typeof(UserDepartmentModel), this);
    //    }

    //    public UserDepartmentViewModel(User_Department model)
    //    {
    //        BaseProgram.CopyProperties(typeof(User_Department), model, typeof(UserDepartmentModel), this);
    //        mode = Constants.FORM_MODE_UNCHANGED;
    //    }
    //}

}
