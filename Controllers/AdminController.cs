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
        public ActionResult ChiTietSanPham(int id)
        {
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
        public ActionResult ChiTietNCC(int id)
        {
            var dsNCC = database.NCungCaps.FirstOrDefault(NCC => NCC.MaNCC == id);

            if (dsNCC == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Lấy danh sách các loại sản phẩm và nhà cung cấp
            ViewBag.MaNCC = new SelectList(database.NCungCaps.ToList(), "MaNCC", "TenNCC");

            return View(dsNCC);
        }
        public ActionResult EditNCC(int id)
        {
            // Lấy thông tin sản phẩm cần chỉnh sửa từ cơ sở dữ liệu
            var dsNCC = database.NCungCaps.FirstOrDefault(NCC => NCC.MaNCC == id);

            if (dsNCC == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Lấy danh sách các loại sản phẩm và nhà cung cấp
            ViewBag.MaNCC = new SelectList(database.NCungCaps.ToList(), "MaNCC", "TenNCC");

            return View(dsNCC);
        }

        [HttpPost]
        public ActionResult EditNCC(NCungCap nCungCap)
        {
            if (ModelState.IsValid)
            {
                // Lấy sản phẩm từ cơ sở dữ liệu
                var nCungCapDB = database.NCungCaps.FirstOrDefault(NCC => NCC.MaNCC == nCungCap.MaNCC);

                if (nCungCapDB != null)
                {
                    // Cập nhật thông tin sản phẩm từ form chỉnh sửa
                    nCungCapDB.TenNCC = nCungCap.TenNCC;
                    nCungCapDB.DiaChi = nCungCap.DiaChi;
                    nCungCapDB.SDT = nCungCap.SDT;


                    // Lưu thay đổi vào cơ sở dữ liệu
                    database.SaveChanges();

                    ViewBag.ThongBao = "Chỉnh sửa nhà cung cấp thành công";
                    return RedirectToAction("NCungCap");
                }
                else
                {
                    ViewBag.ThongBao = "Nhà cung cấp không tồn tại";
                }
            }

            return View(nCungCap);

        }
        public ActionResult DeleteNCC(int id)
        {
            var dsNCC = database.NCungCaps.FirstOrDefault(NCC => NCC.MaNCC == id);

            if (dsNCC != null)
            {
                database.NCungCaps.Remove(dsNCC);
                database.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            }

            return RedirectToAction("NCungCap"); // Chuyển hướng người dùng sau khi xóa (không cần trang xác nhận)
        }
        public ActionResult ThemLoaiSP()
        {
            ViewBag.MaLoaiSP = new SelectList(database.LoaiSPs.ToList(), "MaLoaiSP", "TenLoaiSP");
            return View();
        }
        [HttpPost]
        public ActionResult ThemLoaiSP(LoaiSP loaiSP)
        {
            ViewBag.MaLoaiSP = new SelectList(database.LoaiSPs.ToList(), "MaLoaiSP", "TenLoaiSP");
            return View(loaiSP);
        }
        public ActionResult ThemNCC()
        {
            ViewBag.MaNCC = new SelectList(database.LoaiSPs.ToList(), "MaNCC", "TenNCC");
            return View();
        }

        [HttpPost]
        public ActionResult ThemNCC(NCungCap nCungCap) => ViewBag.MaNCC = new SelectList(database.LoaiSPs.ToList(), "MaNCC", "TenNCC");
        public ActionResult ChiTietLoaiSP(int id)
        {
            var dsLoaiSP = database.LoaiSPs.FirstOrDefault(LoaiSP => LoaiSP.MaLoaiSP == id);

            if (dsLoaiSP == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Lấy danh sách các loại sản phẩm và nhà cung cấp
            ViewBag.MaLoaiSP = new SelectList(database.LoaiSPs.ToList(), "MaLoaiSP", "TenLoaiSP");
            return View(dsLoaiSP);
        }
        public ActionResult EditLoaiSP(int id)
        {
            // Lấy thông tin sản phẩm cần chỉnh sửa từ cơ sở dữ liệu
            var dsLoaiSP = database.LoaiSPs.FirstOrDefault(LoaiSP => LoaiSP.MaLoaiSP == id);

            if (dsLoaiSP == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Lấy danh sách các loại sản phẩm và nhà cung cấp
            ViewBag.MaLoaiSP = new SelectList(database.LoaiSPs.ToList(), "MaLoaiSP", "TenLoaiSP");

            return View(dsLoaiSP);
        }

        [HttpPost]
        public ActionResult EditLoaiSP(LoaiSP loaiSP)
        {
            if (ModelState.IsValid)
            {
                // Lấy sản phẩm từ cơ sở dữ liệu
                var LoaiSPDB = database.LoaiSPs.FirstOrDefault(LoaiSP => LoaiSP.MaLoaiSP == loaiSP.MaLoaiSP);

                if (LoaiSPDB != null)
                {
                    // Cập nhật thông tin sản phẩm từ form chỉnh sửa
                    LoaiSPDB.TenLoaiSP = loaiSP.TenLoaiSP;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    database.SaveChanges();

                    ViewBag.ThongBao = "Chỉnh sửa nhà cung cấp thành công";
                    return RedirectToAction("LoaiSP");
                }
                else
                {
                    ViewBag.ThongBao = " ";
                }
            }

            return View(loaiSP);

        }
        public ActionResult DeleteLoaiSP(int id)
        {
            var dsLoaiSP = database.LoaiSPs.FirstOrDefault(LoaiSP => LoaiSP.MaLoaiSP == id);

            if (dsLoaiSP != null)
            {
                database.LoaiSPs.Remove(dsLoaiSP);
                database.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            }

            return RedirectToAction("LoaiSP"); // Chuyển hướng người dùng sau khi xóa (không cần trang xác nhận)
        }
        public ActionResult HoaDon(int? page)
        {
            var dsHoaDon = database.DONDATHANGs.ToList();
            //Tạo biến cho biết số sách mỗi trang
            int pageSize = 5;
            //Tạo biến số trang
            int pageNum = (page ?? 1);
            return View(dsHoaDon.OrderBy(hd => hd.MaHD).ToPagedList(pageNum, pageSize));
        }
        public ActionResult EditHoaDon(int id)
        {
            // Lấy thông tin sản phẩm cần chỉnh sửa từ cơ sở dữ liệu
            var dsLoaiSP = database.LoaiSPs.FirstOrDefault(LoaiSP => LoaiSP.MaLoaiSP == id);

            if (dsLoaiSP == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            // Lấy danh sách các loại sản phẩm và nhà cung cấp
            ViewBag.MaLoaiSP = new SelectList(database.LoaiSPs.ToList(), "MaLoaiSP", "TenLoaiSP");

            return View(dsLoaiSP);
        }

        [HttpPost]
        public ActionResult EditHoaDon(DONDATHANG hoaDon)
        {
            if (ModelState.IsValid)
            {
                // Lấy sản phẩm từ cơ sở dữ liệu
                var HOADON = database.DONDATHANGs.FirstOrDefault(LoaiSP => LoaiSP.MaHD == hoaDon.MaHD);

                if (HOADON != null)
                {
                    // Cập nhật thông tin sản phẩm từ form chỉnh sửa
                    HOADON.MaHD = hoaDon.MaHD;
                    HOADON.MaKH = hoaDon.MaKH;
                    HOADON.Tennguoinhan = hoaDon.Tennguoinhan;
                    HOADON.Dagiao = hoaDon.Dagiao;
                    HOADON.Diachinhan = hoaDon.Diachinhan;
                    HOADON.Email = hoaDon.Email;
                    HOADON.HTGiaohang = hoaDon.HTGiaohang;
                    HOADON.HTThanhtoan = hoaDon.HTThanhtoan;
                    HOADON.Trigia = hoaDon.Trigia;
                    HOADON.NgayDH = hoaDon.NgayDH;
                    HOADON.Ngaygiaohang = hoaDon.Ngaygiaohang;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    database.SaveChanges();

                    ViewBag.ThongBao = "Chỉnh sửa nhà cung cấp thành công";
                    return RedirectToAction("LoaiSP");
                }
                else
                {
                    ViewBag.ThongBao = " ";
                }
            }

            return View(loaiSP);

        }
        public ActionResult DeleteLoaiSP(int id)
        {
            var dsLoaiSP = database.LoaiSPs.FirstOrDefault(LoaiSP => LoaiSP.MaLoaiSP == id);

            if (dsLoaiSP != null)
            {
                database.LoaiSPs.Remove(dsLoaiSP);
                database.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            }

            return RedirectToAction("LoaiSP"); // Chuyển hướng người dùng sau khi xóa (không cần trang xác nhận)
        }

    }
}