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
    
    public partial class DetailBill
    {
        public int DetailBillID { get; set; }
        public Nullable<int> BillID { get; set; }
        public string ProductID { get; set; }
        public string Unit { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Note { get; set; }
        public int Price { get; set; }
    
        public virtual Bill Bill { get; set; }
        public virtual Product Product { get; set; }
    }
}
