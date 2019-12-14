using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookingTicketRestAPICoreDapper_Data.Models;
using BookingTicketRestAPICoreDapper_Data.Repositories;

namespace BookingTicketRestAPICoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyHeThongRapController : ControllerBase
    {
        public IQuanLyHeThongRapRespository _quanLyHeThongRapRespository;

        public QuanLyHeThongRapController(IQuanLyHeThongRapRespository quanLyHeThongRapRespository)
        {
            _quanLyHeThongRapRespository = quanLyHeThongRapRespository;
        }

        [HttpGet("LayThongTinHeThongRap")]
        public async Task<IActionResult> LayThongTinHeThongRap(string MaHeThongRap)
        {
            var heThongRap = await _quanLyHeThongRapRespository.LayThongTinHeThongRap(MaHeThongRap);
            return Ok(heThongRap);
        }

        [HttpGet("LayThongTinCumRapTheoHeThong")]
        public async Task<IActionResult> LayThongTinCumRapTheoHeThongRap(string MaHeThongRap)
        {
            var result = await _quanLyHeThongRapRespository.LayThongTinCumRapTheoHeThongRap(MaHeThongRap);
            return Ok(result);
        }

        [HttpGet("LayThongTinLichChieuTheoHeThong")]
        public async Task<IActionResult> LayThongTinLichChieuHeThongRap(string MaHeThongRap)
        {
            var heThongRap = await _quanLyHeThongRapRespository.LayThongTinLichChieuHeThongRap(MaHeThongRap);
            return Ok(heThongRap);
        }

        [HttpPost("ThemHeThongRap")]
        public async Task<IActionResult> ThemHeThongRap(HeThongRap heThongRap)
        {
            _quanLyHeThongRapRespository.ThemHeThongRap(heThongRap);
            return Ok();
        }

        [HttpPut("CapNhatHeThongRap")]
        public async Task<IActionResult> CapNhatHeThongRap(HeThongRap heThongRapCapNhat)
        {
            _quanLyHeThongRapRespository.CapNhatHeThongRap(heThongRapCapNhat);
            return Ok();
        }

        [HttpDelete("XoaHeThongRap")]
        public async Task<IActionResult> XoaHeThongRap(int maHeThongRap)
        {

            _quanLyHeThongRapRespository.XoaHeThongRap(maHeThongRap);
            return Ok();
        }
    }
}