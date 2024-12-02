using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models.Utilities
{
    public class UserData
    {
        public UserData() { }

        public long Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string IsAdmin { get; set; }
        public string UseAD { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string Language { get; set; }
        public List<object> Menus { get; set; }
        public string LocationCode { get; set; }
        public string SERVER { get; set; }

        public string GetDisplayName()
        {
            return "[" + this.Username + "] " + this.Fullname;
        }
    }
}
