//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Manament_Store_API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BanDich
    {
        public int IDBanDich { get; set; }
        public int IDNgonNgu { get; set; }
        public string KhoaDich { get; set; }
        public string GiaTriDich { get; set; }
    
        public virtual NgonNgu NgonNgu { get; set; }
    }
}
