using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Manament_Store_API.Service
{
    public class IsCheckAccountBankSupplier
    {
        public bool IsBankAccountValid(string bankAccount)
        {
            int minLength = 9;
            if(string.IsNullOrWhiteSpace(bankAccount) || bankAccount.Length < minLength)
            {
                return false;
            }
            string pattern = @"^\d{8,15}$";
            return Regex.IsMatch(bankAccount, pattern);
        }
    }
}