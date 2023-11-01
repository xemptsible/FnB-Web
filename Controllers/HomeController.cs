using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebFnB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Ăn cho tròn! Uống cho đã!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Cần liên lạc với chúng tôi?";

            return View();
        }
    }
}