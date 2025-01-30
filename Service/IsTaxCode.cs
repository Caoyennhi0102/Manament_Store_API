using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Manament_Store_API.Service
{
    public class IsTaxCode
    {
        public bool CheckTaxCode(string taxCode)
        {
            if (string.IsNullOrEmpty(taxCode))
            {
                return false;
            }
            string pattern = @"^\d{10}$|^\d{14}$";
            return Regex.IsMatch(taxCode, pattern);
        }
    }
}