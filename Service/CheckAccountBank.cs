using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class CheckAccountBank
    {
        public bool CheckSalaryAccount(string salaryAccount)
        {
            // Kiểm tra nếu tài khoản lương là null hoặc trống
            if (string.IsNullOrEmpty(salaryAccount))
            {
                return false;
            }
            // Kiểm tra nếu tài khoản lương có độ dài là 8 và chỉ chứa các ký tự số
            if (salaryAccount.Length == 8 && salaryAccount.All(char.IsDigit))
            {
                // Tài khoản lương hợp lệ
                return true;
            }
            // Tài khoản lương không hợp lệ
            return false;
        }
    }
}