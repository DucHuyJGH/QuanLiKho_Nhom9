using QLK.Website.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace QLK.Website.Controllers
{
    public class DetailInputController : Controller
    {
        // GET: DetailInput
        DetailInput k = new DetailInput();
        QLKEntities db = new QLKEntities();
        Detailimportcoupon detailimportcoupon = new Detailimportcoupon();
     

        // GET: Input
        public ActionResult Index(string SearchString,string currentFilter, int? page, DateTime ? datein, DateTime ? dateout)
        {
          
            if (Request.Cookies["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var pro = db.Products.ToList();
            List<UnitSelectlist> unitSelectlists = new List<UnitSelectlist>();
         
            foreach (var item in pro)
            {
                if (unitSelectlists.Count() < 1)
                {
                    unitSelectlists.Add(new UnitSelectlist
                    {
                        unit = item.Unit
                    });
                }
                else
                {
                    var kt = unitSelectlists.Where(o => o.unit == item.Unit).ToList();
                    if (kt.Count() < 1)
                    {
                        unitSelectlists.Add(new UnitSelectlist
                        {
                            unit = item.Unit
                        });
                    }

                }

            }
            ViewBag.unit = unitSelectlists.ToList();
            var detaiImport = db.Detailimportcoupons.ToList();
            var tong = db.Detailimportcoupons.Sum(s => s.Quantity);
            var imp = db.Importcoupons.ToList();
            ViewBag.detailimport = detaiImport;
            ViewBag.tong = tong;
            ViewBag.ImproID = imp;
            ViewBag.Product = pro;
            ViewBag.datenow = DateTime.Now;

            var dtin = DateTime.Parse("1/1/2020");
            var datei = datein ?? dtin;
            var dateo = dateout ?? DateTime.Now;
            ViewBag.datein = datei;
            ViewBag.dateout = dateo;


            var listdetailinput = new List<Detailimportcoupon>();
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

                listdetailinput = db.Detailimportcoupons.Where(o => (o.Product.ProductName.Contains(SearchString) || o.Importcoupon.ImportName.Contains(SearchString) || o.Location.Contains(SearchString) || o.Unit.Contains(SearchString)) && (o.Importcoupon.Date >=datei && o.Importcoupon.Date<=dateo)).ToList();

            }
             else
             {
                listdetailinput = db.Detailimportcoupons.Where(o=>o.Importcoupon.Date>=datei && o.Importcoupon.Date <=dateo) .ToList();
             }
            ViewBag.currentFilter = SearchString;
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            listdetailinput = listdetailinput.OrderByDescending(n => n.DetailimportID).ToList();

            return View(listdetailinput.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult Create()
        {
            var Por = db.Products.ToList();
            var items = db.Importcoupons.ToList();

            ViewBag.ImproID = items;
            ViewBag.Product = Por;

            return View();
        }
        //[HttpPost]
        //public JsonResult Create(Detailimportcoupon Ipc)
        //{
        //    bool result = false;
        //    if (Ipc.ImportID != null && Ipc.ProductID != null && Ipc.Unit != null & Ipc.Quantity != null)
        //    {
        //        k.Add(Ipc);
        //        result = true;
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);

        //}
     
    
    [HttpPost]
        public JsonResult Create1(Detailimportcoupon Ipc)
        {
         bool result = false;
         if(Ipc != null)
            {
                if (Ipc.ImportID != null && Ipc.ProductID != null && Ipc.Unit != null & Ipc.Quantity != null)
                {
                    k.Add1(Ipc);
                    result = true;
                }
                db.SaveChanges();
            }    
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Create(string ImportID, string ProductID, string Unit,string Quantity, string Location,string Price)
        {

            bool result = false;
            //string[] importID = ImportID.Split(',');
            //string[] productID = ProductID.Split(',');
            //string[] unit1 = Unit.Split(',');
            //string[] quantity = Quantity.Split(',');
            //string[] location = Location.Split(',');
            try { 
                List<string> importID = ImportID.Split(',').ToList();
                var count = importID.Count();
                List<string> productID = ProductID.Split(',').ToList();
                List<string> unit1 = Unit.Split(',').ToList();
                List<string> quantity = Quantity.Split(',').ToList();
                List<string> location = Location.Split(',').ToList();
                List<string> price = Price.Split(',').ToList();
         
                for (int i = 0; i < count; i++)
                {
                    Detailimportcoupon dt = new Detailimportcoupon();
                    
                        dt.ImportID = int.Parse(importID[i]);
                        dt.ProductID = productID[i];
                        dt.Unit = unit1[i];
                        dt.Quantity = int.Parse(quantity[i]);
                        dt.Location = location[i];
                        dt.Price = int.Parse(price[i]);
                        db.Detailimportcoupons.Add(dt);

                        result = true;
                 
                }
            db.SaveChanges();
            }
            catch (Exception e)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //sua product theo id
        //public ActionResult Edit(int id)
        //{
        //    var ImproId = db.Importcoupons.ToList();
        //    var ProID = db.Products.ToList();
        //    ViewBag.ImproID = ImproId;
        //    ViewBag.ProductID = ProID;

        //    return View(k.listDetailID(id));
        //}
        //[HttpPost]
        //public ActionResult Edit(Detailimportcoupon Ipc)
        //{
        //    var ImproId = db.Importcoupons.ToList();
        //    var ProID = db.Products.ToList();
        //    ViewBag.ImproID = ImproId;
        //    ViewBag.ProductID = ProID;
        //    if (Ipc.ImportID != null && Ipc.ProductID != null && Ipc.Quantity !=null && Ipc.Unit != null)
        //    {

        //        k.Edit(Ipc);
        //        return RedirectToAction("index");
        //    }
        //    else
        //        return View();
        //}

        public ActionResult Edit(int id)
        {
            var ImproId = db.Importcoupons.ToList();
            ViewBag.ImproID = ImproId;
            var pro = db.Products.ToList();
            ViewBag.pro = pro;
            var model = db.Detailimportcoupons.Where(o => o.DetailimportID == id).FirstOrDefault();
            return PartialView("Partialdetailimpout", model);
        }
        [HttpPost]
        public JsonResult Edit(Detailimportcoupon Ipc)
        {
            var ImproId = db.Importcoupons.ToList();
            ViewBag.ImproID = ImproId;
            var pro = db.Products.ToList();
            ViewBag.pro = pro;
            bool result = false;
            if (Ipc.ImportID != null && Ipc.ProductID != null && Ipc.Quantity != null && Ipc.Unit != null)
            {
                k.Edit(Ipc);
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Xoa 1 product
        //public ActionResult Delete(int id)
        //{

        //    var model = db.Detailimportcoupons;

        //    foreach(var obj in model)
        //    {
        //        if(model != null)
        //        {
        //            var product = db.DetailBills.FirstOrDefault(o => o.ProductID == obj.ProductID);
        //            if (product == null)
        //            {

        //                k.Delete(id);
        //            }
        //        }      
        //    }

        //    return RedirectToAction("index");

        //}
        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool result = false;
           
            var detailimp = db.Detailimportcoupons.FirstOrDefault(o => o.DetailimportID == id);
            if (detailimp != null)
            {
                var kt = db.DetailBills.FirstOrDefault(o => o.ProductID == detailimp.ProductID);
                if (kt == null)
                {
                    db.Detailimportcoupons.Remove(detailimp);
                    db.SaveChanges();
                    result = true;
                }
                   
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id)
        {

            return View(k.listDetailID(id));
        }
        public ActionResult SelectDate( DateTime date)
        {
            var date7 = db.Importcoupons.Where(o => o.Date == date).ToList();
            return View(date7);
        }
    }
}