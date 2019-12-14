using System.Collections.Generic;

namespace BookingTicketRestAPICoreDapper_Data.Models
{
    public partial class HeThongRap
    {
        public string MaHeThongRap { get; set; }
        public string TenHeThongRap { get; set; }
        public string BiDanh { get; set; }
        public string Logo { get; set; }

        public IEnumerable<CumRap> DanhSachCumRap { get; set; }
    }
}