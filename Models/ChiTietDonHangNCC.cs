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
    using System.ComponentModel.DataAnnotations;

    public partial class ChiTietDonHangNCC
    {
        [Display(Name = "Mã Chi tiết Đơn Đặt Hàng")]
        public int MaChiTietDonHang { get; set; }
        [Display(Name = "Mã Đơn Đặt Hàng")]
        public int MaDatHang { get; set; }
        [Display(Name = "Mã Sản Phẩm")]
        public string MaSanPham { get; set; }
        [Display(Name = "Tên Sản Phẩm")]
        public string TenSanPham { get; set; }
        [Display(Name = "Đơn Vị Tính")]
        public string DonViTinh { get; set; }
        [Display(Name = "Số Lượng")]
        public int SoLuong { get; set; }
        [Display(Name = "Đơn Giá")]
        public decimal DonGia { get; set; }
        [Display(Name = "Chiết Khấu")]
        public decimal ChietKhau { get; set; }
        [Display(Name = "Thành Tiền")]
        [DisplayFormat(DataFormatString ="{0:N0} VND")]
        public decimal ThanhTien { get; set; }
        [Display(Name = "Ngày Sản Xuất")]
        public System.DateTime NgaySX { get; set; }
        [Display(Name = "Ngày Hết Hạn")]
        public System.DateTime NgayHH { get; set; }
    
        public virtual DonHangNCC DonHangNCC { get; set; }
    }
}
