using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data.Models
{
    public class Ve
    {
        public int MaVe { get; set; }
        public int MaThanhVien { get; set; }
        public int MaThanhToan { get; set; }
        //public string TaiKhoan { get; set; }
        public int MaGhe { get; set; }
        public int? MaLoaiGhe { get; set; }
        public DateTime? NgayDat { get; set; }
        public decimal? GiaVe { get; set; }
        public int MaLichChieu { get; set; }
        public string ChietKhau { get; set; }
        public bool TrangThaiHuy { get; set; }
    }
}
