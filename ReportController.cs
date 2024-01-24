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
    public class ReportController : Controller
    {
        QLKEntities db = new QLKEntities();
        ListReport Listrp = new ListReport();
        List<TotalPriceImport> TotalPriceImports = new List<TotalPriceImport>();
        List<TotalPricebill> TotalPricebills = new List<TotalPricebill>();
        // GET: Report
        public ActionResult Index( string SearchString, string currentFilter, int ? page)
        {
           
            Session["search"] = SearchString;
           
            if (Request.Cookies["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var listreport = new List<Report>();
          
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
                    listreport = Listrp.SearchRp(SearchString);
            }
            else
            {
                listreport = Listrp.ListRp();
            }


          
            ViewBag.UploadPhotoProcut = db.UploadPhotoProducts.ToList();
            ViewBag.currentFilter = SearchString;
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            listreport = listreport.OrderByDescending(n => n.ProductID).ToList();

            return View(listreport.ToPagedList(pageNumber, pageSize));
            //return View(Listrp.ListRp());


        }

        //public ActionResult Detailsinput(string id)
        //{
        //    return View(db.Detailimportcoupons.Where(o => o.ProductID == id).ToList());
        //}
        public ActionResult Detailsinput1(string id)
        {
            var tong = db.Detailimportcoupons.Where(o => o.ProductID == id).Sum(i => i.Quantity);
            ViewBag.tong = tong;
            var model = db.Detailimportcoupons.Where(o => o.ProductID == id).ToList();
            var listdetailinputreport = model.OrderByDescending(n => n.DetailimportID).ToList();
            return PartialView("PartialDetailinput", listdetailinputreport);
        }

        public ActionResult Detailsoutput(string id)
        {
            var tong = db.DetailBills.Where(o => o.ProductID == id).Sum(i => i.Quantity);
            ViewBag.tong = tong;
            var model = db.DetailBills.Where(o => o.ProductID == id).ToList();
            var listdetailoutputreport = model.OrderByDescending(n => n.DetailBillID).ToList();
            return PartialView("PartialDetailouput", listdetailoutputreport);
        }

        //public ActionResult Detailsoutput(string id)
        //{
        //    var model = db.Detailimportcoupons.Where(o => o.ProductID == id).ToList();
        //    return PartialView("PartialDetail", model);
        //}


        public FileResult Export()
        {
            var str = Session["search"];

            QLKEntities entities = new QLKEntities();
            System.Data.DataTable dt = new System.Data.DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[11] {
                                             new DataColumn("Mã mặt hàng"),
                                             new DataColumn("Mã loại hàng"),
                                            new DataColumn("Tên mặt hàng"),
                                             new DataColumn("Tên loại hàng"),
                                            new DataColumn("Đơn vị tính"),
                                            new DataColumn("Tổng nhập"),
                                            new DataColumn("Tổng giá vốn"),
                                             new DataColumn("Tổng xuất"),
                                              new DataColumn("Tổng giá bán"),
                                            new DataColumn("Tồn") ,
                                             new DataColumn("Lãi/lỗ")});

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
            var detailsim = db.Detailimportcoupons.ToList();
            foreach (var iem in detailsim)
            {
                var g = db.Detailimportcoupons.Where(o => o.DetailimportID == iem.DetailimportID).ToList();
                TotalPriceImports.Add(new TotalPriceImport
                {
                    ID = iem.DetailimportID,
                    IDProduct = iem.ProductID,
                    total = iem.Price * (int)iem.Quantity,
                });
            }

            var detailsout = db.DetailBills.ToList();
            foreach (var iem in detailsout)
            {
                var g = db.DetailBills.Where(o => o.DetailBillID == iem.DetailBillID).ToList();
                TotalPricebills.Add(new TotalPricebill
                {
                    ID = iem.DetailBillID,
                    IDProduct = iem.ProductID,
                    total = iem.Price * (int)iem.Quantity,
                });
            }


            List<Report> reports = new List<Report>();

            if(str != null)
            {
                var temp = db.Products.Where(o=>o.ProductName.Contains((string)str) || o.Category.CategoryName.Contains((string)str) || o.Unit.Contains((string)str)).ToList();
                foreach (var ovbj in temp)
                {
                    //lấy product id o detailbill
                    var bill = db.DetailBills.ToList().FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                    //lấy productid o detailimport
                    var import = db.Detailimportcoupons.ToList().FirstOrDefault(o => o.ProductID == ovbj.ProductID);

                    if (import != null)
                    {
                        if (bill == null)
                        {

                            reports.Add(new Report
                            {
                                Photo = ovbj.Photo,
                                ProductID = ovbj.ProductID,
                                CategoryID = (int)ovbj.CategoryID,
                                ProductName = ovbj.ProductName,
                                CategoryName = ovbj.Category.CategoryName,
                                QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity),
                                QuantityOut = 0,
                                total = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity),
                                Unit = ovbj.Unit,
                                TotalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                TotalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                TotalPriceTotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),
                            });
                        }
                        else
                        {
                            reports.Add(new Report
                            {
                                Photo = ovbj.Photo,
                                ProductID = ovbj.ProductID,
                                CategoryID = (int)ovbj.CategoryID,
                                ProductName = ovbj.ProductName,
                                CategoryName = ovbj.Category.CategoryName,
                                QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity),
                                QuantityOut = (int)db.DetailBills.Where(o => o.ProductID == bill.ProductID).Sum(o => o.Quantity),
                                total = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity)
                                - (int)db.DetailBills.Where(o => o.ProductID == bill.ProductID).Sum(o => o.Quantity),
                                Unit = ovbj.Unit,
                                 TotalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                TotalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                TotalPriceTotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),
                            });
                        }
                    }
                }
            }  
            else
            {
                var temp = db.Products.ToList();
                foreach (var ovbj in temp)
                {
                    //lấy product id o detailbill
                    var bill = db.DetailBills.ToList().FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                    //lấy productid o detailimport
                    var import = db.Detailimportcoupons.ToList().FirstOrDefault(o => o.ProductID == ovbj.ProductID); 

                    if (import != null)
                    {
                        if (bill == null)
                        {

                            reports.Add(new Report
                            {
                                Photo = ovbj.Photo,
                                ProductID = ovbj.ProductID,
                                CategoryID = (int)ovbj.CategoryID,
                                ProductName = ovbj.ProductName,
                                CategoryName = ovbj.Category.CategoryName,
                                QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity),
                                QuantityOut = 0,
                                total = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity),
                                Unit = ovbj.Unit,
                                 TotalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                TotalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                TotalPriceTotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),
                            });
                        }
                        else
                        {
                            reports.Add(new Report
                            {
                                Photo = ovbj.Photo,
                                ProductID = ovbj.ProductID,
                                CategoryID = (int)ovbj.CategoryID,
                                ProductName = ovbj.ProductName,
                                CategoryName = ovbj.Category.CategoryName,
                                QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity),
                                QuantityOut = (int)db.DetailBills.Where(o => o.ProductID == bill.ProductID).Sum(o => o.Quantity),
                                total = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity)
                                - (int)db.DetailBills.Where(o => o.ProductID == bill.ProductID).Sum(o => o.Quantity),
                                Unit = ovbj.Unit,
                                 TotalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                TotalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                TotalPriceTotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),
                            });
                        }
                    }
                }
            }    
        
            foreach (var ovjb in reports)
            {
                dt.Rows.Add(ovjb.ProductID, ovjb.CategoryID, ovjb.ProductName, ovjb.CategoryName, ovjb.Unit, ovjb.QuantityIm,ovjb.TotalPricein, ovjb.QuantityOut,ovjb.TotalPriceou,ovjb.total ,ovjb.TotalPriceTotal);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BaoCao.xlsx");
                }
            }
        }
    }
}