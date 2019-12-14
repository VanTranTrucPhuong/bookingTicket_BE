using BookingTicketRestAPICoreDapper_Data.Models;
using BookingTicketRestAPICoreDapper_Data.Models.ViewModel;
using BookingTicketRestAPICoreDapper_Utilities;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTicketRestAPICoreDapper_Data.Repositories
{
    public interface IQuanLyHeThongRapRespository
    {
        Task<IEnumerable<HeThongRap>> LayThongTinHeThongRap(string key);

        Task<object> LayThongTinLichChieuHeThongRap(string maHeThongRap);

        Task<object> LayThongTinCumRapTheoHeThongRap(string maHeThongRap);

        void ThemHeThongRap(HeThongRap heThongRap);

        void CapNhatHeThongRap(HeThongRap heThongRap);

        void XoaHeThongRap(int maHeThongRap);
    }

    public class QuanLyHeThongRapRespository : IQuanLyHeThongRapRespository
    {
        private readonly string connectionString;
        private ThongBaoLoi thongBaoLoi = new ThongBaoLoi();

        public QuanLyHeThongRapRespository(string _connectionString)
        {
            {
                this.connectionString = _connectionString;
            }
        }

        public async Task<IEnumerable<HeThongRap>> LayThongTinHeThongRap(string maHeThongRap)
        {
            IEnumerable<HeThongRap> heThongRap = null;
            using (var connection = new SqlConnection(connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@MAHETHONGRAP", maHeThongRap);
                heThongRap = connection.Query<HeThongRap>("HE_THONG_RAP_GET", p, commandType: CommandType.StoredProcedure);
                List<HeThongRap> dsHTR = new List<HeThongRap>();
                foreach (var item in heThongRap)
                {
                    HeThongRap heThongRapVM = new HeThongRap();
                    heThongRapVM.MaHeThongRap = item.MaHeThongRap;
                    heThongRapVM.TenHeThongRap = item.TenHeThongRap;
                    heThongRapVM.Logo = item.Logo;
                    heThongRapVM.BiDanh = item.BiDanh;
                    heThongRapVM.DanhSachCumRap = connection.Query<CumRap>("SELECT * FROM [dbo].[CUMRAP] WHERE MaHeThongRap = '" + item.MaHeThongRap + "'", commandType: CommandType.Text);
                    dsHTR.Add(heThongRapVM);
                }
                return dsHTR;
            }
        }

        public async Task<object> LayThongTinCumRapTheoHeThongRap(string maHeThongRap)
        {
            var connection = new SqlConnection(connectionString);
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();

            if (string.IsNullOrEmpty(maHeThongRap))
            {
                return await thongBaoLoi.thongBaoLoi(ThongBaoLoi.Loi400, "Mã hệ thống rạp không tồn tại!");
            }
            var param = new DynamicParameters();
            param.Add("@MAHETHONGRAP", maHeThongRap);

            var listCumRap = connection.Query("CUM_RAP_GET", param, commandType: CommandType.StoredProcedure);

            //IEnumerable<ThongTinCumRapViewModel> lstResult = lstCumRap.Select(n => new ThongTinCumRapVM { MaCumRap = n.MaCumRap, TenCumRap = n.TenCumRap, DiaChi = n.ThongTin, DanhSachRap = db.Rap.Where(e => e.MaCumRap == n.MaCumRap).Select(p => new RapVM { MaRap = p.MaRap, TenRap = p.TenRap }) });

            return listCumRap;
        }

        public void CapNhatHeThongRap(HeThongRap heThongRap)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@MAHETHONGRAP", heThongRap.MaHeThongRap);
                p.Add("@TENHETHONGRAP", heThongRap.TenHeThongRap);
                p.Add("@BIDANH", heThongRap.BiDanh);
                p.Add("@LOGO", heThongRap.Logo);
                connection.Query<HeThongRap>("HE_THONG_RAP_UPDATE", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void ThemHeThongRap(HeThongRap heThongRap)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@TENHETHONGRAP", heThongRap.TenHeThongRap);
                p.Add("@BIDANH", heThongRap.BiDanh);
                p.Add("@LOGO", heThongRap.Logo);
                connection.Query<HeThongRap>("HE_THONG_RAP_INSERT", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void XoaHeThongRap(int maHeThongRap)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@MAHETHONGRAP", maHeThongRap);
                connection.Query<HeThongRap>("HE_THONG_RAP_DELETE", p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<object> LayThongTinLichChieuHeThongRap(string maHeThongRap)
        {
            var connection = new SqlConnection(connectionString);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            IEnumerable<HeThongRap> heThongRapList = connection.Query<HeThongRap>("SELECT * FROM [dbo].[HETHONGRAP]");
            if (!string.IsNullOrEmpty(maHeThongRap))
            {
                var heThongRap = heThongRapList.Where(n => n.MaHeThongRap == maHeThongRap);
            }
            List<_HeThongRap> ListKetQua = new List<_HeThongRap>();

            //LichChieuMapView lstLichChieu = null;
            //if (!string.IsNullOrEmpty(maHeThongRap))
            //{
            var lichChieuList = connection.Query<LichChieuMapView>("SELECT * FROM [dbo].[LICHCHIEU] WHERE MaHeThongRap = '" + maHeThongRap + "'", commandType: CommandType.Text);
            //}

            foreach (var groupHeThongRap in lichChieuList.GroupBy(n => new { n.MaHeThongRap, n.TenHeThongRap }))
            {
                _HeThongRap htr = new _HeThongRap();
                htr.MaHeThongRap = groupHeThongRap.Key.MaHeThongRap;
                htr.TenHeThongRap = connection.QuerySingleOrDefault<string>("SELECT TenHeThongRap FROM [dbo].[HETHONGRAP] WHERE MaHeThongRap = '" + htr.MaHeThongRap + "'", commandType: CommandType.Text);
                foreach (var cumRap in groupHeThongRap.GroupBy(n => new { n.MaCumRap, n.TenCumRap, n.ThongTin }))
                {
                    _CumRap _cumRap = new _CumRap();
                    _cumRap.MaCumRap = cumRap.Key.MaCumRap;
                    //var chiTietCumRap = connection.QuerySingleOrDefault<string>("SELECT TenCumRap, Thong T FROM [dbo].[CUMRAP] WHERE MaCumRap = " + _cumRap.MaCumRap, commandType: CommandType.Text);
                    _cumRap.TenCumRap = connection.QuerySingleOrDefault<string>("SELECT TenCumRap FROM [dbo].[CUMRAP] WHERE MaCumRap = '" + _cumRap.MaCumRap + "'", commandType: CommandType.Text);
                    _cumRap.ThongTin = connection.QuerySingleOrDefault<string>("SELECT ThongTin FROM [dbo].[CUMRAP] WHERE MaCumRap = '" + _cumRap.MaCumRap + "'", commandType: CommandType.Text);
                    foreach (var lichChieu in cumRap.GroupBy(n => new { n.MaPhim, n.TenPhim }))
                    {
                        _LichChieuTheoPhim _lichChieu = new _LichChieuTheoPhim();
                        _lichChieu.MaPhim = lichChieu.Key.MaPhim;
                        var phim = connection.QuerySingleOrDefault<Phim>("SELECT * FROM [dbo].[PHIM] WHERE MaPhim = " + _lichChieu.MaPhim, commandType: CommandType.Text);
                        _lichChieu.TenPhim = phim.TenPhim;
                        _lichChieu.HinhAnh = phim.HinhAnh;
                        _lichChieu.MoTa = phim.MoTa;
                        foreach (var lc in lichChieu)
                        {
                            _LichChieu _lc = new _LichChieu();
                            _lc.ThoiGianChieu = lc.NgayChieuGioChieu;
                            _lc.MaLichChieu = lc.MaLichChieu;
                            _lc.MaRap = lc.MaRap;
                            _lc.TenRap = connection.QuerySingleOrDefault<string>("SELECT TenRap FROM [dbo].[RAP] WHERE MaRap = " + _lc.MaRap, commandType: CommandType.Text);
                            _lc.GiaVe = lc.GiaVe;
                            _lichChieu.ListNgayChieuGioChieu.Add(_lc);
                        }
                        _cumRap.ListPhim.Add(_lichChieu);
                    }
                    htr.ListCumRap.Add(_cumRap);
                }
                ListKetQua.Add(htr);
            }
            connection.Close();
            return ListKetQua;
        }
    }
}