using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models
{
    public class ResultData
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string message_code { get; set; }
        public object data { get; set; }

        public int totalRecords { get; set; }
    }

    public class ResultByteArrayData: ResultData
    {
        public byte[] data_byte { get; set; }
    }
}
