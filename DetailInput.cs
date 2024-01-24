using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace QLK.Website.Model
{
    public class DetailInput
    {
        QLKEntities db = new QLKEntities();

     
        public IEnumerable<Detailimportcoupon>  List()
        {
            return db.Detailimportcoupons.ToList();
        }

        public Detailimportcoupon listDetailID(int id)
        {

            return db.Detailimportcoupons.First(m => m.DetailimportID.CompareTo(id) == 0);
        }
       

        public void Add(Detailimportcoupon Ipc)
        {
            Detailimportcoupon h = new Detailimportcoupon();
            h.DetailimportID = Ipc.DetailimportID;
            h.ImportID = Ipc.ImportID;
            h.ProductID = Ipc.ProductID;
            h.Unit = Ipc.Unit;
            h.Price = Ipc.Price;
            h.Quantity = Ipc.Quantity;
            h.Location = Ipc.Location;
            db.Detailimportcoupons.Add(Ipc);
            db.SaveChanges();
        }

        public void Add1(Detailimportcoupon Ipc)
        {
            Detailimportcoupon h = new Detailimportcoupon();
            h.DetailimportID = Ipc.DetailimportID;
            h.ImportID = Ipc.ImportID;
            h.ProductID = Ipc.ProductID;
            h.Unit = Ipc.Unit;
            h.Price = Ipc.Price;
            h.Quantity = Ipc.Quantity;
            h.Location = Ipc.Location;
            db.Detailimportcoupons.Add(Ipc);
            db.SaveChanges();
        }

        //public void Add1(Detailimportcoupon Ipc)
        //{
        //    Detailimportcoupon p = new Detailimportcoupon();

        //    p.DetailimportID = Ipc.DetailimportID;
        //    p.ImportID = Ipc.ImportID;
        //    p.ProductID = Ipc.ProductID;
        //    p.Unit = Ipc.Unit;
        //    p.Quantity = Ipc.Quantity;
        //    db.Detailimportcoupons.Add(p);
        //    db.SaveChanges();

        //}
        public void Edit(Detailimportcoupon Ipc)
        {
            
                Detailimportcoupon p = listDetailID(Ipc.DetailimportID);
                p.ProductID = Ipc.ProductID;
                p.ImportID = Ipc.ImportID;
                p.ProductID = Ipc.ProductID;
                p.Unit = Ipc.Unit;
                p.Quantity = Ipc.Quantity;
            p.Price = Ipc.Price;
            p.Location = Ipc.Location;
            db.SaveChanges();
               
           
        }
        //public void Delete(int id)
        //{
        //    Detailimportcoupon p = listDetailID(id);
        //    db.Detailimportcoupons.Remove(p);
        //    db.SaveChanges();
        //}

    }
}