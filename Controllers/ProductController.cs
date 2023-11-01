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
        QLBANHANGEntities1 db = new QLBANHANGEntities1();
        // GET: Product
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var dsSP = db.SPs.ToList();
            return View(dsSP.ToPagedList(pageNum, pageSize));
        }

        public ActionResult LayLSP()
        {
            var dsLSP = db.LoaiSPs.ToList();
            return PartialView(dsLSP);
        }

        public ActionResult LayNCC()
        {

            var dsNhaCC = db.NCungCaps.ToList();
            return PartialView(dsNhaCC);
        }
        public ActionResult SPTheoNhaCungCap(int id, int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var dsSachTheoNhaCungCap = db.SPs.Where( sp => sp.MaNCC == id).ToList();
            return View("Index", dsSachTheoNhaCungCap.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SPTheoLSP(int id, int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var dsLoaiSP = db.SPs.Where(sp => sp.MaLoaiSP == id).ToList();
            return View("Index", dsLoaiSP.ToPagedList(pageNum,pageSize));
        }
        public ActionResult Details(int id)
        {
            var sach = db.SPs.FirstOrDefault(s => s.MaSP == id);
            return View(sach);
        }
    }
}