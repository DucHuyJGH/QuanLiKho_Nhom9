using PagedList;
using QLK.Website.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLK.Website.Controllers
{
    public class WarehouseController : Controller
    {
        QLKEntities db = new QLKEntities();
        ListWarehouse p = new ListWarehouse();
        Bill b = new Bill();
        Importcoupon i = new Importcoupon();
        // GET: Product
        public ActionResult Index(string SearchString, string currentFilter, int ? page)
        {
            //if (Session["Username"] == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            if (Request.Cookies["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var ware = db.Warehouses;
            ViewBag.Ware = ware;
            var ListWare = new List<Warehouse>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                ListWare = db.Warehouses.Where(o => o.WarehouseName.Contains(SearchString)).ToList();
            }
            else
            {
                ListWare = db.Warehouses.ToList();
            }
            ViewBag.currentFilter = SearchString;
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            ListWare = ListWare.OrderByDescending(n => n.WarehouseID).ToList();

            return View(ListWare.ToPagedList(pageNumber, pageSize));



        }
        //tao 1 product
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Create(Warehouse warehouse)
        {
            bool result = false;
            if (warehouse.WarehouseName != null && warehouse.Soluong != null)
            {
                p.Add(warehouse);
                result = true;
            }
            return Json(result,JsonRequestBehavior.AllowGet);
            
        }
        //sua product theo id
        //public ActionResult Edit(int id)
        //{

        //    return View(p.listWareID(id));

        //}

        //[HttpPost]
        //public ActionResult Edit(Warehouse warehouse)
        //{

        //    if (warehouse.WarehouseName != null && warehouse.Soluong != null)
        //        {
        //            p.Edit(warehouse);
        //            return RedirectToAction("index");
        //        }

        //    return View();
        //}
        public ActionResult Edit(int id)
        {
          
            var model = db.Warehouses.Where(o => o.WarehouseID == id).FirstOrDefault();
            return PartialView("EditPartial", model);
        }

        [HttpPost]
        public JsonResult Edit(Warehouse warehouse)
        {
          
            bool result = false;
            if (warehouse.WarehouseName != null && warehouse.Soluong != null)
            {
                p.Edit(warehouse);
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Delete(int id)
        {
            bool result = false;
            var input = db.Warehouses.Where(o => o.WarehouseID == id);
            if (input != null)
            {
                var bill = db.Bills.FirstOrDefault(o => o.WarehouseID == id);
                var imp = db.Importcoupons.FirstOrDefault(o => o.WarehouseID == id);
                if (bill == null && imp == null)
                {
                    p.Delete(id);
                    result = true;
                }

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}