using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class Input
    {
        QLKEntities db = new QLKEntities();

        public IEnumerable<Importcoupon> List()
        {
            return db.Importcoupons.ToList();
        }
        public Importcoupon listID(int id)
        {
            return db.Importcoupons.First(m => m.ImportID.CompareTo(id) == 0);
        }
        public void Add(Importcoupon Ipc)
        {
            var model = new Importcoupon();
            model.Date =  DateTime.Now;
            model.WarehouseID = Ipc.WarehouseID;
            model.Photo = Ipc.Photo;

            db.Importcoupons.Add(Ipc);
            db.SaveChanges();
        }
        public void Edit(Importcoupon Ipc)
        {

            Importcoupon p = listID(Ipc.ImportID);
            p.ImportID = Ipc.ImportID;
            p.ImportName = Ipc.ImportName;
            p.Date = Ipc.Date;
            p.WarehouseID = Ipc.WarehouseID;
            p.Photo = Ipc.Photo;
            db.SaveChanges();
        }
        public void Delete(int id)
        {
            Importcoupon p = listID(id);
            db.Importcoupons.Remove(p);
            db.SaveChanges();
        }
        List<Importcoupon> ListSelecter7dates = new List<Importcoupon>();
        public List<Importcoupon> ListSelecter7date(string str)
        { 
                DateTime DateEnd = DateTime.Now;
                DateTime DateStart = DateEnd - new TimeSpan(7,0,0,0);
                 ListSelecter7dates = db.Importcoupons.Where(o => o.Date >DateStart || (o.ImportName.Contains(str) || o.Warehouse.WarehouseName.Contains(str))).ToList();
                return ListSelecter7dates;
        }
        public List<Importcoupon> ListSelecter7date()
        {
            DateTime DateEnd = DateTime.Now;
            DateTime DateStart = DateEnd - new TimeSpan(7, 0, 0, 0);
            ListSelecter7dates = db.Importcoupons.Where(o=> o.Date > DateStart).ToList();
            return ListSelecter7dates;
        }
        List<Importcoupon> ListSelecter30dates = new List<Importcoupon>();
        public List<Importcoupon> ListSelecter30date(string srt)
        {
            DateTime DateEnd = DateTime.Now;
            DateTime DateStart = DateEnd - new TimeSpan(30, 0, 0, 0);
            ListSelecter30dates =db.Importcoupons.Where(o => o.Date > DateStart &&( o.ImportName.Contains(srt) || o.Warehouse.WarehouseName.Contains(srt))).ToList();
            return ListSelecter30dates;
        }
        public List<Importcoupon> ListSelecter30date()
        {
            DateTime DateEnd = DateTime.Now;
            DateTime DateStart = DateEnd - new TimeSpan(30, 0, 0, 0);
            ListSelecter30dates = db.Importcoupons.Where(o => o.Date > DateStart).ToList();
            return ListSelecter30dates;
        }



    }
}