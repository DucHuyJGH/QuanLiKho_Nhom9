using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class ListCTOutput
    {
        public int DetailBillID { get; set; }
        public int BillID { get; set; }
        public string ProductID { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
       
    }
}