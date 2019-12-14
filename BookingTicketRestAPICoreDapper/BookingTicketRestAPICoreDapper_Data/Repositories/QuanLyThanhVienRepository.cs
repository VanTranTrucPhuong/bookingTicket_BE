using BookingTicketRestAPICoreDapper_Data.Models;
using BookingTicketRestAPICoreDapper_Data.Models.ViewModel;
using BookingTicketRestAPICoreDapper_Utilities;
using BookingTicketRestAPICoreDapper_Utilities.Dtos;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookingTicketRestAPICoreDapper_Data.Repositories
{
    public interface IQuanLyThanhVienRepository
    {
        Task<IEnumerable<LoaiThanhVien>> LayDanhSachLoaiThanhVien();

        Task<IEnumerable<ThanhVien>> LayDanhSachThanhVien(string taiKhoan);

        Task<PagedResult<ThanhVien>> LayDanhSachThanhVienPhanTrang(string keyword, int pageIndex, int pageSize);

        Task<Object> DangNhap(ThongTinDangNhapViewModel thongTinDangNhap);

        Task<dynamic> DangKy(ThanhVienViewModel thongTinDangKy);

        Task<Object> LayThongTinTaiKhoan(string taiKhoan);

        Task<Object> CapNhatThongTinThanhVien(ThanhVienViewModel thongTinCapNhat);

        Task<Object> XoaThanhVien(string taiKhoan);
    }

    public class QuanLyThanhVienRespository : IQuanLyThanhVienRepository
    {
        private readonly string connectionString;
        private ThongBaoLoi thongBaoLoi = new ThongBaoLoi();
        private const string SECRET_KEY = "987654321123456789";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

        public QuanLyThanhVienRespository(string _connectionString)
        {
            this.connectionString = _connectionString;
        }

        public async Task<Object> CapNhatThongTinThanhVien(ThanhVienViewModel thongTinCapNhat)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@TAIKHOAN", thongTinCapNhat.TaiKhoan);
                param.Add("@HOTEN", thongTinCapNhat.HoTen);
                param.Add("@MATKHAU", thongTinCapNhat.MatKhau);
                param.Add("@EMAIL", thongTinCapNhat.Email);
                param.Add("@SODIENTHOAI", thongTinCapNhat.SoDienThoai);
                param.Add("@MALOAITHANHVIEN", thongTinCapNhat.MaLoaiThanhVien);
                param.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                var result = connection.Execute("THANH_VIEN_UPDATE", param, commandType: CommandType.StoredProcedure);

                int returnValue = param.Get<int>("@ReturnValue");

                if (returnValue == -2)
                {
                    var response = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Tài khoản không tồn tại!");
                    return response;
                }
                if (returnValue == -1)
                {
                    var response = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Tài khoản đã tồn tại!");
                    return response;
                }
                if (returnValue == 1)
                {
                    var response = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Email đã tồn tại!");
                    return response;
                }
                if (returnValue == 2)
                {
                    var response = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Mã loại thành viên không hợp lệ!");
                    return response;
                }
                return returnValue;
            }
        }

        public async Task<dynamic> DangKy(ThanhVienViewModel thongTinDangKy)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@TAIKHOAN", thongTinDangKy.TaiKhoan);
                param.Add("@HOTEN", thongTinDangKy.HoTen);
                param.Add("@MATKHAU", thongTinDangKy.MatKhau);
                param.Add("@EMAIL", thongTinDangKy.Email);
                param.Add("@SODIENTHOAI", thongTinDangKy.SoDienThoai);
                param.Add("@MALOAITHANHVIEN", thongTinDangKy.MaLoaiThanhVien);
                param.Add("@BIDANH", "LoaiBoKyTu");
                param.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                var result = connection.Execute("DANG_KY", param, commandType: CommandType.StoredProcedure);

                int returnValue = param.Get<int>("@ReturnValue");

                if (returnValue == -1)
                {
                    var response = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Tài khoản đã tồn tại!");
                    return response;
                }
                if (returnValue == 1)
                {
                    var response = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Email đã tồn tại!");
                    return response;
                }
                if (returnValue == 2)
                {
                    var response = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Mã loại thành viên không hợp lệ!");
                    return response;
                }

                //ThanhVien thanhVienInsert = Mapper.Map<ThanhVienViewModel, ThanhVien>(thongTinDangKy);
                //thanhVienInsert.BiDanh = LoaiND.KhachHang;
                //ndInsert.BiDanh = LoaiBoKyTu.bestLower(ndInsert.HoTen);
                //db.NguoiDung.Add(ndInsert);
                return returnValue;
            }
        }

        //Đăng nhập
        public async Task<Object> DangNhap(ThongTinDangNhapViewModel thongTinDangNhap)
        {
            ThanhVien thanhVien;
            using (var connection = new SqlConnection(connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@TAIKHOAN", thongTinDangNhap.TaiKhoan);
                param.Add("@MATKHAU", thongTinDangNhap.MatKhau);
                thanhVien = connection.QuerySingleOrDefault<ThanhVien>("DANG_NHAP", param, commandType: CommandType.StoredProcedure);
            }

            if (thanhVien != null)
            {
                ThanhVienDangNhapViewModel thanhVienDNVM = new ThanhVienDangNhapViewModel
                {
                    MaThanhVien = thanhVien.MaThanhVien,
                    TaiKhoan = thanhVien.TaiKhoan,
                    HoTen = thanhVien.HoTen,
                    Email = thanhVien.HoTen,
                    SoDienThoai = thanhVien.SoDienThoai,
                    MaLoaiThanhVien = thanhVien.MaLoaiThanhVien
                };

                string accessToken = GenerateToken(thanhVienDNVM);
                thanhVienDNVM.AccessToken = accessToken;

                return thanhVienDNVM;
            }

            var response = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Tài khoản hoặc mật khẩu không đúng!");
            return response;
        }

        public async Task<IEnumerable<LoaiThanhVien>> LayDanhSachLoaiThanhVien()
        {
            IEnumerable<LoaiThanhVien> loaiThanhVien = null;
            using (var connection = new SqlConnection(connectionString))
            {
                loaiThanhVien = connection.Query<LoaiThanhVien>("LOAI_THANH_VIEN_GET", commandType: CommandType.StoredProcedure);
            }
            return loaiThanhVien;
        }

        public async Task<IEnumerable<ThanhVien>> LayDanhSachThanhVien(string taiKhoan)
        {
            IEnumerable<ThanhVien> thanhVien = null;
            using (var connection = new SqlConnection(connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@TAIKHOAN", taiKhoan);
                thanhVien = connection.Query<ThanhVien>("THANH_VIEN_GETALL", param, commandType: CommandType.StoredProcedure);
            }
            return thanhVien;
        }

        public async Task<PagedResult<ThanhVien>> LayDanhSachThanhVienPhanTrang(string keyword, int pageIndex, int pageSize)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                var param = new DynamicParameters();
                param.Add("@KEYWORD", keyword);
                param.Add("@PAGEINDEX", pageIndex);
                param.Add("@PAGESIZE", pageSize);
                param.Add("@totalRow", dbType: System.Data.DbType.Int32,
                    direction: System.Data.ParameterDirection.Output);

                var result = await connection.QueryAsync<ThanhVien>("THANH_VIEN_GETPAGING",
                    param, null, null, System.Data.CommandType.StoredProcedure);

                int totalRow = param.Get<int>("@totalRow");

                var pagedResult = new PagedResult<ThanhVien>()
                {
                    Items = result.ToList(),
                    TotalRow = totalRow,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                return pagedResult;
            }
        }

        public async Task<Object> XoaThanhVien(string taiKhoan)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                var param = new DynamicParameters();
                param.Add("@TAIKHOAN", taiKhoan);
                param.Add("@ReturnValue", dbType: System.Data.DbType.Int32,
                    direction: System.Data.ParameterDirection.ReturnValue);
                var result = await connection.QueryAsync("THANH_VIEN_DELETE", param, commandType: CommandType.StoredProcedure);

                int returnValue = param.Get<int>("@ReturnValue");

                if (returnValue == -2)
                {
                    var response = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Tài khoản không tồn tại!");
                    return response;
                }

                //ThanhVien thanhVienInsert = Mapper.Map<ThanhVienViewModel, ThanhVien>(thongTinDangKy);
                //thanhVienInsert.BiDanh = LoaiND.KhachHang;
                //ndInsert.BiDanh = LoaiBoKyTu.bestLower(ndInsert.HoTen);
                //db.NguoiDung.Add(ndInsert);
                return returnValue;
            }
        }

        public async Task<object> LayThongTinTaiKhoan(string taiKhoan)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                ThanhVien thanhVien = connection.QuerySingleOrDefault<ThanhVien>("SELECT * FROM dbo.ThanhVien WHERE TaiKhoan = '" + taiKhoan + "'", commandType: CommandType.Text);
                if (thanhVien == null)
                {
                    // I wish to return an error response how can i do that?
                    var response = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi400, "Tài khoản không hợp lệ!");
                    return response;
                }

                IEnumerable<Ve> listDatVe = connection.Query<Ve>("SELECT * FROM dbo.VE WHERE MaThanhVien = ' " + thanhVien.MaThanhVien + "'", commandType: CommandType.Text);
                List<ThongTinDatVe> listThongTinDatVe = new List<ThongTinDatVe>();

                if (listDatVe.Count() != 0)
                {
                   
                    Ghe ghe = new Ghe();
                    foreach (var item in listDatVe.GroupBy(n => new { n.MaThanhToan }))
                    {
                        //, MaGhe = item.MaGhe, TenGhe = ghe.TenGhe, MaRap = ghe.MaRap, TenRap = rap.TenRap,MaHeThongRap = cumRap.MaHeThongRap,TenHeThongRap = connection.QuerySingleOrDefault("SELECT TenHeThongRap FROM dbo.HETHONGRAP WHERE MaHeThongRap = " + cumRap.MaHeThongRap, commandType: CommandType.Text)
                        ThongTinDatVe thongTinDatVe = new ThongTinDatVe();
                        Rap rap = new Rap();
                        CumRap cumRap = new CumRap();
                        LichChieu lichChieu = new LichChieu();

                        thongTinDatVe.MaThanhToan = item.Key.MaThanhToan;
                        thongTinDatVe.TrangThaiHuy = connection.QuerySingleOrDefault<bool>("SELECT TrangThaiHuy FROM dbo.THANHTOAN WHERE MaThanhToan = " + item.Key.MaThanhToan, commandType: CommandType.Text);

                        foreach (var CTVe in item)
                        {
                            ThongTinGhe thongTinGhe = new ThongTinGhe();
                            lichChieu = connection.QuerySingleOrDefault<LichChieu>("SELECT * FROM dbo.LICHCHIEU WHERE MaLichChieu = " + CTVe.MaLichChieu, commandType: CommandType.Text);
                            //rap = connection.QuerySingleOrDefault<Rap>("SELECT * FROM dbo.RAP WHERE MaRap = " + lichChieu.MaRap, commandType: CommandType.Text);
                            //cumRap = connection.QuerySingleOrDefault<CumRap>("SELECT * FROM dbo.CUMRAP WHERE MaCumRap = '" + rap.MaCumRap + "'", commandType: CommandType.Text);
                            ghe = await connection.QuerySingleOrDefaultAsync<Ghe>("SELECT * FROM dbo.GHE WHERE MaGhe = " + CTVe.MaGhe, commandType: CommandType.Text);

                            thongTinDatVe.MaLichChieu = CTVe.MaLichChieu;
                            thongTinGhe.MaGhe = ghe.MaGhe;
                            thongTinGhe.TenGhe = ghe.TenGhe;
                            thongTinGhe.MaVe = CTVe.MaVe;
                            thongTinGhe.NgayDat = CTVe.NgayDat.Value;

                            thongTinDatVe.TenPhim = connection.QuerySingleOrDefault<string>("SELECT TenPhim FROM dbo.Phim WHERE MaPhim = " + lichChieu.MaPhim, commandType: CommandType.Text);
                            thongTinDatVe.GiaVe = CTVe.GiaVe.Value;
                            thongTinDatVe.NgayChieuGioChieu = lichChieu.NgayChieuGioChieu;
                            thongTinDatVe.MaRap = lichChieu.MaRap;
                            thongTinDatVe.TenRap = connection.QuerySingleOrDefault<string>("SELECT TenRap FROM dbo.Rap WHERE MaRap = " + lichChieu.MaRap, commandType: CommandType.Text);
                            thongTinDatVe.MaHeThongRap = lichChieu.MaHeThongRap;
                            thongTinDatVe.MaCumRap = lichChieu.MaCumRap;
                            thongTinDatVe.TenCumRap = connection.QuerySingleOrDefault<string>("SELECT TenCumRap FROM dbo.CumRap WHERE MaCumRap = '" + lichChieu.MaCumRap +"'", commandType: CommandType.Text);
                            thongTinDatVe.DanhSachGhe.Add(thongTinGhe);

                        }
                        listThongTinDatVe.Add(thongTinDatVe);
                        //thongTinDatVe.ThoiLuongPhim = item.MaLichChieuNavigation.ThoiLuong.Value;
                    }
                }


                //ThongTinTaiKhoanVM ttTK = Mapper.Map<NguoiDung, ThongTinTaiKhoanVM>(tt);
                ThongTinTaiKhoanVM thongTinTaiKhoan = new ThongTinTaiKhoanVM();
                thongTinTaiKhoan.MaThanhVien = thanhVien.MaThanhVien;
                thongTinTaiKhoan.LoaiNguoiDung = await connection.QuerySingleOrDefaultAsync<string>("SELECT TenLoai FROM dbo.LOAITHANHVIEN WHERE MaLoaiThanhVien = " + thanhVien.MaLoaiThanhVien, commandType: CommandType.Text);
                thongTinTaiKhoan.TaiKhoan = thanhVien.TaiKhoan;
                thongTinTaiKhoan.MatKhau = thanhVien.MatKhau;
                thongTinTaiKhoan.HoTen = thanhVien.HoTen;
                thongTinTaiKhoan.Email = thanhVien.Email;
                thongTinTaiKhoan.SoDT = thanhVien.SoDienThoai;
                thongTinTaiKhoan.SoLuongVeDaMua = thanhVien.SoLuongVeMua;
                thongTinTaiKhoan.ThongTinDatVe = listThongTinDatVe;
                return thongTinTaiKhoan;
            }
        }

        //Tạo AccessToken
        private string GenerateToken(ThanhVienDangNhapViewModel thanhVienDNVM)
        {
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                    claims: new Claim[] {
                        new Claim(ClaimTypes.Name,thanhVienDNVM.TaiKhoan),
                        new Claim(ClaimTypes.Role,thanhVienDNVM.MaLoaiThanhVien.ToString()),
                    },
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
                    signingCredentials: new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256)

                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}