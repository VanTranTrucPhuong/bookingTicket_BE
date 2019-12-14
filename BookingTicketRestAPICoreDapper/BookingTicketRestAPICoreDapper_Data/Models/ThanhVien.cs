namespace BookingTicketRestAPICoreDapper_Data.Models
{
    public partial class ThanhVien
    {
        public int MaThanhVien { get; set; }
        public string TaiKhoan { get; set; }
        public string HoTen { get; set; }
        public string MatKhau { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public int MaLoaiThanhVien { get; set; }
        public int SoLuongVeMua { get; set; }
        public string BiDanh { get; set; }
        public bool DaXoa { get; set; }
    }
}