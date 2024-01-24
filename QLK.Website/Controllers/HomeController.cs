using QLK.Website.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace QLK.Website.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        QLKEntities db = new QLKEntities();
        List<TotalPriceImport> TotalPriceImports = new List<TotalPriceImport>();
        List<TotalPricebill> TotalPricebills = new List<TotalPricebill>();
        public ActionResult Index()
        {
          
            if (Request.Cookies["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var countdetailinput = db.Detailimportcoupons.Count();
            ViewBag.countdetailinput = countdetailinput;

            var countdetailOutput = db.DetailBills.Count();
            ViewBag.countdetailOutput = countdetailOutput;

            var warehouse = db.Warehouses.Count();
            ViewBag.Warehouse = warehouse;

            var supplier = db.Suppliers.Count();
            ViewBag.supplier = supplier;

            var sproduct = db.Products.Count();
            ViewBag.sproduct = sproduct;

            var category = db.Categories.Count();
            ViewBag.category = category;
            return View();
        }

        public ActionResult chart (DateTime ? datein , DateTime ?  dateout)
        {
            return View();
        }

        public ActionResult history(DateTime ? datein, DateTime? dateout, DateTime? datein1, DateTime? dateout1)
        {
            DateTime now = DateTime.Now - new TimeSpan(31, 0, 0, 0);
            DateTime dti = datein ?? now;
            DateTime dto = dateout ?? DateTime.Now;
            ViewBag.datenow = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.now = now;
                List<int> count = new List<int>();
                List<DateTime> dttimes = new List<DateTime>();
                for (int i = 1; i <= 31 ; i++)
                {
                    DateTime dt = dti + new TimeSpan(i, 0, 0, 0);
                    dttimes.Add(dt);
                }
                var datetimein = dttimes.Where(o=>o.Date >= dti && o.Date <= dto) .ToList();
                foreach (var item in datetimein)
                {
                    var kt = db.Importcoupons.Where(o => o.Date == item.Date);
                    if (kt.Count() > 0)
                    {

                        var sum = db.Detailimportcoupons.Where(o => o.Importcoupon.Date == item.Date).Sum(o => o.Quantity);
                    if(sum != null)
                    {
                        count.Add((int)sum);
                    }    
                    else
                    {
                        sum = 0;
                        count.Add((int)sum);
                    }    
                        
                    }
                    else
                    {
                        var sum = 0;
                        count.Add(sum);

                    }
                } 
                var rep = count;
                ViewBag.datetime = Newtonsoft.Json.JsonConvert.SerializeObject(datetimein.ToList().Select(o=>o.Date.ToString("dd/MM")));
                ViewBag.rep = count.ToList();

                List<int> count1 = new List<int>();
                //var datetimeout = db.Bills.Where(o => o.Date >= dti && o.Date <= dto).Select(o => o.Date).Distinct();
                foreach (var item in datetimein)
                {
                var kt = db.Bills.Where(o => o.Date == item.Date);
                if (kt.Count() > 0)
                {
                    var sum1 = db.DetailBills.Where(o => o.Bill.Date == item.Date).Sum(o => o.Quantity);
                    if(sum1 != null)
                    {
                        count1.Add((int)sum1);
                    }   
                    else
                    {
                        sum1 = 0;
                        count1.Add((int)sum1);
                    }    
                    
                }
                else
                {
                    var sum1 = 0;
                    count1.Add(sum1);

                }
                }
                var rep1 = count1;
                //ViewBag.datetime1 = Newtonsoft.Json.JsonConvert.SerializeObject(datetimein.ToList().Select(o => o.Date.ToString("dd/MM")));
                ViewBag.rep1 = count1.ToList();
            
            DateTime dti1 = datein1 ?? now;
            DateTime dto1 = dateout1 ?? DateTime.Now;
            List<ExInput> ExInputs = new List<ExInput>();
            var temp = db.Detailimportcoupons.Where(o => o.Importcoupon.Date >= dti1 && o.Importcoupon.Date <= dto1).ToList(); 
            foreach (var ovbj in temp)
            {

                var Pr = db.Products.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                var sup = db.Suppliers.FirstOrDefault(o => o.SupplierID == Pr.SupplierID);
                var im = db.Importcoupons.FirstOrDefault(o => o.ImportID == ovbj.ImportID);
                var cate = db.Categories.FirstOrDefault(o => o.CategoryID == Pr.CategoryID);
                ExInputs.Add(new ExInput
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
                    Tolalpricein = ovbj.Price * (int)ovbj.Quantity,


                });
            }
          
            List<int> count2 = new List<int>();
            foreach (var item in datetimein)
            {
                var kt = ExInputs.Where(o => o.Date == item.Date);
                if (kt.Count() > 0)
                {
                    var sum1 = ExInputs.Where(o => o.Date == item.Date).Sum(o => o.Price);
                    count2.Add(sum1);
                }
                else
                {
                    var sum1 = 0;
                    count2.Add(sum1);

                }
                
            }
            ViewBag.r2 = count2.ToList();
            List<ExOutput> ExOutputs = new List<ExOutput>();
                var temp1 = db.DetailBills.Where(o => o.Bill.Date >= dti && o.Bill.Date <= dto);
                foreach (var ovbj in temp1)
                {

                    var Pr = db.Products.FirstOrDefault(o => o.ProductID == ovbj.ProductID);
                    var sup = db.Suppliers.FirstOrDefault(o => o.SupplierID == Pr.SupplierID);
                    var bi = db.Bills.FirstOrDefault(o => o.BillID == ovbj.BillID);
                    var cate = db.Categories.FirstOrDefault(o => o.CategoryID == Pr.CategoryID);
                    ExOutputs.Add(new ExOutput
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
                        Quantityout = (int)ovbj.Quantity,
                        Price = ovbj.Price,
                        Tolalpriceou = ovbj.Price * (int)ovbj.Quantity
                    });
                }

            List<int> count3 = new List<int>();
            foreach (var item in datetimein)
            {
                var kt = ExOutputs.Where(o => o.Date == item.Date);
                if (kt.Count() > 0)
                {
                    var sum3 = ExOutputs.Where(o => o.Date == item.Date).Sum(o => o.Price);
                    count3.Add(sum3);
                }
                else
                {
                    var sum3 = 0;
                    count3.Add(sum3);

                }

            }
            ViewBag.r3 = count3.ToList();


            List<int> count4 = new List<int>();
            foreach (var item in datetimein)
            {
                var kt = ExInputs.Where(o => o.Date == item.Date);
                var kt1 = ExOutputs.Where(o => o.Date == item.Date);
                if (kt == null && kt1 == null)
                {
                    var sum4 = 0;
                    count4.Add(sum4);
                } 
                else
                {
                    var sum4 = - (ExInputs.Where(o => o.Date == item.Date).Sum(o => o.Price) - ExOutputs.Where(o => o.Date == item.Date).Sum(o => o.Price));
                    count4.Add(sum4);
                    
                }

            }
            ViewBag.r4 = count4.ToList();
            return View();
        }

      
    }
}