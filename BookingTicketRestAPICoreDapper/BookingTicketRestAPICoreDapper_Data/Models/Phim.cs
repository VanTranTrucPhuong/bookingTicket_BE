using System;

namespace BookingTicketRestAPICoreDapper_Data.Models
{
    public class Phim
    {
        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public string BiDanh { get; set; }
        public string Trailer { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
        public int ThoiLuong { get; set; }
        public DateTime NgayKhoiChieu { get; set; }
        public float DanhGia { get; set; }
        public string DienVien { get; set; }
        public string DaoDien { get; set; }
        public string DoTuoi { get; set; }
        public bool DaXoa { get; set; }
        public bool KhoiChieu { get; set; }
        public bool SapChieu { get; set; }
    }
}