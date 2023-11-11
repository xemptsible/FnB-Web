using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFnB.Models
{
    public partial class Comment
    {
        public int ID { get; set; }
        public int Rating { get; set; }
        public string MoTa { get; set; }
        public Nullable<System.DateTime> NgayDG { get; set; }
        public int MaKH { get; set; }
        public int MaSP { get; set; }
    }
}