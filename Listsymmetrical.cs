using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace QLK.Website.Model
{
    public class Listsymmetrical
    {
        DetailBill detailBill = new DetailBill();
        Detailimportcoupon detailimportcoupon = new Detailimportcoupon();
        QLKEntities db = new QLKEntities();
        List<symmetrical> baoCaos = new List<symmetrical>();
        List<TotalPriceImport> TotalPriceImports = new List<TotalPriceImport>();
        List<TotalPricebill> TotalPricebills = new List<TotalPricebill>();
        public List<symmetrical> ListBy()
        {
           
            if(db.DetailBills.Sum(o => o.Quantity) != null && db.Detailimportcoupons.Sum(p => p.Quantity ) != null)
            {
            int detailquantity = (int)db.DetailBills.Sum(o => o.Quantity);
             int importquantity = (int)db.Detailimportcoupons.Sum(p => p.Quantity);
             int tong = importquantity - detailquantity;
            }
           

          
            var detailsim = db.Detailimportcoupons.ToList();
            foreach (var iem in detailsim)
            {
                var g = db.Detailimportcoupons.Where(o => o.DetailimportID == iem.DetailimportID).ToList();
                TotalPriceImports.Add(new TotalPriceImport
                {
                    ID = iem.DetailimportID,
                    IDProduct =iem.ProductID,
                    total = iem.Price * (int)iem.Quantity,
                }) ; 
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
               //lấy product id o detailbill
                var bill = db.DetailBills.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                //lấy productid o detailimport
                var import = db.Detailimportcoupons.FirstOrDefault(o => o.ProductID == ovbj.ProductID);

                if (import != null)
                {
                    if (bill == null)
                    {
                        int r = 0;
                        baoCaos.Add(new symmetrical
                        {
                            ProductID = ovbj.ProductID,
                            ProductName = ovbj.ProductName,
                            QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity),
                            QuantityOut = r,
                            total = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity) - r,
                            Unit = ovbj.Unit,
                            totalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            totalPriceou = TotalPricebills.Where(o=>o.IDProduct == ovbj.ProductID).Sum(o=>o.total),
                            totalPricetotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),
                        }); 
                    }
                    else
                    {
                      
                        baoCaos.Add(new symmetrical
                        {
                           
                            ProductID = ovbj.ProductID,
                            ProductName = ovbj.ProductName,
                            QuantityIm = (int)db.Detailimportcoupons.Where(o=>o.ProductID== import.ProductID).Sum(o=>o.Quantity),// (int)import.Quantity,
                            QuantityOut = (int)db.DetailBills.Where(o => o.ProductID == bill.ProductID).Sum(o => o.Quantity),
                            total = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity)
                            - (int)db.DetailBills.Where(o => o.ProductID == bill.ProductID).Sum(o => o.Quantity),
                            Unit = ovbj.Unit,
                            totalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            totalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            totalPricetotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),

                        });
                    }    
                }
               
            }
          
           
            return baoCaos;
        }

        public List<symmetrical> Searchsym(string str)
        {
            
            if (db.DetailBills.Sum(o => o.Quantity) != null && db.Detailimportcoupons.Sum(p => p.Quantity) != null)
            {
                int detailquantity = (int)db.DetailBills.Sum(o => o.Quantity);
                int importquantity = (int)db.Detailimportcoupons.Sum(p => p.Quantity);
                int tong = importquantity - detailquantity;
            }


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

            int totalPrice = TotalPriceImports.Sum(o => o.total);
            var temp = db.Products.Where(o=>o.ProductName.Contains(str) || o.Unit.Contains(str));
            foreach (var ovbj in temp)
            {
                //lấy product id o detailbill
                var bill = db.DetailBills.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                //lấy productid o detailimport
                var import = db.Detailimportcoupons.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                if (import != null)
                {
                    if (bill == null)
                    {
                        int r = 0;
                       
                        baoCaos.Add(new symmetrical
                        {
                            ProductID = ovbj.ProductID,
                            ProductName = ovbj.ProductName,
                            QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity),
                            QuantityOut = r,
                            total = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity) - r,
                            Unit = ovbj.Unit,
                            totalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            totalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            totalPricetotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),
                        });
                    }
                    else
                    {
                       
                        baoCaos.Add(new symmetrical
                        {

                            ProductID = ovbj.ProductID,
                            ProductName = ovbj.ProductName,
                            QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity),// (int)import.Quantity,
                            QuantityOut = (int)db.DetailBills.Where(o => o.ProductID == bill.ProductID).Sum(o => o.Quantity),
                            total = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity)
                            - (int)db.DetailBills.Where(o => o.ProductID == bill.ProductID).Sum(o => o.Quantity),
                            Unit = ovbj.Unit,
                            totalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            totalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                            totalPricetotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),

                        });
                    }
                }

            }

            return baoCaos;
        }


    }
}