
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
    
public partial class ModuleType
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ModuleType()
    {

        this.ModuleTypeDetail = new HashSet<ModuleTypeDetail>();

    }


    public long Id { get; set; }

    public long ModuleId { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

    public string EditedBy { get; set; }

    public Nullable<System.DateTime> EditedDate { get; set; }

    public string IsDeleted { get; set; }

    public string DeletedBy { get; set; }

    public Nullable<System.DateTime> DeletedDate { get; set; }



    public virtual Module Module { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ModuleTypeDetail> ModuleTypeDetail { get; set; }

}

}