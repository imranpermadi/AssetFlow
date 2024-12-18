
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
    
public partial class User
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public User()
    {

        this.ReleaseMatrixDetail = new HashSet<ReleaseMatrixDetail>();

        this.UserGroup = new HashSet<UserGroup>();

        this.UserPassLog = new HashSet<UserPassLog>();

        this.UserRole = new HashSet<UserRole>();

        this.UserCompany = new HashSet<UserCompany>();

        this.UserDepartment = new HashSet<UserDepartment>();

        this.UserLocation = new HashSet<UserLocation>();

    }


    public long Id { get; set; }

    public string Username { get; set; }

    public string Fullname { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public string LocationCode { get; set; }

    public string DepartmentCode { get; set; }

    public string IsAdmin { get; set; }

    public string UseAD { get; set; }

    public Nullable<System.DateTime> LastAccessDate { get; set; }

    public string FirstLogin { get; set; }

    public string Language { get; set; }

    public string CreatedBy { get; set; }

    public System.DateTime CreatedDate { get; set; }

    public string EditedBy { get; set; }

    public Nullable<System.DateTime> EditedDate { get; set; }

    public string IsDeleted { get; set; }

    public Nullable<System.DateTime> DeletedDate { get; set; }

    public string DeletedBy { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ReleaseMatrixDetail> ReleaseMatrixDetail { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UserGroup> UserGroup { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UserPassLog> UserPassLog { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UserRole> UserRole { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UserCompany> UserCompany { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UserDepartment> UserDepartment { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UserLocation> UserLocation { get; set; }

}

}
