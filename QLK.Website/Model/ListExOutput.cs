using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class ListExOutput
    {
        QLKEntities db = new QLKEntities();
        List<ExOutput> ExOutputs = new List<ExOutput>();
        public List<ExOutput> ListExoutput(DateTime datein, DateTime dateout)
        {
            var temp = db.DetailBills.Where(o => o.Bill.Date >= datein && o.Bill.Date <= dateout);
            foreach (var ovbj in temp)
            {

                var Pr = db.Products.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                var sup = db.Suppliers.FirstOrDefault(o => o.SupplierID == Pr.SupplierID);
                var bi = db.Bills.FirstOrDefault(o => o.BillID == ovbj.BillID);
                var cate = db.Categories.FirstOrDefault(o => o.CategoryID == Pr.CategoryID);
                ExOutputs.Add(new ExOutput
                {

                    Photo = Pr.Photo,
                    DetailsOutputID = ovbj.DetailBillID,
                    ProductID = Pr.ProductID,
                    CategoryID = cate.CategoryID,
                    BillID = (int)ovbj.BillID,
                    ProductName = Pr.ProductName,
                    SupplierName = sup.SupplierName,
                    CategoryName = cate.CategoryName,
                    Date = (DateTime)bi.Date,
                    Unit = ovbj.Unit,
                    Quantityout = (int)ovbj.Quantity,
                    Price = ovbj.Price,
                    Tolalpriceou = ovbj.Price * (int)ovbj.Quantity
                });
            }

            return ExOutputs;

        }
        public List<ExOutput> ListExoutputSearch( string str, DateTime datein, DateTime dateout)
        {
            var temp = db.DetailBills.Where(o=>(o.Product.ProductName.Contains(str) || o.ProductID.Contains(str) || o.Product.Category.CategoryName.Contains(str) || o.Product.Supplier.SupplierName.Contains(str)) && (o.Bill.Date >= datein && o.Bill.Date <= dateout));
            foreach (var ovbj in temp)
            {

                var Pr = db.Products.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                var sup = db.Suppliers.FirstOrDefault(o => o.SupplierID == Pr.SupplierID);
                var bi = db.Bills.FirstOrDefault(o => o.BillID == ovbj.BillID);
                var cate = db.Categories.FirstOrDefault(o => o.CategoryID == Pr.CategoryID);
                ExOutputs.Add(new ExOutput
                {

                    Photo = Pr.Photo,
                    DetailsOutputID = ovbj.DetailBillID,
                    ProductID = Pr.ProductID,
                    CategoryID = cate.CategoryID,
                    BillID = (int)ovbj.BillID,
                    ProductName = Pr.ProductName,
                    SupplierName = sup.SupplierName,
                    CategoryName = cate.CategoryName,
                    Date = (DateTime)bi.Date,
                    Unit = ovbj.Unit,
                    Quantityout = (int)ovbj.Quantity,
                    Price = ovbj.Price,
                    Tolalpriceou = ovbj.Price * (int)ovbj.Quantity
                });
            }

            return ExOutputs;

        }


    }
}