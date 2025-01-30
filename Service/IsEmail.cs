using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class IsEmail
    {
        public bool CheckEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
            {
                return false;
            }
            return true;
        }
    }
}