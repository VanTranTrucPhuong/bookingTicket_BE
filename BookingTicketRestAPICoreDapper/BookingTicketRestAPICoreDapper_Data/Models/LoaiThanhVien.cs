using System.Collections.Generic;

namespace BookingTicketRestAPICoreDapper_Data.Models
{
    public partial class LoaiThanhVien
    {
        public LoaiThanhVien()
        {
            ThanhVien = new HashSet<ThanhVien>();
        }

        public string MaLoaiThanhVien { get; set; }
        public string TenLoai { get; set; }

        public virtual ICollection<ThanhVien> ThanhVien { get; set; }
    }
}