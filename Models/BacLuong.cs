//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Manament_Store_API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BacLuong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BacLuong()
        {
            this.HDLaoDongs = new HashSet<HDLaoDong>();
            this.Luongs = new HashSet<Luong>();
        }
    
        public int Bac { get; set; }
        public decimal LuongCoBan { get; set; }
        public int MaNhanVien { get; set; }
    
        public virtual NhanVien NhanVien { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HDLaoDong> HDLaoDongs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Luong> Luongs { get; set; }
    }
}
