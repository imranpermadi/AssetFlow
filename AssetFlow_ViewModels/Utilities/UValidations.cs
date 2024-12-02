using FAMS_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_ViewModels.Utilities
{
    public class UValidations
    {
        /// <summary>
        /// Check if SAP Order can be edited.
        /// SAP Order is not editable if it has one or more Order(s) assigned to transporter(s)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sapOrder"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        //public static bool CanEditSAPOrder(DMSEntities db, SAP_Order sapOrder, out string message)
        //{
        //    var result = false;
        //    message = "";

        //    try
        //    {
        //        var orders = db.Orders.Where(r => r.SAP_No == sapOrder.Document_No && !r.Is_Deleted).FirstOrDefault();
        //        if (orders != null)
        //            throw new Exception("");

        //        //if (sapOrder.Mark_Complete)
        //        //{

        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        message = ex.Message;
        //    }

        //    return result;
        //}
    }
}
