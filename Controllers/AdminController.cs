using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFnB.Models;
using PagedList;

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
            var SP = database.SPs.ToList();
            int pageSize = 7;
            int pageNum = (page ?? 1);
            return View(SP.OrderBy(sp => sp.MaSP).ToPagedList(pageNum,pageSize));
        }
    }
}