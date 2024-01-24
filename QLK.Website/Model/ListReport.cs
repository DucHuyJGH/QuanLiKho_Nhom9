using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class ListReport
    {
        QLKEntities db = new QLKEntities();
        List<Report> Reports = new List<Report>();
        List<TotalPriceImport> TotalPriceImports = new List<TotalPriceImport>();
        List<TotalPricebill> TotalPricebills = new List<TotalPricebill>();
        public List<Report> ListRp()
        {
            var detailsim = db.Detailimportcoupons.ToList();
            foreach (var iem in detailsim)
            {
                var g = db.Detailimportcoupons.Where(o => o.DetailimportID == iem.DetailimportID).ToList();
                TotalPriceImports.Add(new TotalPriceImport
                {
                    ID = iem.DetailimportID,
                    IDProduct = iem.ProductID,
                    total = iem.Price * (int)iem.Quantity,
                });
            }

            var detailsout = db.DetailBills.ToList();
            foreach (var iem in detailsout)
            {
                var g = db.DetailBills.Where(o => o.DetailBillID == iem.DetailBillID).ToList();
                TotalPricebills.Add(new TotalPricebill
                {
                    ID = iem.DetailBillID,
                    IDProduct = iem.ProductID,
                    total = iem.Price * (int)iem.Quantity,
                });
            }

            var temp = db.Products.ToList(); 
            foreach (var ovbj in temp)
            {
               
                var Debill = db.DetailBills.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                var Deimport = db.Detailimportcoupons.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
               


                if ( Deimport != null  )
                {
                   
                    if (Debill != null)
                    {   
                        Reports.Add(new Report
                        {

                            Photo = ovbj.Photo,
                            ProductID = ovbj.ProductID,
                            CategoryID = (int)ovbj.CategoryID,
                            ProductName = ovbj.ProductName,
                            SupplierName = ovbj.Supplier.SupplierName,
                            CategoryName = ovbj.Category.CategoryName,
                            Unit = ovbj.Unit,
                            QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == Deimport.ProductID).Sum(o => o.Quantity),
                            QuantityOut = (int)db.DetailBills.Where(o => o.ProductID == Debill.ProductID).Sum(o => o.Quantity),
                            total = (int)db.Detailimportcoupons.Where(o => o.ProductID == Deimport.ProductID).Sum(o => o.Quantity) - (int)db.DetailBills.Where(o => o.ProductID == Debill.ProductID).Sum(o => o.Quantity),
                            TotalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            TotalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            TotalPriceTotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),

                        });
                    }
                   else
                    {
                        Reports.Add(new Report
                        {

                            Photo = ovbj.Photo,
                            ProductID = ovbj.ProductID,
                            CategoryID = (int)ovbj.CategoryID,
                            ProductName = ovbj.ProductName,
                            SupplierName = ovbj.Supplier.SupplierName,
                            CategoryName = ovbj.Category.CategoryName,
                            //dateinput = (DateTime)inp.Date,
                            Unit = ovbj.Unit,
                            QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == Deimport.ProductID).Sum(o => o.Quantity),
                            QuantityOut = 0,
                            total = (int)db.Detailimportcoupons.Where(o => o.ProductID == Deimport.ProductID).Sum(o => o.Quantity),
                            TotalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            TotalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            TotalPriceTotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),

                        });

                    }    

                }    
                
            }
            
            return Reports;

        }
        public List<Report> SearchRp(string str)
        {
            var detailsim = db.Detailimportcoupons.ToList();
            foreach (var iem in detailsim)
            {
                var g = db.Detailimportcoupons.Where(o => o.DetailimportID == iem.DetailimportID).ToList();
                TotalPriceImports.Add(new TotalPriceImport
                {
                    ID = iem.DetailimportID,
                    IDProduct = iem.ProductID,
                    total = iem.Price * (int)iem.Quantity,
                });
            }

            var detailsout = db.DetailBills.ToList();
            foreach (var iem in detailsout)
            {
                var g = db.DetailBills.Where(o => o.DetailBillID == iem.DetailBillID).ToList();
                TotalPricebills.Add(new TotalPricebill
                {
                    ID = iem.DetailBillID,
                    IDProduct = iem.ProductID,
                    total = iem.Price * (int)iem.Quantity,
                });
            }
            var temp = db.Products.Where(s => s.ProductName.Contains(str) || s.ProductID.Contains(str) || s.Category.CategoryName.Contains(str) || s.Unit.Contains(str));
            foreach (var ovbj in temp)
            {
                var Debill = db.DetailBills.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                var Deimport = db.Detailimportcoupons.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                //var outputid = db.Bills.FirstOrDefault(o => o.BillID == ovbj1.BillID);
              
                if (Deimport != null)
                {
                    //var inp = db.Importcoupons.FirstOrDefault(o => o.ImportID == Deimport.ImportID);
                    if (Debill != null)
                    {
                        Reports.Add(new Report
                        {

                            Photo = ovbj.Photo,
                            ProductID = ovbj.ProductID,
                            CategoryID = (int)ovbj.CategoryID,
                            ProductName = ovbj.ProductName,
                            SupplierName = ovbj.Supplier.SupplierName,
                            CategoryName = ovbj.Category.CategoryName,
                            //dateinput = (DateTime)inp.Date,
                            Unit = ovbj.Unit,
                            QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == Deimport.ProductID).Sum(o => o.Quantity),
                            QuantityOut = (int)db.DetailBills.Where(o => o.ProductID == Debill.ProductID).Sum(o => o.Quantity),
                            total = (int)db.Detailimportcoupons.Where(o => o.ProductID == Deimport.ProductID).Sum(o => o.Quantity) - (int)db.DetailBills.Where(o => o.ProductID == Debill.ProductID).Sum(o => o.Quantity),
                            TotalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            TotalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            TotalPriceTotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),

                        });
                    }
                    else
                    {
                        Reports.Add(new Report
                        {

                            Photo = ovbj.Photo,
                            ProductID = ovbj.ProductID,
                            CategoryID = (int)ovbj.CategoryID,
                            ProductName = ovbj.ProductName,
                            SupplierName = ovbj.Supplier.SupplierName,
                            CategoryName = ovbj.Category.CategoryName,
                            //dateinput = (DateTime)inp.Date,
                            Unit = ovbj.Unit,
                            QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == Deimport.ProductID).Sum(o => o.Quantity),
                            QuantityOut = 0,
                            total = (int)db.Detailimportcoupons.Where(o => o.ProductID == Deimport.ProductID).Sum(o => o.Quantity),
                            TotalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            TotalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            TotalPriceTotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),

                        });

                    }

                }

            }

            return Reports;

        }
       
        public List<Report> SearchRpdate(DateTime datein, DateTime dateout)
        {
            var tempinport = from p in db.Importcoupons
                       where (p.Date >= datein && p.Date <= dateout)
                       select p;
       
            var tempbill = from d in db.Bills
                   where (d.Date >= datein && d.Date <= dateout)
                   select d;
           
            var temp = db.Products;
            foreach (var ovbj in temp)
            {
                var Debill = db.DetailBills.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                var Deimport = db.Detailimportcoupons.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                var temp1 = db.DetailBills;

                //var outputid = db.Bills.FirstOrDefault(o => o.BillID == ovbj1.BillID);
                //var inp = db.Importcoupons.FirstOrDefault(o => o.ImportID == Deimport.ImportID);

                if (Deimport != null)
                {
                    if (Debill != null)
                    {
                        Reports.Add(new Report
                        {

                            Photo = ovbj.Photo,
                            ProductID = ovbj.ProductID,
                            CategoryID = (int)ovbj.CategoryID,
                            ProductName = ovbj.ProductName,
                            SupplierName = ovbj.Supplier.SupplierName,
                            CategoryName = ovbj.Category.CategoryName,
                            Unit = ovbj.Unit,
                            QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == Deimport.ProductID).Sum(o => o.Quantity),
                            QuantityOut = (int)db.DetailBills.Where(o => o.ProductID == Debill.ProductID).Sum(o => o.Quantity),
                            total = (int)db.Detailimportcoupons.Where(o => o.ProductID == Deimport.ProductID).Sum(o => o.Quantity) - (int)db.DetailBills.Where(o => o.ProductID == Debill.ProductID).Sum(o => o.Quantity),

                        });
                    }
                    else
                    {
                        Reports.Add(new Report
                        {

                            Photo = ovbj.Photo,
                            ProductID = ovbj.ProductID,
                            CategoryID = (int)ovbj.CategoryID,
                            ProductName = ovbj.ProductName,
                            SupplierName = ovbj.Supplier.SupplierName,
                            CategoryName = ovbj.Category.CategoryName,
                            Unit = ovbj.Unit,
                            QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == Deimport.ProductID).Sum(o => o.Quantity),
                            QuantityOut = 0,
                            total = (int)db.Detailimportcoupons.Where(o => o.ProductID == Deimport.ProductID).Sum(o => o.Quantity),

                        });

                    }

                }

            }

            return Reports;



        }
    }
}