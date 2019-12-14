using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data.Models.ViewModel
{
    public class LichChieuVM
    {
        public int MaLichChieu { get; set; }
        public int? MaRap { get; set; }
        public int? MaPhim { get; set; }
        public string TenPhim { get; set; }
        public DateTime NgayChieuGioChieu { get; set; }
        public decimal GiaVe { get; set; }
        public int? ThoiLuong { get; set; }
        public string MaHeThongRap { get; set; }
        public string MaCumRap { get; set; }
    }
}
