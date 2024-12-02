using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models.Utilities
{
    public class UException : Exception
    {
        public UException()
        {
        }

        public UException(string message)
            : base(message)
        {
        }

        public UException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
