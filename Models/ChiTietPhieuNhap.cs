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
    
    public partial class ChiTietPhieuNhap
    {
        public string MaPhieuNhap { get; set; }
        public string MaHangHoa { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public System.DateTime NgayHetHan { get; set; }
        public System.DateTime NgaySanXuat { get; set; }
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string DonViTinh { get; set; }
        public int MaDonDatHang { get; set; }
    
        public virtual HangHoa HangHoa { get; set; }
        public virtual PhieuNhapHang PhieuNhapHang { get; set; }
    }
}
