using FAMS_Models.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
    
    public class LoginResultModel
    {
        public LoginResultModel() { }

        public long Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool UseAD { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Contact_Person { get; set; }
        public string Phone { get; set; }
        public string fcmtoken { get; set; }
        public bool IsBlocked { get; set; }

        public List<MenuModel> Menus { get; set; }

    }

    public class ChangePasswordModel
    {
        public string username { get; set; }
        public string currentPass { get; set; }
        public string newPass { get; set; }
        [DefaultValue(false)]
        public bool changeFirstLogin { get; set; }
    }

    public class WebLoginResultModel
    {
        public DateTime token_expiry { get; set; }
        public UserData user_data { get; set; } 
    }
}
