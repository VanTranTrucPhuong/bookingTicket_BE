using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data.Models.ViewModel
{
    public class LichChieuInsertVM
    {
        public int MaPhim { get; set; }
        public DateTime NgayChieuGioChieu { get; set; }

        public int MaRap { get; set; }
        public decimal GiaVe { get; set; }
    }
}
