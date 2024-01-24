﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class ExOutput
    {
        public string Photo { get; set; }
        public int DetailsOutputID { get; set; }
        public string ProductID { get; set; }
        public int CategoryID { get; set; }
        public int BillID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
        public string Unit { get; set; }
        public int Quantityout { get; set; }
        public int Price { get; set; }
        public int Tolalpriceou { get; set; }
    }
}