using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class EmployeeID
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public EmployeeID()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        public int GenerateEmployeeID()
        {
            // Lấy mã nhân viên cao nhất hiện tại
            var maxID = _sqlConnectionServer.NhanViens.Select(nv => nv.MaNhanVien).
                DefaultIfEmpty(0).Max();

            // Tăng mã lên 1
            int newID = maxID + 1;

            // Kiểm tra nếu mã vượt quá 5 chữ số
            if (newID > 99999)
            {
                throw new InvalidOperationException("Mã nhân viên đã vượt quá giới hạn 5 chữ số!");
            }

            return newID;

        }
        //Phương thức định dạng mã nhân viên 
        public string FormatEmployeeID(int employeeID)
        {
            // Chuyển mã nhân viên thành chuỗi 5 chữ số
            return employeeID.ToString("D5");
        }
    }
}