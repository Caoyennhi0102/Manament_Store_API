using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Manament_Store_API.Service
{
    public class WebsiteValidator
    {
        public bool CheckWebsite(string website)
        {
            int minLength = 5;
            if(string.IsNullOrWhiteSpace(website) || website.Length <  minLength)
            {
                return false;
            }

            string pattern = @"^(https?:\/\/)?([\w\-]+\.)+[\w]{2,}(:\d{1,5})?(\/\S*)?$";
            return Regex.IsMatch(website, pattern, RegexOptions.IgnoreCase);

        }
    }
}