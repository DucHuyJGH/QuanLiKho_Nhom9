using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using QLK.Website.Model;
namespace QLK.Website.Model
{
    public  class ListProduct
    {
        Detailimportcoupon detailimportcoupon = new Detailimportcoupon();
        QLKEntities s = new QLKEntities();

        
        public IEnumerable<Product> List()
        {
            return s.Products.ToList();
        }
        public Product listID (string id)
        {
            return s.Products.First(m=> m.ProductID.CompareTo(id)==0);
        }
        public void Add(Product product) {

            s.Products.Add(product);
            s.SaveChanges();
        }
        public void Edit(Product product)
        {

            Product p = listID(product.ProductID);

            p.ProductID = product.ProductID;
            p.ProductName = product.ProductName;
            p.Photo = product.Photo;
            p.Unit = product.Unit;
            p.CategoryID = product.CategoryID;
            p.SupplierID = product.SupplierID;
            p.Price = product.Price;
            s.SaveChanges();

        }
        public void Delete( string id)
        {        
                 Product p = listID(id);
                s.Products.Remove(p);   
                 s.SaveChanges();

        }

    }
}