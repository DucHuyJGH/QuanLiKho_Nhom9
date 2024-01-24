using QLK.Website.Helpers;
using QLK.Website.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace QLK.Website.Controllers
{
    public class AccountController : Controller
    {
        string tr = "Lỗi! Sai tên hoặc mật khẩu!";
        QLKEntities db = new QLKEntities();
        public ActionResult Login ()
        {
           
            return View();
        }

     
        public static string GetMD5(string str)
        {
           
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] fromData = Encoding.UTF8.GetBytes(str);
                byte[] targetData = md5.ComputeHash(fromData);
                string byte2String = null;

                for (int i = 0; i < targetData.Length; i++)
                {
                    byte2String += targetData[i].ToString("x2");

                }
                return byte2String;
            
            
        }
        public ActionResult register()
        {
            return View();
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult register(SystemDatabase systemDatabase)
        {
            //var checkid = db.SystemDatabases.FirstOrDefault(o => o.Id == systemDatabase.Id);
            var check = db.SystemDatabases.FirstOrDefault(o => o.Username == systemDatabase.Username);
            if(systemDatabase.Password != null)
            {
                if (check == null)
                {
                    systemDatabase.Password = GetMD5(systemDatabase.Password);
                    db.SystemDatabases.Add(systemDatabase);
                    db.SaveChanges();
                    ViewBag.confirn = "Đăng ký thành công!!";
                }
                else
                {
                    ViewBag.error = "Tên đăng nhập đã tồn tại!";
                }
               
            }
            else
            {
                ViewBag.error = "Đăng ký thất bại!";
                return View();
            }
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Login(string Username, string Password)
        {
          
            if (ModelState.IsValid)
            {

                var f_password = GetMD5(Password);
                var data = db.SystemDatabases.Where(s => s.Username.Equals(Username) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    
                    HttpCookie ck = new HttpCookie("Username");
                    ck.Value = Username;
                    ck.Expires = DateTime.Now.AddDays(5);
                    Response.Cookies.Add(ck);
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ViewBag.error = tr;
                    return View();
                }
               
            }
            return View();
        }
        public ActionResult ChangePass(string UserName,string Password, string newPassword, string newPasswordagain)
        {

            
            var data = db.SystemDatabases.FirstOrDefault(s => s.Username.Equals(UserName));
                if (data != null)
                {
                    var f_password = GetMD5(Password);
                    if (f_password == data.Password && newPassword == newPasswordagain) 
                    {
                        data.Password = GetMD5(newPassword);
                        db.SaveChanges();
                        ViewBag.messenger = "Đổi mật khẩu thành công!";
                        if(Request.Cookies["Username"] != null)
                        {
                        return RedirectToAction("Index", "Home");
                        }    
                    }
                    else
                    {
                        ViewBag.messenger = "Đổi mật khẩu thất bại!";
                    }
                   
            }
               
            return View();
        
     }

        public ActionResult Logout()
        {
            if (Request.Cookies["Username"] != null)
            {
                Response.Cookies["Username"].Expires = DateTime.Now.AddDays(-1);
            }
          
                System.Web.Security.FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Account");
        }
    }
}