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
    
    public partial class User
    {
        public int UserID { get; set; }
        public int MaNhanVien { get; set; }
        public string UserName { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public Nullable<System.DateTime> TGDangNhap { get; set; }
        public Nullable<System.DateTime> TGDangXuat { get; set; }
        public bool Locked { get; set; }
        public string DiaChiIP { get; set; }
        public Nullable<bool> DNLanDau { get; set; }
        public string TrangThai { get; set; }
        public string Roles { get; set; }
        public string MaQuyen { get; set; }
        public Nullable<int> MaNVQL { get; set; }
        public string TrangThaiDuyetQL { get; set; }
        public Nullable<System.DateTime> NgayCapNhat { get; set; }
    
        public virtual NhanVien NhanVien { get; set; }
        public virtual Quyen Quyen { get; set; }
    }
}
