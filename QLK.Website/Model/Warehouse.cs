//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLK.Website.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Warehouse
    {
        public Warehouse()
        {
            this.Bills = new HashSet<Bill>();
            this.Importcoupons = new HashSet<Importcoupon>();
        }
    
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public Nullable<double> Soluong { get; set; }
    
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<Importcoupon> Importcoupons { get; set; }
    }
}
