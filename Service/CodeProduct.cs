using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Manament_Store_API.Models;
namespace Manament_Store_API.Service
{
    public class CodeProduct
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public CodeProduct()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        public string GenerateMaHangHoa()
        {
            var lastRecord = _sqlConnectionServer.HangHoas.OrderByDescending(h => h.MaHangHoa).FirstOrDefault();
            int nextNumber = 1;
            if(lastRecord != null && !string.IsNullOrEmpty(lastRecord.MaHangHoa))
            {
                string numberPart = lastRecord.MaHangHoa.Substring(3);
                if(int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }

            }
            return "HH_" + nextNumber.ToString("D5");
        }
    }
}