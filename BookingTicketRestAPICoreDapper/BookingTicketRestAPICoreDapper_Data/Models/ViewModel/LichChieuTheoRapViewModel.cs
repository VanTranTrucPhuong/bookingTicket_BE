using System;
using System.Collections.Generic;

namespace BookingTicketRestAPICoreDapper_Data.Models.ViewModel
{
    public class LichChieuTheoRapViewModel
    {
        public ThongTinPhim ThongTinPhim = new ThongTinPhim();

        public List<GheTheoHangVM> DanhSachGhe { get; set; }
    }

    public class ThongTinPhim
    {
        public int MaLichChieu { get; set; }
        public string TenCumRap { get; set; }
        public string TenRap { get; set; }
        public string DiaChi { get; set; }
        public string TenPhim { get; set; }
        public string HinhAnh { get; set; }
        public string DoTuoi { get; set; }
        public string NgayChieu { get; set; }
        public string GioChieu { get; set; }
    }

    public class GheViewModel
    {
        public int? MaGhe { get; set; }
        //public string TenHang { get; set; }
        public string TenGhe { get; set; }
        public Nullable<int> MaRap { get; set; }
        public string LoaiGhe { get; set; }
        public int STT { get; set; }
        public int PhuThu { get; set; }
        public Nullable<decimal> GiaVe { get; set; }
        public bool DaDat { get; set; }

        public string TaiKhoanNguoiDat { get; set; }
    }

    public class GheTheoHangVM
    {
        public string TenHang { get; set; }
        public List<GheViewModel> DanhSachGheTheoHang = new List<GheViewModel>();
    }
}