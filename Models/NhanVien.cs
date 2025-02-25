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
    
    public partial class NhanVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhanVien()
        {
            this.BacLuongs = new HashSet<BacLuong>();
            this.Congs = new HashSet<Cong>();
            this.HDLaoDongs = new HashSet<HDLaoDong>();
            this.HoaDons = new HashSet<HoaDon>();
            this.KPIs = new HashSet<KPI>();
            this.Luongs = new HashSet<Luong>();
            this.PhieuNhapHangs = new HashSet<PhieuNhapHang>();
            this.PhieuTraHangs = new HashSet<PhieuTraHang>();
            this.Pos = new HashSet<Pos>();
            this.QLCaNVs = new HashSet<QLCaNV>();
            this.ThongKeBanHangs = new HashSet<ThongKeBanHang>();
            this.Users = new HashSet<User>();
            this.YeuCauNghiPheps = new HashSet<YeuCauNghiPhep>();
            this.CuaHangs = new HashSet<CuaHang>();
            this.DonHangNCCs = new HashSet<DonHangNCC>();
        }
    
        public int MaNhanVien { get; set; }
        public int MaCuaHang { get; set; }
        public string MaBoPhan { get; set; }
        public string MaChucVu { get; set; }
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public System.DateTime NgaySinh { get; set; }
        public string CCCD { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        public System.DateTime NgayVaoLam { get; set; }
        public int NgayPhepNam { get; set; }
        public Nullable<int> SoPhepDaDung { get; set; }
        public string TKNhanLuong { get; set; }
        public string TrangThai { get; set; }
        public Nullable<int> SoNgayPhepConLai { get; set; }
        public string TrangThaiDuyetQL { get; set; }
        public string TrinhDoHocVan { get; set; }
        public string DiaChiTamtru { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BacLuong> BacLuongs { get; set; }
        public virtual BoPhan BoPhan { get; set; }
        public virtual ChucVu ChucVu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cong> Congs { get; set; }
        public virtual CuaHang CuaHang { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HDLaoDong> HDLaoDongs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KPI> KPIs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Luong> Luongs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuNhapHang> PhieuNhapHangs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuTraHang> PhieuTraHangs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pos> Pos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QLCaNV> QLCaNVs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThongKeBanHang> ThongKeBanHangs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<YeuCauNghiPhep> YeuCauNghiPheps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CuaHang> CuaHangs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHangNCC> DonHangNCCs { get; set; }
    }
}
