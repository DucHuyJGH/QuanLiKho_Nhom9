using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class Profitandloss
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public DateTime date { get; set; }
        public int QuantityIm { get; set; }
        public int QuantityOut { get; set; }
        public int total { get; set; }
        public int totalPricein { get; set; }
        public int totalPriceou { get; set; }
        public int totalPrice { get; set; }
        public int totalPricetotal { get; set; }

    }
}