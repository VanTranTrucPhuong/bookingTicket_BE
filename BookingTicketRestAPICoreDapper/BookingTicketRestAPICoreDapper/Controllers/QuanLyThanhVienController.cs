using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingTicketRestAPICoreDapper_Data.Models;
using BookingTicketRestAPICoreDapper_Data.Models.ViewModel;
using BookingTicketRestAPICoreDapper_Data.Repositories;
using BookingTicketRestAPICoreDapper_Utilities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookingTicketRestAPICoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyThanhVienController : ControllerBase
    {
        private readonly IQuanLyThanhVienRepository _quanLyThanhVienRepository;
        private const string SECRET_KEY = "987654321123456789";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));


        public QuanLyThanhVienController(IQuanLyThanhVienRepository quanLyThanhVienRepository)
        {
            _quanLyThanhVienRepository = quanLyThanhVienRepository;
        }
        // GET: LayDanhSachLoaiThanhVien
        [HttpGet("LayDanhSachLoaiThanhVien")]
        public async Task<IActionResult> LayDanhSachLoaiThanhVien()
        {
            IEnumerable<LoaiThanhVien> loaiThanhVien = await _quanLyThanhVienRepository.LayDanhSachLoaiThanhVien();
            return Ok(loaiThanhVien);
        }
        // GET: LayDanhSachThanhVien
        [HttpGet("LayDanhSachThanhVien")]
        public async Task<IActionResult> LayDanhSachThanhVien(string taiKhoan)
        {
            IEnumerable<ThanhVien> thanhVien = await _quanLyThanhVienRepository.LayDanhSachThanhVien(taiKhoan);
            return Ok(thanhVien);
        }
        // GET: LayDanhSachThanhVienPhanTrang
        [HttpGet("LayDanhSachThanhVienPhanTrang")]
        public async Task<IActionResult> LayDanhSachThanhVienPhanTrang(string keyword, int pageIndex, int pageSize)
        {
            PagedResult<ThanhVien> thanhVien = await _quanLyThanhVienRepository.LayDanhSachThanhVienPhanTrang(keyword, pageIndex, pageSize);
            return Ok(thanhVien);
        }
        // POST: api/QuanLyThanhVien
        [HttpPost("DangNhap")]
        public async Task<IActionResult> DangNhap(ThongTinDangNhapViewModel thongTinDangNhap)
        {
            Object thanhVienDNVM = await _quanLyThanhVienRepository.DangNhap(thongTinDangNhap);
            return Ok(thanhVienDNVM);
        }
        // POST: api/QuanLyThanhVien
        [HttpPost("DangKy")]
        public async Task<IActionResult> DangKy(ThanhVienViewModel thongTinDangKy)
        {
            var thanhVienDKVM = await _quanLyThanhVienRepository.DangKy(thongTinDangKy);
            return Ok(thanhVienDKVM);
        }

        [HttpGet("LayThongTinTaiKhoan")]
        public async Task<IActionResult> LayThongTinTaiKhoan(string taiKhoan)
        {
            var result = await _quanLyThanhVienRepository.LayThongTinTaiKhoan(taiKhoan);
            return Ok(result);
        }

        // PUT: api/QuanLyThanhVien
        [HttpPut("CapNhatThongTinThanhVien")]
        public async Task<IActionResult> CapNhatThongTinThanhVien(ThanhVienViewModel thongTinCapNhat)
        {
            var thanhVienCapNhat = await _quanLyThanhVienRepository.CapNhatThongTinThanhVien(thongTinCapNhat);
            return Ok(thanhVienCapNhat);
        }
        // DELETE: api/QuanLyThanhVien
        [Authorize(Roles = "QuanTri")]
        [HttpDelete("XoaThanhVien")]
        public async Task<IActionResult> XoaThanhVien(string taiKhoan)
        {
            var thanhVienXoa = await _quanLyThanhVienRepository.XoaThanhVien(taiKhoan);
            return Ok(thanhVienXoa);
        }
    }
}
