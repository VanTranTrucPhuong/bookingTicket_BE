using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data.Models.ViewModel
{
    public class ThanhVienDangNhapViewModel
    {
        public int MaThanhVien { get; set; }
        public string TaiKhoan { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public int MaLoaiThanhVien { get; set; }
        public string AccessToken { get; set; }
    }
}
