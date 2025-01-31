using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class IsDate
    {
        public bool IsValidDateTime(DateTime inputDateTime)
        {
            // Lấy ngày giờ hiện tại
            DateTime today = DateTime.Today;
            if(inputDateTime.Date < today)
            {
                Console.WriteLine("Lỗi: Không thể chọn ngày giờ trong quá khứ!");
                return false;
            }
            return true;
        }
    }
}