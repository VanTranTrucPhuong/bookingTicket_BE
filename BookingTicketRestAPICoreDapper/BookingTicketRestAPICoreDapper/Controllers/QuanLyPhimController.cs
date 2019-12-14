using BookingTicketRestAPICoreDapper_Data.Models;
using BookingTicketRestAPICoreDapper_Data.Models.ViewModel;
using BookingTicketRestAPICoreDapper_Data.Repositories;
using BookingTicketRestAPICoreDapper_Utilities;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static BookingTicketRestAPICoreDapper_Utilities.Common;

namespace BookingTicketRestAPICoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyPhimController : ControllerBase
    {
        public IQuanLyPhimRespository _quanLyPhimRespository;
        private ThongBaoLoi thongBaoLoi = new ThongBaoLoi();
        private readonly string _connectionString;

        public QuanLyPhimController(IQuanLyPhimRespository quanLyPhimRespository, IConfiguration configuration)
        {
            _quanLyPhimRespository = quanLyPhimRespository;
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        [HttpGet("LayDanhSachPhim")]
        public async Task<IActionResult> LayDanhSachPhim(string key)
        {
            IEnumerable<Phim> phim = await _quanLyPhimRespository.LayDanhSachPhim(key);
            return Ok(phim);
        }

        [HttpGet("LayDanhSachPhimPhanTrang")]
        public async Task<IActionResult> LayDanhSachPhimPhanTrang(string keyword, int pageIndex, int pageSize)
        {
            var phim = await _quanLyPhimRespository.LayDanhSachPhimPhanTrang(keyword, pageIndex, pageSize);
            return Ok(phim);
        }

        [HttpGet("LayThongTinPhimFull")]
        public async Task<IActionResult> LayThongTinPhimFull(int maPhim)
        {
            var phim = await _quanLyPhimRespository.LayThongTinPhimFull(maPhim);
            return Ok(phim);
        }

        [HttpGet("LayThongTinPhim")]
        public async Task<IActionResult> LayThongTinPhim(int maPhim)
        {
            var phim = await _quanLyPhimRespository.LayThongTinPhim(maPhim);
            return Ok(phim);
        }

        [HttpPost("ThemPhim")]
        public async Task<IActionResult> ThemPhim(PhimInsert phim)
        {
            var result = await _quanLyPhimRespository.ThemPhim(phim);
            return Ok(result);
        }

        [HttpPut("CapNhatPhim")]
        public async Task<IActionResult> CapNhatPhim(PhimViewModel phimCapNhat)
        {
            var result = await _quanLyPhimRespository.CapNhatPhim(phimCapNhat);
            return Ok(result);
        }

        [HttpDelete("XoaPhim")]
        public async Task<IActionResult> XoaPhim(int maPhim)
        {
            var result = await _quanLyPhimRespository.XoaPhim(maPhim);
            return Ok(result);
        }

        [HttpGet("TimKiemPhim")]
        public async Task<IActionResult> TimKiemPhim(int maPhim, string maCumRap, string ngayChieuGioChieu)
        {
            var result = await _quanLyPhimRespository.TimKiemPhim(maPhim, maCumRap, ngayChieuGioChieu);
            return Ok(result);
        }

        private const int TenMegaBytes = 1024 * 1024;

        [HttpPost("UploadHinhAnhPhim")]
        public async Task<IActionResult> UploadHinhAnhPhim()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                IFormFile file = Request.Form.Files[0];
                string tenPhim = Request.Form["tenPhim"];
                tenPhim = LoaiBoKyTu.bestLower(tenPhim);

                if (string.IsNullOrEmpty(tenPhim))
                {
                    var result = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Tên phim không hợp lệ");
                    return Ok(result);
                }

                //if (file.Length > TenMegaBytes)
                //{
                //    var result = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Dung lượng file vượt quá 1 MB!");
                //    return Ok(result);
                //}
                if (file.ContentType == "image/png" || file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/gif")
                {
                    try
                    {
                        // Kiểm tra tên phim có tồn tại hay không để upload ảnh
                        var kh = connection.Query("SELECT * FROM dbo.Phim WHERE BiDanh = '" + tenPhim + "'", commandType: CommandType.Text);
                        if (kh.Count() == 0)
                        {
                            var result = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Phim không tồn tại không thể upload file");
                            return Ok(result);        
                        }

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/HinhAnh", tenPhim + "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1]);
                        var stream = new FileStream(path, FileMode.Create);
                        file.CopyTo(stream);
                        return Ok("Upload file thành công !");
                    }
                    catch
                    {
                        var response = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Upload file không thành công!");
                        return Ok(response);
                    }
                }
                else
                {
                    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Định dạng file không hợp lệ!");
                }
            }
        }

        [HttpPost("BinhLuanPhim")]
        public async Task<IActionResult> BinhLuanPhim(BinhLuan binhLuan)
        {
            var result = await _quanLyPhimRespository.ThemBinhLuan(binhLuan);
            return Ok(result);
        }
    }
}