
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
    
public partial class ModuleTaskDetail
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ModuleTaskDetail()
    {

        this.RequestTask = new HashSet<RequestTask>();

    }


    public long Id { get; set; }

    public long ModuleTaskId { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public string Mandatory { get; set; }

    public string PIC { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

    public string EditedBy { get; set; }

    public Nullable<System.DateTime> EditedDate { get; set; }

    public string IsDeleted { get; set; }

    public string DeletedBy { get; set; }

    public Nullable<System.DateTime> DeletedDate { get; set; }



    public virtual ModuleTask ModuleTask { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<RequestTask> RequestTask { get; set; }

}

}
