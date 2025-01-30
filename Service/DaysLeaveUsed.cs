using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class DaysLeaveUsed
    {
        private readonly SqlConnectionServer _sqlConnectionServer;
        public DaysLeaveUsed()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }

        public int CalculateUsedLeave(int manhanvien)
        {
            var usedLeaveRecords = _sqlConnectionServer.YeuCauNghiPheps
                .Where(lr => lr.MaNhanVien == manhanvien && lr.TrangThai == "Đã Duyệt")
                .Select(lr => lr.SoNgayNghi).ToList();
            int totalUsedLeave = usedLeaveRecords.Sum();
            return totalUsedLeave;
        }
        public int CalculateRemainingLeave(int totalAnnualLeave, int manhanvien)
        {
            int usedLeave = CalculateUsedLeave(manhanvien);
            return totalAnnualLeave - usedLeave;
        }
    }
}