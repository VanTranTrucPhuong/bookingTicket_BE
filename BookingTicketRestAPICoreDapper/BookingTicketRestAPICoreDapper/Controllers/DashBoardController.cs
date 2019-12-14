using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingTicketRestAPICoreDapper_Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingTicketRestAPICoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        public IDashboardRespository _dashboardRespository;

        public DashBoardController(IDashboardRespository dashboardRespository)
        {
            _dashboardRespository = dashboardRespository;
        }

        [HttpGet("ThongKeVeTheoHTR")]
        public async Task<IActionResult> ThongKeVeTheoHTR(DateTime NGAYBD, DateTime NGAYKT, string MaHeThongRap)
        {
            var result = await _dashboardRespository.ThongKeVeTheoHTR(NGAYBD, NGAYKT, MaHeThongRap);
            return Ok(result);
        }

        [HttpGet("ThongKeVeTheoNam")]
        public async Task<IActionResult> ThongKeVeTheoNam(int nam, string maHeThongRap)
        {
            var result = await _dashboardRespository.ThongKeVeTheoNam(nam, maHeThongRap);
            return Ok(result);
        }

        [HttpGet("ThongKeDoanhThuTheoHTR")]
        public async Task<IActionResult> ThongKeDoanhThuTheoHTR(DateTime NGAYBD, DateTime NGAYKT, string MaHeThongRap)
        {
            var result = await _dashboardRespository.ThongKeDoanhThuTheoHTR(NGAYBD, NGAYKT, MaHeThongRap);
            return Ok(result);
        }

        [HttpGet("ThongKeDoanhThuTheoNam")]
        public async Task<IActionResult> ThongKeDoanhThuTheoNam(int nam, string maHeThongRap)
        {
            var result = await _dashboardRespository.ThongKeDoanhThuTheoNam(nam, maHeThongRap);
            return Ok(result);
        }
    }
}