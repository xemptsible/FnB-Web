using WebFnB.Models;
using PagedList;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBookStore.Controllers
{
    public class AdminController : Controller
    {
        // Use DbContext to manage database
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
                if (string.IsNullOrEmpty(admin.TenDN))
                    ModelState.AddModelError(string.Empty, "User name không được để trống");
                if (string.IsNullOrEmpty(admin.MatKhau))
                    ModelState.AddModelError(string.Empty, "Password không được để trống");
                //Kiểm tra có admin này hay chưa
                var adminDB = database.Admins.FirstOrDefault(ad => ad.TenDN ==
                admin.TenDN && ad.MatKhau == admin.MatKhau);
                if (adminDB == null)
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật không đúng");
                else
                {
                    Session["Admin"] = adminDB;

                    ViewBag.ThongBao = "Đăng nhập admin thành công";
                    return RedirectToAction("Index", "Admin");
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
            return View(dsSanPham.OrderBy(sp => sp.MaSP).ToPagedList(pageNum, pageSize));
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

        public ActionResult NCC(int? page)
        {
            var dsNCC = database.NCungCaps.ToList();
            //Tạo biến cho biết số sách mỗi trang
            int pageSize = 5;
            //Tạo biến số trang
            int pageNum = (page ?? 1);
            return View(dsNCC.OrderBy(ncc => ncc.MaNCC).ToPagedList(pageNum, pageSize));
        }

        public ActionResult LoaiSP(int? page)
        {
            var dsLoaiSP = database.LoaiSPs.ToList();
            //Tạo biến cho biết số sách mỗi trang
            int pageSize = 5;
            //Tạo biến số trang
            int pageNum = (page ?? 1);
            return View(dsLoaiSP.OrderBy(lsp => lsp.MaLoaiSP).ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        public ActionResult EditSanPham(int id)
        {
            // Lấy thông tin sản phẩm cần chỉnh sửa từ cơ sở dữ liệu
            var sanPham = database.SPs.FirstOrDefault(sp => sp.MaSP == id);

            if (sanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Lấy danh sách các loại sản phẩm và nhà cung cấp
            ViewBag.MaLoaiSP = new SelectList(database.LoaiSPs.ToList(), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(database.NCungCaps.ToList(), "MaNCC", "TenNCC");

            return View(sanPham);
        }

        [HttpPost]
        public ActionResult EditSanPham(SP sanPham)
        {
            if (ModelState.IsValid)
            {
                // Lấy sản phẩm từ cơ sở dữ liệu
                var sanPhamDB = database.SPs.FirstOrDefault(sp => sp.MaSP == sanPham.MaSP);

                if (sanPhamDB != null)
                {
                    // Cập nhật thông tin sản phẩm từ form chỉnh sửa
                    sanPhamDB.TenSP = sanPham.TenSP;
                    sanPhamDB.MoTa = sanPham.MoTa;
                    sanPhamDB.GiaBan = sanPham.GiaBan;
                    sanPhamDB.GiaNhap = sanPham.GiaNhap;
                    sanPhamDB.Anh = sanPham.Anh;
                    sanPhamDB.MaLoaiSP = sanPham.MaLoaiSP;
                    sanPhamDB.MaNCC = sanPham.MaNCC;
                    sanPhamDB.SoLuongTon = sanPham.SoLuongTon;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    database.SaveChanges();

                    ViewBag.ThongBao = "Chỉnh sửa sản phẩm thành công";
                    return RedirectToAction("SanPham");
                }
                else
                {
                    ViewBag.ThongBao = "Sản phẩm không tồn tại";
                }
            }

            return View(sanPham);
        }
        public ActionResult Delete(int id)
        {
            var sanPham = database.SPs.FirstOrDefault(sp => sp.MaSP == id);

            if (sanPham != null)
            {
                database.SPs.Remove(sanPham);
                database.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            }

            return RedirectToAction("SanPham"); // Chuyển hướng người dùng sau khi xóa (không cần trang xác nhận)
        }

    }
}