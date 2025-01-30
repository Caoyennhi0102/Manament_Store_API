using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manament_Store_API.Service
{
    public class CodePermissions
    {
        public string CreateCodePermissions(string tenQuyen)
        {
            if (string.IsNullOrWhiteSpace(tenQuyen))
            {
                return "Tên quyền không được để trống.";
            }
            var words = tenQuyen.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var codePermissions = string.Concat(words.Select(w => w[0])).ToUpper();
            return codePermissions;
        }
    }
}