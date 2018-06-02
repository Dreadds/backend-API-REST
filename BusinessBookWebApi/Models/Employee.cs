//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessBookWebApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.Company = new HashSet<Company>();
            this.Sale = new HashSet<Sale>();
        }
    
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string DNI { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public string State { get; set; }
        public string Sex { get; set; }
        public string Users { get; set; }
        public string Password { get; set; }
        public Nullable<int> TokenEmployeeId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Company> Company { get; set; }
        public virtual Location Location { get; set; }
        public virtual TokenEmployee TokenEmployee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sale> Sale { get; set; }
    }
}
