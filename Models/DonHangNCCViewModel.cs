using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Models
{
    public class DonHangNCCViewModel
    {
        public DonHangNCC DonHangNCC { get; set; }
        public List<ChiTietDonHangNCC> ChiTietDonHangNCCs { get; set; }

        public DonHangNCCViewModel()
        {
            ChiTietDonHangNCCs = new List<ChiTietDonHangNCC>();
        }
    }
}