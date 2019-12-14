using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookingTicketRestAPICoreDapper_Utilities
{
    public class ThongBaoLoi : ControllerBase
    {
        public static readonly int Loi404 = 404;
        public static readonly int Loi405 = 405;
        public static readonly int Loi403 = 403;
        public static readonly int Loi400 = 400;
        public static readonly int Loi500 = 500;

        public async Task<ContentResult> thongBaoLoi(int code, string message)
        {
            ContentResult myErrorModel = new ContentResult();
            myErrorModel.ContentType = "text/plain";
            myErrorModel.StatusCode = code;
            myErrorModel.Content = message;
            return myErrorModel;
        }

        public ContentResult thongBaoLoi_(int code, string message)
        {
            ContentResult myErrorModel = new ContentResult();
            myErrorModel.ContentType = "text/plain";
            myErrorModel.StatusCode = code;
            myErrorModel.Content = message;
            return myErrorModel;
        }
    }
}
