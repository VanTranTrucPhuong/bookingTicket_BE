using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data.Models.ViewModel
{
    public class RapViewModel_DSGhe
    {
        public int? MaRap { get; set; }
        public string TenRap { get; set; }
        public int SoGhe { get; set; }
        public List<GheTheoHangVM> DanhSachGhe { get; set; }
    }
}
