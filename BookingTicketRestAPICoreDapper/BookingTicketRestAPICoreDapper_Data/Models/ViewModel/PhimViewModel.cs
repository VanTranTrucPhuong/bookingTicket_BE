using System;
using System.Collections.Generic;

namespace BookingTicketRestAPICoreDapper_Data.Models.ViewModel
{
    public class PhimViewModel
    {
        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public string BiDanh { get; set; }
        public string Trailer { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
        public string NgayKhoiChieu { get; set; }
        public float DanhGia { get; set; }
        public string DienVien { get; set; }
        public string DaoDien { get; set; }
        public string DoTuoi { get; set; }
        public bool DaXoa { get; set; }
        public bool KhoiChieu { get; set; }
        public bool SapChieu { get; set; }
        public IEnumerable<BinhLuanViewModel> ListBinhLuan { get; set; }
    }

    public class BinhLuanViewModel {
        public int MaThanhVien { get; set; }
        public int MaPhim { get; set; }
        public string TaiKhoan { get; set; }
        public string NoiDungBinhLuan { get; set; }
        public float ChiSoDanhGia { get; set; }
    }

    public class PhimInsert
    {
        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public string BiDanh { get; set; }
        public string Trailer { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
        public string NgayKhoiChieu { get; set; }
        public string DanhGia { get; set; }
        public string DienVien { get; set; }
        public string DaoDien { get; set; }
        public string DoTuoi { get; set; }
        public bool DaXoa { get; set; }
        public bool KhoiChieu { get; set; }
        public bool SapChieu { get; set; }

    }

}