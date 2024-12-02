using FAMS_Models.Base;
using System;

namespace FAMS_Models
{
    public class AssetModel : BaseModel
    {
        public long Id { get; set; }

        public string SAPNo { get; set; }

        public string SAPSubNo { get; set; }

        public string SAPLocation { get; set; }

        public Nullable<int> SAPYear { get; set; }

        public System.DateTime SAPDeactivationDate { get; set; }

        public string SAPCompanyCode { get; set; }

        public string SAPPlant { get; set; }

        public string SAPCostCenter { get; set; }

        public Nullable<System.DateTime> SAPAppropriationDate { get; set; }

        public string AssetNo { get; set; }

        public string AssetType { get; set; }

        public string AssetName { get; set; }

        public string Location { get; set; }

        public string WBS { get; set; }

        public string AFCE { get; set; }

        public string CalculationType { get; set; }

        public Nullable<double> Qty { get; set; }

        public string UoM { get; set; }

        public Nullable<double> Conversion { get; set; }

        public Nullable<double> Price { get; set; }

        public string PriceUoM { get; set; }

        public string Currency { get; set; }

        public string Ownership { get; set; }

        public string Owner { get; set; }

        public string Remarks { get; set; }

        public string CertificateNo { get; set; }

        public string CertificateType { get; set; }

        public string CertificateAttachment { get; set; }

        public string FLStorageName { get; set; }

        public Nullable<long> FLDocId { get; set; }
    }
}
