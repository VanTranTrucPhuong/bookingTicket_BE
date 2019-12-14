using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data.Models.ViewModel
{
    public class ThongTinTaiKhoanVM
    {
        public int MaThanhVien { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDT { get; set; }
        public string LoaiNguoiDung { get; set; }
        public int? SoLuongVeDaMua { get; set; }
        public IEnumerable<ThongTinDatVe> ThongTinDatVe { get; set; }
    }
}
