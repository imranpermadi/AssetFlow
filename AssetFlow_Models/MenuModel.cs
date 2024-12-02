using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models
{
    public class MenuModel
    {
        public long id { get; set; }
        public string label { get; set; }
        public string icon { get; set; }
        public string to { get; set; }
        public string badge { get; set; }

        public dynamic query { get; set; }
        public string alias { get; set; }
    }

    public class MenuItemModel : MenuModel
    {
        public List<object> items { get; set; }
    }
}
