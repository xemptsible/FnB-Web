﻿using System;
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
        // GET: Product
        public ActionResult Index()
        {
            return View();
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