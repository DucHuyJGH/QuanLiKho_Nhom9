using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class Report
    {
        public string Photo { get; set; }
        public string ProductID { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
        public string Unit { get; set; }
        //public DateTime dateinput { get; set; }
        public int QuantityIm { get; set; }
        public int QuantityOut { get; set; }
        public int total { get; set; }
        public int TotalPricein { get; set; }
        public int TotalPriceou { get; set; }
        public int TotalPriceTotal { get; set; }
    }
}