using BookingTicketRestAPICoreDapper_Data.Models;
using BookingTicketRestAPICoreDapper_Data.Models.ViewModel;
using BookingTicketRestAPICoreDapper_Utilities;
using BookingTicketRestAPICoreDapper_Utilities.Dtos;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static BookingTicketRestAPICoreDapper_Utilities.Common;

namespace BookingTicketRestAPICoreDapper_Data.Repositories
{
    public interface IQuanLyPhimRespository
    {
        Task<IEnumerable<Phim>> LayDanhSachPhim(string key);

        Task<object> LayDanhSachPhimPhanTrang(string keyword, int pageIndex, int pageSize);

        Task<object> LayDanhSachPhimTheoNgay(string keyword, int pageIndex, int pageSize, string ngay = "");

        Task<object> LayThongTinPhimFull(int maPhim);

        Task<PhimViewModel> LayThongTinPhim(int maPhim);

        Task<object> ThemPhim(PhimInsert phim);

        Task<object> CapNhatPhim(PhimViewModel phim);

        Task<object> XoaPhim(int maPhim);

        Task<object> ThemBinhLuan(BinhLuan binhLuan);

        Task<object> TimKiemPhim(int maPhim, string maCumRap, string ngayChieuGioChieu);
    }

    public class QuanLyPhimRespository : IQuanLyPhimRespository
    {
        private readonly string connectionString;
        private ThongBaoLoi thongBaoLoi = new ThongBaoLoi();
        string hostName =  "https://localhost:44350/HinhAnh/";

        public QuanLyPhimRespository(string _connectionString)
        {
            {
                this.connectionString = _connectionString;
            }
        }

        public async Task<object> LayDanhSachPhimPhanTrang(string keyword, int pageIndex, int pageSize)
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

                var result = await connection.QueryAsync<Phim>("PHIM_GETPAGING",
                    param, null, null, System.Data.CommandType.StoredProcedure);

                int totalRow = param.Get<int>("@totalRow");

