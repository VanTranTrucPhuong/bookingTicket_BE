using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data.Models.ViewModel
{
    public class ThanhVienViewModel
    {
        public string TaiKhoan { get; set; }
        public string HoTen { get; set; }
        public string MatKhau { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public int MaLoaiThanhVien { get; set; }
    }
}
