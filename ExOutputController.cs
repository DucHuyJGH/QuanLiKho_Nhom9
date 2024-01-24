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
    public class ExOutputController : Controller
    {
        // GET: ExOutput
        QLKEntities db = new QLKEntities();
        ListExOutput Ex = new ListExOutput();

        public ActionResult Index(string SearchString, string currentFilter, int? page, DateTime ? datein, DateTime ? dateout )
        {
            //if (Session["Username"] == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
          
            if (Request.Cookies["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var dtin = DateTime.Parse("1/1/2020");
            var datei = datein ?? dtin;
            var dateo = dateout ?? DateTime.Now;
            ViewBag.datein = datei;
            ViewBag.dateout = dateo;
            Session["search"] = SearchString;
            Session["dtin"] = datei;
            Session["dtout"] = dateo;
            ViewBag.datenow = DateTime.Now;
            if (db.DetailBills != null)
            {
                var sl = db.DetailBills.Sum(o => o.Quantity);
                var dem = db.DetailBills.Count();
                ViewBag.sl = sl;
                ViewBag.dem = dem;
            }
            else
            {
                ViewBag.sl = 0;
                ViewBag.dem = 0;
            }
            var listrExOutput = new List<ExOutput>();

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

                listrExOutput = Ex.ListExoutputSearch(SearchString, datei, dateo);
            }
            else
            {
                listrExOutput = Ex.ListExoutput(datei, dateo);
            }
            ViewBag.UploadPhotoProcut = db.UploadPhotoProducts.ToList();
            ViewBag.currentFilter = SearchString;
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            listrExOutput = listrExOutput.OrderByDescending(n => n.DetailsOutputID).ToList();

            return View(listrExOutput.ToPagedList(pageNumber, pageSize));
        }


        public FileResult Export()
        {
            var str = Session["search"];
            var datein = Session["dtin"];
            var dateout = Session["dtout"];
            QLKEntities db = new QLKEntities();
            System.Data.DataTable dt = new System.Data.DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[] {
                                            new DataColumn("Mã chi tiết phiếu xuất"),
                                            new DataColumn("Ngày xuất"),
                                            new DataColumn("Tên mặt hàng"),
                                            new DataColumn("Tên loại mặt hàng"),
                                            new DataColumn("Tên nhà cung cấp"),
                                            new DataColumn("Đơn vị tính"),
                                            new DataColumn("Đơn giá"),
                                            new DataColumn("Số lượng"),
                                            new DataColumn("Thành tiền")
            });
          
            var kt = db.DetailBills.FirstOrDefault(o => o.Quantity == null);
            if(kt != null)
            {
                var kt1 = db.Detailimportcoupons.FirstOrDefault (o => o.Quantity == null) ;
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



            List<ExOutput> exOutputs = new List<ExOutput>();
            if(str != null)
            {
                var temp = db.DetailBills.Where(o => (o.Product.ProductName.Contains((string)str) || o.ProductID.Contains((string)str) || o.Product.Category.CategoryName.Contains((string)str) || o.Product.Supplier.SupplierName.Contains((string)str)) && (o.Bill.Date >= (DateTime)datein && o.Bill.Date <= (DateTime)dateout));
                foreach (var ovbj in temp)
                {

                    var Pr = db.Products.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                    var sup = db.Suppliers.FirstOrDefault(o => o.SupplierID == Pr.SupplierID);
                    var bi = db.Bills.FirstOrDefault(o => o.BillID == ovbj.BillID);
                    var cate = db.Categories.FirstOrDefault(o => o.CategoryID == Pr.CategoryID);
                    exOutputs.Add(new ExOutput
                    {

                        Photo = Pr.Photo,
                        DetailsOutputID = ovbj.DetailBillID,
                        ProductID = Pr.ProductID,
                        CategoryID = cate.CategoryID,
                        BillID = (int)ovbj.BillID,
                        ProductName = Pr.ProductName,
                        SupplierName = sup.SupplierName,
                        CategoryName = cate.CategoryName,
                        Date = (DateTime)bi.Date,
                        Unit = ovbj.Unit,
                        Price =ovbj.Price,
                        Quantityout = (int)ovbj.Quantity,
                        Tolalpriceou = ovbj.Price * (int)ovbj.Quantity
                    });
                }
                foreach (var ovjb in exOutputs)
                {
                    dt.Rows.Add(ovjb.DetailsOutputID, ovjb.Date, ovjb.ProductName, ovjb.SupplierName, ovjb.CategoryName, ovjb.Unit, ovjb.Price, ovjb.Quantityout, ovjb.Tolalpriceou);
                }
            }
            else
            {
                var temp = db.DetailBills.Where(o => o.Bill.Date >= (DateTime)datein && o.Bill.Date <= (DateTime)dateout);
                foreach (var ovbj in temp)
                {

                    var Pr = db.Products.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                    var sup = db.Suppliers.FirstOrDefault(o => o.SupplierID == Pr.SupplierID);
                    var bi = db.Bills.FirstOrDefault(o => o.BillID == ovbj.BillID);
                    var cate = db.Categories.FirstOrDefault(o => o.CategoryID == Pr.CategoryID);
                    exOutputs.Add(new ExOutput
                    {

                        Photo = Pr.Photo,
                        DetailsOutputID = ovbj.DetailBillID,
                        ProductID = Pr.ProductID,
                        CategoryID = cate.CategoryID,
                        BillID = (int)ovbj.BillID,
                        ProductName = Pr.ProductName,
                        SupplierName = sup.SupplierName,
                        CategoryName = cate.CategoryName,
                        Date = (DateTime)bi.Date,
                        Unit = ovbj.Unit,
                        Price = ovbj.Price,
                        Quantityout = (int)ovbj.Quantity,
                        Tolalpriceou = ovbj.Price * (int)ovbj.Quantity
                    });
                }
                foreach (var ovjb in exOutputs)
                {
                    dt.Rows.Add(ovjb.DetailsOutputID, ovjb.Date, ovjb.ProductName, ovjb.SupplierName, ovjb.CategoryName,ovjb.Unit,ovjb.Price, ovjb.Quantityout,ovjb.Tolalpriceou);
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BaoCaoXuatKho.xlsx");
                }
            }
        }
    }
}