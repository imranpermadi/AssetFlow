using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Models.Utilities
{
    public class SAPHelperModel
    {
        public string filter_order_type { get; set; }
        public string filter_transfer_type { get; set; }
        public string[] filter_sap_no { get; set; }
        public string[] filter_location { get; set; }
        public string filter_discharge_port { get; set; }
        public string[] filter_document_type { get; set; }
        public DateTime? filter_document_date_from { get; set; }
        public DateTime? filter_document_date_to { get; set; }
        public string[] filter_company { get; set; }
        public string[] filter_business_segment { get; set; }
        public string[] filter_plant { get; set; }
        public string[] filter_distribution { get; set; }
        public string[] filter_material { get; set; }
        public string[] filter_create_by { get; set; }
    }

    public class SAPFilterRequestModel
    {
        public string OrderType { get; set; }
        public string TransferType { get; set; }
        public string DocDateFrom { get; set; }
        public string DocDateTo { get; set; }
        public string[] Companies { get; set; }
        public string[] BusinessSegments { get; set; }
        public string[] SAPNo { get; set; }
        public string[] Materials { get; set; }
        public string[] Locations { get; set; }
        public string[] Plants { get; set; }
        public string DischargePort { get; set; }
        public string[] DocumentType { get; set; }
        public string[] Distribution { get; set; }
        public string[] CreateBy { get; set; }
        public string ReferenceNo { get; set; }
    }

    public class SAPModel
    {
        public bool is_checked { get; set; }
        public bool disabled { get; set; }

        public string order_type { get; set; }
        public string document_date { get; set; }
        public string delivery_order_no { get; set; }
        public string document_no { get; set; }
        public string po_no { get; set; }
        public string ln_so_no { get; set; }
        public string ln_sto_no { get; set; }
        public string customer_vendor { get; set; }
        public string incoterm { get; set; }
        public string estate { get; set; }
        public string plant { get; set; }
        public string tolerance { get; set; }
        public string distribution { get; set; }
        public string document_type { get; set; }
        public string company { get; set; }
        public string product_category { get; set; }
        public string purchase_organization { get; set; }
        public string sales_organization { get; set; }
        public string collective_ln { get; set; }
        public string loading { get; set; }
        public string destination { get; set; }
        public decimal? freight_cost { get; set; }

        public decimal? freight_total_cost { get; set; }
        public decimal? freight_cost_base { get; set; }
        public decimal? freight_cost_per { get; set; }
        public string freight_cost_uom { get; set; }
        public string truck_type { get; set; }

        public string transfer_type { get; set; }

        public string incoterm2 { get; set; }
        public string transporter { get; set; }
        public string ln_do_third_party { get; set; }

        public string mark_deleted { get; set; }
        public string valid_from { get; set; }
        public string valid_to { get; set; }

        public string item_no { get; set; }


        public List<SAPDetailModel> details { get; set; }

    }

    public class SAPDetailModel
    {
        public string document_item_no { get; set; }
        public string commodity { get; set; }
        public decimal qty { get; set; }
        public string uom { get; set; }
        public string so_do_no { get; set; }
        public string po_so_no { get; set; }
        public string so_interco { get; set; }
        public string so_interco_item { get; set; }
        public string po_interco { get; set; }
        public string reference_po { get; set; }
        public string reference_so { get; set; }
        public string address { get; set; }
        public string ln_so_item_no { get; set; }
        public string ln_sto_item_no { get; set; }

        public decimal? tolerance { get; set; }
        public decimal? freight_cost { get; set; }
        public decimal? base_freight_cost { get; set; }
        public decimal? base_freight_per { get; set; }
        public string base_freight_uom { get; set; }


        public decimal? sto_qty { get; set; }
        public string sto_uom { get; set; }
        public decimal? freight_cost_master { get; set; }
        public string freight_value { get; set; }

        public string sales_district { get; set; }

    }
}
