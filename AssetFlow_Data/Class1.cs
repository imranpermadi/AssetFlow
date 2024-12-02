using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Data
{
    public partial class DataEntities: FAMSEntities
    {



		public override int SaveChanges()
		{
			var a = "";

			return base.SaveChanges();
		}


    }
}
