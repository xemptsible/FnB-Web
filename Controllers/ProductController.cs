using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebFnB.Models;

namespace WebFnB.Controllers
{
    public class ProductController : Controller
    {
        QLBANHANGEntities db = new QLBANHANGEntities();
        // GET: Product
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            var dsSP = db.SPs.ToList();

            return View(dsSP.ToPagedList(pageNum, pageSize));
        }

        public ActionResult SPTheoNhaCungCap(int id)
        {
            var dsSachTheoNhaCungCap = db.SPs.Where( sp => sp.MaNCC == id).ToList();
            return View("Index", dsSachTheoNhaCungCap);
        }
        public ActionResult SPTheoLSP(int id)
        {
            var dsSachNXB = db.SPs.Where(sp => sp.MaLoaiSP == id).ToList();
            return View("Index", dsSachNXB);
        }
        public ActionResult Details(int id)
        {
            var sach = db.SPs.FirstOrDefault(s => s.MaSP == id);
            return View(sach);
        }
    }
}