using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data.Models.ViewModel
{
    public class ThongTinDatVe
    {
        public string TenPhim { get; set; }
        public int MaThanhToan { get; set; }
        public bool TrangThaiHuy { get; set; }
        public int? MaLichChieu { get; set; }
        public DateTime? NgayChieuGioChieu { get; set; }
        public decimal GiaVe { get; set; }
        public int ThoiLuongPhim { get; set; }
        public string MaHeThongRap { get; set; }
        public string TenHeThongRap { get; set; }
        public string MaCumRap { get; set; }
        public string TenCumRap { get; set; }
        public int? MaRap { get; set; }
        public string TenRap { get; set; }

        public List<ThongTinGhe> DanhSachGhe = new List<ThongTinGhe>();

    }

    public class ThongTinGhe
    {
        public int MaVe { get; set; }
        public DateTime NgayDat { get; set; }
        public int MaGhe { get; set; }
        public string TenGhe { get; set; }
    }
}
