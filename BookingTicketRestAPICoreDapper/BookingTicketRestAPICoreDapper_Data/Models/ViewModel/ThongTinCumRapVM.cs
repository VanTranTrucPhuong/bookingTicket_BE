using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data.Models.ViewModel
{
    public class ThongTinCumRapViewModel
    {
        public string MaCumRap { get; set; }
        public string TenCumRap { get; set; }
        public string DiaChi { get; set; }
        public IEnumerable<RapViewModel> DanhSachRap { get; set; }
    }

    public class RapViewModel
    {
        public int? MaRap { get; set; }
        public string TenRap { get; set; }
        public int SoGhe { get; set; }
    }


}
