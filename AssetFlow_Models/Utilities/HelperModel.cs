using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models.Utilities
{
    public class AssignModel
    {
        public AssignModel()
        {
            transporters = new List<AssignTransporterModel>();
        }

        public List<long> id { get; set; }
        public DateTime[] stuffing_date { get; set; }
        public string shipment_type { get; set; }
        public string charge_oa_company { get; set; }
        public long loading_destination_from_id { get; set; }
        public long loading_destination_to_id { get; set; }
        public string transporter_notes { get; set; }
        public DateTime? freight_date { get; set; }

        public List<AssignTransporterModel> transporters { get; set; }

    }


    public class AssignTransporterModel
    {
        public string transporter_code { get; set; }
        public decimal freight_cost { get; set; }
        public long truck_type_id { get; set; }
        public decimal? freight_per { get; set; }
        public string freight_uom { get; set; }
        public bool freight_value { get; set; }
        public decimal? freight_suggested { get; set; }
        public string freight_criteria { get; set; }
        public string assign_reason { get; set; }
        public string assign_reason_others { get; set; }
        public int freight_container { get; set; }
        public decimal? freight_total_cost { get; set; }
        public string transporter_notes { get; set; }

        public decimal? freight_kg { get; set; }

        public List<AssignTransporterDetailModel> freight_material { get; set; }

        public int? system_not { get; set; }
        public int? user_not { get; set; }
        public decimal? freight_total_cost_user_not { get; set; }
        public decimal? truck_capacity { get; set; }


        #region Vue Purpose

        public string transporter_name { get; set; }

        public bool? require_reason { get; set; }
        public bool? require_reason_others { get; set; }

        public string exceed_message { get; set; }

        public string freight_master { get; set; }
        #endregion
    }

    public class AssignTransporterDetailModel
    {
        public long Id { get; set; }
        public string Item_No { get; set; }
        public string Material_Code { get; set; }
        public string Material_Name { get; set; }
        public decimal Qty { get; set; }
        public string UOM { get; set; }

        public decimal Freight_Master { get; set; }
        public decimal Freight_Cost { get; set; }
        public decimal Freight_Total_Cost { get; set; }
        public string Freight_Per { get; set; }
        public string Freight_UOM { get; set; }

        public decimal Qty_KG { get; set; }
        public decimal Freight_Total_Cost_User_NoT { get; set; }
        public decimal Freight_Master_M { get; set; }
        public decimal Freight_Master_Per { get; set; }
        public int System_NoT{ get; set; }
        public int User_NoT { get; set; }
       
    }

    public class TransporterSelectModel
    {
        public string key { get; set; }
        public string transporter_code { get; set; }
        public string transporter_name { get; set; }
        public long truck_type_id { get; set; }
        public string truck_type_name { get; set; }
        public string remarks_oa { get; set; }
        public decimal? per { get; set; }
        public string numtrip { get; set; }
        public string uom { get; set; }
        public bool is_value { get; set; }
        public string remarks { get; set; }
        public string master_cost { get; set; }
        public decimal cost { get; set; }
        public decimal total_cost { get; set; }
        public decimal total_kg { get; set; }
        public decimal total_based { get; set; }
        public decimal recommended { get; set; }

        public decimal total_cost_user_not { get; set; }
        public decimal total_kg_user { get; set; }
        public int system_not { get; set; }
        public int user_not { get; set; }
        public decimal truck_capacity { get; set; }
        public decimal best_price_uom { get; set; }
        public decimal recommended_kg { get; set; }
        public decimal recommended_total { get; set; }
        public decimal total_different { get; set; }
        public decimal different_per_uom { get; set; }
        public decimal different_percentage { get; set; }

        public List<AssignTransporterDetailModel> materials { get; set; }
    }

    public class AssignDriverHeaderModel
    {
        public long order_id { get; set; }
        public List<AssignDriverModel> drivers { get; set; }
    }

    public class AddToShipmentModel
    {
        public long order_id { get; set; }
        public string shipment_no { get; set; }
        public List<AssignDriverMaterialModel> materials { get; set; }
    }

    public class MergeHeaderModel
    {
        public long[] order_ids { get; set; }
        public List<AssignDriverModel> drivers { get; set; }
    }

    public class AssignDriverModel
    {
        public string mode { get; set; }
        public long? Driver_Id { get; set; }
        public string Driver_Name { get; set; }
        public string License_No { get; set; }
        public long? Vehicle_Id { get; set; }
        public string Vehicle_Name { get; set; }

        public List<AssignDriverMaterialModel> materials { get; set; }
    }

    public class AssignDriverMaterialModel
    {
        public long Id { get; set; }
        public string Item_No { get; set; }
        public string Material_Code { get; set; }
        public string Material_Name { get; set; }
        public decimal Qty { get; set; }
        public string UOM { get; set; }

    }


    /// <summary>
    /// Used to retrieve list of transporters when assigning transporters
    /// </summary>
    public class TransporterFreightModel
    {
        public long loading { get; set; }
        public long destination { get; set; }
        public string[] exclude_transporters { get; set; }
        public List<AssignTransporterDetailModel> materials { get; set; }
        public int container { get; set; }
        public string charge_oa_company { get; set; }

        //NEW ADDITION
        public string warehouse_code { get; set; }
        public string document_type { get; set; }

        public DateTime freight_date { get; set; }

        public long[] sap_order_ids { get; set; }


        //ONLY USED IN REFRESH FREIGHT COST
        public List<AssignTransporterModel> selected_transporters { get; set; }

    }

    public class BackendModel
    {
        public UserData user { get; set; }
        public string data { get; set; }
    }


    public class RequestSlotModel
    {
        public DateTime appointment_date { get; set; }
        public string warehouse_code { get; set; }
        public long business_segment_id { get; set; }
    }

    public class AppointmentHeaderModel
    {
        public long shipment_id { get; set; }
        public string warehouse_code { get; set; }
        public DateTime? appointment_date { get; set; }
        public long business_segment_id { get; set; }
        public long appointment_matrix_schedule_id { get; set; }
    }


    public class MergeModel
    {
        public long[] order_ids { get; set; }
        public List<AssignDriverModel> drivers { get; set; }
    }

    public class ApprovalHelperModel
    {
        public ApprovalActionHelperModel[] data { get; set; }
        public string status { get; set; }

    }

    public class ApprovalActionHelperModel
    {
        public long id { get; set; }
        public string remarks { get; set; }

        public string ref_no { get; set; }
        public bool success { get; set; }
        public string message { get; set; }

    }

    public class TransporterActionModel
    {
        public long id { get; set; }
        public string status { get; set; }
        public string reason { get; set; }

        public string ref_no { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class MergeSAPSimulationModel
    {
        public List<String> sap_no { get; set; }
        public string order_type { get; set; }
        public string warehouse_code { get; set; }
        public string document_type { get; set; }
        public string shipment_type { get; set; }

        public string transporter_code { get; set; }

        public bool is_first { get; set; }
    }

    public class EditOrderMergeSimulationModel: MergeSAPSimulationModel
    {
        public long order_id { get; set; }
    }


    public class ChangeStuffingDateModel
    {
        public long order_id { get;set; }
        public DateTime stuffing_date_from { get; set; }
        public DateTime stuffing_date_to { get; set; }
    }

    public class ChangeLanguageModel
    {
        public string language { get; set; }
    }

}
