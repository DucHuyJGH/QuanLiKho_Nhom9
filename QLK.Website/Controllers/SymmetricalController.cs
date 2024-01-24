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
    public class SymmetricalController : Controller
    {
        QLKEntities db = new QLKEntities();
        Listsymmetrical listsym = new Listsymmetrical();
        List<TotalPriceImport> TotalPriceImports = new List<TotalPriceImport>();
        List<TotalPricebill> TotalPricebills = new List<TotalPricebill>();

        public ActionResult Index( string SearchString, string currentFilter, int ? page)
        {
           
            if (Request.Cookies["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.kt = true;
            ViewBag.messe = "Mặt hàng sắp hết";
            Session["search"] = SearchString;
            if (db.DetailBills.Sum(o => o.Quantity) != null && db.Detailimportcoupons.Sum(p => p.Quantity) != null)
            {
                
                int detailquantity = (int)db.DetailBills.Sum(o => o.Quantity);
                int importquantity = (int)db.Detailimportcoupons.Sum(p => p.Quantity);
                int tong = importquantity - detailquantity;
                ViewBag.tong = tong;
                ViewBag.detailquantity = detailquantity;
                ViewBag.importquantity = importquantity;

            

            }
            else { 
                  if(db.DetailBills.Sum(o => o.Quantity) == null && db.Detailimportcoupons.Sum(p => p.Quantity) != null)
                     {
                    int detailquantity = 0;
                    int importquantity = (int)db.Detailimportcoupons.Sum(p => p.Quantity);
                    int tong = importquantity - detailquantity;
                    ViewBag.tong = tong;
                    ViewBag.detailquantity = detailquantity;
                    ViewBag.importquantity = importquantity;
                     }
                else
                     {
                
                     if (db.Detailimportcoupons.Sum(p => p.Quantity) == null)
                          {
                            int detailquantity = 0;
                            int importquantity = 0;
                            int tong = importquantity - detailquantity;
                            ViewBag.tong = tong;
                            ViewBag.detailquantity = detailquantity;
                            ViewBag.importquantity = importquantity;
                            }
                    else
                    {

                        int detailquantity = 0;
                        int importquantity = 0;
                        int tong = 0;
                        ViewBag.tong = tong;
                        ViewBag.detailquantity = detailquantity;
                        ViewBag.importquantity = importquantity;
                    } 
                }
  
            }

            var listSym = new List<symmetrical>();

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
                listSym = listsym.Searchsym(SearchString).ToList();
            }
            else
            {
                //listreport = db.Reports.ToList();
                listSym = listsym.ListBy().ToList(); 
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

            int TatolPricein = TotalPriceImports.Sum(o => o.total);
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

            int TatolPriceout = TotalPricebills.Sum(o => o.total);

            int TatolPricetotal = -(TotalPriceImports.Sum(o => o.total) - TotalPricebills.Sum(o => o.total)) ;

            ViewBag.TatolPricein = TatolPricein;
            ViewBag.TatolPriceout = TatolPriceout;
            ViewBag.TatolPricetotal = TatolPricetotal;

            ViewBag.currentFilter = SearchString;
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            listSym = listSym.OrderByDescending(n => n.ProductID).ToList();

            return View(listSym.ToPagedList(pageNumber, pageSize));

            //return View(listsym.ListBy());


        }

        public ActionResult Details1(string id)
        {
            var tong = db.Detailimportcoupons.Where(o => o.ProductID == id).Sum(i => i.Quantity);
            ViewBag.tong = tong;
            var model = db.Detailimportcoupons.Where(o => o.ProductID == id).ToList();
            var ListPartialDetail1 = model.OrderByDescending(n => n.DetailimportID).ToList();
            return PartialView("PartialDetail1", ListPartialDetail1);
        }
        public ActionResult Details2(string id)
        {
            var tong = db.DetailBills.Where(o => o.ProductID == id).Sum(i => i.Quantity);
            ViewBag.tong = tong;
            //ViewBag.impot = db.Detailimportcoupons.ToList();
            var model = db.DetailBills.Where(o => o.ProductID == id).ToList();
            var ListPartialDetail2 = model.OrderByDescending(n => n.DetailBillID).ToList();
            return PartialView("PartialDetail2", ListPartialDetail2);
        }

        public FileResult Export()
        {
            QLKEntities entities = new QLKEntities();
            var str = Session["search"];
            System.Data.DataTable dt = new System.Data.DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[9] {
                                             new DataColumn("Mã mặt hàng"),
                                            new DataColumn("Tên mặt hàng"),
                                            new DataColumn("Đơn vị tính"),
                                             new DataColumn("Tổng nhập"),
                                               new DataColumn("Tổng giá vốn"),
                                            new DataColumn("Tổng xuất"),
                                            new DataColumn("Tổng giá bán"),
                                             new DataColumn("Tồn kho"),
                                            new DataColumn("Lãi/Lỗ") });

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

            List<symmetrical> symmetricals = new List<symmetrical>();
            if( str != null)
            {
                var temp = db.Products.Where(o=>o.ProductName.Contains((string) str) || o.Unit.Contains((string) str));
                foreach (var ovbj in temp)
                {
                    //lấy product id o detailbill
                    var bill = db.DetailBills.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                    //lấy productid o detailimport
                    var import = db.Detailimportcoupons.FirstOrDefault(o => o.ProductID == ovbj.ProductID);

                    if (import != null)
                    {
                        if (bill == null)
                        {
                            int r = 0;
                            symmetricals.Add(new symmetrical
                            {
                                ProductID = ovbj.ProductID,
                                ProductName = ovbj.ProductName,
                                QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity),
                                QuantityOut = r,
                                total = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity) - r,
                                Unit = ovbj.Unit,
                                totalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                totalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                totalPricetotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),
                            });
                        }
                        else
                        {

                            symmetricals.Add(new symmetrical
                            {

                                ProductID = ovbj.ProductID,
                                ProductName = ovbj.ProductName,
                                QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity),// (int)import.Quantity,
                                QuantityOut = (int)db.DetailBills.Where(o => o.ProductID == bill.ProductID).Sum(o => o.Quantity),
                                total = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity)
                                - (int)db.DetailBills.Where(o => o.ProductID == bill.ProductID).Sum(o => o.Quantity),
                                Unit = ovbj.Unit,
                                totalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                totalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                totalPricetotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),

                            });
                        }
                    }

                }
            } 
            else
            {
                var temp = db.Products;
                foreach (var ovbj in temp)
                {
                    //lấy product id o detailbill
                    var bill = db.DetailBills.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                    //lấy productid o detailimport
                    var import = db.Detailimportcoupons.FirstOrDefault(o => o.ProductID == ovbj.ProductID);

                    if (import != null)
                    {
                        if (bill == null)
                        {
                            int r = 0;
                            symmetricals.Add(new symmetrical
                            {
                                ProductID = ovbj.ProductID,
                                ProductName = ovbj.ProductName,
                                QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity),
                                QuantityOut = r,
                                total = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity) - r,
                                Unit = ovbj.Unit,
                                totalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                totalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                totalPricetotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),
                            });
                        }
                        else
                        {

                            symmetricals.Add(new symmetrical
                            {

                                ProductID = ovbj.ProductID,
                                ProductName = ovbj.ProductName,
                                QuantityIm = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity),// (int)import.Quantity,
                                QuantityOut = (int)db.DetailBills.Where(o => o.ProductID == bill.ProductID).Sum(o => o.Quantity),
                                total = (int)db.Detailimportcoupons.Where(o => o.ProductID == import.ProductID).Sum(o => o.Quantity)
                                - (int)db.DetailBills.Where(o => o.ProductID == bill.ProductID).Sum(o => o.Quantity),
                                Unit = ovbj.Unit,
                                totalPricein = TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                totalPriceou = TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total),
                                totalPricetotal = -(TotalPriceImports.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total) - TotalPricebills.Where(o => o.IDProduct == ovbj.ProductID).Sum(o => o.total)),

                            });
                        }
                    }

                }
            }    
            
            foreach (var ovjb in symmetricals)
            {
                dt.Rows.Add( ovjb.ProductID, ovjb.ProductName, ovjb.Unit,ovjb.QuantityIm,ovjb.totalPricein, ovjb.QuantityOut,ovjb.totalPriceou, ovjb.total, ovjb.totalPricetotal);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CanDoiKho.xlsx");
                }
            }
        }
    }
}