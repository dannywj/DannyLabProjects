using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MemcachedWeb.Models;

namespace MemcachedWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            //return View();
            string name = "danny";
            var cacheData = MemcachedWeb.Models.MemcachedHelper.GetCache("name");
            if (cacheData == null)
            {
                MemcachedWeb.Models.MemcachedHelper.SetCache("name", name, DateTime.Now.AddDays(1));
                Response.Write("set cache ok");
            }
            else
            {
                Response.Write("get data from cache:" + cacheData.ToString());
            }
            return null;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
