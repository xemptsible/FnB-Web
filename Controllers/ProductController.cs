using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFnB.Models;

namespace WebFnB.Controllers
{
    public class ProductController : Controller
    {
        QLBANHANGEntities db = new QLBANHANGEntities();
        private List<SanPham> LaySP(int soluong)
        {
            return db.SanPhams.OrderByDescending(sp => sp.TenSP).Take(soluong).ToList();
        }
        // GET: Product
        public ActionResult Index()
        {
            var dsSP = LaySP(10);
            return View(dsSP);
        }

        public ActionResult SPTheoNhaCungCap(int id)
        {
            var dsSachTheoNhaCungCap = db.SanPhams.Where( sp => sp.MaNCC == id).ToList();
            return View("Index", dsSachTheoNhaCungCap);
        }
        public ActionResult SPTheoLSP(int id)
        {
            var dsSachNXB = db.SanPhams.Where(sp => sp.MaLoaiSP == id).ToList();
            return View("Index", dsSachNXB);
        }
        public ActionResult Details(int id)
        {
            var sach = db.SanPhams.FirstOrDefault(s => s.MaSP == id);
            return View(sach);
        }
    }
}