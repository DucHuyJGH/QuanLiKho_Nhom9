using QLK.Website.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace QLK.Website.Controllers
{
    public class DetailOutputController : Controller      
    {
        QLKEntities db = new QLKEntities();   
        DetailOutput d = new DetailOutput();

        public ActionResult Index(string SearchString, string currentFilter, int? page, DateTime ? datein, DateTime ? dateout)
        {
           
            if (Request.Cookies["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var detailOp = db.DetailBills.ToList();
            var Pro = db.Products.ToList();
            var bill = db.Bills.ToList();
            ViewBag.product = Pro;
            ViewBag.detailOp = detailOp;
            ViewBag.bill = bill;
            var tong = db.DetailBills.Sum(s => s.Quantity);
            ViewBag.tong = tong;
            ViewBag.datenow = DateTime.Now;

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

            var dtin = DateTime.Parse("1/1/2020");
            var datei = datein ?? dtin;
            var dateo = dateout ?? DateTime.Now;
            ViewBag.datein = datei;
            ViewBag.dateout = dateo;

            var listdetailoutput = new List<DetailBill>();
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
                listdetailoutput = db.DetailBills.Where(o => (o.Product.ProductName.Contains(SearchString) || o.Bill.BillName.Contains(SearchString) || o.Unit.Contains(SearchString)|| o.Note.Contains(SearchString)) &&(o.Bill.Date >= datei && o.Bill.Date <=dateo)).ToList();
                }
                 else
                     {
                         listdetailoutput = db.DetailBills.Where(o=>o.Bill.Date>=datei && o.Bill.Date<=dateo).ToList();
                     }
            ViewBag.currentFilter = SearchString;
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            listdetailoutput = listdetailoutput.OrderByDescending(n => n.DetailBillID).ToList();

            return View(listdetailoutput.ToPagedList(pageNumber, pageSize));


        }

        public ActionResult Create()
        {
            
            var billid = db.Bills.ToList();
            var items = db.Detailimportcoupons.ToList();
        
               ViewBag.data = items;
               ViewBag.billid = billid;

            return View();
        }

        [HttpPost]
        public JsonResult Create(DetailBill detail)
        {
            bool result = false;
            var kt = db.Detailimportcoupons.FirstOrDefault(o => o.ProductID == detail.ProductID);
            if(kt != null)
            {
                if (detail.Unit != null && detail.Quantity != null)
                {
                    d.Add(detail);
                    result = true;
                }
            }    
           
            return Json(result, JsonRequestBehavior.AllowGet);

                //return RedirectToAction("index");
    
        }

        [HttpPost]
        public JsonResult Create1(string BillID, string ProductID, string Unit, string Price, string Quantity , string Note)
        {
            bool result = false;
            try
            {
                List<string> billID = BillID.Split(',').ToList();
                var count = billID.Count();
                List<string> productID = ProductID.Split(',').ToList();
                List<string> unit1 = Unit.Split(',').ToList();
                List<string> price = Price.Split(',').ToList();
                List<string> quantity = Quantity.Split(',').ToList();
                List<string> note = Note.Split(',').ToList();

                for (int i = 0; i < count; i++)
                {
                    string pro = productID[i];
                    var kt = db.Detailimportcoupons.FirstOrDefault(o => o.ProductID == pro);
                    if (kt != null)
                    {
                        DetailBill dt = new DetailBill();
                        dt.BillID = int.Parse(billID[i]);
                        dt.ProductID = productID[i];
                        dt.Unit = unit1[i];
                        dt.Quantity = int.Parse(quantity[i]);
                        dt.Note = note[i];
                        dt.Price = int.Parse(price[i]);
                        db.DetailBills.Add(dt);
                        result = true;
                    }
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
        //    var pro = db.Products.ToList();
        //    ViewBag.pro = pro;
        //    var bill = db.Bills.ToList();
        //    ViewBag.bill = bill;
        //    return View(d.listDetailBillID(id));
        //}
        //[HttpPost]

        //public ActionResult Edit(DetailBill detail)
        //{
        //    var pro = db.Products.ToList();
        //    ViewBag.pro = pro;
        //    var bill = db.Bills.ToList();
        //    ViewBag.bill = bill;
        //    if (detail.BillID != null && detail.ProductID != null && detail.Unit != null && detail.Quantity !=  null)
        //    {
        //        d.Edit(detail);
        //        return RedirectToAction("index");
        //    }
        //    else
        //        return View(); 

        //}

        public ActionResult Edit(int id)
        {

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
            ViewBag.pro = pro;
            var bill = db.Bills.ToList();
            ViewBag.bill = bill;
            var model = db.DetailBills.Where(o => o.DetailBillID == id).FirstOrDefault();
            return PartialView("Partialdetailoutput", model);
        }

        [HttpPost]
        public JsonResult Edit(DetailBill detail)
        {
            var pro = db.Products.ToList();
            ViewBag.pro = pro;
            var bill = db.Bills.ToList();
            ViewBag.bill = bill;
            bool result = false;
            if (detail.BillID != null && detail.ProductID != null && detail.Unit != null && detail.Quantity != null)
            {
                d.Edit(detail);
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        //Xoa 1 product
        //public ActionResult Delete(int id)
        //{
        //    d.Delete(id);
        //    return RedirectToAction("index");
        //}
        public JsonResult Delete( int id)
        {
            bool result = false;
            var detailout = db.DetailBills.Where(o => o.DetailBillID == id);
            if(detailout != null )
            {
                d.Delete(id);
                result = true;

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int id)
        {
            return View(d.listDetailBillID(id));
        }
      
    }
}