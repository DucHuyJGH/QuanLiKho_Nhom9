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
    
    public class CategoryController : Controller
    {
        
        Categori p = new Categori();
        QLKEntities db = new QLKEntities();
        [Route]
        public ActionResult Index(string currentFilter, string SearchString, int? page)

        {
            if (Request.Cookies["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //ViewData["Message"] = Session["Mesenger"];
            var category = db.Categories.ToList();
            ViewBag.category = category;
            ViewBag.page = page;
            var listcate = new List<Category>();
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
                listcate = db.Categories.Where(o => o.CategoryName.Contains(SearchString)).ToList();
            }
            else
            {
                listcate = db.Categories.ToList();
            }
            ViewBag.currentFilter = SearchString;
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            listcate = listcate.OrderByDescending(n => n.CategoryID).ToList();
            return View(listcate.ToPagedList(pageNumber, pageSize));


        }
        //tao 1 product
       
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Create(Category cat)
        {
            bool result = false;
            //Session["Mesenger"] = null; 
            if (cat.CategoryName != null && cat.Description != null)
            {
                p.Add(cat);
                result = true;
                 //Session["Mesenger"] = "Đã thêm thành công!";


            }
            return Json(result, JsonRequestBehavior.AllowGet);
                //return View();

        }

        public ActionResult Edit(int id)
        {
            var model = db.Categories.Where(o => o.CategoryID == id).FirstOrDefault();
            return PartialView("Partial", model);
        }

        [HttpPost]
        public JsonResult Edit(Category categori)
        {

            bool result = false;
            if (categori.CategoryName != null && categori.Description != null)
            {
                p.Edit(categori);
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool result = false;
            
            var cate = db.Categories.FirstOrDefault(o => o.CategoryID == id);
            if (cate != null)
            {
                var kt = db.Products.FirstOrDefault(i => i.CategoryID == id);
                if(kt == null)
                {
                    db.Categories.Remove(cate);
                    db.SaveChanges();
                    result = true;
                }    
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Import(HttpPostedFileBase excelfile)
        {
            bool result = false;
            try {
                if ( excelfile != null)
                {
                    if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                    {
                        var InputFileName = System.IO.Path.GetFileName(excelfile.FileName);
                        var ServerSavePath = System.IO.Path.Combine(Server.MapPath("~/Excel/") + InputFileName);
                        excelfile.SaveAs(ServerSavePath);
                        string path = Server.MapPath("~/Excel/" + excelfile.FileName);

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                            excelfile.SaveAs(path);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                                excelfile.SaveAs(path);


                                Excel.Application application = new Excel.Application();
                                Excel.Workbook workbook = application.Workbooks.Open(path);
                                Excel.Worksheet worksheet = workbook.ActiveSheet;
                                Excel.Range range = worksheet.UsedRange;

                                for (int row = 1; row <= range.Rows.Count; row++)
                                {
                                    Category cate = new Category();
                                    cate.CategoryName = ((Excel.Range)range.Cells[row, 1]).Text;
                                    cate.Description = ((Excel.Range)range.Cells[row, 2]).Text;
                                    db.Categories.Add(cate);
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
            //ViewBag.categori = db.Categories.ToList();
            //var model = db.Categories.ToList();
            //return PartialView("_PartialImportEx", model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}