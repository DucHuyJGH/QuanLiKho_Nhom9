using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using QLK.Website.Model;
using PagedList;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using Excel = Microsoft.Office.Interop.Excel;

namespace QLK.Website.Controllers
{
    public class ProductController : Controller
    {
        QLKEntities db = new QLKEntities();
        ListProduct p = new ListProduct();

        // GET: Product
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {

            if (Request.Cookies["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<UnitSelectlist> unitSelectlists = new List<UnitSelectlist>();
           
            var pro = db.Products.ToList();
            foreach(var item in pro)
            {
              if(unitSelectlists.Count()<1)
                {
                    unitSelectlists.Add(new UnitSelectlist
                    {
                        unit = item.Unit
                    });
                } 
              else
                {
                    var kt = unitSelectlists.Where(o => o.unit == item.Unit).ToList();
                    if (kt.Count()<1)
                    {
                            unitSelectlists.Add(new UnitSelectlist
                            {
                                unit = item.Unit
                            });
                    }
                    
                }    

            }
            ViewBag.unit = unitSelectlists.ToList();
            var product = db.Products.ToList();
            var CateID = db.Categories.ToList();
            var SuppID = db.Suppliers.ToList();
            ViewBag.Category = CateID;
            ViewBag.Supplier = SuppID;
            ViewBag.Product = product;
            
            ViewBag.UploadPhotoProcut = db.UploadPhotoProducts.ToList();
            var listproduct = new List<Product>();

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
                listproduct = db.Products.Where(o => o.ProductName.Contains(SearchString)).ToList();
            }
            else
            {
                listproduct = db.Products.ToList();
            }
            ViewBag.currentFilter = SearchString;
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            listproduct = listproduct.OrderByDescending(n => n.ProductID).ToList();

            return View(listproduct.ToPagedList(pageNumber, pageSize));

        }
          public ActionResult Create ()
            {
                return View();
            }

      

        [HttpPost]
        public JsonResult Create(Product product, HttpPostedFileBase[] file)
        {
            bool result = false;
            var CateID = db.Categories.ToList();
            var SuppID = db.Suppliers.ToList();
            ViewBag.Category = CateID;
            ViewBag.Supplier = SuppID;
            var kt = db.Products.Where(o => o.ProductID == product.ProductID).FirstOrDefault();

            if (file != null)
            {
                if (kt == null)
                {

                  
                    if (product.CategoryID != null && product.SupplierID != null && product.ProductName != null && product.Unit != null)
                    {
                        db.Products.Add(product);
                        result = true;
                    }
                
                    foreach (HttpPostedFileBase fl in file)
                    {

                        var InputFileName = System.IO.Path.GetFileName(fl.FileName);
                        var ServerSavePath = System.IO.Path.Combine(Server.MapPath("~/Image/Img/") + InputFileName);
                        fl.SaveAs(ServerSavePath);
                        UploadPhotoProduct upfl = new UploadPhotoProduct();
                        upfl.IDProduct = product.ProductID;
                        upfl.Path = "~/Image/Img/" + InputFileName;
                        db.UploadPhotoProducts.Add(upfl);
                      

                    }
                    db.SaveChanges();

                }
            }
            return Json(new { status = result }, JsonRequestBehavior.AllowGet);

        }



        //[HttpPost]
        //public ActionResult Create(Product product, HttpPostedFileBase file)
        //{
        //    var CateID = db.Categories.ToList();
        //    var SuppID = db.Suppliers.ToList();
        //    ViewBag.Category = CateID;
        //    ViewBag.Supplier = SuppID;

        //    var kt = db.Products.Where(o => o.ProductID == product.ProductID).FirstOrDefault();

        //    if (file != null)
        //    {
        //        if (kt == null)
        //        {
        //            string fileName = System.IO.Path.GetFileName(file.FileName);
        //            //string url = Server.MapPath("~/Image/Img/" + fileName);
        //            //file.SaveAs(url);
        //            if (product.CategoryID != null && product.SupplierID != null && product.ProductName != null && product.Unit != null)
        //            {
        //                product.Photo = "~/Image/Img/" + fileName;
        //                db.Products.Add(product);
        //                db.SaveChanges();
        //                return RedirectToAction("index");
        //            }
        //            else
        //                return View();
        //        }
        //        ViewBag.error = "Bạn đã nhập trùng mã mặt hàng!";

        //    }
        //    return View();

        //}

        //public ActionResult Edit(int id)
        //{

        //        var pro = db.Products.ToList();
        //        var cate = db.Categories.ToList();
        //        var sup = db.Suppliers.ToList();
        //        ViewBag.cate = cate;
        //        ViewBag.pro = pro;
        //        ViewBag.sup = sup;

        //    return View(p.listID(id));
        //}

        //[HttpPost]
        //public ActionResult Edit(Product product)
        //{
        //    var pro = db.Products.ToList();
        //    var cate = db.Categories.ToList();
        //    var sup = db.Suppliers.ToList();
        //    ViewBag.cate = cate;
        //    ViewBag.pro = pro;
        //    ViewBag.sup = sup;

        //    if (product.CategoryID != null && product.SupplierID != null
        //        && product.ProductName != null && product.Unit != null
        //        && product.Photo != null)
        //    {
        //        p.Edit(product);
        //        return RedirectToAction("index");
        //    }
        //    else
        //        return View();
        //}

        public ActionResult Edit(string id)
        {

            var pro = db.Products.ToList();
            var cate = db.Categories.ToList();
            var sup = db.Suppliers.ToList();
            ViewBag.UploadphotoProduct = db.UploadPhotoProducts.ToList();
            ViewBag.cate = cate;
            ViewBag.pro = pro;
            ViewBag.sup = sup;
            var model = db.Products.Where(o => o.ProductID == id).FirstOrDefault();
            return PartialView("Partial", model);
        }

        [HttpPost]
        public JsonResult Edit(Product product, HttpPostedFileBase[] file1)
        {
            bool result = false;
            var pro = db.Products.ToList();
            var cate = db.Categories.ToList();
            var sup = db.Suppliers.ToList();
            ViewBag.cate = cate;
            ViewBag.pro = pro;
            ViewBag.sup = sup;
            if (product.ProductName != null && product.CategoryID != null && product.SupplierID != null && product.Unit != null)
            {
                p.Edit(product);
                result = true;
            }
            if (file1 != null)
            {
               
                foreach (HttpPostedFileBase fl in file1)
                {

                    var InputFileName = System.IO.Path.GetFileName(fl.FileName);
                    var ServerSavePath = System.IO.Path.Combine(Server.MapPath("~/Image/Img/") + InputFileName);
                    fl.SaveAs(ServerSavePath);
                    UploadPhotoProduct upfl = new UploadPhotoProduct();
                    upfl.IDProduct = product.ProductID;
                    upfl.Path = "~/Image/Img/" + InputFileName;
                    db.UploadPhotoProducts.Add(upfl);


                }
             

            }
            db.SaveChanges();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //Xoa 1 product
        //public ActionResult Delete(int id)
        //{

        //    var bill = db.DetailBills.FirstOrDefault(o => o.ProductID == id);
        //    var ipm = db.Detailimportcoupons.FirstOrDefault(o => o.ProductID == id);

        //    if (bill == null && ipm == null)
        //    {
        //        p.Delete(id);
        //        return RedirectToAction("index");
        //    }
        //    else
        //        return RedirectToAction("index");


        //}
        public JsonResult Delete(string id)
        {

            bool result = false;
            
            var Pro = db.Products.Where(o => o.ProductID == id);
            if (Pro != null)
            {
                var bill = db.DetailBills.FirstOrDefault(o => o.ProductID == id);
                var ipm = db.Detailimportcoupons.FirstOrDefault(o => o.ProductID == id);
                
                if (bill == null && ipm == null)
                {
                    p.Delete(id);
                    var photopro = db.UploadPhotoProducts.Where(o => o.IDProduct == id).ToList();
                    if(photopro != null)
                    {
                        foreach (var item in photopro)
                        {
                            db.UploadPhotoProducts.Remove(item);

                        }
                    }    
                    result = true;
                }
            }
            db.SaveChanges();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Deletephoto(string id)
        {
            bool result = false;
            var kt = db.UploadPhotoProducts.Where(o => o.IDProduct == id).ToList();
            if(kt!= null)
            {
                foreach(var item in kt)
                {
                    db.UploadPhotoProducts.Remove(item);
                    result = true;
                }    
               
            }    
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult import(HttpPostedFileBase exfile)
        {
            bool result = false;
            try {

                if (exfile != null)
                {

                    if (exfile.FileName.EndsWith("xls") || exfile.FileName.EndsWith("xlsx"))
                    {
                        var InputFileName = System.IO.Path.GetFileName(exfile.FileName);
                        var ServerSavePath = System.IO.Path.Combine(Server.MapPath("~/Excel/") + InputFileName);
                        exfile.SaveAs(ServerSavePath);

                        string path = Server.MapPath("~/Excel/" + exfile.FileName);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                            exfile.SaveAs(path);
                            Excel.Application application = new Excel.Application();
                            Excel.Workbook workbook = application.Workbooks.Open(path);
                            Excel.Worksheet worksheet = workbook.ActiveSheet;
                            Excel.Range range = worksheet.UsedRange;
                            for (int row = 2; row <= range.Rows.Count; row++)
                            {
                                int cateID = int.Parse(((Excel.Range)range.Cells[row, 2]).Text);
                                int supID = int.Parse(((Excel.Range)range.Cells[row, 6]).Text);
                                string ProID = ((Excel.Range)range.Cells[row, 1]).Text;
                                var ktcateID = db.Categories.Where(o => o.CategoryID == cateID).ToList();
                                var ktsupID = db.Suppliers.Where(o => o.SupplierID == supID).ToList();
                                var ktProID = db.Products.Where(o => o.ProductID == ProID).ToList();

                                if (ktcateID.Count() > 0 || ktsupID.Count() > 0)
                                {
                                    Product Pr = new Product();
                                    Pr.ProductID = ((Excel.Range)range.Cells[row, 1]).Text;
                                    Pr.CategoryID = int.Parse(((Excel.Range)range.Cells[row, 2]).Text);
                                    Pr.ProductName = ((Excel.Range)range.Cells[row, 3]).Text;
                                    Pr.Unit = ((Excel.Range)range.Cells[row, 4]).Text;
                                    Pr.Price = int.Parse(((Excel.Range)range.Cells[row, 5]).Text);
                                    Pr.SupplierID = int.Parse(((Excel.Range)range.Cells[row, 6]).Text);
                                    result = true;
                                    db.Products.Add(Pr);
                                }
                            }
                            db.SaveChanges();

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