using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QLK.Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "Category",
              url: "Danh-sach-loai-san-pham/{action}/{id}",
              defaults: new
              {
                  controller = "Category",
                  action = "Index",
                  id = UrlParameter.Optional
              }
          );
            routes.MapRoute(
             name: "register",
             url: "Dang-ky/{id}",
             defaults: new
             {
                 controller = "Account",
                 action = "register",
                 id = UrlParameter.Optional
             }
         );
            routes.MapRoute(
            name: "ChangePass",
            url: "Doi-mat-khau/{id}",
            defaults: new
            {
                controller = "Account",
                action = "ChangePass",
                id = UrlParameter.Optional
            }
        );
            routes.MapRoute(
            name: "history",
            url: "Lich-su-bien-dong/{id}",
            defaults: new
            {
                controller = "Home",
                action = "history",
                id = UrlParameter.Optional
            }
        );
            routes.MapRoute(
             name: "Login",
             url: "Dang-nhap/{id}",
             defaults: new
             {
                 controller = "Account",
                 action = "Login",
                 id = UrlParameter.Optional
             }
         );

            routes.MapRoute(
             name: "Product",
             url: "Danh-sach-san-pham/{action}/{id}",
             defaults: new
             {
                 controller = "Product",
                 action = "Index",
                 id = UrlParameter.Optional
             }
         );

            routes.MapRoute(
             name: "DetailInput",
             url: "Danh-sach-chi-tiet-phieu-nhap/{action}/{id}",
             defaults: new
             {
                 controller = "DetailInput",
                 action = "Index",
                 id = UrlParameter.Optional
             }
         );
            routes.MapRoute(
             name: "DetailOutput",
             url: "Danh-sach-chi-tiet-phieu-xuat/{action}/{id}",
             defaults: new
             {
                 controller = "DetailOutput",
                 action = "Index",
                 id = UrlParameter.Optional
             }
         );

            routes.MapRoute(
            name: "Home",
            url: "Trang-chu/{action}/{id}",
            defaults: new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            }
        );
            routes.MapRoute(
             name: "Input",
             url: "Danh-sach-phieu-nhap/{action}/{id}",
             defaults: new
             {
                 controller = "Input",
                 action = "Index",
                 id = UrlParameter.Optional
             }
         );
            routes.MapRoute(
             name: "Output",
             url: "Danh-sach-phieu-xuat/{action}/{id}",
             defaults: new
             {
                 controller = "Output",
                 action = "Index",
                 id = UrlParameter.Optional
             }
         );
            routes.MapRoute(
             name: "Report",
             url: "Bao-cao-kho/{action}/{id}",
             defaults: new
             {
                 controller = "Report",
                 action = "Index",
                 id = UrlParameter.Optional
             }
         );
            routes.MapRoute(
             name: "ExInput",
             url: "Bao-cao-nhap-kho/{action}/{id}",
             defaults: new
             {
                 controller = "ExInput",
                 action = "Index",
                 id = UrlParameter.Optional
             }
         );
            routes.MapRoute(
            name: "ExOutput",
            url: "Bao-cao-xuat-kho/{action}/{id}",
            defaults: new
            {
                controller = "ExOutput",
                action = "Index",
                id = UrlParameter.Optional
            }
        );
            routes.MapRoute(
           name: "Supplier",
           url: "Danh-sach-nha-cung-cap/{action}/{id}",
           defaults: new
           {
               controller = "Supplier",
               action = "Index",
               id = UrlParameter.Optional
           }
       );
            routes.MapRoute(
           name: "Symmetrical",
           url: "Can-doi-kho/{action}/{id}",
           defaults: new
           {
               controller = "Symmetrical",
               action = "Index",
               id = UrlParameter.Optional
           }
       );
            routes.MapRoute(
          name: "Warehouse",
          
          url: "Danh-sach-kho/{action}/{id}",
          defaults: new
          {
              controller = "Warehouse",
              action = "Index",
              id = UrlParameter.Optional
          }
      );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }

                );

           
        }
    }
}
