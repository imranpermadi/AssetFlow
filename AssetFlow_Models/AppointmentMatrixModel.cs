using FAMS_Models.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models
{
    public class AppointmentMatrixModel
    {
        public AppointmentMatrixModel() 
        {
            Schedules = new List<AppointmentMatrixScheduleModel>();
            ProductCategories = new List<AppointmentMatrixBusinessSegmentModel>();
        }

        public string mode { get; set; }
        public long Id { get; set; }
        public Nullable<long> Company_Id { get; set; }
        public Nullable<long> Business_Segment_Id { get; set; }
        public string Product_Category { get; set; }
        public string Warehouse_Code { get; set; }
        public System.DateTime Valid_From { get; set; }
        public System.DateTime Valid_To { get; set; }
        public System.DateTime Created_Date { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Edited_Date { get; set; }
        public string Edited_By { get; set; }
        public bool Is_Deleted { get; set; }
        public Nullable<System.DateTime> Deleted_Date { get; set; }
        public string Deleted_By { get; set; }

        #region CustomAttributes

        public List<AppointmentMatrixScheduleModel> Schedules { get; set; }
        public List<AppointmentMatrixBusinessSegmentModel> ProductCategories { get; set; }

        public string Warehouse_Name { get; set; }
        public string Business_Segment_Name { get; set; }



        #endregion
    }

    public class AppointmentMatrixScheduleModel
    {
        public AppointmentMatrixScheduleModel() { }
        public string mode { get; set; }
        public long Id { get; set; }
        public long Appointment_Matrix_Id { get; set; }
        public string Slot_Name { get; set; }
        public System.DateTime Time_From { get; set; }
        public System.DateTime Time_To { get; set; }
        public int Max_Slot { get; set; }
        public System.DateTime Created_Date { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Edited_Date { get; set; }
        public string Edited_By { get; set; }
        public bool Is_Deleted { get; set; }
        public Nullable<System.DateTime> Deleted_Date { get; set; }
        public string Deleted_By { get; set; }

        #region CustomAttributes
        public int Remaining_Slot { get; set; }
        public bool disabled { get; set; }
        #endregion
    }

    public class AppointmentMatrixBusinessSegmentModel
    {
        public AppointmentMatrixBusinessSegmentModel() { }

        public string mode { get; set; }
        public long Id { get; set; }
        public long Appointment_Matrix_Id { get; set; }
        public long Business_Segment_Id { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Edited_Date { get; set; }
        public string Edited_By { get; set; }
        public bool Is_Deleted { get; set; }
        public Nullable<System.DateTime> Deleted_Date { get; set; }
        public string Deleted_By { get; set; }

    }
}
