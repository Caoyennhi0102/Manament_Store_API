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
    
    public partial class KhuyenMai
    {
        public int MaKhuyenMai { get; set; }
        public string MaHangHoa { get; set; }
        public string TenCTKM { get; set; }
        public decimal PhanTramGG { get; set; }
        public System.DateTime NgayBD { get; set; }
        public System.DateTime NgayKT { get; set; }
    
        public virtual HangHoa HangHoa { get; set; }
    }
}
