using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLK.Website.Model
{
    public class ListWarehouse
    {
        QLKEntities s = new QLKEntities();
        Bill b = new Bill();
        Importcoupon i = new Importcoupon();

        public IEnumerable<Warehouse> List()
        {
            return s.Warehouses.ToList();
        }
        public Warehouse listWareID(int id)
        {
            return s.Warehouses.First(m => m.WarehouseID.CompareTo(id) == 0);
        }
        public void Add(Warehouse warehouse)
        {

            s.Warehouses.Add(warehouse);
            s.SaveChanges();
        }
        public void Edit(Warehouse warehouse)
        {

            Warehouse p = listWareID(warehouse.WarehouseID);

            p.WarehouseID = warehouse.WarehouseID;
            p.WarehouseName = warehouse.WarehouseName;
            p.Soluong = warehouse.Soluong;
            s.SaveChanges();
        }
        public void Delete(int id)
        {   
                Warehouse p = listWareID(id);
                s.Warehouses.Remove(p);
                s.SaveChanges();

        }
    }
}