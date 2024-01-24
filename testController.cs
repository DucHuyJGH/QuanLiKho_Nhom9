using QLK.Website.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace QLK.Website.Controllers
{
    public class testController : Controller
    {
        QLKEntities db = new QLKEntities();
        // GET: test
        public ActionResult test()
        {
            return View();
        }
        public ActionResult Index(string currentFilter, string SearchString, int? page)

        {
            if (Request.Cookies["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

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
        public PartialViewResult getpaging(int ? page)
        {
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            var list = db.Categories.ToList();
            return PartialView("_PartialView", list.ToPagedList(pageNumber, pageSize));
        }
    }
}