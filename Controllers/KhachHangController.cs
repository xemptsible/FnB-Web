using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFnB.Models;
using PagedList;

namespace WebFnB.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        QLBANHANGEntities database = new QLBANHANGEntities();
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login", "Admin");

            return View();
        }
        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            // Sử dụng lại hàm Login từ AdminController
            return RedirectToAction("Login", "Admin", new { admin.TenDN, admin.MatKhau });
        }
        public ActionResult DanhSachKhachHang(int? page)
        {
            var dsKhachHang = database.KHs.ToList();

            int pageSize = 10;
            int pageNum = (page ?? 1);

            return View(dsKhachHang.ToPagedList(pageNum, pageSize));
        }
        public ActionResult EditKhachHang(int id)
        {
            var khachHang = database.KHs.FirstOrDefault(kh => kh.MaKH == id);

            if (khachHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(khachHang);
        }

        [HttpPost]
        public ActionResult EditKhachHang(KH khachHang)
        {
            if (ModelState.IsValid)
            {
                var khachHangDB = database.KHs.FirstOrDefault(kh => kh.MaKH == khachHang.MaKH);

                if (khachHangDB != null)
                {
                    // Cập nhật thông tin khách hàng từ form chỉnh sửa
                    khachHangDB.TaiKhoan = khachHang.TaiKhoan;
                    khachHangDB.MatKhau = khachHang.MatKhau;
                    khachHangDB.TenKH = khachHang.TenKH;
                    khachHangDB.DiaChi = khachHang.DiaChi;
                    khachHangDB.Email = khachHang.Email;
                    khachHangDB.SDT = khachHang.SDT;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    database.SaveChanges();

                    ViewBag.ThongBao = "Chỉnh sửa thông tin khách hàng thành công";
                    return RedirectToAction("DanhSachKhachHang");
                }
                else
                {
                    ViewBag.ThongBao = "Khách hàng không tồn tại";
                }
            }

            return View(khachHang);
        }
        public ActionResult DeleteKhachHang(int id)
        {
            var khachHang = database.KHs.FirstOrDefault(kh => kh.MaKH == id);

            if (khachHang != null)
            {
                return View(khachHang);
            }
            else
            {
                ViewBag.ThongBao = "Khách hàng không tồn tại";
                return RedirectToAction("DanhSachKhachHang");
            }
        }
        [HttpPost, ActionName("DeleteKhachHang")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDeleteKhachHang(int id)
        {
            var khachHang = database.KHs.FirstOrDefault(kh => kh.MaKH == id);

            if (khachHang != null)
            {
                // Xóa khách hàng từ cơ sở dữ liệu
                database.KHs.Remove(khachHang);
                database.SaveChanges();

                ViewBag.ThongBao = "Xóa khách hàng thành công";
                return RedirectToAction("DanhSachKhachHang");
            }
            else
            {
                ViewBag.ThongBao = "Khách hàng không tồn tại";
                return RedirectToAction("DanhSachKhachHang");
            }
        }
    }
}