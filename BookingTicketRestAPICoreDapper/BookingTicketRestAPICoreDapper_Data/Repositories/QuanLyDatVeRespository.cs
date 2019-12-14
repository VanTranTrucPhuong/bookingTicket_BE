using BookingTicketRestAPICoreDapper_Data.Models;
using BookingTicketRestAPICoreDapper_Data.Models.ViewModel;
using BookingTicketRestAPICoreDapper_Utilities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTicketRestAPICoreDapper_Data.Repositories
{
    public interface IQuanLyDatVeRespository
    {
        //Đặt vé, Lấy danh sách phòng vé, Tạo Lịch chiếu
        Task<object> DatVe(DanhSachVeDatViewModel danhSachVeDat);

        Task<object> HuyVe(int MaThanhToan, int mucHoanTien);

        Task<object> LayDanhSachPhongVe(int maLichChieu);

        Task<object> PutDanhSachVeDangDat(DanhSachVeDangDatVM danhSachGheDangDat);

        Task<IEnumerable<LichChieuVM>> LayDanhSachLichChieu();

        Task<object> TaoLichChieu(LichChieuInsertVM lichChieuInsert);
    }

    public class QuanLyDatVeRespository : IQuanLyDatVeRespository
    {
        private readonly string connectionString;
        private ThongBaoLoi thongBaoLoi = new ThongBaoLoi();

        public QuanLyDatVeRespository(string _connectionString)
        {
            {
                this.connectionString = _connectionString;
            }
        }

        //Đặt vé
        public async Task<object> DatVe(DanhSachVeDatViewModel danhSachVeDat)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var thanhVien = connection.QuerySingleOrDefault("SELECT TaiKhoan FROM [dbo].[THANHVIEN] " +
                    "WHERE TaiKhoan = '" + danhSachVeDat.TaiKhoan + "'", commandType: CommandType.Text);

                var maThanhVien = connection.QuerySingleOrDefault<int>("SELECT MaThanhVien FROM [dbo].[THANHVIEN] " +
                    "WHERE TaiKhoan = '" + danhSachVeDat.TaiKhoan + "'", commandType: CommandType.Text);

                if (thanhVien == null)
                {
                    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Tài khoản người dùng không tồn tại!");
                }
                if (danhSachVeDat.DanhSachVe == null)
                {
                    return "Danh sách vé rỗng!";
                }
                else
                {
                    var p = new DynamicParameters();
                    p.Add("@TONGTIEN", danhSachVeDat.TongTien);
                    p.Add("@MALICHCHIEU", danhSachVeDat.MaLichChieu);
                    p.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                    connection.Execute("THANH_TOAN_INSERT", p, commandType: CommandType.StoredProcedure);
                    int returnValue = p.Get<int>("@ReturnValue");

                    foreach (var veDat in danhSachVeDat.DanhSachVe)
                    {
                        // Ve ve = new Ve();
                        var param = new DynamicParameters();
                        param.Add("@MATHANHVIEN", maThanhVien);
                        param.Add("@MAGHE", veDat.MaGhe);
                        param.Add("@MATHANHTOAN", returnValue);
                        param.Add("@MALOAIGHE", 1);
                        param.Add("@NGAYDAT", DateTime.Now);
                        param.Add("@GIAVE", veDat.GiaVe);
                        param.Add("@MALICHCHIEU", danhSachVeDat.MaLichChieu);
                        param.Add("@CHIETKHAU", "0.5");
                        await connection.QueryAsync<Ve>("VE_INSERT", param, commandType: CommandType.StoredProcedure);
                    }
                    //TheaterHub.FetchTheater();
                    return returnValue;
                    //return returnValue;
                }
            }
        }

        public async Task<object> HuyVe(int maThanhToan, int mucHoanTien)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var thanhToan = connection.QuerySingleOrDefault("SELECT * FROM [dbo].[ThanhToan] WHERE MaThanhToan = " + maThanhToan, commandType: CommandType.Text);

                if (thanhToan == null)
                {
                    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Mã thanh toán không hợp lệ!");
                }
                var param = new DynamicParameters();
                param.Add("@MATHANHTOAN", maThanhToan);
                param.Add("@MUCHOANTIEN", mucHoanTien);
                connection.Execute("VE_HUY", param, commandType: CommandType.StoredProcedure);
            }
            return "Yêu cầu hủy vé được chấp nhận!";
        }

        public async Task<IEnumerable<LichChieuVM>> LayDanhSachLichChieu()
        {
            var connection = new SqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            IEnumerable<LichChieuVM> lc = connection.Query<LichChieuVM>("SELECT * FROM dbo.View_LICH_CHIEU", commandType: CommandType.Text);
            return lc;
        }

        //Lấy danh sách phòng vé
        public async Task<object> LayDanhSachPhongVe(int maLichChieu)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                if (maLichChieu == 0)
                {
                    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Mã lịch chiếu không hợp lệ!");
                }
                LichChieu lichChieu = connection.QuerySingleOrDefault<LichChieu>("SELECT * FROM [dbo].[LICHCHIEU] " +
                    "WHERE MaLichChieu = " + maLichChieu, commandType: CommandType.Text);
                if (lichChieu == null)
                {
                    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Mã lịch chiếu không tồn tại!");
                }
                decimal giaVe = lichChieu.GiaVe;

                var listDanhSachGheDuocDat = connection.Query<Ve>("SELECT * FROM [dbo].[VE] " +
                    "WHERE MaLichChieu = " + maLichChieu, commandType: CommandType.Text);

                bool flag = false;

                LichChieuTheoRapViewModel rap = new LichChieuTheoRapViewModel();
                List<GheTheoHangVM> listGhe = new List<GheTheoHangVM>();
                int? MaRap = lichChieu.MaRap;

                IEnumerable<Ghe> listGheTheoRap = connection.Query<Ghe>("SELECT * FROM [dbo].[GHE] WHERE MaRap = " + lichChieu.MaRap, commandType: CommandType.Text);
                IEnumerable<LoaiGhe> listLoaiGhe = connection.Query<LoaiGhe>("SELECT * FROM dbo.LOAIGHE", commandType: CommandType.Text);
                IEnumerable<ThanhVien> listThanhVien = connection.Query<ThanhVien>("SELECT * FROM dbo.THANHVIEN", commandType: CommandType.Text);
                foreach (var phongRap in listGheTheoRap.GroupBy(n => new { n.TenHang })) //Lấy ra rạp đang chiếu
                {
                    GheTheoHangVM gheTheoHang = new GheTheoHangVM();
                    gheTheoHang.TenHang = phongRap.Key.TenHang;
                    foreach (var pr in phongRap)
                    {
                        flag = false;
                        GheViewModel ghe = new GheViewModel();
                        LoaiGhe loaiGhe = new LoaiGhe();
                        loaiGhe = listLoaiGhe.Single(n => n.MaLoaiGhe == pr.MaLoaiGhe);
                        ghe.MaRap = pr.MaRap;
                        // (phongRap.MaLoaiGheNavigation.ChietKhau * giaVe) / 100 +
                        int phuThu = loaiGhe.PhuThu;
                        ghe.GiaVe = (phuThu * giaVe) / 100 + giaVe;
                        ghe.LoaiGhe = loaiGhe.TenLoaiGhe;
                        ghe.MaGhe = pr.MaGhe;
                        ghe.TenGhe = pr.TenGhe;
                        ghe.STT = pr.SoThuTu;
                        ghe.PhuThu = phuThu;
                        foreach (var rapDatVe in listDanhSachGheDuocDat) //Lấy ra danh sách ghế được đặt
                        {
                            //ghe.TaiKhoanNguoiDat= connection.QuerySingleOrDefault<string>("SELECT TaiKhoan FROM [dbo].[THANHVIEN] WHERE TaiKhoan = '" + rapDatVe.TaiKhoan + "'", commandType: CommandType.Text);

                            if (pr.MaGhe == rapDatVe.MaGhe && !rapDatVe.TrangThaiHuy)
                            {
                                ghe.TaiKhoanNguoiDat = listThanhVien.Single(n => n.MaThanhVien == rapDatVe.MaThanhVien).ToString();
                                flag = true;
                            }
                        }
                        if (flag == true)
                        {
                            ghe.DaDat = true;
                        }
                        gheTheoHang.DanhSachGheTheoHang.Add(ghe);
                    }
                    listGhe.Add(gheTheoHang);
                }

                rap.DanhSachGhe = listGhe;
                rap.ThongTinPhim.NgayChieu = lichChieu.NgayChieuGioChieu.ToString("dd/MM/yyyy");
                rap.ThongTinPhim.GioChieu = lichChieu.NgayChieuGioChieu.ToString("hh:mm");
                //rap.ThongTinPhim.hinhAnh = DomainImage + LichChieu.MaPhimNavigation.HinhAnh;
                rap.ThongTinPhim.MaLichChieu = maLichChieu;
                rap.ThongTinPhim.TenRap = connection.QuerySingleOrDefault<string>("SELECT TenRap FROM [dbo].[RAP] WHERE MaRap = '" + lichChieu.MaRap + "'", commandType: CommandType.Text);
                rap.ThongTinPhim.TenCumRap = connection.QuerySingleOrDefault<string>("SELECT TenCumRap FROM [dbo].[CUMRAP] WHERE MaCumRap = '" + lichChieu.MaCumRap + "'", commandType: CommandType.Text);
                rap.ThongTinPhim.DiaChi = connection.QuerySingleOrDefault<string>("SELECT ThongTin FROM [dbo].[CUMRAP] WHERE MaCumRap = '" + lichChieu.MaCumRap + "'", commandType: CommandType.Text);
                var phim = connection.QuerySingleOrDefault<Phim>("SELECT * FROM [dbo].[PHIM] WHERE MaPhim = '" + lichChieu.MaPhim + "'", commandType: CommandType.Text);
                rap.ThongTinPhim.TenPhim = phim.TenPhim;
                rap.ThongTinPhim.DoTuoi = phim.DoTuoi;
                return rap;
            }
        }

        public async Task<object> PutDanhSachVeDangDat(DanhSachVeDangDatVM danhSachGheDangDat)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@TENTAIKHOAN", danhSachGheDangDat.TaiKhoan);
                param.Add("@DANHSACHGHE", danhSachGheDangDat.DanhSachGhe);
                param.Add("@MALICHCHIEU", danhSachGheDangDat.MaLichChieu);
                connection.Execute("PUT_DS_GHE_DANG_DAT", param, commandType: CommandType.StoredProcedure);
                return "Thêm danh sách ghế đang đặt thành công!";
            }
        }

        public async Task<object> TaoLichChieu(LichChieuInsertVM lichChieuInsert)
        {
            var connection = new SqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            Phim phim = connection.QuerySingleOrDefault<Phim>("SELECT * FROM [dbo].[PHIM] WHERE MaPhim = " + lichChieuInsert.MaPhim, commandType: CommandType.Text);

            if (phim == null)
            {
                return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Mã phim không hợp lệ!");
            }

            LichChieu lichModel = new LichChieu();
            //DateTime temp;
            //try
            //{
            //    if (DateTime.TryParse(lichChieuInsert.NgayChieuGioChieu, out temp))
            //    {
            //        lichModel.NgayChieuGioChieu = DateTimes.ConvertDateHour(lichChieuInsert.NgayChieuGioChieu);
            //    }
            //    else
            //    {
            //        return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Ngày chiếu giờ chiếu không hợp lệ (có định dạng dd/MM/yyyy hh:mm:ss) !");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Ngày chiếu giờ chiếu  không hợp lệ (có định dạng dd/MM/yyyy hh:mm:ss) !");
            //}
            lichModel.NgayChieuGioChieu = lichChieuInsert.NgayChieuGioChieu;

            Rap kiemTraRap = connection.QuerySingleOrDefault<Rap>("SELECT * FROM [dbo].[RAP] WHERE MaRap = " + lichChieuInsert.MaRap, commandType: CommandType.Text);

            if (kiemTraRap == null)
            {
                return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Mã rạp không tồn tại !");
            }

            CumRap kiemTraCumRap = connection.QuerySingleOrDefault<CumRap>("SELECT * FROM [dbo].[CUMRAP] WHERE MaCumRap = '" + kiemTraRap.MaCumRap + "'", commandType: CommandType.Text);
            if (kiemTraCumRap == null)
            {
                return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Mã cụm rạp không tồn tại !");
            }

            HeThongRap kiemTraHeThongRap = connection.QuerySingleOrDefault<HeThongRap>("SELECT * FROM [dbo].[HETHONGRAP] WHERE MaHeThongRap = '" + kiemTraCumRap.MaHeThongRap + "'", commandType: CommandType.Text);
            if (kiemTraHeThongRap == null)
            {
                return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Mã hệ thống rạp không tồn tại !");
            }

            var listLichChieu = connection.Query<LichChieu>("SELECT * FROM [dbo].[LICHCHIEU]", commandType: CommandType.Text);
            var kiemTraLichChieu = listLichChieu.Where(n => n.NgayChieuGioChieu.Date == lichModel.NgayChieuGioChieu.Date && n.MaPhim == phim.MaPhim && n.MaCumRap == kiemTraCumRap.MaCumRap && n.MaHeThongRap == kiemTraHeThongRap.MaHeThongRap && n.MaRap == lichChieuInsert.MaRap);

            if (kiemTraLichChieu.Count() > 0)
            {
                return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Lịch chiếu đã bị trùng !");
            }

            if (lichChieuInsert.GiaVe > 200000 || lichChieuInsert.GiaVe < 75000)
            {
                return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi500, "Giá từ 75.000 - 200.000");
            }
            //Lấy mã rạp ngẫu nhiên không có trong lst đó

            var param = new DynamicParameters();
            param.Add("@MARAP", lichChieuInsert.MaRap);
            param.Add("@MAPHIM", lichChieuInsert.MaPhim);
            param.Add("@NGAYCHIEUGIOCHIEU", lichModel.NgayChieuGioChieu);
            param.Add("@GIAVE", lichChieuInsert.GiaVe);
            param.Add("@MAHETHONGRAP", kiemTraHeThongRap.MaHeThongRap);
            param.Add("@MACUMRAP", kiemTraCumRap.MaCumRap);

            await connection.QueryAsync("LICH_CHIEU_INSERT", param, commandType: CommandType.StoredProcedure);
            return "Thêm lịch chiếu thành công !";
            //return lichModel.NgayChieuGioChieu.Date;
        }

        //Lấy đưa danh sách vé đang đặt 

    }
}