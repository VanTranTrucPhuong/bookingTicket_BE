using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Data.Models.ViewModel
{
    public class CumRapViewModel
    {
        public string MaCumRap { get; set; }
        public string TenCumRap { get; set; }
        public string ThongTin { get; set; }      
        public IEnumerable<Rap> DanhSachRap { get; set; }
    }
}
