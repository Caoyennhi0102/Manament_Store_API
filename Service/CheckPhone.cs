using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Manament_Store_API.Service
{
    public class CheckPhone
    {
        public bool IsPhoneNumberValid(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }
            if (phoneNumber.StartsWith("84"))
            {
                string withoutCountryCode = phoneNumber.Substring(2);
                return withoutCountryCode.Length == 9 && Regex.IsMatch(withoutCountryCode, @"^\d{9}$");
            }
            if (phoneNumber.StartsWith("028"))
            {
                string withoutCountryCode = phoneNumber.Substring(3);
                return withoutCountryCode.Length == 8 && Regex.IsMatch(withoutCountryCode, @"^\d{8}$");
            }
            if (phoneNumber.StartsWith("0"))
            {
                return phoneNumber.Length == 10 && Regex.IsMatch(phoneNumber, @"^\d{10}$");
            }
            return false;
        }
    }
}