using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFnB.Models
{
    public class MatHangMua
    {
        QLBANHANGEntities1 db = new QLBANHANGEntities1();
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string Anh { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuong { get; set; }

       /* public IEnumerable<CartItem> Items
        {
            get { return items; }
        }*/

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
        /*public void Update_quantity(int id, int _new_quan)
        {
            var item = items.Find(s => s._product.ProductID == id);
            if (item != null)
                item._quantity = _new_quan;
        }
        // Phương thức xóa sản phẩm trong giỏ hàng
        public void Remove_CartItem(int id)
        {
            items.RemoveAll(s => s._product.ProductID == id);
        }*/
    }
}