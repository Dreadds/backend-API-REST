
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
    
public partial class District
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public District()
    {

        this.Location = new HashSet<Location>();

    }


    public int DistrictId { get; set; }

    public string Name { get; set; }

    public string State { get; set; }

    public int ProvinceId { get; set; }



    public virtual Province Province { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Location> Location { get; set; }

}

}
