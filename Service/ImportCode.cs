using Manament_Store_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class ImportCode
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public ImportCode()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }

        public string GenerateImportCode()
        {
            var lastRecord = _sqlConnectionServer.PhieuNhapHangs.OrderByDescending(p => p.MaPhieuNhap)
                .FirstOrDefault();

            int nextNumber = 1;

            if(lastRecord != null)
            {
                string numberPart = lastRecord.MaPhieuNhap.Substring(3);
                int lastNumber;
                if(int.TryParse(numberPart, out lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }

            }
            return "NH_" + nextNumber.ToString("D5");
        }
    }
}