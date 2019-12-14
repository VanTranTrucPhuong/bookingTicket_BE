using BookingTicketRestAPICoreDapper_Data.Models.ViewModel;
using BookingTicketRestAPICoreDapper_Utilities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace BookingTicketRestAPICoreDapper_Data.Repositories
{
    public interface IDashboardRespository
    {
        //Đặt vé, Lấy danh sách phòng vé, Tạo Lịch chiếu
        Task<object> ThongKeVeTheoHTR(DateTime NGAYBD, DateTime NGAYKT, string MaHeThongRap);
        Task<object> ThongKeVeTheoNam(int nam, string maHeThongRap);
        Task<object> ThongKeDoanhThuTheoHTR(DateTime NGAYBD, DateTime NGAYKT, string MaHeThongRap);
        Task<object> ThongKeDoanhThuTheoNam(int nam, string maHeThongRap);


    }

    public class DashboardRespository : IDashboardRespository
    {
        private readonly string connectionString;
        private ThongBaoLoi thongBaoLoi = new ThongBaoLoi();

        public DashboardRespository(string _connectionString)
        {
            {
                this.connectionString = _connectionString;
            }
        }

        public async Task<object> ThongKeDoanhThuTheoHTR(DateTime NGAYBD, DateTime NGAYKT, string MaHeThongRap)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@NGAYBD", NGAYBD);
                param.Add("@NGAYKT", NGAYKT);
                param.Add("@MAHETHONGRAP", MaHeThongRap);
                var danhSachThongKe = connection.Query("THONG_KE_DOANH_THU", param, commandType: CommandType.StoredProcedure);
                return danhSachThongKe;
            }
        }

        public async Task<object> ThongKeDoanhThuTheoNam(int nam, string maHeThongRap)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@NAM", nam);
                param.Add("@MAHETHONGRAP", maHeThongRap);
                var danhSachThongKe = connection.Query("THONG_KE_DOANH_THU_THEO_NAM", param, commandType: CommandType.StoredProcedure);
                return danhSachThongKe;
            }
        }

        public async Task<object> ThongKeVeTheoHTR(DateTime NGAYBD, DateTime NGAYKT, string MaHeThongRap)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@NGAYBD", NGAYBD);
                param.Add("@NGAYKT", NGAYKT);
                param.Add("@MAHETHONGRAP", MaHeThongRap);
                var danhSachThongKe = connection.Query("THONG_KE_SO_LUONG_VE", param, commandType: CommandType.StoredProcedure);
                return danhSachThongKe;
            }
        }

        public async Task<object> ThongKeVeTheoNam(int nam, string maHeThongRap)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@NAM", nam);
                param.Add("@MAHETHONGRAP", maHeThongRap);
                var danhSachThongKe = connection.Query("THONG_KE_SO_LUONG_VE_THEO_NAM", param, commandType: CommandType.StoredProcedure);
                return danhSachThongKe;
            }
        }
    }
}
