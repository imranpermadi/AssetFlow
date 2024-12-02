using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models.MobileModels
{
    public class MOrderModel
    {
        public string result { get; set; }
        public string mode { get; set; }

        public long Id { get; set; }
        public string Company_Name { get; set; }

        public string Business_Segment_Name { get; set; }

        public string SAP_No { get; set; }
        public string Order_No { get; set; }
        public DateTime Order_Date { get; set; }

        public long Order_Type_Id { get; set; }

        public string Order_Type_Name { get; set; }

        public string Transporter_Code { get; set; }

        public string Shipment_No { get; set; }

        public string Shipment_Type { get; set; }

        public string Shipment_Type_Name { get; set; }
        public string Shipment_Type_Category { get; set; }

        public long? Loading_Destination_From_Id { get; set; }
        public string Loading_Destination_From_Name { get; set; }
        public long? Loading_Destination_To_Id { get; set; }
        public string Loading_Destination_To_Name { get; set; }

        public long? Truck_Type_Id { get; set; }
        public string Truck_Type_Name { get; set; }
        public string Warehouse_Code { get; set; }
        public string Warehouse_Name { get; set; }

        public string WB_Trans_Type { get; set; }

        public string WB_Loading_Type { get; set; }

        public string Warehouse_Location { get; set; }
        public string Warehouse_Location_Name { get; set; }

        public long Warehouse_Location_Id { get; set; }
        public string Transporter_Notes { get; set; }
        public string Estate { get; set; }
        public string Delivery_Address { get; set; }

        public DateTime? Stuffing_Date_From { get; set; }

        public DateTime? Stuffing_Date_To { get; set; }

        public string Incoterm2_Name { get; set; }

        public string Dispatch_Status { get; set; }
        public DateTime? Dispatch_Status_Date { get; set; }
        public string Dispatch_Status_Remarks { get; set; }

        public bool? Mark_Complete { get; set; }

        public string Transfer_Type { get; set; }

        public string Transporter_Name { get; set; }
        public string Customer_Vendor_Name { get; set; }

        public string Customer_Vendor_Address { get; set; }

        public string Remarks { get; set; }

        public string Collective_LN { get; set; }

        public string Status { get; set; }

        public List<MOrderDetailModel> Details { get; set; }
        public string Company_Code { get; set; }

    
        #region CustomFlag
       
        public bool canAssignDriver { get; set; }

        #endregion



    }


    public class MOrderDetailModel
    {
        public string result { get; set; }
        public string mode { get; set; }
        public long Id { get; set; }
        public long Order_Id { get; set; }
        public int Item_No { get; set; }
        public string SAP_Item_No { get; set; }

        public string Material_Code { get; set; }
        public string Material_Name { get; set; }

        public Decimal Qty { get; set; }
        public Decimal Initial_Qty { get; set; }
        public Decimal? Real_Qty { get; set; }
        public Decimal? Shipment_Qty { get; set; }
        public Decimal? OS_Qty { get; set; }

        public bool Has_Shipment { get; set; }
        public bool Has_Ongoing { get; set; }

        public string UOM { get; set; }

        public string Company_Code { get; set; }
        public string Order_No { get; set; }
        public string SAP_No { get; set; }
    }



}
