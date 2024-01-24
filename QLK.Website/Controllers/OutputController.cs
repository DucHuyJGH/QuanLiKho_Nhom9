using PagedList;
using QLK.Website.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLK.Website.Controllers
{
    public class OutputController : Controller
    {

        DetailOutput deta = new DetailOutput();
        Output p = new Output();
        QLKEntities db = new QLKEntities();
      
        // GET: Input
        public ActionResult Index(string SearchString, string currentFilter, int ? page, DateTime ? datein, DateTime ? dateout)
        {
      
            if (Request.Cookies["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var k = Session["idoutput"];
            ViewBag._id = k;
            var h = Session["Searchoutput"];
            ViewBag.searchform = h;
            ViewBag.UploadphotoBill = db.UploadPhotoBills.ToList();
            var dtin = DateTime.Parse("1/1/2020");
            var datei = datein ?? dtin;
            var dateo = dateout ?? DateTime.Now;
            ViewBag.datein = datei;
            ViewBag.dateout = dateo;
            ViewBag.datenow = DateTime.Now;
            if (k != null)
            {
                var detailbill = db.DetailBills.Where(o => o.BillID == (int)k).ToList();
                ViewBag.detailbill = detailbill;
            }
            else
            {
                var detailbill = db.DetailBills.ToList();
                ViewBag.detailbill = detailbill;
            }

            ViewBag.pg = Session["page"];

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
            ViewBag.pro = pro;
            var warehouses = db.Warehouses;
            ViewBag.Warehouses = warehouses;
            var bill = db.Bills.ToList();
            ViewBag.bill = bill;
            var ListOutput = new List<Bill>();
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
                    if(datei != null && dateo != null)
                    {
                    ListOutput = db.Bills.Where(o => (o.Warehouse.WarehouseName.Contains(SearchString) || o.BillName.Contains(SearchString)) && (o.Date >= datei && o.Date <= dateo)).ToList();
                    }
                    else
                    {
                    ListOutput = db.Bills.Where(o => o.Warehouse.WarehouseName.Contains(SearchString) || o.BillName.Contains(SearchString)).ToList();
                     }    
                 
                }
                else
                 {
                    if (datei != null && dateo != null)
                    {
                    ListOutput = db.Bills.Where(o=> o.Date >= datei && o.Date <= dateo).ToList();
                    }
                    else
                    {
                    ListOutput = db.Bills.ToList();
                    }    
                 }
            ViewBag.currentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            ListOutput = ListOutput.OrderByDescending(n => n.BillID).ToList();

            return View(ListOutput.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            var warehouses = db.Warehouses;
            ViewBag.Warehouses = warehouses;
            return View();
        }
        [HttpPost]
        public JsonResult Create(Bill output, HttpPostedFileBase[] file)
        {
            
            bool result = false;
           
            if (file != null)
            {
                if (output.WarehouseID != null && output.BillName != null)
                {
                    var date = DateTime.Now;
                    output.Date = date;
                    //output.Photo = "~/Image/Img/" + fileName;
                    p.Add(output);
                    result = true;
                }
                foreach (HttpPostedFileBase fl in file)
                {
                    var InputFileName = System.IO.Path.GetFileName(fl.FileName);
                    var ServerSavePath = System.IO.Path.Combine(Server.MapPath("~/Image/Img/") + InputFileName);
                    fl.SaveAs(ServerSavePath);
                    UploadPhotoBill upfl = new UploadPhotoBill();
                    upfl.IdBill = output.BillID;
                    upfl.Path = "~/Image/Img/" + InputFileName;
                    db.UploadPhotoBills.Add(upfl);                 

                }
            }
            db.SaveChanges();

            return Json(new { status = result }, JsonRequestBehavior.AllowGet);
        }
        //sua product theo id
        //public ActionResult Edit(int id)
        //{
        //    return View(p.listBillID(id));
        //}

        //[HttpPost]
        //public ActionResult Edit(Bill output)
        //{
        //    if (output.Date != null && output.WarehouseID != null)
        //    {
        //        p.Edit(output);
        //        return RedirectToAction("Index");
        //    }
        //    else
        //        return View();

        //}
        public ActionResult Edit(int id)
        {
            ViewBag.UploadphotoBill = db.UploadPhotoBills.ToList();
            var inpu = db.Warehouses.ToList();
            ViewBag.ware = inpu;
            var model = db.Bills.Where(o => o.BillID == id).FirstOrDefault();
            return PartialView("EditOpPartial", model);
        }

        [HttpPost]
        public JsonResult Edit(Bill Ipc , HttpPostedFileBase[] file)
        {
            var inpu = db.Warehouses.ToList();
            ViewBag.ware = inpu;
            bool result = false;

            if (Ipc.WarehouseID != null && Ipc.BillName != null)
            {
                p.Edit(Ipc);
                result = true;
            }
            if(file!=null)
            {
                foreach (HttpPostedFileBase fl in file)
                {
                    var InputFileName = System.IO.Path.GetFileName(fl.FileName);
                    var ServerSavePath = System.IO.Path.Combine(Server.MapPath("~/Image/Img/") + InputFileName);
                    fl.SaveAs(ServerSavePath);
                    UploadPhotoBill upfl = new UploadPhotoBill();
                    upfl.IdBill = Ipc.BillID;
                    upfl.Path = "~/Image/Img/" + InputFileName;
                    db.UploadPhotoBills.Add(upfl);

                }
            }
            db.SaveChanges();
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        //Xoa 1 product
        //public ActionResult Delete(int id)
        //{
        //    var im = db.DetailBills.FirstOrDefault(o => o.BillID == id);
        //    if (im == null)
        //    {
        //        p.Delete(id);
        //        return RedirectToAction("index");
        //    }

        //    return RedirectToAction("index");
        //}

        public JsonResult Delete(int id)
        {
            bool result = false;
            var input = db.Bills.Where(o => o.BillID == id);
            if (input != null)
            {
                var deletephoto = db.UploadPhotoBills.Where(o => o.IdBill == id).ToList();
                if (deletephoto != null)
                {
                    foreach(var item in deletephoto)
                    {
                        db.UploadPhotoBills.Remove(item);
                    }    
                }    
                var kt = db.DetailBills.FirstOrDefault(o => o.BillID == id);
                if (kt == null)
                {
                    p.Delete(id);
                    result = true;
                }

            }
            db.SaveChanges();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult getpaging(int? page)
        {
            var searchfrom = Session["Searchoutput"];
            var kt = Session["idoutput"];
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
                if(searchfrom != null)
                {
                    var list = db.DetailBills.Where(o => o.BillID == (int)kt && (o.Product.ProductName.Contains((string)searchfrom) || o.Bill.BillName.Contains((string)searchfrom) || o.Note.Contains((string)searchfrom) || o.Unit.Contains((string)searchfrom))).ToList();
                    list = list.OrderByDescending(n => n.DetailBillID).ToList();
                    return PartialView("_PartialView", list.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    var list = db.DetailBills.Where(o => o.BillID == (int)kt).ToList();
                    list = list.OrderByDescending(n => n.DetailBillID).ToList();
                    return PartialView("_PartialView", list.ToPagedList(pageNumber, pageSize));

                }    
            }
            else
            {
                if(searchfrom != null )
                {
                    var list = db.DetailBills.Where(o=>o.Product.ProductName.Contains((string)searchfrom) ||  o.Bill.BillName.Contains((string)searchfrom) || o.Note.Contains((string)searchfrom) || o.Unit.Contains((string)searchfrom)).ToList();
                    list = list.OrderByDescending(n => n.DetailBillID).ToList();
                    return PartialView("_PartialView", list.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    var list = db.DetailBills.ToList();
                    list = list.OrderByDescending(n => n.DetailBillID).ToList();
                    return PartialView("_PartialView", list.ToPagedList(pageNumber, pageSize));
                } 
                
                
            }

           
        }

        public ActionResult DetailsPar1(int id)
        {
            var billid = db.Bills.ToList();
            var items = db.Detailimportcoupons.ToList();
            ViewBag.data = items;
            ViewBag.billid = billid;
            var model = db.DetailBills.Where(o => o.BillID == id).ToList();
            return PartialView("Details1", model);
        }

        public JsonResult Search(string id)
        {
            bool result = false;
            if (id != null)
            {
                Session["Searchoutput"] = id;
                var test = Session["Searchoutput"];
                result = true;
            }
            else
            {
                result = true;
                Session["Searchoutput"] = null;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult kt( int id)
        {
           
            bool result = false;
            Session["idoutput"] = id;
            var test =Session["idoutput"];
            if (test != null)
             {
                result = true;
            }
     
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult DeleteSession()
        {
           
            Session["idoutput"] = null;

            return Json(new { result = true}, JsonRequestBehavior.AllowGet);
        }

    }
}