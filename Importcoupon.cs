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
    
    public partial class Importcoupon
    {
        public Importcoupon()
        {
            this.Detailimportcoupons = new HashSet<Detailimportcoupon>();
        }
    
        public int ImportID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public string ImportName { get; set; }
        public string Photo { get; set; }
    
        public virtual ICollection<Detailimportcoupon> Detailimportcoupons { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}