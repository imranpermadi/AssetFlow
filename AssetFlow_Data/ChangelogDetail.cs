
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
    
public partial class ChangelogDetail
{

    public long Id { get; set; }

    public long ChangelogId { get; set; }

    public string Description { get; set; }

    public string Info1 { get; set; }

    public System.DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<System.DateTime> EditedDate { get; set; }

    public string EditedBy { get; set; }

    public string IsDeleted { get; set; }

    public Nullable<System.DateTime> DeletedDate { get; set; }

    public string DeletedBy { get; set; }



    public virtual Changelog Changelog { get; set; }

}

}
