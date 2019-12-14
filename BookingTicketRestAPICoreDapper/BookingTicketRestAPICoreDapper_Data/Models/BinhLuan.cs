using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data.Models
{
    public class BinhLuan
    {
        public int MaThanhVien { get; set; }
        public int MaPhim { get; set; }
        public string NoiDungBinhLuan { get; set; }
        public float ChiSoDanhGia { get; set; }
    }
}
