using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFnB.Models
{
    public class NguoiDung
    {
        QLBANHANGEntities db = new QLBANHANGEntities();

        public int TenDN { get; set; }
        public int MatKhau { get; set; }
        public int HoTen { get; set; }
        public int VaiTro { get; set; }
    }
}