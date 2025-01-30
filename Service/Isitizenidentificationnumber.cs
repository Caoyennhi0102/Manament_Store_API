using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class Isitizenidentificationnumber
    {
        // Phương thức kiểm tra số căn cước công dân 
        public bool Checkcitizenidentificationnumber(string cccd)
        {
            // Kiểm tra CCCD không được null hoặc rỗng
            if (string.IsNullOrEmpty(cccd))
            {
                return false;
            }
            // Kiểm tra độ dài CCCD
            if (cccd.Length != 12)
            {
                return false;
            }
            // Kiểm tra CCCD chỉ chứa số
            if (!cccd.All(char.IsDigit))
            {
                return false;
            }
            return true;
        }
    }
}