using FAMS_Models;
using FAMS_Models.Resources;
using FAMS_Models.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FAMS_API.Utilities
{
    public class UReport
    {
        static string SA_tdHeaderR = @"border: 1px solid #000000; padding: 5px; text-align:right; white-space: nowrap;";
        static string SA_tr = "border: 1px solid #000000; padding: 5px; background: #FFFF0F0; white-space: nowrap;";
        static string SA_trFooter = "border: 1px solid #000000; padding: 5px; background: #F0F0F0; white-space: nowrap;";
        static string SA_tdC = @"border: 1px solid #000000; padding: 5px; text-align:center; white-space: nowrap;";
        static string SA_tdL = @"border: 1px solid #000000; padding: 5px; text-align:left; white-space: nowrap;";
        static string SA_tdR = @"border: 1px solid #000000; padding: 5px; text-align:right; white-space: nowrap;";
        static string borderStyle = "border: 1px solid black;border-width:thin;padding: 3px;white-space:nowrap!important";
        static string multilineStyle = "border: 1px solid black;border-width:thin;padding: 3px;white-space:pre-line!important";
        static string headerStyle = "border: 1px solid black;border-width:thin;padding: 3px;text-align:center; vertical-align:middle;";
        static string footerStyle = "border: 1px solid black;border-width:thin;padding: 3px;";

        public static void printOrderReport(DataTable dt, string docDate, string reportName, bool isRelation, out string strHead, out string strJSON, out string strFoot)
        {
            StringBuilder sReports = new StringBuilder();
            StringBuilder tHead = new StringBuilder();
            StringBuilder tFoot = new StringBuilder();
            StringBuilder sb = new StringBuilder();

            strFoot = "";
            strHead = $@"<thead class='header'>
                            <tr>
                                <th style='{headerStyle}'>No</th>
                                <th style='{headerStyle}'>{DisplayNames.ORDER_TYPE}</th>
                                <th style='{headerStyle}'>{DisplayNames.COMPANY}</th>
                                <th style='{headerStyle}'>{DisplayNames.DOC_DATE}</th>
                                <th style='{headerStyle}'>{DisplayNames.SAP_NO}</th>
                                <th style='{headerStyle}'>{DisplayNames.ORDER_NO}</th>
                                <th style='{headerStyle}'>{DisplayNames.ITEM_NO}</th>
                                <th style='{headerStyle}'>{DisplayNames.BUSINESS_SEGMENT}</th>
                                <th style='{headerStyle}'>{DisplayNames.CUSTOMER_VENDOR}</th>
                                <th style='{headerStyle}'>{DisplayNames.DELIVERY_ADDRESS}</th>
                                <th style='{headerStyle}'>{DisplayNames.STATUS}</th>
                                <th style='{headerStyle}'>{DisplayNames.TRANSPORTER_ACCEPT_STATUS}</th>
                                <th style='{headerStyle}'>{DisplayNames.TRANSPORTER}</th>
                                <th style='{headerStyle}'>{DisplayNames.TRUCK_TYPE}</th>
                                <th style='{headerStyle}'>{DisplayNames.LOADING}</th>
                                <th style='{headerStyle}'>{DisplayNames.DESTINATION}</th>
                                <th style='{headerStyle}'>{DisplayNames.SHIPMENT_TYPE}</th>
                                <th style='{headerStyle}'>{DisplayNames.MATERIAL}</th>
                                <th style='{headerStyle}'>{DisplayNames.MATERIAL_NAME}</th>
                                <th style='{headerStyle}'>{DisplayNames.QTY}</th>
                                <th style='{headerStyle}'>{DisplayNames.OUTSTANDING_QTY}</th>
                                <th style='{headerStyle}'>{DisplayNames.REAL_QTY}</th>
                                <th style='{headerStyle}'>{DisplayNames.UOM}</th>
                                <th style='{headerStyle}'>{DisplayNames.WAREHOUSE}</th>
                                <th style='{headerStyle}'>{DisplayNames.DISTRIBUTION}</th>
                                <th style='{headerStyle}'>{DisplayNames.DOC_TYPE}</th>
                                <th style='{headerStyle}'>{DisplayNames.INCOTERM}</th>
                                <th style='{headerStyle}'>{DisplayNames.INCOTERM2}</th>
                                <th style='{headerStyle}'>{DisplayNames.CREATED_DATE}</th>
                                <th style='{headerStyle}'>{DisplayNames.CREATED_BY}</th>
                                <th style='{headerStyle}'>{DisplayNames.EDITED_DATE}</th>
                                <th style='{headerStyle}'>{DisplayNames.EDITED_BY}</th>
                            </tr>
                        </thead>";

            if (isRelation)
            {
                strHead = $@"<thead class='header'>
                            <tr>
                                <th style='{headerStyle}'>No</th>
                                <th style='{headerStyle}'>{DisplayNames.ORDER_TYPE}</th>
                                <th style='{headerStyle}'>{DisplayNames.COMPANY}</th>
                                <th style='{headerStyle}'>{DisplayNames.DOC_DATE}</th>
                                <th style='{headerStyle}'>{DisplayNames.SAP_NO}</th>
                                <th style='{headerStyle}'>{DisplayNames.ORDER_NO}</th>
                                <th style='{headerStyle}'>{DisplayNames.ITEM_NO}</th>
                                <th style='{headerStyle}'>{DisplayNames.BUSINESS_SEGMENT}</th>
                                <th style='{headerStyle}'>{DisplayNames.CUSTOMER_VENDOR}</th>
                                <th style='{headerStyle}'>{DisplayNames.DELIVERY_ADDRESS}</th>
                                <th style='{headerStyle}'>{DisplayNames.LOADING}</th>
                                <th style='{headerStyle}'>{DisplayNames.DESTINATION}</th>
                                <th style='{headerStyle}'>{DisplayNames.SHIPMENT_TYPE}</th>
                                <th style='{headerStyle}'>{DisplayNames.MATERIAL}</th>
                                <th style='{headerStyle}'>{DisplayNames.MATERIAL_NAME}</th>
                                <th style='{headerStyle}'>{DisplayNames.QTY}</th>
                                <th style='{headerStyle}'>{DisplayNames.OUTSTANDING_QTY}</th>
                                <th style='{headerStyle}'>{DisplayNames.REAL_QTY}</th>
                                <th style='{headerStyle}'>{DisplayNames.UOM}</th>
                                <th style='{headerStyle}'>{DisplayNames.WAREHOUSE}</th>
                                <th style='{headerStyle}'>{DisplayNames.DISTRIBUTION}</th>
                                <th style='{headerStyle}'>{DisplayNames.DOC_TYPE}</th>
                                <th style='{headerStyle}'>{DisplayNames.INCOTERM}</th>
                                <th style='{headerStyle}'>{DisplayNames.INCOTERM2}</th>
                                <th style='{headerStyle}'>{DisplayNames.CREATED_DATE}</th>
                                <th style='{headerStyle}'>{DisplayNames.CREATED_BY}</th>
                                <th style='{headerStyle}'>{DisplayNames.EDITED_DATE}</th>
                                <th style='{headerStyle}'>{DisplayNames.EDITED_BY}</th>
                            </tr>
                        </thead>";
            }


            int columnCount = Regex.Matches(strHead, "</th>").Count;
            /*var strLabel = printHeaderLabel(reportName, docDate);
            strHead = strHead.Replace("$labelRow", strLabel);*/

            List<object> allrows = new List<object>();
            List<string> childrow;
            List<int> colAlignRight = new List<int>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                string style = borderStyle;
                string mStyle = borderStyle + ";mso-number-format:\"\\@\"";
                string numberStyle = borderStyle + ";mso-number-format:#\\,##0";

                sb.Append("<tr>");
                for (int z = 0; z < dt.Columns.Count; z++)
                {
                    var aFields = dt.Columns;

                    string sLine = row[aFields[z]].ToString();
                    Type type = row[aFields[z]].GetType();
                    string headerStyle = style;
                    string data = "";

                    if ((type == typeof(Double)) || (type == typeof(float)) || (type == typeof(Decimal)))
                    {
                        /*if (!aFields[z].ColumnName.Contains("Qty"))
                            data = string.Format("{0:N0}", Convert.ToDecimal(row[aFields[z]]));
                        else
                            data = string.Format("{0:G29}", Convert.ToDecimal(row[aFields[z]]));*/
                        data = string.Format("{0:N0}", Convert.ToDecimal(row[aFields[z]]));
                        headerStyle = numberStyle;
                    }
                    else if (type == typeof(int))
                    {
                        data = string.Format("{0:N0}", (int)row[aFields[z]]);
                        headerStyle = numberStyle;
                    }
                    else if (type == typeof(DateTime))
                    {
                        if (!string.IsNullOrEmpty(row[aFields[z]].ToString()))
                        {
                            data = ((DateTime)row[aFields[z]]).ToString("dd/MM/yyyy");
                        }
                    }
                    else
                    {
                        data = row[aFields[z]].ToString();

                        if (aFields[z].ColumnName == "Material_Code" || aFields[z].ColumnName == "SAP_No")
                        {
                            //data = row[aFields[z]].ToString().Replace("\n", "<br/>");
                            headerStyle = mStyle;
                        }

                        if (aFields[z].ColumnName == "Row_Num")
                        {
                            data = (i + 1).ToString();
                        }
                    }

                    sb.Append($"<td style='{headerStyle}'>" + data + "</td>");
                }
                sb.Append("</tr>");
            }
            //string JSONString = string.Empty;
            //JSONString = JsonConvert.SerializeObject(allrows);
            //strJSON = JSONString;

            strJSON = sb.ToString();
        }

        public static void printFreightCostReport(DataTable dt, string docDate, string reportName, string showMerged, out string strHead, out string strJSON, out string strFoot)
        {
            StringBuilder sReports = new StringBuilder();
            StringBuilder tHead = new StringBuilder();
            StringBuilder tFoot = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            
            strFoot = "";
            strHead = $@"<thead class='header'>
                            <tr>
                                <th style='{headerStyle}'>No</th>
                                <th style='{headerStyle}'>{DisplayNames.ORDER_TYPE}</th>
                                <th style='{headerStyle}'>{DisplayNames.COMPANY}</th>
                                <th style='{headerStyle}'>{DisplayNames.DOC_DATE}</th>
                                <th style='{headerStyle}'>{DisplayNames.SAP_NO}</th>
                                <th style='{headerStyle}'>{DisplayNames.TRANSPORTER}</th>
                                <th style='{headerStyle}'>{DisplayNames.LOADING}</th>
                                <th style='{headerStyle}'>{DisplayNames.DESTINATION}</th>
                                <th style='{headerStyle}'>{DisplayNames.TRUCK_TYPE}</th>
                                <th style='{headerStyle}'>{DisplayNames.CURRENCY}</th>
                                <th style='{headerStyle}'>{DisplayNames.FREIGHT_MASTER}</th>
                                <th style='{headerStyle}'>{DisplayNames.FREIGHT_COST}</th>
                                <th style='{headerStyle}'>{DisplayNames.NUM_TRIP}</th>
                                <th style='{headerStyle}'>{DisplayNames.UOM}</th>
                                <th style='{headerStyle}'>{DisplayNames.FREIGHT_TOTAL_COST}</th>
                                <th style='{headerStyle}'>{DisplayNames.BUSINESS_SEGMENT}</th>
                                <th style='{headerStyle}'>{DisplayNames.ORDER_NO}</th>
                                <th style='{headerStyle}'>{DisplayNames.ITEM_NO}</th>
                                <th style='{headerStyle}'>{DisplayNames.MATERIAL}</th>
                                <th style='{headerStyle}'>{DisplayNames.MATERIAL_NAME}</th>
                                <th style='{headerStyle}'>{DisplayNames.QTY}</th>
                                <th style='{headerStyle}'>{DisplayNames.UOM}</th>
                                <th style='{headerStyle}'>{DisplayNames.WAREHOUSE}</th>
                                <th style='{headerStyle}'>{DisplayNames.DISTRIBUTION}</th>
                                <th style='{headerStyle}'>{DisplayNames.DOC_TYPE}</th>
                                <th style='{headerStyle}'>SAP Freight Cost</th>
                                <th style='{headerStyle}'>Status Release</th>
                                <th style='{headerStyle}'>{DisplayNames.SHIPMENT_TYPE}</th>
                                <th style='{headerStyle}'>{DisplayNames.CREATED_DATE}</th>
                                <th style='{headerStyle}'>{DisplayNames.CREATED_BY}</th>
                            </tr>
                        </thead>";

            int columnCount = Regex.Matches(strHead, "</th>").Count;
            /*var strLabel = printHeaderLabel(reportName, docDate);
            strHead = strHead.Replace("$labelRow", strLabel);*/

            List<object> allrows = new List<object>();
            List<string> childrow;
            List<int> colAlignRight = new List<int>();

            List<String> excludeRender = new List<string>()
            {
                "IDX", "IDX1"
            };

            List<String> excludeMerge = new List<string>()
            {
                "Business_Segment_Name", "Order_No", "Item_No", "Material_Code", "Material_Description", "Qty", "UOM"
            };

            sb.Append("<tbody>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                string style = borderStyle;
                string mStyle = borderStyle + ";mso-number-format:\"\\@\"";

                string rowNum = row["IDX"].ToString();

                sb.Append("<tr>");
                for (int z = 0; z < dt.Columns.Count; z++)
                {
                    var aFields = dt.Columns;

                    if (excludeRender.Contains(aFields[z].ColumnName))
                        continue;

                    string sLine = row[aFields[z]].ToString();
                    Type type = row[aFields[z]].GetType();
                    string headerStyle = style;
                    string data = "";

                    if ((type == typeof(Double)) || (type == typeof(float)) || (type == typeof(Decimal)))
                    {
                        /*if (!aFields[z].ColumnName.Contains("Qty"))
                            data = string.Format("{0:N0}", Convert.ToDecimal(row[aFields[z]]));
                        else
                            data = string.Format("{0:G29}", Convert.ToDecimal(row[aFields[z]]));*/
                        data = string.Format("{0:N0}", Convert.ToDecimal(row[aFields[z]]));
                        headerStyle += ";mso-number-format:#\\,##0";
                    }
                    else if (type == typeof(int))
                    {
                        data = string.Format("{0:N0}", (int)row[aFields[z]]);
                        headerStyle += ";mso-number-format:#\\,##0";
                    }
                    else if (type == typeof(DateTime))
                    {
                        if (!string.IsNullOrEmpty(row[aFields[z]].ToString()))
                        {
                            data = ((DateTime)row[aFields[z]]).ToString("dd/MM/yyyy");
                        }
                    }
                    else if (type == typeof(Boolean))
                    {
                        var d = (Boolean)row[aFields[z]];

                        data = d ? "Y" : "N";
                    }
                    else if (type == typeof(Boolean?))
                    {
                        var d = (Boolean?)row[aFields[z]];

                        if (d.HasValue) data = d.Value ? "Y" : "N";
                        else data = "N";
                    }
                    else
                    {

                        if (aFields[z].ColumnName == "Material_Code" || aFields[z].ColumnName == "SAP_No")
                        {
                            headerStyle = mStyle;
                        }
                    

                        data = row[aFields[z]].ToString();

                        if (aFields[z].ColumnName == "Row_Num")
                        {
                            data = (i + 1).ToString();
                        }

                    }

                    var mergeString = "";
                    if (showMerged == "Y")
                    {
                        if (rowNum != "1")
                        {
                            if (!excludeMerge.Contains(aFields[z].ColumnName))
                            {
                                continue;
                            }
                            else
                            {
                                mergeString = "";
                            }
                        }
                        else
                        {
                            if (!excludeMerge.Contains(aFields[z].ColumnName))
                            {
                                mergeString = $"rowspan=\"{row["IDX1"].ToString()}\"";
                            }


                        }
                    }
                    else mergeString = "";


                    sb.Append($"<td style='{headerStyle}' {mergeString}>" + data + "</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</tbody>");
            //string JSONString = string.Empty;
            //JSONString = JsonConvert.SerializeObject(allrows);
            //strJSON = JSONString;

            strJSON = sb.ToString();
        }

        public static void printShipmentReport(DataTable dt, string docDate, string showMerged, string reportName, out string strHead, out string strJSON, out string strFoot)
        {
            StringBuilder sReports = new StringBuilder();
            StringBuilder tHead = new StringBuilder();
            StringBuilder tFoot = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            strJSON = "<tbody>@body</tbody>";
            strFoot = "<tfoot>@foot</tfoot>";

            strHead = $@"<thead class='header'>
                            <tr>
                                <th style='{headerStyle}'>No</th>
                                <th style='{headerStyle}'>{DisplayNames.ORDER_TYPE}</th>
                                <th style='{headerStyle}'>{DisplayNames.COMPANY}</th>
                                <th style='{headerStyle}'>{DisplayNames.DOC_DATE}</th>
                                <th style='{headerStyle}'>{DisplayNames.SHIPMENT_NO}</th>
                                <th style='{headerStyle}'>{DisplayNames.SAP_NO}</th>
                                <th style='{headerStyle}'>{DisplayNames.LOADING}</th>
                                <th style='{headerStyle}'>{DisplayNames.DESTINATION}</th>
                                <th style='{headerStyle}'>{DisplayNames.TRANSPORTER}</th>
                                <th style='{headerStyle}'>{DisplayNames.DRIVER}</th>
                                <th style='{headerStyle}'>{DisplayNames.VEHICLE}</th>
                                <th style='{headerStyle}'>{DisplayNames.TRUCK_TYPE}</th>
                                <th style='{headerStyle}'>{DisplayNames.STATUS}</th>
                                <th style='{headerStyle}'>{DisplayNames.PROGRESS_STATUS}</th>
                                <th style='{headerStyle}'>Tgl Masuk Pabrik</th>
                                <th style='{headerStyle}'>Tgl Keluar Pabrik</th>
                                
                                <th style='{headerStyle}'>{DisplayNames.BUSINESS_SEGMENT}</th>
                                <th style='{headerStyle}'>{DisplayNames.MATERIAL}</th>
                                <th style='{headerStyle}'>{DisplayNames.MATERIAL_NAME}</th>
                                <th style='{headerStyle}'>{DisplayNames.QTY}</th>
                                <th style='{headerStyle}'>{DisplayNames.REAL_QTY}</th>
                                <th style='{headerStyle}'>EPOD Quantity</th>
                                <th style='{headerStyle}'>{DisplayNames.UOM}</th>
                                <th style='{headerStyle}'>{DisplayNames.STUFFING_DATE}</th>
                                <th style='{headerStyle}'>{DisplayNames.APPOINTMENT_STATUS}</th>
                                <th style='{headerStyle}'>{DisplayNames.APPOINTMENT_DATE}</th>
                                <th style='{headerStyle}'>{DisplayNames.APPOINTMENT_TIME}</th>
                                <th style='{headerStyle}'>{DisplayNames.APPOINTMENT_APPROVAL_DATE}</th>
                                <th style='{headerStyle}'>{DisplayNames.DELIVERY_ADDRESS}</th>
                                <th style='{headerStyle}'>{DisplayNames.SHIPMENT_TYPE}</th>
                                <th style='{headerStyle}'>{DisplayNames.INCOTERM}</th>
                                <th style='{headerStyle}'>{DisplayNames.WAREHOUSE}</th>
                                <th style='{headerStyle}'>{DisplayNames.CUSTOMER_VENDOR}</th>
                                <th style='{headerStyle}'>{DisplayNames.DISTRIBUTION}</th>
                                
                                <th style='{headerStyle}'>{DisplayNames.DOC_TYPE}</th>
                                <th style='{headerStyle}'>{DisplayNames.ORDER_NO}</th>
                                <th style='{headerStyle}'>{DisplayNames.CREATED_DATE}</th>
                                <th style='{headerStyle}'>{DisplayNames.CREATED_BY}</th>
                                <th style='{headerStyle}'>{DisplayNames.EDITED_DATE}</th>
                                <th style='{headerStyle}'>{DisplayNames.EDITED_BY}</th>
                            </tr>
                        </thead>";

            int columnCount = Regex.Matches(strHead, "</th>").Count;
            /*var strLabel = printHeaderLabel(reportName, docDate);
            strHead = strHead.Replace("$labelRow", strLabel);*/

            List<object> allrows = new List<object>();
            List<string> childrow;
            List<int> colAlignRight = new List<int>();

            List<String> excludeRender = new List<string>()
            {
                "IDX", "IDX1", "Shipment_Type_Category"
            };

            List<String> renderCollective = new List<string>()
            {
                "Driver_Name", "Vehicle_No", "Truck_Type_Name"
            };

            List<string> renderIndividual = new List<string>()
            {
                "SAP_No", "Order_No", "Business_Segment_Name", "Material_Code", "Material_Description", "Shipment_Qty", "Real_Qty", "UOM"
            };

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                string style = borderStyle;
                string mStyle = borderStyle + ";mso-number-format:\"\\@\"";

                string rowNum = row["IDX"].ToString();
                string shipmentTypeCategory = row["Shipment_Type_Category"].ToString();


                sb.Append("<tr>");
                for (int z = 0; z < dt.Columns.Count; z++)
                {
                    var aFields = dt.Columns;
                    string sLine = row[aFields[z]].ToString();
                    Type type = row[aFields[z]].GetType();
                    string headerStyle = style;
                    string data = "";

                    if (excludeRender.Contains(aFields[z].ColumnName))
                        continue;

                    if ((type == typeof(Double)) || (type == typeof(float)) || (type == typeof(Decimal)))
                    {
                        //data = string.Format("{0:G29}", Convert.ToDecimal(row[aFields[z]]));
                        data = string.Format("{0:N0}", Convert.ToDecimal(row[aFields[z]]));
                        headerStyle += ";mso-number-format:#\\,##0";
                    }
                    else if (type == typeof(int))
                    {
                        data = string.Format("{0:N0}", (int)row[aFields[z]]);
                        headerStyle += ";mso-number-format:#\\,##0";
                    }
                    else if (type == typeof(DateTime))
                    {
                        if (!string.IsNullOrEmpty(row[aFields[z]].ToString()))
                        {
                            if (aFields[z].ColumnName == "Registration_In_Date" || aFields[z].ColumnName == "Registration_Out_Date")
                            {
                                data = ((DateTime)row[aFields[z]]).ToString("dd/MM/yyyy HH:mm:ss");
                            }
                            else
                                data = ((DateTime)row[aFields[z]]).ToString("dd/MM/yyyy");
                        }
                    }
                    else
                    {
                        if (aFields[z].ColumnName == "Material_Code" || aFields[z].ColumnName == "SAP_No")
                        {
                            headerStyle = mStyle;
                        }

                        data = row[aFields[z]].ToString();

                        if (aFields[z].ColumnName == "Row_Num")
                        {
                            data = (i + 1).ToString();
                        }
                    }

                    var mergeString = "";

                    //if (showMerged == "Y")
                    //{
                    //    if (
                    //      (shipmentTypeCategory == Constants.ShipmentTypeCategory.INDIVIDUAL && renderIndividual.Contains(aFields[z].ColumnName)) ||
                    //      (shipmentTypeCategory == Constants.ShipmentTypeCategory.COLLECTIVE && renderCollective.Contains(aFields[z].ColumnName))
                    //  )
                    //    {
                    //        mergeString = "";
                    //    }
                    //    else
                    //    {
                    //        if (rowNum != "1")
                    //        {
                    //            if (
                    //               (shipmentTypeCategory == Constants.ShipmentTypeCategory.INDIVIDUAL && renderIndividual.Contains(aFields[z].ColumnName)) ||
                    //               (shipmentTypeCategory == Constants.ShipmentTypeCategory.COLLECTIVE && renderCollective.Contains(aFields[z].ColumnName))
                    //           )
                    //            {
                    //                mergeString = "";
                    //            }
                    //            else
                    //            {
                    //                continue;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            mergeString = $"rowspan=\"{row["IDX1"].ToString()}\"";
                    //        }
                    //    }
                    //}
                    //else mergeString = "";

                    //if (optionalRender.Contains(aFields[z].ColumnName) && shipmentTypeCategory == Constants.ShipmentTypeCategory.COLLECTIVE)
                    //{
                    //    if (rowNum != "1") continue;
                    //}

                    sb.Append($"<td style='{headerStyle}' {mergeString}>" + data + "</td>");

                }
                sb.Append("</tr>");
            }
            //string JSONString = string.Empty;
            //JSONString = JsonConvert.SerializeObject(allrows);
            //strJSON = JSONString;
            strJSON = strJSON.Replace("@body", sb.ToString());
            strFoot = strFoot.Replace("@foot", tFoot.ToString());
        }

        //public static void printServiceLevelReport(List<ServiceLevelHeader> dataReport, string docDate, string reportByModel, string showMerged, string reportName, out string strHead, out string strJSON, out string strFoot)
        //{
        //    StringBuilder sReports = new StringBuilder();
        //    StringBuilder tHead = new StringBuilder();
        //    StringBuilder tFoot = new StringBuilder();
        //    StringBuilder sb = new StringBuilder();
        //    strJSON = "<tbody>@body</tbody>";
        //    strFoot = "<tfoot>@foot</tfoot>";

        //    var dateHeaderLabel = "";
        //    var subOrderHeaderLabel = "";
        //    var subTotalDOHeaderLabel = "";

        //    if(dataReport.Count > 0)
        //    {
        //        foreach (var detail in dataReport[0].Details)
        //        {

        //            if (!String.IsNullOrEmpty(reportByModel))
        //            {
        //                if(reportByModel == "Truck")
        //                {
        //                    dateHeaderLabel += $@"
        //                            <th colspan='4' style='{headerStyle}'>{detail.date.ToString("dd/MM/yyyy")}</th>";

        //                    subOrderHeaderLabel += $@"
        //                            <th rowspan='2' style='{headerStyle}'>Order</th>
        //                            <th colspan='3' style='{headerStyle}'>By Truck</th>";

        //                    subTotalDOHeaderLabel += $@"
        //                            <th style='{headerStyle}'>Total DO</th>
        //                            <th style='{headerStyle}'>Realization</th>
        //                            <th style='{headerStyle}'>%</th>";
        //                }
        //                else
        //                {
        //                    dateHeaderLabel += $@"
        //                            <th colspan='5' style='{headerStyle}'>{detail.date.ToString("dd/MM/yyyy")}</th>";

        //                    subOrderHeaderLabel += $@"
        //                            <th rowspan='2' style='{headerStyle}'>Order</th>
        //                            <th colspan='4' style='{headerStyle}'>By DO Qty</th>";

        //                    subTotalDOHeaderLabel += $@"
        //                            <th style='{headerStyle}'>Total DO</th>
        //                            <th style='{headerStyle}'>Realization</th>
        //                            <th style='{headerStyle}'>UOM</th>
        //                            <th style='{headerStyle}'>%</th>";
        //                }
        //            }
        //        }
        //    }


        //    strHead = $@"<thead class='header'>
        //                    <tr>
        //                        <th rowspan='3' style='{headerStyle}background-color: #BFE1B6;'>No</th>
        //                        <th rowspan='3' style='{headerStyle}background-color: #BFE1B6;'>{DisplayNames.TRANSPORTER}</th>
        //                        <th rowspan='3' style='{headerStyle}background-color: #BFE1B6;'>Truck Allocation (Unit)</th>
        //                        <th colspan='10' style='{headerStyle}background-color: #FFFF9B;'>Grand Total ({docDate})</th>
        //                        {dateHeaderLabel}
        //                    </tr>

        //                    <tr class='second'>
        //                        <th rowspan='2' style='{headerStyle}background-color: #FFFF9B;'>Order</th>
        //                        <th colspan='4' style='{headerStyle}background-color: #FFFF9B;'>By Truck</th>
        //                        <th colspan='5' style='{headerStyle}background-color: #FFFF9B;'>By DO Qty</th>
        //                        {subOrderHeaderLabel}
        //                    </tr>

        //                    <tr class='third'>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>Total DO</th>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>Realization</th>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>%</th>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>Average</th>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>Total DO</th>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>Realization</th>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>UOM</th>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>%</th>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>Average</th>
        //                        {subTotalDOHeaderLabel}
        //                    </tr>

        //                </thead>";

        //    List<decimal> SumAllocation = new List<decimal>();
        //    List<decimal> SumOrder = new List<decimal>();
        //    List<decimal> SumDOByTruck = new List<decimal>();
        //    List<decimal> SumRealizationByTruck = new List<decimal>();
        //    List<decimal> SumDOByQty = new List<decimal>();
        //    List<decimal> SumRealizationByQty = new List<decimal>();
        //    List<string> SumUOM = new List<string>();

        //    var no = 0;
        //    foreach (var data in dataReport)
        //    {
        //        sb.Append("<tr>");

        //        sb.Append($"<td style='{headerStyle}'>{no + 1}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Transporter_Name}</td>");
        //        //sb.Append($"<td style='{headerStyle}'>{data.Allocation_Unit}</td>");

        //        var transporter = dataReport.Where(r => r.Transporter_Name == data.Transporter_Name).ToList();
        //        var transporterCount = transporter.Count();
        //        var transporterIndex = dataReport.FindIndex(r => r.Transporter_Name == data.Transporter_Name);
        //        var rowspan = showMerged == "Y" ? $"rowspan='{transporterCount}'" : "";

        //        if (transporterCount > 1)
        //        {

        //            decimal total_order = transporter.Select(r => r.Total_Order).Sum(r => Convert.ToInt64(r));
        //            decimal total_do_by_truck = transporter.Select(r => r.Total_DO_By_Truck).Sum(r => Convert.ToInt64(r));
        //            decimal total_realization_by_truck = transporter.Select(r => r.Total_Realization_By_Truck).Sum(r => Convert.ToInt64(r));
        //            decimal total_avarage_by_truck = transporter.Select(r => r.Total_Avarage_By_Truck).Sum(r => Convert.ToInt64(r));
        //            decimal total_percentage_by_truck = total_realization_by_truck > 0 ? (total_realization_by_truck / total_do_by_truck) * 100 : 0;

        //            if (no == transporterIndex)
        //            {

        //                sb.Append($"<td style='{headerStyle}' {rowspan}>{data.Allocation_Unit}</td>");

        //                sb.Append($"<td style='{headerStyle}' {rowspan}>{total_order}</td>"); //total order grandtotal
        //                sb.Append($"<td style='{headerStyle}' {rowspan}>{total_do_by_truck}</td>"); //total do by truck
        //                sb.Append($"<td style='{headerStyle}' {rowspan}>{total_realization_by_truck}</td>"); //realization by truck
        //                sb.Append($"<td style='{printPercentageColor(total_percentage_by_truck)}' {rowspan}>{total_percentage_by_truck.ToString("0.0")}%</td>"); //percentage by truck
        //                sb.Append($"<td style='{headerStyle}' {rowspan}>{total_avarage_by_truck.ToString("0")}</td>"); //avarage by truck

        //                SumAllocation.Add(Int16.Parse(data.Allocation_Unit));
        //                SumOrder.Add(total_order);
        //                SumDOByTruck.Add(total_do_by_truck);
        //                SumRealizationByTruck.Add(total_realization_by_truck);
        //            }
        //            else
        //            {
        //                if (showMerged == "N")
        //                {
        //                    sb.Append($"<td style='{headerStyle}'></td>");
        //                    sb.Append($"<td style='{headerStyle}'></td>");
        //                    sb.Append($"<td style='{headerStyle}'></td>");
        //                    sb.Append($"<td style='{headerStyle}'></td>");
        //                    sb.Append($"<td style='{headerStyle}'></td>");
        //                    sb.Append($"<td style='{headerStyle}'></td>");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            sb.Append($"<td style='{headerStyle}'>{data.Allocation_Unit}</td>");
        //            sb.Append($"<td style='{headerStyle}'>{data.Total_Order}</td>"); //total order grandtotal
        //            sb.Append($"<td style='{headerStyle}'>{data.Total_DO_By_Truck}</td>"); //total do by truck
        //            sb.Append($"<td style='{headerStyle}'>{data.Total_Realization_By_Truck}</td>"); //realization by truck
        //            sb.Append($"<td style='{printPercentageColor(data.Total_Percentage_By_Truck)}'>{data.Total_Percentage_By_Truck.ToString("0.0")}%</td>"); //percentage by truck
        //            sb.Append($"<td style='{headerStyle}'>{data.Total_Avarage_By_Truck.ToString("0")}</td>"); //avarage by truck

        //            SumAllocation.Add(Int16.Parse(data.Allocation_Unit));
        //            SumOrder.Add(data.Total_Order);
        //            SumDOByTruck.Add(data.Total_DO_By_Truck);
        //            SumRealizationByTruck.Add(data.Total_Realization_By_Truck);
        //        }


        //        sb.Append($"<td style='{headerStyle}'>{data.Total_DO_By_Qty}</td>"); //total deo by qty
        //        sb.Append($"<td style='{headerStyle}'>{data.Total_Realization_By_Qty}</td>"); //realization by qty
        //        sb.Append($"<td style='{headerStyle}'>{data.UOM}</td>"); //uom
        //        sb.Append($"<td style='{printPercentageColor(data.Total_Percentage_By_Qty)}'>{data.Total_Percentage_By_Qty.ToString("0.0")}%</td>"); //percentage by qty
        //        sb.Append($"<td style='{headerStyle}'>{data.Total_Avarage_By_Qty.ToString("0")}</td>"); //avarage by qty

        //        SumDOByQty.Add(data.Total_DO_By_Qty);
        //        SumRealizationByQty.Add(data.Total_Realization_By_Qty);
        //        if (!SumUOM.Contains(data.UOM))
        //            SumUOM.Add(data.UOM);

        //        foreach (var detail in data.Details)
        //        {
        //            if (transporterCount > 1)
        //            {
        //                decimal total_order = transporter.Select(r => r.Total_Order).Sum(r => Convert.ToInt64(r));

        //                if (no == transporterIndex)
        //                {
        //                    sb.Append($"<td style='{headerStyle}' {rowspan}>{total_order}</td>");
        //                }
        //                else
        //                {
        //                    if (showMerged == "N")
        //                    {
        //                        sb.Append($"<td style='{headerStyle}'></td>");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                sb.Append($"<td style='{headerStyle}'>{detail.order}</td>");
        //            }


        //            if (reportByModel == "Truck")
        //            {
        //                if (transporterCount > 1)
        //                {
        //                    decimal total_do_by_truck = transporter.Select(r => r.Total_DO_By_Truck).Sum(r => Convert.ToInt64(r));
        //                    decimal total_realization_by_truck = transporter.Select(r => r.Total_Realization_By_Truck).Sum(r => Convert.ToInt64(r));
        //                    decimal total_percentage_by_truck = total_realization_by_truck > 0 ? (total_realization_by_truck / total_do_by_truck) * 100 : 0;

        //                    if (no == transporterIndex)
        //                    {
        //                        sb.Append($"<td style='{headerStyle}' {rowspan}>{total_do_by_truck}</td>");
        //                        sb.Append($"<td style='{headerStyle}' {rowspan}>{total_realization_by_truck}</td>");
        //                        sb.Append($"<td style='{printPercentageColor(total_percentage_by_truck)}' {rowspan}>{total_percentage_by_truck.ToString("0.0")}%</td>");
        //                    }
        //                    else
        //                    {
        //                        if (showMerged == "N")
        //                        {
        //                            sb.Append($"<td style='{headerStyle}'></td>");
        //                            sb.Append($"<td style='{headerStyle}'></td>");
        //                            sb.Append($"<td style='{headerStyle}'></td>");
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    sb.Append($"<td style='{headerStyle}'>{detail.total_do_by_truck}</td>");
        //                    sb.Append($"<td style='{headerStyle}'>{detail.total_realization_by_truck}</td>");
        //                    sb.Append($"<td style='{printPercentageColor(detail.percentage_by_truck)}'>{detail.percentage_by_truck.ToString("0.0")}%</td>");
        //                }
        //            }
        //            else
        //            {
        //                sb.Append($"<td style='{headerStyle}'>{detail.total_do_by_qty}</td>");
        //                sb.Append($"<td style='{headerStyle}'>{detail.total_realization_by_qty}</td>");
        //                sb.Append($"<td style='{headerStyle}'>{detail.uom_by_qty}</td>");
        //                sb.Append($"<td style='{printPercentageColor(detail.percentage_by_qty)}'>{detail.percentage_by_qty.ToString("0.0")}%</td>");
        //            }

        //        }

        //        sb.Append("</tr>");
        //        no++;
        //    }

        //    //render baris grand total di bawah
        //    if (dataReport.Count > 0)
        //    {
        //        var SumPercentageByTruck = SumRealizationByTruck.Sum() > 0 ? (SumRealizationByTruck.Sum() / SumDOByTruck.Sum()) * 100 : 0;
        //        var SumPercentageByQty = SumRealizationByQty.Sum() > 0 ? (SumRealizationByQty.Sum() / SumDOByQty.Sum()) * 100 : 0;

        //        sb.Append("<tr style='background-color: #AEC7E7'>");

        //        sb.Append($@"<th style='{headerStyle}' colspan='2'>Grand Total</th>");
        //        sb.Append($@"<th style='{headerStyle}'>{SumAllocation.Sum()}</th>");
        //        sb.Append($@"<th style='{headerStyle}'>{SumOrder.Sum()}</th>");
        //        sb.Append($@"<th style='{headerStyle}'>{SumDOByTruck.Sum()}</th>");
        //        sb.Append($@"<th style='{headerStyle}'>{SumRealizationByTruck.Sum()}</th>");
        //        sb.Append($@"<th style='{printPercentageColor(SumPercentageByTruck)}'>{SumPercentageByTruck.ToString("0.0")}%</th>");
        //        sb.Append($@"<th style='{headerStyle}'></th>");
        //        sb.Append($@"<th style='{headerStyle}'>{SumDOByQty.Sum()}</th>");
        //        sb.Append($@"<th style='{headerStyle}'>{SumRealizationByQty.Sum()}</th>");
        //        sb.Append($@"<th style='{headerStyle}'>{String.Join(" | ", SumUOM)}</th>");
        //        sb.Append($@"<th style='{printPercentageColor(SumPercentageByQty)}'>{SumPercentageByQty.ToString("0.0")}%</th>");
        //        sb.Append($@"<th style='{headerStyle}'></th>");

            
        //        List<decimal> GrandTotalOrderByTruck = new List<decimal>();
        //        List<decimal> GrandTotalDOByTruck = new List<decimal>();
        //        List<decimal> GrandRealizationByTruck = new List<decimal>();
        //        List<decimal> GrandTotalDOByQty = new List<decimal>();
        //        List<decimal> GrandRealizationByQty = new List<decimal>();

        //        //mengambil nilai grand total ke bawah per hari
        //        for (var i = 0; i < dataReport[0].Details.Count; i++)
        //        {
        //            List<decimal> SumOrderDetail = new List<decimal>();
        //            List<decimal> SumDODetailByTruck = new List<decimal>();
        //            List<decimal> SumRealizationDetailByTruck = new List<decimal>();
        //            List<decimal> SumDODetailByQty = new List<decimal>();
        //            List<decimal> SumRealizationDetailByQty = new List<decimal>();

        //            for (var j = 0; j < dataReport.Count; j++)
        //            {
        //                //var transporterCount = dataReport.Where(r => r.Transporter_Name == dataReport[j].Transporter_Name).Count();
        //                //var transporterIndex = dataReport.FindIndex(r => r.Transporter_Name == dataReport[j].Transporter_Name);

        //                //if (transporterCount > 1)
        //                //{
        //                //    if (j == transporterIndex)
        //                //    {
        //                //        SumOrderDetail.Add(dataReport[j].Details[i].order);
        //                //        SumDODetailByTruck.Add(dataReport[j].Details[i].total_do_by_truck);
        //                //        SumRealizationDetailByTruck.Add(dataReport[j].Details[i].total_realization_by_truck);
        //                //    }
        //                //}
        //                //else
        //                //{
        //                //    SumOrderDetail.Add(dataReport[j].Details[i].order);
        //                //    SumDODetailByTruck.Add(dataReport[j].Details[i].total_do_by_truck);
        //                //    SumRealizationDetailByTruck.Add(dataReport[j].Details[i].total_realization_by_truck);
        //                //}

        //                SumOrderDetail.Add(dataReport[j].Details[i].order);
        //                SumDODetailByTruck.Add(dataReport[j].Details[i].total_do_by_truck);
        //                SumRealizationDetailByTruck.Add(dataReport[j].Details[i].total_realization_by_truck);

        //                SumDODetailByQty.Add(dataReport[j].Details[i].total_do_by_qty);
        //                SumRealizationDetailByQty.Add(dataReport[j].Details[i].total_realization_by_qty);
        //            }

        //            GrandTotalOrderByTruck.Add(SumOrderDetail.Sum());
        //            GrandTotalDOByTruck.Add(SumDODetailByTruck.Sum());
        //            GrandRealizationByTruck.Add(SumRealizationDetailByTruck.Sum());
        //            GrandTotalDOByQty.Add(SumDODetailByQty.Sum());
        //            GrandRealizationByQty.Add(SumRealizationDetailByQty.Sum());
        //        }


        //        for(var i = 0; i < GrandTotalOrderByTruck.Count; i++)
        //        {
        //            decimal GrandPercentageByTruck = GrandRealizationByTruck[i] > 0 ? (GrandRealizationByTruck[i] / GrandTotalDOByTruck[i]) * 100 : 0;
        //            decimal GrandPercentageByQty = GrandRealizationByQty[i] > 0 ? (GrandRealizationByQty[i] / GrandTotalDOByQty[i]) * 100 : 0;

        //            sb.Append($@"<th style='{headerStyle}'>{GrandTotalOrderByTruck[i]}</th>");
                    
        //            if (!String.IsNullOrEmpty(reportByModel))
        //            {
        //                if (reportByModel == "Truck")
        //                {
        //                    sb.Append($@"<th style='{headerStyle}'>{GrandTotalDOByTruck[i]}</th>");
        //                    sb.Append($@"<th style='{headerStyle}'>{GrandRealizationByTruck[i]}</th>");
        //                    sb.Append($@"<th style='{printPercentageColor(GrandPercentageByTruck)}'>{GrandPercentageByTruck.ToString("0.0")}%</th>");
        //                }
        //                else
        //                {
        //                    sb.Append($@"<th style='{headerStyle}'>{GrandTotalDOByQty[i]}</th>");
        //                    sb.Append($@"<th style='{headerStyle}'>{GrandRealizationByQty[i]}</th>");
        //                    sb.Append($@"<th style='{headerStyle}'>{String.Join(" | ", SumUOM)}</th>");
        //                    sb.Append($@"<th style='{printPercentageColor(GrandPercentageByQty)}'>{GrandPercentageByQty.ToString("0.0")}%</th>");
        //                }
        //            }
        //        }

        //        sb.Append("</tr>");
        //    }

        //    strJSON = strJSON.Replace("@body", sb.ToString());
        //    strFoot = strFoot.Replace("@foot", tFoot.ToString());
        //}

        //public static void printSummaryServiceLevelReport(List<SummaryServiceLevelHeader> dataReport, string docDate, SummaryServiceLevelReportModel model, string reportName, out string strHead, out string strJSON, out string strFoot)
        //{
        //    StringBuilder sReports = new StringBuilder();
        //    StringBuilder tHead = new StringBuilder();
        //    StringBuilder tFoot = new StringBuilder();
        //    StringBuilder sb = new StringBuilder();
        //    strJSON = "<tbody>@body</tbody>";
        //    strFoot = "<tfoot>@foot</tfoot>";

        //    var dateHeaderLabel = "";
        //    var subOrderHeaderLabel = "";
        //    var subTotalDOHeaderLabel = "";

        //    if (dataReport.Count > 0)
        //    {
        //        foreach (var detail in dataReport[0].Details)
        //        {

        //            if (!String.IsNullOrEmpty(model.Report_By))
        //            {
        //                if (model.Report_By == "Truck & DO Qty")
        //                {
        //                    dateHeaderLabel += $@"
        //                            <th colspan='8' style='{headerStyle}'>{detail.month.ToString("MMM-yy")}</th>";

        //                    subOrderHeaderLabel += $@"
        //                            <th rowspan='2' style='{headerStyle}'>Order</th>
        //                            <th colspan='3' style='{headerStyle}'>By Truck</th>
        //                            <th colspan='4' style='{headerStyle}'>By DO Qty</th>";

        //                    subTotalDOHeaderLabel += $@"
        //                            <th style='{headerStyle}'>Total DO</th>
        //                            <th style='{headerStyle}'>Realization</th>
        //                            <th style='{headerStyle}'>%</th>
        //                            <th style='{headerStyle}'>Total DO</th>
        //                            <th style='{headerStyle}'>Realization</th>
        //                            <th style='{headerStyle}'>UOM</th>
        //                            <th style='{headerStyle}'>%</th>";
        //                }

        //                if (model.Report_By == "Truck")
        //                {
        //                    dateHeaderLabel += $@"
        //                            <th colspan='4' style='{headerStyle}'>{detail.month.ToString("MMM-yy")}</th>";

        //                    subOrderHeaderLabel += $@"
        //                            <th rowspan='2' style='{headerStyle}'>Order</th>
        //                            <th colspan='3' style='{headerStyle}'>By Truck</th>";

        //                    subTotalDOHeaderLabel += $@"
        //                            <th style='{headerStyle}'>Total DO</th>
        //                            <th style='{headerStyle}'>Realization</th>
        //                            <th style='{headerStyle}'>%</th>";
        //                }

        //                if(model.Report_By == "DO Qty")
        //                {
        //                    dateHeaderLabel += $@"
        //                            <th colspan='5' style='{headerStyle}'>{detail.month.ToString("MMM-yy")}</th>";

        //                    subOrderHeaderLabel += $@"
        //                            <th rowspan='2' style='{headerStyle}'>Order</th>
        //                            <th colspan='4' style='{headerStyle}'>By DO Qty</th>";

        //                    subTotalDOHeaderLabel += $@"
        //                            <th style='{headerStyle}'>Total DO</th>
        //                            <th style='{headerStyle}'>Realization</th>
        //                            <th style='{headerStyle}'>UOM</th>
        //                            <th style='{headerStyle}'>%</th>";
        //                }
        //            }
        //        }
        //    }


        //    strHead = $@"<thead class='header'>
        //                    <tr>
        //                        <th rowspan='3' style='{headerStyle}background-color: #BFE1B6;'>No</th>
        //                        <th rowspan='3' style='{headerStyle}background-color: #BFE1B6;'>{DisplayNames.TRANSPORTER}</th>
        //                        <th rowspan='3' style='{headerStyle}background-color: #BFE1B6;'>{DisplayNames.TRUCK_TYPE}</th>
        //                        <th rowspan='3' style='{headerStyle}background-color: #BFE1B6;'>Truck Allocation (Unit)</th>
        //                        <th rowspan='3' style='{headerStyle}background-color: #BFE1B6;'>{DisplayNames.LOADING}</th>
        //                        <th rowspan='3' style='{headerStyle}background-color: #BFE1B6;'>{DisplayNames.DESTINATION}</th>
        //                        <th colspan='8' style='{headerStyle}background-color: #FFFF9B;'>Grand Total ({docDate})</th>
        //                        {dateHeaderLabel}
        //                    </tr>

        //                    <tr class='second'>
        //                        <th rowspan='2' style='{headerStyle}background-color: #FFFF9B;'>Order</th>
        //                        <th colspan='3' style='{headerStyle}background-color: #FFFF9B;'>By Truck</th>
        //                        <th colspan='4' style='{headerStyle}background-color: #FFFF9B;'>By DO Qty</th>
        //                        {subOrderHeaderLabel}
        //                    </tr>

        //                    <tr class='third'>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>Total DO</th>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>Realization</th>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>%</th>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>Total DO</th>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>Realization</th>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>UOM</th>
        //                        <th style='{headerStyle}background-color: #FFFF9B;'>%</th>
        //                        {subTotalDOHeaderLabel}
        //                    </tr>

        //                </thead>";

        //    List<decimal> SumAllocation = new List<decimal>();
        //    List<decimal> SumOrder = new List<decimal>();
        //    List<decimal> SumDOByTruck = new List<decimal>();
        //    List<decimal> SumRealizationByTruck = new List<decimal>();
        //    List<decimal> SumDOByQty = new List<decimal>();
        //    List<decimal> SumRealizationByQty = new List<decimal>();
        //    List<string> SumUOM = new List<string>();

        //    var no = 0;
        //    foreach (var data in dataReport)
        //    {
        //        var truckTypeCount = dataReport.Where(r => r.Transporter_Name == data.Transporter_Name && r.Truck_Type_Name == data.Truck_Type_Name).Count();
        //        var truckTypeindex = dataReport.FindIndex(r => r.Transporter_Name == data.Transporter_Name && r.Truck_Type_Name == data.Truck_Type_Name);
        //        var existTransporterCount = dataReport.Where(r => r.Transporter_Name == data.Transporter_Name
        //                                                      && r.Truck_Type_Name == data.Truck_Type_Name
        //                                                      && r. Loading == data.Loading
        //                                                      && r.Destination == data.Destination)
        //                                               .Count();

        //        var existTransporterIndex = dataReport.FindIndex(r => r.Transporter_Name == data.Transporter_Name
        //                                                      && r.Truck_Type_Name == data.Truck_Type_Name
        //                                                      && r.Loading == data.Loading
        //                                                      && r.Destination == data.Destination);

        //        sb.Append("<tr>");

        //        sb.Append($"<td style='{headerStyle}'>{no + 1}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Transporter_Name}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{(model.Truck_Type.Count == 0 ? "ALL" : data.Truck_Type_Name)}</td>");

        //        //Allocation Unit
        //        if (truckTypeCount >= 1)
        //        {
        //            if (no == truckTypeindex)
        //            {
        //                sb.Append($"<td style='{headerStyle}'>{data.Allocation_Unit}</td>");
        //                SumAllocation.Add(Int16.Parse(data.Allocation_Unit));
        //            }
        //            else
        //            {
        //                sb.Append($"<td style='{headerStyle}'></td>");
        //            }
        //        }
                
                
        //        sb.Append($"<td style='{headerStyle}'>{(model.Loading_Destination_From.Count == 0 ? "ALL" : data.Loading)}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{(model.Loading_Destination_To.Count == 0 ? "ALL" : data.Destination)}</td>");


        //        //Kolom Grand Total
        //        if (existTransporterCount >= 1)
        //        {
        //            if (no == existTransporterIndex)
        //            {
        //                sb.Append($"<td style='{headerStyle}'>{data.Total_Order}</td>"); //total order grandtotal
        //                sb.Append($"<td style='{headerStyle}'>{data.Total_DO_By_Truck}</td>"); //total do by truck
        //                sb.Append($"<td style='{headerStyle}'>{data.Total_Realization_By_Truck}</td>"); //realization by truck
        //                sb.Append($"<td style='{printPercentageColor(data.Total_Percentage_By_Truck)}'>{data.Total_Percentage_By_Truck.ToString("0.0")}%</td>"); //percentage by truck

        //                SumOrder.Add(data.Total_Order);
        //                SumDOByTruck.Add(data.Total_DO_By_Truck);
        //                SumRealizationByTruck.Add(data.Total_Realization_By_Truck);
        //            }
        //            else
        //            {
        //                sb.Append($"<td style='{headerStyle}'></td>");
        //                sb.Append($"<td style='{headerStyle}'></td>");
        //                sb.Append($"<td style='{headerStyle}'></td>");
        //                sb.Append($"<td style='{headerStyle}'></td>");
        //            }
        //        }
                
        //        sb.Append($"<td style='{headerStyle}'>{data.Total_DO_By_Qty}</td>"); //total deo by qty
        //        sb.Append($"<td style='{headerStyle}'>{data.Total_Realization_By_Qty}</td>"); //realization by qty
        //        sb.Append($"<td style='{headerStyle}'>{data.UOM}</td>"); //uom
        //        sb.Append($"<td style='{printPercentageColor(data.Total_Percentage_By_Qty)}'>{data.Total_Percentage_By_Qty.ToString("0.0")}%</td>"); //percentage by qty

        //        SumDOByQty.Add(data.Total_DO_By_Qty);
        //        SumRealizationByQty.Add(data.Total_Realization_By_Qty);
        //        if (!SumUOM.Contains(data.UOM))
        //            SumUOM.Add(data.UOM);

        //        //data detail
        //        foreach (var detail in data.Details)
        //        {

        //            if (existTransporterCount >= 1)
        //            {
        //                if (no == existTransporterIndex)
        //                {
        //                    sb.Append($"<td style='{headerStyle}'>{detail.order}</td>");
        //                }
        //                else
        //                {
        //                    sb.Append($"<td style='{headerStyle}'></td>");
        //                }
        //            }

        //            if (model.Report_By.Contains("Truck"))
        //            {
        //                if (existTransporterCount >= 1)
        //                {
        //                    if (no == existTransporterIndex)
        //                    {
        //                        sb.Append($"<td style='{headerStyle}'>{detail.total_do_by_truck}</td>");
        //                        sb.Append($"<td style='{headerStyle}'>{detail.total_realization_by_truck}</td>");
        //                        sb.Append($"<td style='{printPercentageColor(detail.percentage_by_truck)}'>{detail.percentage_by_truck.ToString("0.0")}%</td>");
        //                    }
        //                    else
        //                    {
        //                        sb.Append($"<td style='{headerStyle}'></td>");
        //                        sb.Append($"<td style='{headerStyle}'></td>");
        //                        sb.Append($"<td style='{headerStyle}'></td>");
        //                    }
        //                }
        //            }
                    
        //            if(model.Report_By.Contains("DO Qty"))
        //            {
        //                sb.Append($"<td style='{headerStyle}'>{detail.total_do_by_qty}</td>");
        //                sb.Append($"<td style='{headerStyle}'>{detail.total_realization_by_qty}</td>");
        //                sb.Append($"<td style='{headerStyle}'>{detail.uom_by_qty}</td>");
        //                sb.Append($"<td style='{printPercentageColor(detail.percentage_by_qty)}'>{detail.percentage_by_qty.ToString("0.0")}%</td>");
        //            }
        //        }

        //        sb.Append("</tr>");

        //        //subtotal row
        //        if(model.Show_Sub_Total == "Y")
        //        {
        //            try
        //            {
        //                var transporter = dataReport.Where(r => r.Transporter_Name == data.Transporter_Name).ToList();
        //                var transporterIndex = dataReport.FindIndex(r => r.Transporter_Name == data.Transporter_Name);
        //                var transporterCount = transporter.Count();

        //                var index = (transporterIndex + transporterCount) - 1;

        //                var truck_type = transporter.GroupBy(r => new { r.Truck_Type_Name, r.Loading, r.Destination })
        //                                           .Select(r => r.FirstOrDefault())
        //                                           .ToList();

        //                var allocation = truck_type.Select(r => r.Allocation_Unit).Sum(r => Convert.ToInt32(r));
        //                decimal order = truck_type.Select(r => r.Total_Order).Sum();
        //                decimal DOByTruck = truck_type.Select(r => r.Total_DO_By_Truck).Sum();
        //                decimal realizationByTruck = truck_type.Select(r => r.Total_Realization_By_Truck).Sum();
        //                decimal percentageByTruck = realizationByTruck > 0 ? (realizationByTruck / DOByTruck) * 100 : 0;
        //                decimal DOByQty = transporter.Select(r => r.Total_DO_By_Qty).Sum();
        //                decimal realizationByQty = transporter.Select(r => r.Total_Realization_By_Qty).Sum();
        //                var UOM = transporter.Select(r => r.UOM).Distinct().ToArray();
        //                decimal percentageByQty = realizationByQty > 0 ? (realizationByQty / DOByQty) * 100 : 0;

        //                if (no == index)
        //                {
        //                    sb.Append($"<tr style='background-color: #AFAAAA'>");
        //                    sb.Append($"<th style='{headerStyle}' colspan='3'>TOTAL {dataReport[transporterIndex].Transporter_Name}</th>");
        //                    sb.Append($"<th style='{headerStyle}'>{allocation}</th>");
        //                    sb.Append($"<th style='{headerStyle}'></th>");
        //                    sb.Append($"<th style='{headerStyle}'></th>");
        //                    sb.Append($"<th style='{headerStyle}'>{order}</th>");
        //                    sb.Append($"<th style='{headerStyle}'>{DOByTruck}</th>");
        //                    sb.Append($"<th style='{headerStyle}'>{realizationByTruck}</th>");
        //                    sb.Append($"<th style='{printPercentageColor(percentageByTruck)}'>{percentageByTruck.ToString("0.0")}%</th>");
        //                    sb.Append($"<th style='{headerStyle}'>{DOByQty}</th>");
        //                    sb.Append($"<th style='{headerStyle}'>{realizationByQty}</th>");
        //                    sb.Append($"<th style='{headerStyle}'>{String.Join(" | ", UOM)}</th>");
        //                    sb.Append($"<th style='{printPercentageColor(percentageByQty)}'>{percentageByQty.ToString("0.0")}%</th>");

        //                    for (var i = 0; i < data.Details.Count; i++)
        //                    {

        //                        List<decimal> SubOrder = new List<decimal>();
        //                        List<decimal> SubDOByTruck = new List<decimal>();
        //                        List<decimal> SubRealizationByTruck = new List<decimal>();
        //                        List<decimal> SubDOByQty = new List<decimal>();
        //                        List<decimal> SubRealizationByQty = new List<decimal>();

        //                        for (var j = 0; j < truck_type.Count; j++)
        //                        {
        //                            SubOrder.Add(truck_type[j].Details[i].order);
        //                            SubDOByTruck.Add(truck_type[j].Details[i].total_do_by_truck);
        //                            SubRealizationByTruck.Add(truck_type[j].Details[i].total_realization_by_truck);
        //                        }

        //                        for(var j = 0; j < transporterCount; j++)
        //                        {
        //                            SubDOByQty.Add(transporter[j].Details[i].total_do_by_qty);
        //                            SubRealizationByQty.Add(transporter[j].Details[i].total_realization_by_qty);
        //                        }

        //                        decimal PercentageByTruck = SubRealizationByTruck.Sum() > 0 ? (SubRealizationByTruck.Sum() / SubDOByTruck.Sum()) * 100 : 0;
        //                        decimal PercentageByQty = SubRealizationByQty.Sum() > 0 ? (SubRealizationByQty.Sum() / SubDOByQty.Sum()) * 100 : 0;

        //                        sb.Append($"<th style='{headerStyle}'>{SubOrder.Sum()}</th>");

        //                        if (model.Report_By.Contains("Truck"))
        //                        {
        //                            sb.Append($"<th style='{headerStyle}'>{SubDOByTruck.Sum()}</th>");
        //                            sb.Append($"<th style='{headerStyle}'>{SubRealizationByTruck.Sum()}</th>");
        //                            sb.Append($"<th style='{printPercentageColor(PercentageByTruck)}'>{PercentageByTruck.ToString("0.0")}%</th>");
        //                        }

        //                        if (model.Report_By.Contains("DO Qty"))
        //                        {
        //                            sb.Append($"<th style='{headerStyle}'>{SubDOByQty.Sum()}</th>");
        //                            sb.Append($"<th style='{headerStyle}'>{SubRealizationByQty.Sum()}</th>");
        //                            sb.Append($"<th style='{headerStyle}'>{String.Join(" | ", UOM)}</th>");
        //                            sb.Append($"<th style='{printPercentageColor(PercentageByQty)}'>{PercentageByQty.ToString("0.0")}%</th>");
        //                        }

        //                    }

        //                    sb.Append("</tr>");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                var message = ex.Message;
        //            }
        //        }

        //        no++;
        //    }

        //    //render baris grand total di bawah
        //    if (dataReport.Count > 0)
        //    {
        //        decimal SumPercentageByTruck = SumRealizationByTruck.Sum() > 0 ? (SumRealizationByTruck.Sum() / SumDOByTruck.Sum()) * 100 : 0;
        //        decimal SumPercentageByQty = SumRealizationByQty.Sum() > 0 ? (SumRealizationByQty.Sum() / SumDOByQty.Sum()) * 100 : 0;

        //        sb.Append("<tr style='background-color: #AEC7E7'>");

        //        sb.Append($@"<th style='{headerStyle}' colspan='3'>Grand Total</th>");
        //        sb.Append($@"<th style='{headerStyle}'>{SumAllocation.Sum()}</th>");
        //        sb.Append($@"<th style='{headerStyle}'></th>");
        //        sb.Append($@"<th style='{headerStyle}'></th>");
        //        sb.Append($@"<th style='{headerStyle}'>{SumOrder.Sum()}</th>");
        //        sb.Append($@"<th style='{headerStyle}'>{SumDOByTruck.Sum()}</th>");
        //        sb.Append($@"<th style='{headerStyle}'>{SumRealizationByTruck.Sum()}</th>");
        //        sb.Append($@"<th style='{printPercentageColor(SumPercentageByTruck)}'>{SumPercentageByTruck.ToString("0.0")}%</th>");
        //        sb.Append($@"<th style='{headerStyle}'>{SumDOByQty.Sum()}</th>");
        //        sb.Append($@"<th style='{headerStyle}'>{SumRealizationByQty.Sum()}</th>");
        //        sb.Append($@"<th style='{headerStyle}'>{String.Join(" | ", SumUOM)}</th>");
        //        sb.Append($@"<th style='{printPercentageColor(SumPercentageByQty)}'>{SumPercentageByQty.ToString("0.0")}%</th>");


        //        List<decimal> GrandTotalOrderByTruck = new List<decimal>();
        //        List<decimal> GrandTotalDOByTruck = new List<decimal>();
        //        List<decimal> GrandRealizationByTruck = new List<decimal>();
        //        List<decimal> GrandTotalDOByQty = new List<decimal>();
        //        List<decimal> GrandRealizationByQty = new List<decimal>();

        //        //mengambil nilai grand total ke bawah per bulan
        //        for (var i = 0; i < dataReport[0].Details.Count; i++)
        //        {
        //            List<decimal> SumOrderDetail = new List<decimal>();
        //            List<decimal> SumDODetailByTruck = new List<decimal>();
        //            List<decimal> SumRealizationDetailByTruck = new List<decimal>();
        //            List<decimal> SumDODetailByQty = new List<decimal>();
        //            List<decimal> SumRealizationDetailByQty = new List<decimal>();

        //            for (var j = 0; j < dataReport.Count; j++)
        //            {
        //                var transporterCount = dataReport.Where(r => r.Transporter_Name == dataReport[j].Transporter_Name
        //                                                        && r.Truck_Type_Name == dataReport[j].Truck_Type_Name
        //                                                        && r.Loading == dataReport[j].Loading
        //                                                        && r.Destination == dataReport[j].Destination).Count();

        //                var transporterIndex = dataReport.FindIndex(r => r.Transporter_Name == dataReport[j].Transporter_Name
        //                                                            && r.Truck_Type_Name == dataReport[j].Truck_Type_Name
        //                                                            && r.Loading == dataReport[j].Loading
        //                                                            && r.Destination == dataReport[j].Destination);

        //                if (transporterCount >= 1)
        //                {
        //                    if (j == transporterIndex)
        //                    {
        //                        SumOrderDetail.Add(dataReport[j].Details[i].order);
        //                        SumDODetailByTruck.Add(dataReport[j].Details[i].total_do_by_truck);
        //                        SumRealizationDetailByTruck.Add(dataReport[j].Details[i].total_realization_by_truck);
        //                    }
        //                }

        //                SumDODetailByQty.Add(dataReport[j].Details[i].total_do_by_qty);
        //                SumRealizationDetailByQty.Add(dataReport[j].Details[i].total_realization_by_qty);
        //            }

        //            GrandTotalOrderByTruck.Add(SumOrderDetail.Sum());
        //            GrandTotalDOByTruck.Add(SumDODetailByTruck.Sum());
        //            GrandRealizationByTruck.Add(SumRealizationDetailByTruck.Sum());
        //            GrandTotalDOByQty.Add(SumDODetailByQty.Sum());
        //            GrandRealizationByQty.Add(SumRealizationDetailByQty.Sum());
        //        }

        //        //render baris grandtotal di bawah perbulan
        //        for (var i = 0; i < GrandTotalOrderByTruck.Count; i++)
        //        {
        //            decimal GrandPercentageByTruck = GrandRealizationByTruck[i] > 0 ? (GrandRealizationByTruck[i] / GrandTotalDOByTruck[i]) * 100 : 0;
        //            decimal GrandPercentageByQty = GrandRealizationByQty[i] > 0 ? (GrandRealizationByQty[i] / GrandTotalDOByQty[i]) * 100 : 0;

        //            sb.Append($@"<th style='{headerStyle}'>{GrandTotalOrderByTruck[i]}</th>");

        //            if (!String.IsNullOrEmpty(model.Report_By))
        //            {
        //                if (model.Report_By.Contains("Truck"))
        //                {
        //                    sb.Append($@"<th style='{headerStyle}'>{GrandTotalDOByTruck[i]}</th>");
        //                    sb.Append($@"<th style='{headerStyle}'>{GrandRealizationByTruck[i]}</th>");
        //                    sb.Append($@"<th style='{printPercentageColor(GrandPercentageByTruck)}'>{GrandPercentageByTruck.ToString("0.0")}%</th>");
        //                }

        //                if(model.Report_By.Contains("DO Qty"))
        //                {
        //                    sb.Append($@"<th style='{headerStyle}'>{GrandTotalDOByQty[i]}</th>");
        //                    sb.Append($@"<th style='{headerStyle}'>{GrandRealizationByQty[i]}</th>");
        //                    sb.Append($@"<th style='{headerStyle}'>{String.Join(" | ", SumUOM)}</th>");
        //                    sb.Append($@"<th style='{printPercentageColor(GrandPercentageByQty)}'>{GrandPercentageByQty.ToString("0.0")}%</th>");
        //                }
        //            }
        //        }

        //        sb.Append("</tr>");
        //    }

        //    strJSON = strJSON.Replace("@body", sb.ToString());
        //    strFoot = strFoot.Replace("@foot", tFoot.ToString());
        //}

        //public static void printTransporterUtilizationReport(List<TransporterUtilizationHeader> dataReport, TransporterUtilizationTotal grandTotal, TransporterUtilizationReportModel model, string reportName, out string strHead, out string strJSON, out string strFoot)
        //{
        //    StringBuilder sReports = new StringBuilder();
        //    StringBuilder tHead = new StringBuilder();
        //    StringBuilder tFoot = new StringBuilder();
        //    StringBuilder sb = new StringBuilder();
        //    strJSON = "<tbody>@body</tbody>";
        //    strFoot = "<tfoot>@foot</tfoot>";

        //    strHead = $@"<thead class='header'>
        //                    <tr>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>No</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>{DisplayNames.TRANSPORTER}</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>{DisplayNames.LOADING}</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>{DisplayNames.DESTINATION}</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>Realization Qty (MT)</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>% Realization Qty</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>Total Truck (Unit)</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>%Truck</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>Avg Qty by Truck (MT)</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>Total Freight (IDR)</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>% Freight</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>Freight by KG (IDR)</th>
        //                    </tr>
        //                </thead>";


        //    headerStyle = headerStyle + ";mso-number-format:\"\\@\"";

        //    var no = 0;
        //    foreach(var data in dataReport)
        //    {
        //        sb.Append("<tr>");

        //        sb.Append($"<td style='{headerStyle}'>{no + 1}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Transporter_Name}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{(model.Loading_Destination_From.Count == 0 ? "ALL" : data.Loading)}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{(model.Loading_Destination_To.Count == 0 ? "ALL" : data.Destination)}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Realization_Qty.ToString("N0", CultureInfo.InvariantCulture)}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Percentage_Realization_Qty.ToString("0.00")}%</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Total_Truck.ToString("N0", CultureInfo.InvariantCulture)}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Percentage_Truck.ToString("0.00")}%</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Avg_Qty_By_Truck.ToString("N2", CultureInfo.InvariantCulture)}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Total_Freight.ToString("N0",CultureInfo.InvariantCulture)}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Percentage_Freight.ToString("0.00")}%</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Freight_By_Kg.ToString("N2", CultureInfo.InvariantCulture)}</td>");

        //        sb.Append("</tr>");

        //        //Sub Total
        //        if(model.Show_Sub_Total == "Y")
        //        {
        //            var transporter = dataReport.Where(r => r.Transporter_Name == data.Transporter_Name).ToList();
        //            var transporterIndex = dataReport.FindIndex(r => r.Transporter_Name == data.Transporter_Name);
        //            var transpoterCount = transporter.Count();

        //            decimal SubTotalRealQty = transporter.Select(r => r.Realization_Qty).Sum(r => Convert.ToInt64(r));
        //            decimal SubTotalPercentageRealQty = SubTotalRealQty > 0 ? (SubTotalRealQty / grandTotal.Total_Realization_Qty) * 100 : 0;
        //            decimal SubTotalTruck = transporter.Select(r => r.Total_Truck).Sum(r => Convert.ToInt64(r));
        //            decimal SubTotalPercentageTruck = SubTotalTruck > 0 ? (SubTotalTruck / grandTotal.Total_Truck) * 100 : 0;
        //            decimal SubTotalAvgQtyByKg = SubTotalRealQty > 0 && SubTotalTruck > 0 ? (SubTotalRealQty / SubTotalTruck) : 0;
        //            decimal SubTotalFreight = transporter.Select(r => r.Total_Freight).Sum(r => Convert.ToInt64(r));
        //            decimal SubTotalPercentageFreight = SubTotalFreight > 0 ? (SubTotalFreight / grandTotal.Total_Freight) * 100 : 0;
        //            decimal SubTotaFreightByKg = SubTotalFreight > 0 && SubTotalRealQty > 0 ? (SubTotalFreight / SubTotalRealQty / 1000) : 0;

        //            var SubTotalIndex = (transporterIndex + transpoterCount) - 1;

        //            if(no == SubTotalIndex)
        //            {
        //                sb.Append("<tr style='background-color: #AFAAAA'>");
        //                sb.Append($@"<th colspan='4' style='{headerStyle}'>TOTAL {dataReport[transporterIndex].Transporter_Name}</th>");
        //                sb.Append($@"<th style='{headerStyle}'>{SubTotalRealQty.ToString("N0", CultureInfo.InvariantCulture)}</th>");
        //                sb.Append($@"<th style='{headerStyle}'>{SubTotalPercentageRealQty.ToString("0.00")}%</th>");
        //                sb.Append($@"<th style='{headerStyle}'>{SubTotalTruck.ToString("N0", CultureInfo.InvariantCulture)}</th>");
        //                sb.Append($@"<th style='{headerStyle}'>{SubTotalPercentageTruck.ToString("0.00")}%</th>");
        //                sb.Append($@"<th style='{headerStyle}'>{SubTotalAvgQtyByKg.ToString("N2", CultureInfo.InvariantCulture)}</th>");
        //                sb.Append($@"<th style='{headerStyle}'>{SubTotalFreight.ToString("N0", CultureInfo.InvariantCulture)}</th>");
        //                sb.Append($@"<th style='{headerStyle}'>{SubTotalPercentageFreight.ToString("0.00")}%</th>");
        //                sb.Append($@"<th style='{headerStyle}'>{SubTotaFreightByKg.ToString("N2", CultureInfo.InvariantCulture)}</th>");
        //                sb.Append("</tr>");
        //            }
        //        }

        //        no++;
        //    }

        //    //Grand Total
        //    if(dataReport.Count > 0)
        //    {
        //        sb.Append("<tr style='background-color: #AEC7E7'>");
        //        sb.Append($"<th colspan='4' style='{headerStyle}'>Grand Total</th>");
        //        sb.Append($"<th style='{headerStyle}'>{grandTotal.Total_Realization_Qty.ToString("N0", CultureInfo.InvariantCulture)}</th>");
        //        sb.Append($"<th style='{headerStyle}'>{grandTotal.Total_Percentage_Realization_Qty.ToString("0.00")}%</th>");
        //        sb.Append($"<th style='{headerStyle}'>{grandTotal.Total_Truck.ToString("N0", CultureInfo.InvariantCulture)}</th>");
        //        sb.Append($"<th style='{headerStyle}'>{grandTotal.Total_Percentage_Truck.ToString("0.00")}%</th>");
        //        sb.Append($"<th style='{headerStyle}'>{grandTotal.Total_Avg_Qty_By_Truck.ToString("N2", CultureInfo.InvariantCulture)}</th>");
        //        sb.Append($"<th style='{headerStyle}'>{grandTotal.Total_Freight.ToString("N0", CultureInfo.InvariantCulture)}</th>");
        //        sb.Append($"<th style='{headerStyle}'>{grandTotal.Total_Percentage_Freight.ToString("0.00")}%</th>");
        //        sb.Append($"<th style='{headerStyle}'>{grandTotal.Total_Freight_By_Kg.ToString("N2", CultureInfo.InvariantCulture)}</th>");
        //        sb.Append("</tr>");
        //    }

        //    strJSON = strJSON.Replace("@body", sb.ToString());
        //    strFoot = strFoot.Replace("@foot", tFoot.ToString());
        //}

        //public static void printDOVSDeliveryToCustomerReport(List<DOVSDeliveryToCustomerHeader> dataReport, DOVSDeliveryToCustomerReportModel model, string reportName, out string strHead, out string strJSON, out string strFoot)
        //{
        //    StringBuilder sReports = new StringBuilder();
        //    StringBuilder tHead = new StringBuilder();
        //    StringBuilder tFoot = new StringBuilder();
        //    StringBuilder sb = new StringBuilder();
        //    strJSON = "<tbody>@body</tbody>";
        //    strFoot = "<tfoot>@foot</tfoot>";

        //    strHead = $@"<thead class='header'>
        //                    <tr>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>No</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>Area</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>{DisplayNames.CUSTOMER}</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>{DisplayNames.LOADING}</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>{DisplayNames.DESTINATION}</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>{DisplayNames.BUSINESS_SEGMENT}</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>Qty Order</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>Realization Qty</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>UOM</th>
        //                        <th style='{headerStyle}background-color: #D9D9D9;'>%</th>
        //                    </tr>
        //                </thead>";


        //    headerStyle = headerStyle + ";mso-number-format:\"\\@\"";

        //    var no = 0;
        //    foreach(var data in dataReport)
        //    {
        //        sb.Append("<tr>");

        //        sb.Append($"<td style='{headerStyle}'>{no + 1}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Area}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Customer_Name}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{(model.Loading_Destination_From.Count == 0 ? "ALL" : data.Loading)}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{(model.Loading_Destination_To.Count == 0 ? "ALL" : data.Destination)}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Product_Category}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Qty_Order}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Realization_Qty}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.UOM}</td>");
        //        sb.Append($"<td style='{headerStyle}'>{data.Percentage.ToString("0.00")}%</td>");

        //        sb.Append("</tr>");

        //        //Sub Total
        //        if (model.Show_Sub_Total == "Y")
        //        {
        //            var area = dataReport.Where(r => r.Area == data.Area).ToList();
        //            var areaIndex = dataReport.FindIndex(r => r.Area == data.Area);
        //            var areaCount = area.Count();

        //            decimal Sub_Qty_Order = area.Select(r => r.Qty_Order).Sum(r => Convert.ToInt64(r));
        //            decimal Sub_Real_Qty = area.Select(r => r.Realization_Qty).Sum(r => Convert.ToInt64(r));
        //            decimal Sub_Percentage = Sub_Real_Qty > 0 ? (Sub_Real_Qty / Sub_Qty_Order) * 100 : 0;
        //            string Sub_UOM = String.Join(" | ", area.Select(r => r.UOM).Distinct().ToList());

        //            var SubTotalIndex = (areaIndex + areaCount) - 1;

        //            if (no == SubTotalIndex)
        //            {
        //                sb.Append("<tr style='background-color: #AFAAAA'>");
        //                sb.Append($@"<th colspan='6' style='{headerStyle}'>TOTAL {dataReport[areaIndex].Area}</th>");
        //                sb.Append($@"<th style='{headerStyle}'>{Sub_Qty_Order}</th>");
        //                sb.Append($@"<th style='{headerStyle}'>{Sub_Real_Qty}</th>");
        //                sb.Append($@"<th style='{headerStyle}'>{Sub_UOM}</th>");
        //                sb.Append($@"<th style='{headerStyle}'>{Sub_Percentage.ToString("0.00")}%</th>");
                        
        //                sb.Append("</tr>");
        //            }
        //        }

        //        no++;
        //    }

        //    //Grand Total
        //    if (dataReport.Count > 0)
        //    {
        //        decimal Total_Qty_Order = dataReport.Select(r => r.Qty_Order).Sum(r => Convert.ToInt64(r));
        //        decimal Total_Real_Qty = dataReport.Select(r => r.Realization_Qty).Sum(r => Convert.ToInt64(r));
        //        decimal Total_Percentage = Total_Real_Qty > 0 ? (Total_Real_Qty / Total_Qty_Order) * 100 : 0;

        //        List<string> UOM = dataReport.Select(r => r.UOM).Distinct().ToList();
        //        string Total_UOM = UOM.Count == 1 ? UOM.Select(r => r.ToString()).FirstOrDefault() : "";

        //        sb.Append("<tr style='background-color: #AEC7E7'>");
        //        sb.Append($"<th colspan='6' style='{headerStyle}'>Grand Total</th>");
        //        sb.Append($"<th style='{headerStyle}'>{Total_Qty_Order}</th>");
        //        sb.Append($"<th style='{headerStyle}'>{Total_Real_Qty}</th>");
        //        sb.Append($"<th style='{headerStyle}'>{Total_UOM}</th>");
        //        sb.Append($"<th style='{headerStyle}'>{Total_Percentage.ToString("0.00")}%</th>");
        //        sb.Append("</tr>");
        //    }

        //    strJSON = strJSON.Replace("@body", sb.ToString());
        //    strFoot = strFoot.Replace("@foot", tFoot.ToString());
        //}

        public static string printHeaderLabel(string reportName, string docDate,bool showTitle = false, bool display = false)
        {
            string strLabel = $@"<tr class='{(showTitle ? "" : "noLabel")}' style='background:white!important'>
                                <th style='background:white!important;font-size:22px'>
                                    {reportName}
                                </th>
                            </tr>
                            <tr class='{(display ? "" : "noLabel")}' style='background:white!important'><th style='background:white!important'></th></tr>
                            <tr class='{(display ? "" : "noLabel")}' style='background:white!important'>
                                <th style='background:white!important'>
                                    <table>
                                        <tr>
                                            <td style='{headerStyle}' colspan='2'>{DisplayNames.REPORT_GENERATE_DATE}</td>
                                            <td style='{headerStyle}' colspan='4'>{DateTime.Now.ToString("dd/MM/yyyy")}</td>
                                        </tr>
                                        <tr>
                                            <td style='{headerStyle}' colspan='2'>{DisplayNames.REPORT_DATE}</td>
                                            <td style='{headerStyle}' colspan='4'>{docDate}</td>
                                        </tr>                                        
                                    </table>
                                </th>
                            </tr>
                            <tr class='{(display ? "" : "noLabel")}' style='background:white!important'><th style='background:white!important'></th></tr>";

            return strLabel;
        }

        public static string printHeaderLabelTransporter(string reportName, Dictionary<string, string> reportHeaderLabel, bool showTitle = false, bool display = false)
        {
            int headerLabelCount = reportHeaderLabel.Count;
            int rowCount = 3;
            decimal columnCount = Math.Ceiling((decimal)headerLabelCount / rowCount);

            string strLabel = $@"<tr class='{(showTitle ? "" : "noLabel")}' style='background:white!important'>
                                <th style='background:white!important;font-size:22px'>
                                    {reportName}
                                </th>
                            </tr>
                            <tr class='{(display ? "" : "noLabel")}' style='background:white!important'><th style='background:white!important'></th></tr>
                            <tr class='{(display ? "" : "noLabel")}' style='background:white!important'>
                                <th style='background:white!important'>
                                    <table cellspacing='0'>";

                                        for(int i = 0; i < rowCount; i++)
                                        {
                                            strLabel += "<tr>";

                                            for(int j = 0; j < columnCount; j++)
                                            {
                                                int ij = (j * rowCount) + i;

                                                if(ij < headerLabelCount)
                                                {
                                                    strLabel += $@"
                                                         <td style='{headerStyle};background:#DDDDDD'>{reportHeaderLabel.ElementAt(ij).Key}</td>
                                                         <td style='{headerStyle}'>{reportHeaderLabel.ElementAt(ij).Value}</td>";
                                                }
                                            }

                                            strLabel += "</tr>";
                                        }
                    
                    strLabel += $@" </table>
                                </th>
                            </tr>
                            <tr class='{(display ? "" : "noLabel")}' style='background:white!important'><th style='background:white!important'></th></tr>";

            return strLabel;
        }

        public static string printPercentageColor(decimal percentage)
        {
            var style = "";

            if(percentage < 85)
            {
                //red
                style += $@"{headerStyle}background-color: red;color: white;";
            }
            else if(percentage >= 85 && percentage < 95)
            {
                //yellow
                style += $@"{headerStyle}background-color: yellow;color: black;";
            }
            else if(percentage >= 95)
            {
                //green
                style += $@"{headerStyle}background-color: #7DD259;color: black;";
            }
        
            return style;
        }

        public static string printHeaderLabel(string reportName, string fromDate, string toDate = "", bool showTitle = false, bool display = false, string modul = "", string dept = "", bool isGorupByMaterial = false)
        {
            DateTime from = new DateTime();
            DateTime to = new DateTime();

            var period = "";

            if (modul == "stock_diffrence" || modul == "cost_report" || modul == "usage_report" || modul == "stock_report")
            {
                period = fromDate;
            }
            else
            {
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    from = Convert.ToDateTime(fromDate);
                    to = Convert.ToDateTime(toDate);
                    period = from.ToString("dd/MM/yyyy") + " - " + to.ToString("dd/MM/yyyy");
                }
                else if (!string.IsNullOrEmpty(fromDate))
                {
                    from = Convert.ToDateTime(fromDate);
                    period = from.ToString("dd/MM/yyyy");
                }
                else if (!string.IsNullOrEmpty(toDate))
                {
                    to = Convert.ToDateTime(toDate);
                    period = to.ToString("dd/MM/yyyy");
                }
                else
                {
                    period = "";
                }
            }
            string strLabel = "";
            if (isGorupByMaterial)
                strLabel = $@"<tr class='{(showTitle ? "" : "noLabel")}' style='background:white!important;'>
                                    <th style='background:white!important;font-size:22px'>
                                        {reportName}
                                    </th>
                                </tr>
                                <tr class='{(display ? "" : "noLabel")}' style='background:white!important'><th style='background:white!important'></th></tr>
                                <tr class='{(display ? "" : "noLabel")}' style='background:white!important'>
                                    <th style='background:white!important'>
                                        <table style='word-wrap: break-word;'>
                                            <tr>
                                                <td style='{headerStyle}' colspan='2'>REPORT GENERATE DATE</td>
                                                <td style='{headerStyle}' colspan='4'>{DateTime.Now.ToString("dd/MM/yyyy")}</td>
                                            </tr>
                                            <tr>
                                                <td style='{headerStyle}' colspan='2'>PERIOD DATE</td>
                                                <td style='{headerStyle}' colspan='4'>{period}</td>
                                            </tr>
                                            <tr>
                                                <td style='{headerStyle}' colspan='2'>DEPARTMENT</td>
                                                <td style='{headerStyle};max-width: 60px;' colspan='4'>{(dept != "" ? dept : "ALL")}</td>
                                            </tr>
                                        </table>
                                    </th>
                                </tr>
                                <tr class='{(display ? "" : "noLabel")}' style='background:white!important'><th style='background:white!important'></th></tr>";
            else
                strLabel = $@"<tr class='{(showTitle ? "" : "noLabel")}' style='background:white!important'>
                                    <th style='background:white!important;font-size:22px'>
                                        {reportName}
                                    </th>
                                </tr>
                                <tr class='{(display ? "" : "noLabel")}' style='background:white!important'><th style='background:white!important'></th></tr>
                                <tr class='{(display ? "" : "noLabel")}' style='background:white!important'>
                                    <th style='background:white!important'>
                                        <table>
                                            <tr>
                                                <td style='{headerStyle}' colspan='2'>REPORT GENERATE DATE</td>
                                                <td style='{headerStyle}' colspan='4'>{DateTime.Now.ToString("dd/MM/yyyy")}</td>
                                            </tr>
                                            <tr>
                                                <td style='{headerStyle}' colspan='2'>PERIOD DATE</td>
                                                <td style='{headerStyle}' colspan='4'>{period}</td>
                                            </tr>
                                        </table>
                                    </th>
                                </tr>
                                <tr class='{(display ? "" : "noLabel")}' style='background:white!important'><th style='background:white!important'></th></tr>";
            return strLabel;
        }
        
        public static void DocumentReport(DataTable dt, out string strHead, out string strJSON, out string strFoot, string modul = "")
        {
            StringBuilder sb = new StringBuilder();
          
            strHead = $@"<thead class='header'><tr><th style='{headerStyle}'>No</th>";
            strFoot = "";
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName == "Row_Num") continue;
                strHead += $"<th style='{headerStyle}'>" + column.ColumnName + "</th>";
            }

            strHead += @"</tr></thead>";
            sb.Append("<tbody>");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                string style = borderStyle;
                string mStyle = borderStyle + ";mso-number-format:\"\\@\"";


                sb.Append("<tr>");
                for (int z = 0; z < dt.Columns.Count; z++)
                {
                    var aFields = dt.Columns;

                    Type type = row[aFields[z]].GetType();
                    string headerStyle = style;
                    string data = "";

                    if (aFields[z].ColumnName == "Row_Num")
                        data = (i + 1).ToString();

                    else if ((type == typeof(Double)) || (type == typeof(float)) || (type == typeof(Decimal)))
                    {
                        data = string.Format("{0:N0}", Convert.ToDecimal(row[aFields[z]]));
                        headerStyle += ";mso-number-format:#\\,##0";
                    }
                    else if (type == typeof(int))
                    {
                        data = string.Format("{0:N0}", (int)row[aFields[z]]);
                        headerStyle += ";mso-number-format:#\\,##0";
                    }
                    else if (type == typeof(DateTime))
                    {
                        if (!string.IsNullOrEmpty(row[aFields[z]].ToString()))
                            data = ((DateTime)row[aFields[z]]).ToString("dd/MM/yyyy");
                        headerStyle += ";text-align: center";
                    }
                    else if (type == typeof(Boolean))
                    {
                        var d = (Boolean)row[aFields[z]];
                        data = d ? "Y" : "N";
                    }
                    else if (type == typeof(Boolean?))
                    {
                        var d = (Boolean?)row[aFields[z]];

                        if (d.HasValue) data = d.Value ? "Y" : "N";
                        else data = "N";
                    }
                    else
                    {
                     
                            data = row[aFields[z]].ToString();
                    }

                        sb.Append($"<td style='{headerStyle}'>" + data + "</td>");
                }
                sb.Append("</tr>");
            }

            sb.Append("</tbody>");
            strJSON = sb.ToString();

          /*  strFoot = $"<tfoot style='{borderStyle}'>";
            if (modul == "pettycash")
                strFoot += $"<tr style='{footerStyle}'>" +
                                $"<td style='{headerStyle}; padding:5px' colspan=14>TOTAL</td>" +
                                $"<td style='{headerStyle}'>" + string.Format("{0:N0}", Convert.ToDecimal(total_biaya)) + "</td>" +
                           $"</tr>";
            else if (modul == "fuelusage")
                strFoot += $"<tr style='{footerStyle}'>" +
                           $"<td style='{headerStyle}; padding:5px' colspan=8>TOTAL</td>" +
                           $"<td style='{headerStyle}'>" + string.Format("{0:N0}", Convert.ToDecimal(total_biaya)) + "</td>" +
                           $"<td style='{headerStyle}'></td>" +
                           $"<td style='{headerStyle}'></td>" +
                           $"</tr>";

            strFoot += "</tfoot>";*/
        }

        public static void MovementReport(DataTable dt, out string strHead, out string strJSON, out string strFoot, string modul = "")
        {
            StringBuilder sb = new StringBuilder();

            strHead = $@"<thead class='header'><tr><th style='{headerStyle}'>No</th>";
            strFoot = "";
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName == "Row_Num") continue;
                strHead += $"<th style='{headerStyle}'>" + column.ColumnName + "</th>";
            }

            strHead += @"</tr></thead>";
            sb.Append("<tbody>");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                string style = borderStyle;
                string mStyle = borderStyle + ";mso-number-format:\"\\@\"";


                sb.Append("<tr>");
                for (int z = 0; z < dt.Columns.Count; z++)
                {
                    var aFields = dt.Columns;

                    Type type = row[aFields[z]].GetType();
                    string headerStyle = style;
                    string data = "";

                    if (aFields[z].ColumnName == "Row_Num")
                        data = (i + 1).ToString();

                    else if ((type == typeof(Double)) || (type == typeof(float)) || (type == typeof(Decimal)))
                    {
                        data = string.Format("{0:N0}", Convert.ToDecimal(row[aFields[z]]));
                        headerStyle += ";mso-number-format:#\\,##0";
                    }
                    else if (type == typeof(int))
                    {
                        data = string.Format("{0:N0}", (int)row[aFields[z]]);
                        headerStyle += ";mso-number-format:#\\,##0";
                    }
                    else if (type == typeof(DateTime))
                    {
                        if (!string.IsNullOrEmpty(row[aFields[z]].ToString()))
                            data = ((DateTime)row[aFields[z]]).ToString("dd/MM/yyyy");
                        headerStyle += ";text-align: center";
                    }
                    else if (type == typeof(Boolean))
                    {
                        var d = (Boolean)row[aFields[z]];
                        data = d ? "Y" : "N";
                    }
                    else if (type == typeof(Boolean?))
                    {
                        var d = (Boolean?)row[aFields[z]];

                        if (d.HasValue) data = d.Value ? "Y" : "N";
                        else data = "N";
                    }
                    else
                    {

                        data = row[aFields[z]].ToString();
                    }

                    sb.Append($"<td style='{headerStyle}'>" + data + "</td>");
                }
                sb.Append("</tr>");
            }

            sb.Append("</tbody>");
            strJSON = sb.ToString();

            /*  strFoot = $"<tfoot style='{borderStyle}'>";
              if (modul == "pettycash")
                  strFoot += $"<tr style='{footerStyle}'>" +
                                  $"<td style='{headerStyle}; padding:5px' colspan=14>TOTAL</td>" +
                                  $"<td style='{headerStyle}'>" + string.Format("{0:N0}", Convert.ToDecimal(total_biaya)) + "</td>" +
                             $"</tr>";
              else if (modul == "fuelusage")
                  strFoot += $"<tr style='{footerStyle}'>" +
                             $"<td style='{headerStyle}; padding:5px' colspan=8>TOTAL</td>" +
                             $"<td style='{headerStyle}'>" + string.Format("{0:N0}", Convert.ToDecimal(total_biaya)) + "</td>" +
                             $"<td style='{headerStyle}'></td>" +
                             $"<td style='{headerStyle}'></td>" +
                             $"</tr>";

              strFoot += "</tfoot>";*/
        }


    }
}
