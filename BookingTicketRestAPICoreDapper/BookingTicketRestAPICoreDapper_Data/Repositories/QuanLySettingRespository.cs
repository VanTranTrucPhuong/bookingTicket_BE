using BookingTicketRestAPICoreDapper_Data.Models;
using BookingTicketRestAPICoreDapper_Data.Models.ViewModel;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace BookingTicketRestAPICoreDapper_Data.Repositories
{
    public interface IQuanLySettingRespository
    {
        Task<Object> LayDanhSachSetting(string maSetting);
    }

    public class QuanLySettingRespository : IQuanLySettingRespository
    {
        private readonly string connectionString;
        public QuanLySettingRespository(string _connectionString)
        {
            {
                this.connectionString = _connectionString;
            }
        }

        public async Task<Object> LayDanhSachSetting(string maSetting)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var listSetting = await connection.QueryAsync("SELECT * FROM [dbo].[SETTING] WHERE MaSetting = '" + maSetting + "'", commandType: CommandType.Text);
                return listSetting;
            }
        }
    }
}
