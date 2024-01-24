using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class Output
    {
        QLKEntities db = new QLKEntities();

        public IEnumerable<Bill> List()
        {
            return db.Bills.ToList();
        }
        public Bill listBillID(int id)
        {
            return db.Bills.First(m => m.BillID.CompareTo(id) == 0);
        }
        
        public void Add(Bill bill)
        {
            var model = new Importcoupon();
            model.Date = DateTime.Now;
            model.WarehouseID = bill.WarehouseID;
            model.Photo = bill.Photo;
            db.Bills.Add(bill);
            db.SaveChanges();
        }

       
        public void Edit(Bill bill)
        {
            Bill p = listBillID(bill.BillID);
            p.BillID = bill.BillID;
            p.BillName = bill.BillName;
            p.Date = bill.Date;
            p.WarehouseID = bill.WarehouseID;
            p.Photo = bill.Photo;
            db.SaveChanges();
        }
        public void Delete(int id)
        {

                    Bill p = listBillID(id);
                    db.Bills.Remove(p);

                    db.SaveChanges();
     
        }
        public List<ListCTOutput> ListCTOutp(int id)
        {
            //id=billid
            List<ListCTOutput> ListCts = new List<ListCTOutput>();
          
            var bill = db.DetailBills;
            foreach (var oj in bill)
            {
                var model = db.Bills.FirstOrDefault(o => o.BillID == id);
                if(model!= null)
                {
                    ListCts.Add(new ListCTOutput
                    {
                        DetailBillID = oj.DetailBillID,
                        BillID = (int)oj.BillID,
                        ProductID = oj.ProductID,
                        Quantity = (int)oj.Quantity,
                        Unit = oj.Unit
                    });
                }
            }
            return ListCts;
        }
    }
}