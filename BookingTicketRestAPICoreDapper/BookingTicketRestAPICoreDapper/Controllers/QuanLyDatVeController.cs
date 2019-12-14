using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingTicketRestAPICoreDapper_Data.Models.ViewModel;
using BookingTicketRestAPICoreDapper_Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingTicketRestAPICoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyDatVeController : ControllerBase
    {
        public IQuanLyDatVeRespository _quanLyDatVeRespository;

        public QuanLyDatVeController(IQuanLyDatVeRespository quanLyDatVeRespository)
        {
            _quanLyDatVeRespository = quanLyDatVeRespository;
        }

        [HttpPost("DatVe")]
        public async Task<IActionResult> DatVe(DanhSachVeDatViewModel danhSachVeDat)
        {
            var result = await _quanLyDatVeRespository.DatVe(danhSachVeDat);
            return Ok(result);
        }

        [HttpPut("HuyVe")]
        public async Task<IActionResult> HuyVe(int maThanhToan, int mucHoanTien)
        {
            var result = await _quanLyDatVeRespository.HuyVe(maThanhToan, mucHoanTien);
            return Ok(result);
        }


        [HttpGet("LayDanhSachPhongVe")]
        public async Task<IActionResult> LayDanhSachPhongVe(int maLichChieu)
        {
            var result = await _quanLyDatVeRespository.LayDanhSachPhongVe(maLichChieu);
            return Ok(result);
        }
        [HttpPost("PutDanhSachVeDangDat")]
        public async Task<IActionResult> PutDanhSachVeDangDat(DanhSachVeDangDatVM danhSachGheDangDat)
        {
            var result = await _quanLyDatVeRespository.PutDanhSachVeDangDat(danhSachGheDangDat);
            return Ok(result);
        }

        [HttpGet("LayDanhSachLichChieu")]
        public async Task<IActionResult> LayDanhSachLichChieu()
        {
            var result = await _quanLyDatVeRespository.LayDanhSachLichChieu();
            return Ok(result);
        }


        [HttpPost("TaoLichChieu")]
        public async Task<IActionResult> TaoLichChieu(LichChieuInsertVM lichChieuInsert)
        {
            var result = await _quanLyDatVeRespository.TaoLichChieu(lichChieuInsert);
            return Ok(result);
        }
    }
}