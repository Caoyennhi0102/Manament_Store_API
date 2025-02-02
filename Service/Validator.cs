using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Manament_Store_API.Service
{
    public class Validator
    {
        public bool CheckFaxNumber(string faxNumber)
        {
            int minLength = 6;

            if (string.IsNullOrWhiteSpace(faxNumber) || faxNumber.Length < minLength)
            {
                return false;
            }

            string pattern = @"^[0-9][0-9\s\-]{5,}$";
            return Regex.IsMatch(faxNumber, pattern);
        }
    }
}