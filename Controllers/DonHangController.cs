using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFnB.Models;
using PagedList;

namespace WebFnB.Controllers
{
    public class DonHangController : Controller
    {
        QLBANHANGEntities database = new QLBANHANGEntities();

        public ActionResult DanhSachDonHang(int? page)
        {
            var dsDonHang = database.HDs.ToList();

            int pageSize = 10;
            int pageNum = (page ?? 1);

            return View(dsDonHang.ToPagedList(pageNum, pageSize));
        }

        public ActionResult ChiTietDonHang(int id)
        {
            var donHang = database.HDs.FirstOrDefault(dh => dh.MaHD == id);

            if (donHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(donHang);
        }

        [HttpPost]
        public ActionResult EditDonHang(HD donHang)
        {
            if (ModelState.IsValid)
            {
                var donHangDB = database.HDs.FirstOrDefault(dh => dh.MaHD == donHang.MaHD);

                if (donHangDB != null)
                {
                    // Cập nhật thông tin đơn hàng từ form chỉnh sửa
                    donHangDB.NgayDat = donHang.NgayDat;
                    donHangDB.NgayGiao  = donHang.NgayGiao;
                    donHangDB.TinhTrang = donHang.TinhTrang;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    database.SaveChanges();

                    ViewBag.ThongBao = "Chỉnh sửa thông tin đơn hàng thành công";
                    return RedirectToAction("DanhSachDonHang");
                }
                else
                {
                    ViewBag.ThongBao = "Đơn hàng không tồn tại";
                }
            }

            return View(donHang);
        }

        public ActionResult DeleteDonHang(int id)
        {
            var donHang = database.HDs.FirstOrDefault(dh => dh.MaHD == id);

            if (donHang != null)
            {
                return View(donHang);
            }
            else
            {
                ViewBag.ThongBao = "Đơn hàng không tồn tại";
                return RedirectToAction("DanhSachDonHang");
            }
        }

        [HttpPost, ActionName("DeleteDonHang")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDeleteDonHang(int id)
        {
            var donHang = database.HDs.FirstOrDefault(dh => dh.MaHD == id);

            if (donHang != null)
            {
                // Xóa đơn hàng từ cơ sở dữ liệu
                database.HDs.Remove(donHang);
                database.SaveChanges();

                ViewBag.ThongBao = "Xóa đơn hàng thành công";
                return RedirectToAction("DanhSachDonHang");
            }
            else
            {
                ViewBag.ThongBao = "Đơn hàng không tồn tại";
                return RedirectToAction("DanhSachDonHang");
            }
        }
    }
}
