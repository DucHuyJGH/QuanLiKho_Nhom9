using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class Supplierdb
    {

        QLKEntities db = new QLKEntities();

        public IEnumerable<Supplier> List()
        {
            return db.Suppliers.ToList();
        }
        public Supplier listsupplierID(int id)
        {
            return db.Suppliers.First(m => m.SupplierID.CompareTo(id) == 0);
        }
        public void Add(Supplier supplier)
        {
            db.Suppliers.Add(supplier);
            db.SaveChanges();
        }
        public void Edit(Supplier supplier)
        {

            Supplier p = listsupplierID(supplier.SupplierID);
            p.SupplierID = supplier.SupplierID;
            p.SupplierName = supplier.SupplierName;
            p.ContactName = supplier.ContactName;
            p.Adderss = supplier.Adderss;
            p.City = supplier.City;
            p.Country = supplier.Country;
            p.Phone = supplier.Phone;
            p.Zipcode = supplier.Zipcode;
            p.Note = supplier.Note;
            db.SaveChanges();
        }
        public void Delete(int id)
        {
            Supplier p = listsupplierID(id);
            db.Suppliers.Remove(p);
            db.SaveChanges();
        }
       
    }
}