using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFnB.Models
{
    public class CartItem
    {
        // Khai báo một mục sản phẩm mua CartItem
        public SP _product { get; set; }
        public int _quantity { get; set; }
    }

    public class MatHangMua
    {
        QLBANHANGEntities db = new QLBANHANGEntities();
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string Anh { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuong { get; set; }

        public IEnumerable<CartItem> items
        {
            get { return items; }
        }

        public decimal ThanhTien()
        {
            return SoLuong * GiaBan;
        }

        public MatHangMua(int MaSP)
        {
            this.MaSP = MaSP;
            var sp = db.SPs.Single(s => s.MaSP == this.MaSP);
            this.TenSP = sp.TenSP;
            this.Anh = sp.Anh;
            this.GiaBan = decimal.Parse(sp.GiaBan.ToString());
            this.SoLuong = 1;
        }
    }
}