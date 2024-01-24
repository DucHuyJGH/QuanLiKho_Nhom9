using PagedList;
using QLK.Website.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace QLK.Website.Controllers
{
    public class InputController : Controller
    {
      
        Input p = new Input();
      
        QLKEntities db = new QLKEntities();
        // GET: Input

        public ActionResult Index( string SearchString, string currentFilter, int ? page, DateTime ? datein , DateTime ? dateout )
        {
         
            if (Request.Cookies["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var dtin = DateTime.Parse("1/1/2020");
            var datei = datein ?? dtin;
            var dateo = dateout ?? DateTime.Now;
            ViewBag.datein = datei;
            ViewBag.dateout = dateo;
            ViewBag.datenow = DateTime.Now;
            ViewBag.seacrchform = Session["Searchinput"];
            ViewBag._id = Session["idinput"];
            ViewBag.selectdate = Session["selectdate"];
           
            var imp = db.Detailimportcoupons.ToList();
            ViewBag.Imp = imp;
            var warehouses = db.Warehouses;
            ViewBag.Warehouses = warehouses;
            var importname = db.Importcoupons.ToList();
            ViewBag.ImportName = importname;
            ViewBag.UploadphotoImport = db.Uploadfiles.ToList();

            List<UnitSelectlist> unitSelectlists = new List<UnitSelectlist>();
            var pro = db.Products.ToList();
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

            var listinput = new List<Importcoupon>();
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
                    if(datei != null && dateo!= null)
                    {
                    listinput = db.Importcoupons.Where(o => (o.Warehouse.WarehouseName.Contains(SearchString) || o.ImportName.Contains(SearchString)) && (o.Date >= datei && o.Date <= dateo)).ToList();

                     }
                    else
                    {

                    listinput = db.Importcoupons.Where(o => o.Warehouse.WarehouseName.Contains(SearchString) || o.ImportName.Contains(SearchString)).ToList();

                    }    

                //listinput = db.Importcoupons.Where(o => o.Warehouse.WarehouseName.Contains(SearchString) || o.ImportName.Contains(SearchString)).ToList();
                         
                  }
                else
                {
                if(datei != null && dateo != null)
                {
                    listinput = db.Importcoupons.Where(o=>o.Date >=datei && o.Date <= dateo).ToList();
                }    
                else
                {
                    listinput = db.Importcoupons.ToList();
                }    
                  
                     
                }
                    
            ViewBag.currentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            listinput = listinput.OrderByDescending(n => n.ImportID).ToList();

            return View(listinput.ToPagedList(pageNumber, pageSize));

        }


        public ActionResult Create()
        {
            var impc = db.Warehouses.ToList();
            ViewBag.WarehouseID = impc;
            return View();
        }

        [HttpPost]
        public JsonResult Create(Importcoupon Ipc,HttpPostedFileBase[] file)
        {

            bool result = false;
           
            if (file != null)
            {
                if (Ipc.WarehouseID != null && Ipc.ImportName != null)
                {
                    var date = DateTime.Now;
                    Ipc.Date = date;
                    p.Add(Ipc);
                    result = true;
                }
                foreach (HttpPostedFileBase fl in file)
                {

                    var InputFileName = System.IO.Path.GetFileName(fl.FileName);
                    var ServerSavePath = System.IO.Path.Combine(Server.MapPath("~/Image/Img/") + InputFileName);
                    fl.SaveAs(ServerSavePath);
                    Uploadfile upfl = new Uploadfile();
                    upfl.ID = Ipc.ImportID;
                    upfl.Path = "~/Image/Img/" + InputFileName;
                    db.Uploadfiles.Add(upfl);
                    db.SaveChanges();

                }
               
            }    

            //if (file != null)
            //{
            //     string fileName = System.IO.Path.GetFileName(file.FileName);
            //    if (Ipc.WarehouseID != null && Ipc.ImportName != null)
            //   {
            //        var date = DateTime.Now;
            //        Ipc.Date = date;
            //        Ipc.Photo = "~/Image/Img/" + fileName;
            //        p.Add(Ipc);
            //        result = true;
            //    }
            //}

            return Json(new { status = result }, JsonRequestBehavior.AllowGet);

        }

        //[HttpPost]
        //public JsonResult UploadFiles(HttpPostedFileBase[] files)
        //{
            
        //    bool result = false;
        //    if (ModelState.IsValid)
        //    {   
        //        foreach (HttpPostedFileBase file in files)
        //        {
                   
        //            if (file != null)
        //            {
        //                var InputFileName = System.IO.Path.GetFileName(file.FileName);
        //                var ServerSavePath = System.IO.Path.Combine(Server.MapPath("~/Image/Img/") + InputFileName);
        //                result = true;
        //                file.SaveAs(ServerSavePath);
                       
        //            }

        //        }
        //    }
        //    return Json(result,JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Edit(int id)
        {
           
            ViewBag.UploadphotoImport = db.Uploadfiles.ToList();
            var inpu =db.Warehouses.ToList();
            ViewBag.ware = inpu;
            var model = db.Importcoupons.Where(o => o.ImportID == id).FirstOrDefault();
            return PartialView("EditIpPartial", model);
        }

      
        [HttpPost]
        public JsonResult Edit(Importcoupon Ipc, HttpPostedFileBase[] file)
        {
            
            var inpu = db.Warehouses.ToList();
            ViewBag.ware = inpu;
            bool result = false;

                if (Ipc.WarehouseID != null && Ipc.ImportName != null)
                {
                    p.Edit(Ipc);
                    result = true;
                }
                if(file != null)
                {
                    foreach (HttpPostedFileBase fl in file)
                    {

                        var InputFileName = System.IO.Path.GetFileName(fl.FileName);
                        var ServerSavePath = System.IO.Path.Combine(Server.MapPath("~/Image/Img/") + InputFileName);
                        fl.SaveAs(ServerSavePath);
                        Uploadfile upfl = new Uploadfile();
                        upfl.ID = Ipc.ImportID;
                        upfl.Path = "~/Image/Img/" + InputFileName;
                        db.Uploadfiles.Add(upfl);
                  

                    }
                }
            db.SaveChanges();
            return Json(result,JsonRequestBehavior.AllowGet);

        }


        //public ActionResult Delete(int id)
        //{ 

        // var ou = db.Detailimportcoupons.FirstOrDefault(o => o.ImportID == id);
        //   if (ou == null)
        //    {
        //        p.Delete(id);
        //        return RedirectToAction("index");
        //    }

        //    return RedirectToAction("index");
        //}

        public JsonResult Delete(int id)
        {
            bool result = false;
            var input = db.Importcoupons.Where(o => o.ImportID == id);
            if (input != null)
            {
                var deletephoto = db.Uploadfiles.Where(o => o.ID == id).ToList();
                if(deletephoto != null)
                {
                    foreach(var item in deletephoto)
                    {
                        db.Uploadfiles.Remove(item);
                    }    
                }    

                var kt = db.Detailimportcoupons.FirstOrDefault(o => o.ImportID == id);
                if(kt == null)
                {
                    p.Delete(id);
                    result = true;
                }
              
            }
            db.SaveChanges();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetailsPar(int id)
        {
            var model = db.Detailimportcoupons.Where(o => o.ImportID == id).ToList();
            //var Imp = db.Detailimportcoupons.ToList();
            //ViewBag.Imp = Imp;
            return PartialView("PartialDetail", model);
        }

        public ActionResult Details(int id)
        {
            var Imp = db.Detailimportcoupons.ToList();
            ViewBag.Imp = Imp;
            return View(p.listID(id));
        }
        public PartialViewResult getpaging(int? page )
        {
            var searchform = Session["Searchinput"];
            var kt = Session["idinput"];
            ViewBag._id = kt;
           

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
                    var kt1 = unitSelectlists.Where(o => o.unit == item.Unit).ToList();
                    if (kt1.Count() < 1)
                    {
                        unitSelectlists.Add(new UnitSelectlist
                        {
                            unit = item.Unit
                        });
                    }

                }

            }
            ViewBag.unit = unitSelectlists.ToList();
            ViewBag.product = pro;
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            ViewBag.datenow = DateTime.Now;
            if (kt != null)
            { 
                if(searchform != null)
                {
                    var list = db.Detailimportcoupons.Where(o => o.ImportID == (int)kt && (o.Product.ProductName.Contains((string)searchform) || o.Importcoupon.ImportName.Contains((string)searchform) || o.Location.Contains((string)searchform) || o.Unit.Contains((string)searchform))).ToList();
                    list = list.OrderByDescending(n => n.DetailimportID).ToList();
                    return PartialView("_PartialView", list.ToPagedList(pageNumber, pageSize));

                }
                else
                {
                    var list = db.Detailimportcoupons.Where(o => o.ImportID == (int)kt).ToList();
                    list = list.OrderByDescending(n => n.DetailimportID).ToList();
                    return PartialView("_PartialView", list.ToPagedList(pageNumber, pageSize));
                }
                
            }
            else
            {
                if(searchform != null )
                {
                  
                        var list = db.Detailimportcoupons.Where(o => o.Product.ProductName.Contains((string)searchform) || o.Importcoupon.ImportName.Contains((string)searchform) || o.Location.Contains((string)searchform) || o.Unit.Contains((string)searchform)).ToList();
                        list = list.OrderByDescending(n => n.DetailimportID).ToList();
                        return PartialView("_PartialView", list.ToPagedList(pageNumber, pageSize));
                   
                  
                }
                else
                {
                   
                        var list = db.Detailimportcoupons.ToList();
                        list = list.OrderByDescending(n => n.DetailimportID).ToList();
                        return PartialView("_PartialView", list.ToPagedList(pageNumber, pageSize));
                   
                }    
                
            }
        }


        public JsonResult Search(string id)
        {
            bool result = false;
            if(id != null)
            {
                Session["Searchinput"] = id;
                //var test = Session["Searchinput"];
                result = true;
            }
            else
            {
                result = true;
                Session["Searchinput"] = null;
            }    

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult kt(int id)
        {

            bool result = false;
            Session["idinput"] = id;
            var test = Session["idinput"];
            if (test != null)
            {
                result = true;
            }
          

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSession()
        {

            Session["idinput"] = null;

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

       public JsonResult selectdate( int id)
        {
            bool result = false;
           
               if(id != 0)
                {
                    Session["selectdate"] = id;
                    result = true;
                }
               else
                {
                    Session["selectdate"] = null;
                    result = true;
                }    
           
            return Json(result,JsonRequestBehavior.AllowGet);
        }

    }
}