using System;
using System.Collections.Generic;

namespace BookingTicketRestAPICoreDapper_Data.Models.ViewModel
{
    public class _HeThongRap
    {
        public string MaHeThongRap { get; set; }
        public string TenHeThongRap { get; set; }
        public List<_CumRap> ListCumRap = new List<_CumRap>();
    }

    public class _CumRap
    {
        public string MaCumRap { get; set; }
        public string TenCumRap { get; set; }
        public string ThongTin { get; set; }
        public List<_LichChieuTheoPhim> ListPhim = new List<_LichChieuTheoPhim>();
    }

    public class _LichChieuTheoPhim
    {
        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }

        //public string MaRap { get; set; }
        //public string TenRap { get; set; }
        //public decimal? GiaVe { get; set; }
        public List<_LichChieu> ListNgayChieuGioChieu = new List<_LichChieu>();
    }

    public class _LichChieu
    {
        public int MaLichChieu { get; set; }
        public DateTime ThoiGianChieu { get; set; }
        public string MaRap { get; set; }
        public string TenRap { get; set; }
        public decimal? GiaVe { get; set; }
    }

    public class LichChieuMapView
    {
        public int MaLichChieu { get; set; }
        public string MaRap { get; set; }
        public string TenRap { get; set; }
        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public decimal? GiaVe { get; set; }
        public int ThoiLuong { get; set; }
        public string MaHeThongRap { get; set; }
        public string MaCumRap { get; set; }
        public string TenCumRap { get; set; }
        public string TenHeThongRap { get; set; }
        public DateTime NgayChieuGioChieu { get; set; }
        public string ThongTin { get; set; }
    }
}