using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data.Models
{
    public class LichChieu
    {
        public LichChieu()
        {
            Ve = new HashSet<Ve>();
        }

        //public int MaLichChieu { get; set; }
        public int? MaRap { get; set; }
        public int? MaPhim { get; set; }
        public DateTime NgayChieuGioChieu { get; set; }
        public decimal GiaVe { get; set; }
        public int? ThoiLuong { get; set; }
        public string MaHeThongRap { get; set; }
        public string MaCumRap { get; set; }

        public virtual ICollection<Ve> Ve { get; set; }
    }
}
