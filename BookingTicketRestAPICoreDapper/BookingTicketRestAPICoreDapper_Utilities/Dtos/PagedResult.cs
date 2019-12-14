using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTicketRestAPICoreDapper_Utilities.Dtos
{
    public class PagedResult<T>
    {
        public List<T> Items { set; get; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRow { get; set; }
    }
}
