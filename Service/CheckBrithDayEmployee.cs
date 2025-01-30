using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class CheckBrithDayEmployee
    {
        public bool CheckMinimumAge(DateTime ngaySinh)
        {
            // Lấy ngày hiện tại
            DateTime today = DateTime.Today;

            // Tính tuổi của nhân viên bằng cách lấy chênh lệch giữa ngày hiện tại và ngày sinh
            int age = today.Year - ngaySinh.Year;

            // Kiểm tra nếu ngày sinh đã qua năm sinh nhật của nhân viên trong năm nay
            if (ngaySinh.Date > today.AddYears(-age))
            {
                age--;
            }

            // Kiểm tra nếu tuổi của nhân viên đã đủ 18 tuổi
            return age >= 18;
        }
    }
}