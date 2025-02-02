﻿//------------------------------------------------------------------------------
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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class HopDong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HopDong()
        {
            this.NhaCungCaps = new HashSet<NhaCungCap>();
        }

        [Display(Name ="Mã Hợp Đồng")]
        public string MaHopDong { get; set; }
        [Display(Name ="Tên Hợp Đồng")]
        public string TenHopDong { get; set; }
        [Display(Name ="Nội Dung")]
        public string NoiDung { get; set; }
        [Display(Name = "Ngày Lưu")]
        public System.DateTime NgayLuu { get; set; }
        [Display(Name = "Đường Dẫn")]
        public string DuongDan { get; set; }
        [Display(Name = "Thời Hạn Hợp Đồng")]
        public System.DateTime ThoiHanHop { get; set; }
        [Display(Name = "Ngày Ký Hợp Đồng")]
        public System.DateTime NgayKyHopDong { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NhaCungCap> NhaCungCaps { get; set; }
    }
}
