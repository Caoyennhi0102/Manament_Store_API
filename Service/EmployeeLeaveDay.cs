using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class EmployeeLeaveDay
    {
        public int CalculateAnnualLeave(DateTime startDate, int totalAnnualLeave = 12)
        {
            if (startDate > DateTime.Now)
            {
                throw new ArgumentException("Ngày bắt đầu làm việc không thể là ngày trong tương lai.", nameof(startDate));
            }
            int currentYear = DateTime.Now.Year;
            if (startDate.Year > currentYear)
            {
                return 0;
            }
            int startMonth = startDate.Year < currentYear ? 1 : startDate.Month;
            int remainingMonths = 12 - startMonth + 1;

            int annualLeave = (remainingMonths * totalAnnualLeave) / 12;
            return annualLeave;
        }
    }
}