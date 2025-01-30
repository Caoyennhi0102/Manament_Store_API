using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class CheckAddresss
    {
        public bool CheckAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                // Địa chỉ không được để trống
                return false;
            }
            if (address.Length < 10)
            {
                // Địa chỉ quá ngắn
                return false;
            }
            string allowedCharsPattern = @"^[a-zA-Z0-9\s,.-]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(address, allowedCharsPattern))
            {
                // Địa chỉ chứa ký tự không hợp lệ
                return false;
            }
            return true;

        }
    }
}