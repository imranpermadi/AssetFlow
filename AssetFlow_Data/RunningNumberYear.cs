
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
    
public partial class RunningNumberYear
{

    public long Id { get; set; }

    public long RunningNumberId { get; set; }

    public Nullable<int> Year { get; set; }

    public Nullable<int> LastNumber { get; set; }



    public virtual RunningNumber RunningNumber { get; set; }

}

}
