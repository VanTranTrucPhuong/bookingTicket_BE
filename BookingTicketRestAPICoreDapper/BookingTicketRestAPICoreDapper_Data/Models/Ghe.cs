namespace BookingTicketRestAPICoreDapper_Data.Models
{
    public class Ghe
    {
        public int MaGhe { get; set; }
        public string TenHang { get; set; }

        public string TenGhe { get; set; }

        public int MaRap { get; set; }

        public int SoThuTu { get; set; }

        public int MaLoaiGhe { get; set; }

        public bool KichHoat { get; set; }
    }

    public class GheInsert
    {
        public int MaRap { get; set; }
        public string TenHang { get; set; }
        public int SoLuongGhe { get; set; }
    }


}