using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFnB.Models;

namespace WebFnB.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        QLBANHANGEntities db = new QLBANHANGEntities();
        public ActionResult KQTimKiem(string tensp)
        {
            var lstSP = db.SPs.Where(sp => sp.TenSP.Contains(tensp));
            return View(lstSP.OrderBy(s=>s.TenSP));
        }
    }
}