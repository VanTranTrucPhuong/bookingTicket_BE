using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data.Models.ViewModel
{
    public class DanhSachVeDatViewModel
    {
        public int MaLichChieu { get; set; }
        public decimal TongTien { get; set; }
        public ICollection<VeViewModel> DanhSachVe { get; set; }

        public string TaiKhoan { get; set; }
    }

    public class VeViewModel
    {
        public int MaGhe { get; set; }
        public decimal GiaVe { get; set; }
    }
}
