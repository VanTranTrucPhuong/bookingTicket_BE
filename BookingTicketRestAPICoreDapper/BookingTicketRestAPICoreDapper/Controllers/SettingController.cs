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
    public class SettingController : ControllerBase
    {
        public IQuanLySettingRespository _quanLySettingRespository;

        public SettingController(IQuanLySettingRespository quanLySettingRespository)
        {
            _quanLySettingRespository = quanLySettingRespository;
        }

        [HttpGet("LayDanhSachSetting")]
        public async Task<IActionResult> LayDanhSachSetting(string maSetting)
        {
            var result = await _quanLySettingRespository.LayDanhSachSetting(maSetting);
            return Ok(result);
        }
    }
}