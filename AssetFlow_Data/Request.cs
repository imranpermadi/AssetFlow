
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
    
public partial class Request
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Request()
    {

        this.RequestAttachment = new HashSet<RequestAttachment>();

        this.RequestDetail = new HashSet<RequestDetail>();

        this.RequestTask = new HashSet<RequestTask>();

    }


    public long Id { get; set; }

    public string RequestNo { get; set; }

    public string ModuleCode { get; set; }

    public string ModuleTypeCode { get; set; }

    public string ModuleTypeDetailCode { get; set; }

    public string ModuleTaskCode { get; set; }

    public string Status { get; set; }

    public Nullable<System.DateTime> DocumentDate { get; set; }

    public string Text { get; set; }

    public Nullable<System.DateTime> SettlementDate { get; set; }

    public Nullable<System.DateTime> CancelledDate { get; set; }

    public string CreatedName { get; set; }

    public string CreatedCompany { get; set; }

    public string CreatedWorkLocation { get; set; }

    public string CreatedDepartment { get; set; }

    public string CreatedPhone { get; set; }

    public string AuthCompanyCode { get; set; }

    public string AuthLocationCode { get; set; }

    public string AuthDepartmentCode { get; set; }

    public string PoaRequestNo { get; set; }

    public string PoaStatus { get; set; }

    public string PoaType { get; set; }

    public string PoaOther { get; set; }

    public string PoaReceiver { get; set; }

    public string PoaName { get; set; }

    public string PoaEmail { get; set; }

    public string PoaContractNo { get; set; }

    public string PoaPosition { get; set; }

    public string PoaCompany { get; set; }

    public string PoaLocation { get; set; }

    public Nullable<System.DateTime> PoaValidTo { get; set; }

    public string PoaCurrency { get; set; }

    public Nullable<decimal> PoaAmount { get; set; }

    public string PoaVendor { get; set; }

    public string PoaRemarks { get; set; }

    public string PoaAttachment { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

    public string EditedBy { get; set; }

    public Nullable<System.DateTime> EditedDate { get; set; }

    public string IsDeleted { get; set; }

    public string DeletedBy { get; set; }

    public Nullable<System.DateTime> DeletedDate { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<RequestAttachment> RequestAttachment { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<RequestDetail> RequestDetail { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<RequestTask> RequestTask { get; set; }

}

}