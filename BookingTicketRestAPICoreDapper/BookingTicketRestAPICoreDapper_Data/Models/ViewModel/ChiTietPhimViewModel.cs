using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data.Models.ViewModel
{
    public class ChiTietPhimViewModel
    {
        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public string BiDanh { get; set; }
        public string Trailer { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
        public int ThoiLuong { get; set; }
        public string NgayKhoiChieu { get; set; }
        public float DanhGia { get; set; }
        public string DienVien { get; set; }
        public string DaoDien { get; set; }
        public string DoTuoi { get; set; }
        public bool DaXoa { get; set; }
        public bool KhoiChieu { get; set; }
        public bool? SapChieu { get; set; }
        public IEnumerable<TheLoaiPhim> ListTheLoai { get; set; }
        public List<ThongTinRap> LichChieu = new List<ThongTinRap>();
        public List<ThongTinBinhLuan> ThongTinBinhLuan = new List<ThongTinBinhLuan>();
    }

    public class ThongTinLichChieu
    {
        public int MaLichChieu { get; set; }
        public int? MaRap { get; set; }
        public string TenRap { get; set; }

        public DateTime? NgayChieuGioChieu { get; set; }
        public decimal? GiaVe { get; set; }
        public int? ThoiLuong { get; set; }

    }

    public class ThongTinRap
    {
        public string MaCumRap { get; set; }
        public string TenCumRap { get; set; }
        public string ThongTin { get; set; }

        public string MaHeThongRap { get; set; }
        public string TenHeThongRap { get; set; }
        public List<ThongTinLichChieu> ChiTietLichChieu = new List<ThongTinLichChieu>();
    }

    public class ThongTinBinhLuan
    {
        public int? MaThanhVien { get; set; }
        public string TenThanhVien { get; set; }

        public int? MaPhim { get; set; }
        public string NoiDungBinhLuan { get; set; }
        public string ChiSoDanhGia { get; set; }
        public DateTime NgayBinhLuan { get; set; }

    }

    public class TheLoaiPhim
    {
        public int MaTheLoai { get; set; }
        public string TenTheLoai { get; set; }
    }
}
