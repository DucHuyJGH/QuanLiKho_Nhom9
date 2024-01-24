using PagedList;
using QLK.Website.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
namespace QLK.Website.Controllers
{
    public class SupplierController : Controller
    {
       // GET: Category
        Supplierdb p = new Supplierdb();
        QLKEntities db = new QLKEntities();

        // GET: Product
        public ActionResult Index( string SearchString, string currentFilter, int? page)
        {
            //if (Session["Username"] == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            if (Request.Cookies["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var supplier = db.Suppliers.ToList();
            ViewBag.Supplier = supplier;
            var listsupplier = new List<Supplier>();
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
                listsupplier = db.Suppliers.Where(o => o.SupplierName.Contains(SearchString) || 
                o.Adderss.Contains(SearchString) || o.City.Contains(SearchString)|| o.ContactName.Contains(SearchString) ||
                o.Country.Contains(SearchString) || o.Phone.Contains(SearchString)).ToList();
            }
            else
            {
                listsupplier = db.Suppliers.ToList();
            }
            ViewBag.currentFilter = SearchString;
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            listsupplier = listsupplier.OrderByDescending(n => n.SupplierID).ToList();

            return View(listsupplier.ToPagedList(pageNumber, pageSize));

        }
        //tao 1 product
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Supplier cat)
        {
            bool result = false;
            if (cat.ContactName != null && cat.ContactName != null && cat.Adderss != null
                  && cat.City != null && cat.Country != null && cat.Phone != null)
            {
                p.Add(cat);
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
             
       
        }

        public ActionResult Edit(int id)
        {
            var model = db.Suppliers.Where(o => o.SupplierID == id).FirstOrDefault();
            return PartialView("EditsuppPartial", model);

        }
        [HttpPost]
        public JsonResult Edit(Supplier supplier)
        {
            bool result = false;
            if (supplier.SupplierName != null && supplier.ContactName != null && supplier.Adderss != null
                && supplier.City != null && supplier.Zipcode != null && supplier.Country != null && supplier.Phone != null)
            {
                p.Edit(supplier);
                result = true;
            }

            return Json(result,JsonRequestBehavior.AllowGet);

        }


        public JsonResult Delete(int id)
        {
            bool result = false;
            var input = db.Suppliers.Where(o => o.SupplierID == id);
            if (input != null)
            {
                var kt = db.Products.FirstOrDefault(o => o.SupplierID == id);
                if (kt == null)
                {
                    p.Delete(id);
                    result = true;
                }

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult Delete(int id)
        //{

        //    var model = db.Products.FirstOrDefault(o => o.SupplierID == id);
        //    if(model == null)
        //    {

        //        p.Delete(id);

        //        return RedirectToAction("index");
        //    }

        //    return RedirectToAction("index");
        //}
        public JsonResult Import(HttpPostedFileBase exlfile)
        {
            bool result = false;
            try
            {
                if (exlfile != null)
                {
                    if (exlfile.FileName.EndsWith("xls") || exlfile.FileName.EndsWith("xlsx"))
                    {
                        var InputFileName = System.IO.Path.GetFileName(exlfile.FileName);
                        var ServerSavePath = System.IO.Path.Combine(Server.MapPath("~/Excel/") + InputFileName);
                        exlfile.SaveAs(ServerSavePath);
                        string path = Server.MapPath("~/Excel/" + exlfile.FileName);

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                            exlfile.SaveAs(path);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                                exlfile.SaveAs(path);

                                Excel.Application application = new Excel.Application();
                                Excel.Workbook workbook = application.Workbooks.Open(path);
                                Excel.Worksheet worksheet = workbook.ActiveSheet;
                                Excel.Range range = worksheet.UsedRange;
                                for (int row = 2; row <= range.Rows.Count; row++)
                                {
                                    Supplier supplier = new Supplier();
                                    supplier.SupplierName= ((Excel.Range)range.Cells[row, 1]).Text;
                                    supplier.ContactName = ((Excel.Range)range.Cells[row, 2]).Text;
                                    supplier.Adderss = ((Excel.Range)range.Cells[row, 3]).Text;
                                    supplier.City = ((Excel.Range)range.Cells[row, 4]).Text;
                                    supplier.Zipcode = ((Excel.Range)range.Cells[row, 5]).Text;
                                    supplier.Country = ((Excel.Range)range.Cells[row, 6]).Text;
                                    supplier.Phone = ((Excel.Range)range.Cells[row, 7]).Text;
                                    supplier.Note = ((Excel.Range)range.Cells[row, 8]).Text;
                                    db.Suppliers.Add(supplier);
                                    db.SaveChanges();
                                    result = true;
                                }
                            }
                        }
                    }

                }
            }
            catch
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}