                if (totalRow == 0)
                {
                    string returnValue = "Không tìm thấy kết quả!";
                    return returnValue;
                }
                else if (totalRow > 0)
                {
                    var pagedResult = new PagedResult<Phim>()
                    {
                        Items = result.ToList(),
                        TotalRow = totalRow,
                        PageIndex = pageIndex,
                        PageSize = pageSize
                    };

                    return pagedResult;
                }
                return false;
            }
        }

        //Bất đồng bộ
        public async Task<IEnumerable<Phim>> LayDanhSachPhim(string key)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                IEnumerable<Phim> phim = null;

                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                var p = new DynamicParameters();
                p.Add("@KEYWORD", key);
                phim = connection.Query<Phim>("PHIM_GET", p, commandType: CommandType.StoredProcedure);
                return phim;

            }
        }

        //Lấy thông tin phim (mô tả, lịch chiếu, bình luận)
        public async Task<object> LayThongTinPhimFull(int maPhim)
        {
        
            using (var connection = new SqlConnection(connectionString))
            {
                IEnumerable<LichChieuVM> lichChieuList = null;
                ChiTietPhimViewModel chiTietPhim = new ChiTietPhimViewModel(); ;
                var phim = connection.QuerySingleOrDefault<Phim>("SELECT * FROM [dbo].[PHIM] WHERE MaPhim = " + maPhim, commandType: CommandType.Text);

                chiTietPhim.TenPhim = phim.TenPhim;
                chiTietPhim.BiDanh = phim.BiDanh;
                chiTietPhim.DanhGia = phim.DanhGia;
                chiTietPhim.HinhAnh = phim.HinhAnh;
                chiTietPhim.MaPhim = phim.MaPhim;
                chiTietPhim.MoTa = phim.MoTa;
                chiTietPhim.ThoiLuong = phim.ThoiLuong;
                chiTietPhim.Trailer = phim.Trailer;
                chiTietPhim.NgayKhoiChieu = phim.NgayKhoiChieu.ToString("dd/MM/yyyy");
                chiTietPhim.DaoDien = phim.DaoDien;
                chiTietPhim.DienVien = phim.DienVien;
                chiTietPhim.DoTuoi = phim.DoTuoi;
                chiTietPhim.DaXoa = phim.DaXoa;
                chiTietPhim.KhoiChieu = phim.KhoiChieu;
                chiTietPhim.SapChieu = phim.SapChieu;

                chiTietPhim.ListTheLoai = connection.Query<TheLoaiPhim>("SELECT * FROM [dbo].[View_THE_LOAI] WHERE MaPhim = " + maPhim, commandType: CommandType.Text);

                lichChieuList = connection.Query<LichChieuVM>("SELECT * FROM [dbo].[LICHCHIEU] WHERE MaPhim = " + maPhim, commandType: CommandType.Text);

                if (lichChieuList != null)
                {
                    foreach (var ttr in lichChieuList.GroupBy(n => new { n.MaCumRap }))
                    {
                        ThongTinRap thongTinRap = new ThongTinRap();
                        thongTinRap.MaCumRap = ttr.Key.MaCumRap;
                        foreach (var lc in ttr)
                        {
                            ThongTinLichChieu thongTinLC = new ThongTinLichChieu();
                            thongTinRap.MaHeThongRap = lc.MaHeThongRap;
                            thongTinRap.TenHeThongRap = connection.QuerySingleOrDefault<string>("SELECT TenHeThongRap FROM [dbo].[HETHONGRAP] WHERE MaHeThongRap = '" + thongTinRap.MaHeThongRap + "'", commandType: CommandType.Text);
                            thongTinRap.MaCumRap = lc.MaCumRap;
                            thongTinRap.TenCumRap = connection.QuerySingleOrDefault<string>("SELECT TenCumRap FROM [dbo].[CUMRAP] WHERE MaCumRap = '" + thongTinRap.MaCumRap + "'", commandType: CommandType.Text);
                            thongTinRap.ThongTin = connection.QuerySingleOrDefault<string>("SELECT ThongTin FROM [dbo].[CUMRAP] WHERE MaCumRap = '" + thongTinRap.MaCumRap + "'", commandType: CommandType.Text);
                            thongTinLC.MaLichChieu = lc.MaLichChieu;
                            thongTinLC.MaRap = lc.MaRap;
                            thongTinLC.TenRap = connection.QuerySingleOrDefault<string>("SELECT TenRap FROM [dbo].[RAP] WHERE MaRap = " + thongTinLC.MaRap, commandType: CommandType.Text);
                            thongTinLC.NgayChieuGioChieu = lc.NgayChieuGioChieu;
                            thongTinLC.ThoiLuong = lc.ThoiLuong;
                            thongTinLC.GiaVe = lc.GiaVe;
                            thongTinRap.ChiTietLichChieu.Add(thongTinLC);
                        }

                        chiTietPhim.LichChieu.Add(thongTinRap);
                    }
                }

                var binhLuanList = connection.Query("SELECT * FROM [dbo].[BINHLUANDANHGIA] WHERE MaPhim = " + maPhim, commandType: CommandType.Text);
                if (binhLuanList != null)
                {
                    foreach (var binhLuan in binhLuanList)
                    {
                        ThongTinBinhLuan thongTinBL = new ThongTinBinhLuan();
                        thongTinBL.MaThanhVien = binhLuan.MaThanhVien;
                        thongTinBL.TenThanhVien = connection.QuerySingleOrDefault<string>("SELECT TaiKhoan FROM [dbo].[THANHVIEN] WHERE MaThanhVien = " + thongTinBL.MaThanhVien, commandType: CommandType.Text);
                        thongTinBL.NoiDungBinhLuan = binhLuan.NoiDungBinhLuan;
                        thongTinBL.ChiSoDanhGia = binhLuan.ChiSoDanhGia.ToString("0.00");
                        //thongTinBL.NgayBinhLuan = binhLuan.NgayBinhLuan;
                        chiTietPhim.ThongTinBinhLuan.Add(thongTinBL);
                    }
                }
                return chiTietPhim;
            }
        }

        public async Task<PhimViewModel> LayThongTinPhim(int maPhim)
        {
            Phim phim = null;
            using (var connection = new SqlConnection(connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@KEYWORD", maPhim);
                phim = connection.QuerySingleOrDefault<Phim>("SELECT * FROM [dbo].[PHIM] WHERE MaPhim = " + maPhim, p, commandType: CommandType.Text);
                IEnumerable<BinhLuanViewModel> listBinhLuan = connection.Query<BinhLuanViewModel>("SELECT * FROM dbo.View_BINH_LUAN WHERE MaPhim = " + maPhim, commandType: CommandType.Text);

                //PhimViewModel phimVM = Mapper.Map<Phim, PhimViewModel>(phim);
                PhimViewModel phimVM = new PhimViewModel();
                phimVM.TenPhim = phim.TenPhim;
                phimVM.BiDanh = phim.BiDanh;
                phimVM.DanhGia = phim.DanhGia;
                phimVM.HinhAnh = phim.HinhAnh;
                phimVM.MaPhim = phim.MaPhim;
                phimVM.MoTa = phim.MoTa;
                phimVM.Trailer = phim.Trailer;
                phimVM.NgayKhoiChieu = phim.NgayKhoiChieu.ToString("dd/MM/yyyy");
                phimVM.DaoDien = phim.DaoDien;
                phimVM.DienVien = phim.DienVien;
                phimVM.DoTuoi = phim.DoTuoi;
                phimVM.DaXoa = phim.DaXoa;
                phimVM.KhoiChieu = phim.KhoiChieu;
                phimVM.SapChieu = phim.SapChieu;
                phimVM.ListBinhLuan = listBinhLuan;
                return phimVM;
            }
        }

        //Lấy danh sách phim theo ngày
        public Task<object> LayDanhSachPhimTheoNgay(string keyword, int pageIndex, int pageSize, string ngay = "")
        {
            throw new System.NotImplementedException();
        }

        public async Task<object> CapNhatPhim(PhimViewModel phimUpdate)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                IEnumerable<Phim> listPhim = connection.Query<Phim>("SELECT * FROM [dbo].[PHIM]", commandType: CommandType.Text);
                var phim = listPhim.Where(n => n.MaPhim == phimUpdate.MaPhim);
                if (phim == null)
                {
                    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Mã phim không hợp lệ!");
                }
                string tenPhim = LoaiBoKyTu.bestLower(phimUpdate.TenPhim);
                if (string.IsNullOrEmpty(tenPhim))
                {
                    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Tên phim không hợp lệ!");
                }
                var p = listPhim.Where(n => n.BiDanh == tenPhim);
                if (p.Count() > 2)
                {
                    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Tên phim đã tồn tại!");
                }

                //Kiểm tra hình ảnh và trailer
                if (phimUpdate.HinhAnh.Split('.').Count() > 1)
                {
                    phimUpdate.HinhAnh = LoaiBoKyTu.bestLower(phimUpdate.TenPhim) + "." + phimUpdate.HinhAnh.Split('.')[phimUpdate.HinhAnh.Split('.').Length - 1];
                }
                else
                {
                    var response = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Hình ảnh không đúng định dạng!");
                    return response;
                }
                if (!string.IsNullOrEmpty(phimUpdate.Trailer))
                {

                    string newString = phimUpdate.Trailer.Replace("https://www.youtube.com/embed/", "♥");
                    if (newString.Split('♥').Length == 1)
                    {
                        return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Link trailer không hợp lệ link trailer phải có định dạng: https://www.youtube.com/embed/[thamso]");

                    }
                }

                DateTime temp;
                try
                {
                    try
                    {
                        temp = DateTimes.ConvertDate(phimUpdate.NgayKhoiChieu);
                    }
                    catch (Exception ex)
                    {
                        return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
                    }
                }
                catch (Exception ex)
                {
                    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Ngày khởi chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
                }

                var param = new DynamicParameters();
                param.Add("@MAPHIM", phimUpdate.MaPhim);
                param.Add("@TENPHIM", phimUpdate.TenPhim);
                param.Add("@TRAILER", phimUpdate.Trailer);
                param.Add("@HINHANH", hostName + phimUpdate.HinhAnh);
                param.Add("@MOTA", phimUpdate.MoTa);
                param.Add("@NGAYKHOICHIEU", temp);
                param.Add("@DANHGIA", phimUpdate.DanhGia);
                param.Add("@DIENVIEN", phimUpdate.DienVien);
                param.Add("@DAODIEN", phimUpdate.DaoDien);
                param.Add("@DOTUOI", phimUpdate.DoTuoi);
                param.Add("@BIDANH", phimUpdate.BiDanh);
                param.Add("@DAXOA", phimUpdate.DaXoa);
                param.Add("@KHOICHIEU", phimUpdate.KhoiChieu);
                param.Add("@SAPCHIEU", phimUpdate.SapChieu);
                await connection.QueryAsync<Phim>("PHIM_UPDATE", param, commandType: CommandType.StoredProcedure);
                return "Cập nhật phim thành công!";
            }
        }

        public async Task<object> ThemPhim(PhimInsert phim)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                string tenPhim = LoaiBoKyTu.bestLower(phim.TenPhim);
                if (string.IsNullOrEmpty(tenPhim))
                {
                    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Tên phim không hợp lệ!");
                }
                var p = connection.Query("SELECT TenPhim FROM dbo.PHIM WHERE BiDanh = '" + tenPhim + "'", commandType: CommandType.Text);

                if (p.Count() > 1)
                {
                    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Tên phim đã tồn tại!");
                }
                phim.BiDanh = LoaiBoKyTu.bestLower(phim.TenPhim);
                Phim modelInsert = new Phim();
                modelInsert.TenPhim = phim.TenPhim;
                modelInsert.HinhAnh = phim.HinhAnh;
                //DateTime temp;
                //try
                //{
                //    try
                //    {
                //        temp = DateTimes.ConvertDate(phim.NgayKhoiChieu);
                //    }
                //    catch (Exception ex)
                //    {
                //        return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
                //    }
                //}
                //catch (Exception ex)
                //{
                //    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Ngày khởi chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
                //}
                //try
                //{
                //    modelInsert.NgayKhoiChieu = DateTimes.ConvertDate(phim.NgayKhoiChieu);
                //}
                //catch (Exception ex)
                //{
                //    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Ngày chiếu không hợp lệ, Ngày chiếu phải có định dạng dd/MM/yyyy !");
                //}

                if (modelInsert.HinhAnh.Split('.').Count() > 1)
                {
                    modelInsert.HinhAnh = LoaiBoKyTu.bestLower(modelInsert.TenPhim) + "." + modelInsert.HinhAnh.Split('.')[modelInsert.HinhAnh.Split('.').Length - 1];
                }
                else
                {
                    var response = await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Hình ảnh không đúng định dạng!");
                    return response;
                }
                //Kiểm tra link trailer
                modelInsert.Trailer = phim.Trailer;
                string newString = "";
                if (!string.IsNullOrEmpty(modelInsert.Trailer))
                {
                    newString = modelInsert.Trailer.Replace("https://www.youtube.com/embed/", "♥");
                    if (newString.Split('♥').Length == 1)
                    {
                        return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Link trailer không hợp lệ link trailer phải có định dạng: https://www.youtube.com/embed/[thamso]");
                    }
                }

                var param = new DynamicParameters();
                param.Add("@TENPHIM", modelInsert.TenPhim);
                param.Add("@BIDANH", LoaiBoKyTu.bestLower(phim.TenPhim));
                param.Add("@TRAILER", modelInsert.Trailer);
                param.Add("@HINHANH", hostName + modelInsert.HinhAnh);
                param.Add("@MOTA", phim.MoTa);
                param.Add("@NGAYKHOICHIEU", phim.NgayKhoiChieu);
                param.Add("@DANHGIA", phim.DanhGia);
                param.Add("@DIENVIEN", phim.DienVien);
                param.Add("@DAODIEN", phim.DaoDien);
                param.Add("@DOTUOI", phim.DoTuoi);
                param.Add("@DAXOA", phim.DaXoa);
                param.Add("@KHOICHIEU", phim.KhoiChieu);
                param.Add("@SAPCHIEU", phim.SapChieu);

                connection.Query<Phim>("PHIM_INSERT", param, commandType: CommandType.StoredProcedure);

                return "Thêm phim thành công!";
            }
            //return newString.Split('♥').Length + " " + newString;
        }

        public async Task<object> XoaPhim(int maPhim)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                var p = new DynamicParameters();
                Phim phim = connection.QuerySingleOrDefault<Phim>("SELECT * FROM [dbo].[PHIM] WHERE MaPhim = " + maPhim, commandType: CommandType.Text);
                if (phim == null)
                {
                    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Mã phim không hợp lệ!");
                }

                IEnumerable<LichChieu> lichChieuPhim = connection.Query<LichChieu>("SELECT * FROM [dbo].[LICHCHIEU] WHERE MaPhim = " + maPhim, commandType: CommandType.Text);
                if (lichChieuPhim.Count() > 0)
                {
                    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Phim đã xếp lịch chiếu không thể xóa!");
                }

                var param = new DynamicParameters();
                param.Add("@MAPHIM", maPhim);
                await connection.QueryAsync<Phim>("PHIM_DELETE", param, commandType: CommandType.StoredProcedure);

                string key = "";
                IEnumerable<Phim> phimreturn = await this.LayDanhSachPhim(key);
                return phimreturn;
            }
        }

        public async Task<object> TimKiemPhim(int maPhim, string maCumRap, string ngayChieuGioChieu)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@MAPHIM", maPhim);
                param.Add("@MACUMRAP", maCumRap);
                param.Add("@NGAYCHIEUGIOCHIEU", ngayChieuGioChieu);
                param.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                connection.Execute("TIM_KIEM_LICH_CHIEU", param, commandType: CommandType.StoredProcedure);
                int returnValue = param.Get<int>("@ReturnValue");
                return returnValue;
            }
        }

        public async Task<object> ThemBinhLuan(BinhLuan binhLuan)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@MATHANHVIEN", binhLuan.MaThanhVien);
                param.Add("@MAPHIM", binhLuan.MaPhim);
                param.Add("@NOIDUNGBINHLUAN", binhLuan.NoiDungBinhLuan);
                param.Add("@CHISODANHGIA", binhLuan.ChiSoDanhGia);
                connection.Execute("BINH_LUAN_INSERT", param, commandType: CommandType.StoredProcedure);
            }
            return "Bình luận thành công";
        }
    }
}