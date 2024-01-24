using ClosedXML.Excel;
using PagedList;
using QLK.Website.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLK.Website.Controllers
{
    public class ExInputController : Controller
    {
        ListExInput Ex = new ListExInput();
        QLKEntities db = new QLKEntities();
        public ActionResult Index(string SearchString, string currentFilter, int? page, DateTime ? datein, DateTime ? dateout )
        {

            if (Request.Cookies["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            var dtin = DateTime.Parse("1/1/2020");
            var datei = datein ?? dtin;
            var dateo = dateout?? DateTime.Now;
            ViewBag.datein = datei;
            ViewBag.dateout = dateo;
            ViewBag.datenow = DateTime.Now;
            Session["search"] = SearchString;
            Session["dtin"] = datei;
            Session["dtout"] = dateo;

            if (db.Detailimportcoupons != null)
            {
                var sl = db.Detailimportcoupons.Sum(o => o.Quantity);
                var dem = db.Detailimportcoupons.Count();
                ViewBag.sl = sl;
                ViewBag.dem = dem;
            }
            else
            {
                ViewBag.sl = 0;
                ViewBag.dem = 0;
            }    
            var listrExInput = new List<ExInput>();

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

                listrExInput = Ex.ListExinput(SearchString, datei, dateo);
            }
            else
            {
                listrExInput = Ex.ListExinput(datei, dateo);
            }
            ViewBag.currentFilter = SearchString;
            ViewBag.UploadPhotoProcut = db.UploadPhotoProducts.ToList();
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            listrExInput = listrExInput.OrderByDescending(n => n.DetailsImport).ToList();

            return View(listrExInput.ToPagedList(pageNumber, pageSize));
        }

        public FileResult Export()
        {

            var str = Session["search"];
            var datein = Session["dtin"];
            var dateout = Session["dtout"];
                QLKEntities db = new QLKEntities();
                System.Data.DataTable dt = new System.Data.DataTable("Grid");
                dt.Columns.AddRange(new DataColumn[9] {
                                            new DataColumn("Mã chi tiết phiếu nhập"),
                                               new DataColumn("Ngày nhập"),
                                             new DataColumn("Tên mặt hàng"),
                                               new DataColumn("Tên loại hàng"),
                                            new DataColumn("Tên nhà cung cấp"),
                                            new DataColumn("Đơn vị tính"),
                                             new DataColumn("Đơn giá"),
                                             new DataColumn("Số lượng"),
                                              new DataColumn("Thành tiền")
                });

            var kt = db.DetailBills.FirstOrDefault(o => o.Quantity == null);
            if (kt != null)
            {
                var kt1 = db.Detailimportcoupons.FirstOrDefault(o => o.Quantity == null);
                if (kt1 != null)
                {
                    int detailquantity = (int)db.DetailBills.Sum(o => o.Quantity);
                    int importquantity = (int)db.Detailimportcoupons.Sum(p => p.Quantity);
                    int tong = importquantity - detailquantity;

                }
                else
                {
                    int detailquantity = 0;
                    int importquantity = (int)db.Detailimportcoupons.Sum(p => p.Quantity);
                    int tong = importquantity - detailquantity;
                }
            }

            else
            {
                int detailquantity = 0;
                int importquantity = 0;
                int tong = detailquantity - importquantity;
            }
            List<ExInput> exInputs = new List<ExInput>();
                
            if (str != null)
            {
                var temp = db.Detailimportcoupons.Where(o => (o.Unit.Contains((string)str) || o.Product.ProductName.Contains((string)str) || o.ProductID.Contains((string)str) ||
               o.Product.Category.CategoryName.Contains((string)str) || o.Product.Supplier.SupplierName.Contains((string)str)) && (o.Importcoupon.Date >= (DateTime)datein && o.Importcoupon.Date <= (DateTime)dateout));

                foreach (var ovbj in temp)
                {

                    var Pr = db.Products.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                    var sup = db.Suppliers.FirstOrDefault(o => o.SupplierID == Pr.SupplierID);
                    var im = db.Importcoupons.FirstOrDefault(o => o.ImportID == ovbj.ImportID);
                    var cate = db.Categories.FirstOrDefault(o => o.CategoryID == Pr.CategoryID);
                    exInputs.Add(new ExInput
                    {

                        Photo = Pr.Photo,
                        DetailsImport = ovbj.DetailimportID,
                        ProductID = Pr.ProductID,
                        CategoryID = cate.CategoryID,
                        ImportID = (int)ovbj.ImportID,
                        ProductName = Pr.ProductName,
                        SupplierName = sup.SupplierName,
                        CategoryName = cate.CategoryName,
                        Date = (DateTime)im.Date,
                        Unit = ovbj.Unit,
                        Price = ovbj.Price,
                        QuantityIm = (int)ovbj.Quantity,
                        Tolalpricein = ovbj.Price * (int)ovbj.Quantity
                    });
                }

                foreach (var ovjb in exInputs)
                {
                    dt.Rows.Add(ovjb.DetailsImport, ovjb.Date, ovjb.ProductName, ovjb.CategoryName, ovjb.SupplierName, ovjb.Unit, ovjb.Price, ovjb.QuantityIm, ovjb.Tolalpricein);
                }
            }
            else
            {
                var temp = db.Detailimportcoupons.Where(o=> o.Importcoupon.Date >= (DateTime)datein && o.Importcoupon.Date <= (DateTime)dateout).ToList();
                foreach (var ovbj in temp)
                {

                    var Pr = db.Products.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                    var sup = db.Suppliers.FirstOrDefault(o => o.SupplierID == Pr.SupplierID);
                    var im = db.Importcoupons.FirstOrDefault(o => o.ImportID == ovbj.ImportID);
                    var cate = db.Categories.FirstOrDefault(o => o.CategoryID == Pr.CategoryID);
                    exInputs.Add(new ExInput
                    {

                        Photo = Pr.Photo,
                        DetailsImport = ovbj.DetailimportID,
                        ProductID = Pr.ProductID,
                        CategoryID = cate.CategoryID,
                        ImportID = (int)ovbj.ImportID,
                        ProductName = Pr.ProductName,
                        SupplierName = sup.SupplierName,
                        CategoryName = cate.CategoryName,
                        Date = (DateTime)im.Date,
                        Unit = ovbj.Unit,
                        Price = ovbj.Price,
                        QuantityIm = (int)ovbj.Quantity,
                        Tolalpricein = ovbj.Price * (int)ovbj.Quantity
                    });
                }

                foreach (var ovjb in exInputs)
                {
                    dt.Rows.Add( ovjb.DetailsImport, ovjb.Date, ovjb.ProductName, ovjb.CategoryName, ovjb.SupplierName,ovjb.Unit,ovjb.Price, ovjb.QuantityIm,ovjb.Tolalpricein);
                }
            }    
  
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BaoCaoNhapKho.xlsx");
                    }
                }

            
        }
    }    
            
}