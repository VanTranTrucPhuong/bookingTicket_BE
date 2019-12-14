using BookingTicketRestAPICoreDapper_Data.Models;
using BookingTicketRestAPICoreDapper_Data.Models.ViewModel;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTicketRestAPICoreDapper_Data.Repositories
{
    public interface ICumRapRespository
    {
        Task<IEnumerable<CumRapViewModel>> LayDanhSachCumRapTheoHeThong(string maHeThongRap);

        Task<IEnumerable<CumRap>> LayThongTinCumRap(string key);

        Task<ThongTinCumRapViewModel> LayDanhSachRap(string maCumRap);

        Task<object> ThemGhe(List<GheInsert> mangGheInsert);

        Task<object> LayDanhSachGheTheoRap(int maRap);

        void ThemCumRap(CumRap cumRap);

        void CapNhatCumRap(CumRap cumRap);

        void XoaCumRap(int maCumRap);
    }

    public class CumRapRespository : ICumRapRespository
    {
        private readonly string connectionString;

        public CumRapRespository(string _connectionString)
        {
            {
                this.connectionString = _connectionString;
            }
        }

        public async Task<IEnumerable<CumRap>> LayThongTinCumRap(string key)
        {
            IEnumerable<CumRap> cumRap = null;
            using (var connection = new SqlConnection(connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@KEYWORD", key);
                cumRap = connection.Query<CumRap>("CUM_RAP_GET", p, commandType: CommandType.StoredProcedure);
            }
            return cumRap;
        }

        public void CapNhatCumRap(CumRap cumRap)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@MACUMRAP", cumRap.MaCumRap);
                p.Add("@TENCUMRAP", cumRap.TenCumRap);
                p.Add("@THONGTIN", cumRap.ThongTin);
                p.Add("@MAHETHONGRAP", cumRap.MaHeThongRap);
                connection.Query<CumRap>("CUM_RAP_UPDATE", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void ThemCumRap(CumRap cumRap)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@TENCUMRAP", cumRap.TenCumRap);
                p.Add("@THONGTIN", cumRap.ThongTin);
                p.Add("@MAHETHONGRAP", cumRap.MaHeThongRap);
                connection.Query<CumRap>("CUM_RAP_INSERT", p, commandType: CommandType.StoredProcedure);
            }
        }

        public void XoaCumRap(int maCumRap)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var p = new DynamicParameters();
                p.Add("@MACUMRAP", maCumRap);
                connection.Query<CumRap>("CUM_RAP_DELETE", p, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<CumRapViewModel>> LayDanhSachCumRapTheoHeThong(string maHeThongRap)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                IEnumerable<CumRap> listCumRap = connection.Query<CumRap>("SELECT * FROM [dbo].[CUMRAP] WHERE MaHeThongRap = '" + maHeThongRap + "'", commandType: CommandType.Text);
                List<CumRapViewModel> DSRap = new List<CumRapViewModel>();

                foreach (var item in listCumRap)
                {
                    CumRapViewModel cumRap = new CumRapViewModel();

                    cumRap.MaCumRap = item.MaCumRap;
                    cumRap.TenCumRap = item.TenCumRap;
                    cumRap.ThongTin = item.ThongTin;
                    cumRap.DanhSachRap = connection.Query<Rap>("SELECT * FROM [dbo].[RAP] WHERE MaCumRap = '" + item.MaCumRap + "'", commandType: CommandType.Text);
                    DSRap.Add(cumRap);
                }

                return DSRap;  
            }
        }

        public async Task<ThongTinCumRapViewModel> LayDanhSachRap(string maCumRap)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                CumRap cumRap = connection.QuerySingleOrDefault<CumRap>("SELECT * FROM [dbo].[CUMRAP] WHERE MaCumRap = '" + maCumRap + "'", commandType: CommandType.Text);
                IEnumerable<Rap> listRap = connection.Query<Rap>("SELECT * FROM [dbo].[RAP] WHERE MaCumRap = '" + maCumRap + "'", commandType: CommandType.Text);
                List<RapViewModel> DSRap = new List<RapViewModel>();
                ThongTinCumRapViewModel cumRapVM = new ThongTinCumRapViewModel();
                cumRapVM.MaCumRap = maCumRap;
                cumRapVM.TenCumRap = cumRap.TenCumRap;
                cumRapVM.DiaChi = cumRap.ThongTin;
                cumRapVM.DanhSachRap = connection.Query<RapViewModel>("SELECT * FROM [dbo].[RAP] WHERE MaCumRap = '" + maCumRap + "'", commandType: CommandType.Text);

                return cumRapVM;
            }
        }

        public async Task<object> LayDanhSachGheTheoRap(int maRap)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                List<GheTheoHangVM> listGhe = new List<GheTheoHangVM>();
                RapViewModel_DSGhe rapVM = new RapViewModel_DSGhe();
                var listGheTheoRap = connection.Query<Ghe>("SELECT * FROM [dbo].[GHE] WHERE MaRap = " + maRap, commandType: CommandType.Text);
                foreach (var phongRap in listGheTheoRap.GroupBy(n => new { n.TenHang })) //Lấy ra rạp đang chiếu
                {
                    GheTheoHangVM gheTheoHang = new GheTheoHangVM();
                    gheTheoHang.TenHang = phongRap.Key.TenHang;
                    foreach (var pr in phongRap)
                    {
                        GheViewModel ghe = new GheViewModel();
                        LoaiGhe loaiGhe = new LoaiGhe();
                        loaiGhe = connection.QuerySingleOrDefault<LoaiGhe>("SELECT * FROM dbo.LOAIGHE WHERE MaLoaiGhe = " + pr.MaLoaiGhe, commandType: CommandType.Text);
                        ghe.MaRap = maRap;
                        // (phongRap.MaLoaiGheNavigation.ChietKhau * giaVe) / 100 +
                        int phuThu = loaiGhe.PhuThu;
                        ghe.LoaiGhe = loaiGhe.TenLoaiGhe;
                        ghe.MaGhe = pr.MaGhe;
                        ghe.TenGhe = pr.TenGhe;
                        ghe.STT = pr.SoThuTu;
                        ghe.PhuThu = phuThu;                       
                        gheTheoHang.DanhSachGheTheoHang.Add(ghe);
                    }
                    listGhe.Add(gheTheoHang);
                }

                rapVM.DanhSachGhe = listGhe;
                return rapVM;
            }
        }

        public async Task<object> ThemGhe(List<GheInsert> mangGheInsert)
        {
            //using (var connection = new SqlConnection(connectionString))
            //{
            //    foreach (var item in mangGheInsert)
            //    {
            //        // Ve ve = new Ve();
            //        var param = new DynamicParameters();
            //        param.Add("@MARAP", item.MaRap);
            //        param.Add("@TENHANG", item.TenHang);
            //        param.Add("@SOLUONGGHE", item.SoLuongGhe);
            //        await connection.Execute("GHE_INSERT_THEO_HANG", param, commandType: CommandType.StoredProcedure);
            //    }
            //}
            return "";
        }
    }
}