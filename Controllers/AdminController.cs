using WebFnB.Models;
using PagedList;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebFnB.Controllers
{
    public class AdminController : Controller
    {
        QLBANHANGEntities database = new QLBANHANGEntities();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login");
            return View();
        }
        [HttpGet]

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên đăng nhập và mật khẩu có được cung cấp không
                if (string.IsNullOrEmpty(admin.TenDN) || string.IsNullOrEmpty(admin.MatKhau))
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập và mật khẩu không được để trống");
                    return View();
                }

                using (var db = new QLBANHANGEntities()) // Sử dụng using để đảm bảo giải phóng tài nguyên
                {
                    // Tìm admin trong cơ sở dữ liệu
                    var adminDB = db.Admins.FirstOrDefault(a => a.TenDN == admin.TenDN && a.MatKhau == admin.MatKhau);
                    if (adminDB != null)
                    {
                        // Đăng nhập thành công, lưu thông tin admin vào Session
                        Session["Admin"] = adminDB;
                        return RedirectToAction("Index", "Admin"); // Điều hướng đến trang quản trị
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng");
                    }
                }
            }
            return View();
        }
        public ActionResult SanPham(int? page)
        {
            var dsSanPham = database.SPs.ToList();
            //Tạo biến cho biết số sách mỗi trang
            int pageSize = 5;
            //Tạo biến số trang
            int pageNum = (page ?? 1);
            return View(dsSanPham.OrderBy(sp => sp.MaSP).ToPagedList(pageNum,
            pageSize));
        }

        [HttpGet]
        public ActionResult ThemSanPham()
        {
            ViewBag.MaLoaiSP = new SelectList(database.LoaiSPs.ToList(), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(database.LoaiSPs.ToList(), "MaNCC", "TenNCC");
            return View();
        }

        [HttpPost]
        public ActionResult ThemSanPham(SP SanPham, HttpPostedFileBase Anh)
        {
            ViewBag.MaLoaiSP = new SelectList(database.LoaiSPs.ToList(), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(database.LoaiSPs.ToList(), "MaNCC", "TenNCC");
            if (Anh == null)
                return View();
            else
                if (ModelState.IsValid)
            {
                var filename = Path.GetFileName(Anh.FileName);

                var path = Path.Combine(Server.MapPath("~/Images"), filename);

                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hinh da ton tai";
                }
                else
                {
                    Anh.SaveAs(path);
                }
            }
            return RedirectToAction("SanPham");
        }
        public ActionResult ChiTietSP(int id)
        {
            var sanPham = database.SPs.FirstOrDefault(s => s.MaSP == id);

            if (sanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanPham);
        }
    }
}