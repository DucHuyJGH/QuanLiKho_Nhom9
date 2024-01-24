using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class ListExInput
    {
        QLKEntities db = new QLKEntities();
        List<ExInput> ExInputs = new List<ExInput>();
        public List<ExInput> ListExinput(DateTime datein, DateTime dateout)
        {
            var temp = db.Detailimportcoupons.Where(o => o.Importcoupon.Date >= datein && o.Importcoupon.Date <= dateout).ToList(); 
            foreach (var ovbj in temp)
            {
               
                var Pr = db.Products.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                var sup = db.Suppliers.FirstOrDefault(o => o.SupplierID == Pr.SupplierID);
                var im = db.Importcoupons.FirstOrDefault(o => o.ImportID == ovbj.ImportID);
                var cate = db.Categories.FirstOrDefault(o => o.CategoryID == Pr.CategoryID);
                ExInputs.Add(new ExInput
                    {

                        Photo = Pr.Photo,
                        DetailsImport = ovbj.DetailimportID,
                        ProductID = Pr.ProductID,
                        CategoryID = cate.CategoryID,
                        ImportID = (int)ovbj.ImportID,
                        ProductName = Pr.ProductName,
                        SupplierName = sup.SupplierName,
                        CategoryName = cate.CategoryName,
                        Date = (DateTime)im.Date,
                        Unit = ovbj.Unit,
                        Price = ovbj.Price,
                        QuantityIm = (int)ovbj.Quantity,
                        Tolalpricein = ovbj.Price * (int)ovbj.Quantity,


                });
            }

            return ExInputs;

        }

        public List<ExInput> ListExinput(string str, DateTime datein, DateTime dateout)
        {
          
            var temp = db.Detailimportcoupons.Where(o => (o.Unit.Contains(str)||o.Product.ProductName.Contains(str) || o.ProductID.Contains(str)||
            o.Product.Category.CategoryName.Contains(str) || o.Product.Supplier.SupplierName.Contains(str)) && (o.Importcoupon.Date >= datein && o.Importcoupon.Date <= dateout)
          );
            foreach (var ovbj in temp)
            {
                var Pr = db.Products.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                var sup = db.Suppliers.FirstOrDefault(o => o.SupplierID == Pr.SupplierID);
                var im = db.Importcoupons.FirstOrDefault(o => o.ImportID == ovbj.ImportID);
                var cate = db.Categories.FirstOrDefault(o => o.CategoryID == Pr.CategoryID);
                ExInputs.Add(new ExInput
                {

                    Photo = Pr.Photo,
                    DetailsImport = ovbj.DetailimportID,
                    ProductID = Pr.ProductID,
                    CategoryID = cate.CategoryID,
                    ImportID = (int)ovbj.ImportID,
                    ProductName = Pr.ProductName,
                    SupplierName = sup.SupplierName,
                    CategoryName = cate.CategoryName,
                    Date = (DateTime)im.Date,
                    Unit = ovbj.Unit,
                    Price = ovbj.Price,
                    QuantityIm = (int)ovbj.Quantity,
                    Tolalpricein = ovbj.Price * (int)ovbj.Quantity,
                });
            }

            return ExInputs;

        }

        public List<ExInput> ListExinput1( DateTime datein, DateTime dateout)
        {

            var temp = db.Detailimportcoupons.Where(o=> o.Importcoupon.Date >= datein && o.Importcoupon.Date <= dateout);
            foreach (var ovbj in temp)
            {
                var Pr = db.Products.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                var sup = db.Suppliers.FirstOrDefault(o => o.SupplierID == Pr.SupplierID);
                var im = db.Importcoupons.FirstOrDefault(o => o.ImportID == ovbj.ImportID);
                var cate = db.Categories.FirstOrDefault(o => o.CategoryID == Pr.CategoryID);
                ExInputs.Add(new ExInput
                {

                    Photo = Pr.Photo,
                    DetailsImport = ovbj.DetailimportID,
                    ProductID = Pr.ProductID,
                    CategoryID = cate.CategoryID,
                    ImportID = (int)ovbj.ImportID,
                    ProductName = Pr.ProductName,
                    SupplierName = sup.SupplierName,
                    CategoryName = cate.CategoryName,
                    Date = (DateTime)im.Date,
                    Unit = ovbj.Unit,
                    Price = ovbj.Price,
                    QuantityIm = (int)ovbj.Quantity,
                    Tolalpricein = ovbj.Price * (int)ovbj.Quantity,
                });
            }

            return ExInputs;

        }
    }
}