
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
    
public partial class ReleaseMatrixDetail
{

    public long Id { get; set; }

    public long ReleaseId { get; set; }

    public string ReleaseAs { get; set; }

    public long UserId { get; set; }

    public int ApproverLevel { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

    public string EditedBy { get; set; }

    public Nullable<System.DateTime> EditedDate { get; set; }

    public string IsDeleted { get; set; }

    public string DeletedBy { get; set; }

    public Nullable<System.DateTime> DeletedDate { get; set; }



    public virtual ReleaseMatrix ReleaseMatrix { get; set; }

    public virtual User User { get; set; }

}

}
