using BookingTicketRestAPICoreDapper_Data.Models;
using BookingTicketRestAPICoreDapper_Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookingTicketRestAPICoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CumRapController : ControllerBase
    {
        public ICumRapRespository _cumRapRespository;

        public CumRapController(ICumRapRespository cumRapRespository)
        {
            _cumRapRespository = cumRapRespository;
        }

        
        [HttpGet("LayDanhSachCumRapTheoHeThong")]
        public async Task<IActionResult> LayDanhSachCumRapTheoHeThong(string maHeThongRap)
        {
            var result = await _cumRapRespository.LayDanhSachCumRapTheoHeThong(maHeThongRap);
            return Ok(result);
        }

        [HttpGet("LayThongTinCumRap")]
        public async Task<IActionResult> LayThongTinCumRap(string key)
        {
            return Ok(await _cumRapRespository.LayThongTinCumRap(key));
        }

        [HttpGet("LayDanhSachRap")]
        public async Task<IActionResult> LayDanhSachRap(string maCumRap)
        {
            return Ok(await _cumRapRespository.LayDanhSachRap(maCumRap));
        }

        [HttpGet("LayDanhSachGheTheoRap")]
        public async Task<IActionResult> LayDanhSachGheTheoRap(int maRap)
        {
            var result = await _cumRapRespository.LayDanhSachGheTheoRap(maRap);
            return Ok(result);
        }

        [HttpPost("ThemCumRap")]
        public async Task<IActionResult> ThemCumRap(CumRap cumRap)
        {
            _cumRapRespository.ThemCumRap(cumRap);
            return Ok();
        }

        [HttpPut("CapNhatCumRap")]
        public async Task<IActionResult> CapNhatCumRap(CumRap cumRapCapNhat)
        {
            _cumRapRespository.CapNhatCumRap(cumRapCapNhat);
            return Ok();
        }

        [HttpDelete("XoaCumRap")]
        public async Task<IActionResult> XoaCumRap(int maCumRap)
        {
            _cumRapRespository.XoaCumRap(maCumRap);
            return Ok();
        }
    }
}