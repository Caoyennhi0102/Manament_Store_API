using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Models
{
    public class BankApiResponse
    {
        public int Code { get; set; }
        public List<BankInfo> Data { get; set; }
    }
}