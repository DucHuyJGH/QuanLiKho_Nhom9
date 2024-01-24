using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace QLK.Website.Model
{
    public class DetailOutput
    {
        QLKEntities db = new QLKEntities();
       
        public IEnumerable<DetailBill> List()
        {
            return db.DetailBills.ToList();
        }

        public DetailBill listDetailBillID(int id)
        {
           
            return db.DetailBills.First(m => m.DetailBillID.CompareTo(id) == 0);
            
        }
  
        public void Add(DetailBill detail)
        {
            DetailBill p = new DetailBill();

            p.DetailBillID = detail.DetailBillID;
            p.BillID = detail.BillID;
            p.ProductID = detail.ProductID;
            p.Price = detail.Price;
            p.Note = detail.Note;
            p.Unit = detail.Unit;
            p.Quantity = detail.Quantity;
         
            db.DetailBills.Add(detail);
            db.SaveChanges();
             
        }
        public void Add1(DetailBill detail)
        {
            DetailBill p = new DetailBill();

            p.DetailBillID = detail.DetailBillID;
            p.BillID = detail.BillID;
            p.ProductID = detail.ProductID;
            p.Price = detail.Price;
            p.Note = detail.Note;
            p.Unit = detail.Unit;
            p.Quantity = detail.Quantity;
            db.DetailBills.Add(p);
            db.SaveChanges();

        }
        public void Edit(DetailBill detail)
        {

            DetailBill p = listDetailBillID(detail.DetailBillID);
            p.DetailBillID = detail.DetailBillID;
            p.BillID = detail.BillID;
            p.ProductID = detail.ProductID;
            p.Unit = detail.Unit;
            p.Quantity = detail.Quantity;
            p.Price = detail.Price;
            p.Note = detail.Note;

            db.SaveChanges();
        }
        public void Delete(int id)
        {
            DetailBill p = listDetailBillID(id);
            db.DetailBills.Remove(p);
            db.SaveChanges();
        }
       
    }
}