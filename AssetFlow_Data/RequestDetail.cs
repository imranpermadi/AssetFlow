
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace FAMS_Data
{

using System;
    using System.Collections.Generic;
    
public partial class RequestDetail
{

    public long Id { get; set; }

    public long RequestId { get; set; }

    public string Status { get; set; }

    public string SAPNo { get; set; }

    public string SAPSubNo { get; set; }

    public string SAPLocation { get; set; }

    public Nullable<int> SAPYear { get; set; }

    public System.DateTime SAPDeactivationDate { get; set; }

    public string SAPCompanyCode { get; set; }

    public string SAPPlant { get; set; }

    public string SAPCostCenter { get; set; }

    public Nullable<System.DateTime> SAPAppropriationDate { get; set; }

    public string SAPTcode { get; set; }

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

    public string FLCompanyCode { get; set; }

    public string FLDocCategory { get; set; }

    public string FLRemarks { get; set; }

    public Nullable<System.DateTime> FLDocDate { get; set; }

    public string FLBrand { get; set; }

    public string FLRightHolder { get; set; }

    public string FLPublisher { get; set; }

    public string FLObjectLocation { get; set; }

    public string FLPlateNo { get; set; }

    public string FLFrameNo { get; set; }

    public string FLMachineNo { get; set; }

    public string FLOrigin { get; set; }

    public Nullable<System.DateTime> FLPlanDate { get; set; }

    public string FLStorageName { get; set; }

    public Nullable<long> FLStorageId { get; set; }

    public string FLIsParent { get; set; }

    public Nullable<long> FLParentId { get; set; }

    public string FLParentNo { get; set; }

    public Nullable<long> FLDocId { get; set; }

    public string FLType { get; set; }

    public Nullable<long> FLStorageLocationToId { get; set; }

    public string FLTransferNo { get; set; }

    public string DestAssetNo { get; set; }

    public string DestSAPNo { get; set; }

    public string DestSAPSubNo { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

    public string EditedBy { get; set; }

    public Nullable<System.DateTime> EditedDate { get; set; }

    public string IsDeleted { get; set; }

    public string DeletedBy { get; set; }

    public Nullable<System.DateTime> DeletedDate { get; set; }



    public virtual Request Request { get; set; }

}

}
