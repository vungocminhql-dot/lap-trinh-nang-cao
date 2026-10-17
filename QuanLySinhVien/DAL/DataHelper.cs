using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace QuanLySinhVien.Data.DAL
{
    using QuanLySinhVien.Data.Entity;

    public class DataHelper
    {
        private readonly string connectionString =
            "Server=localhost;Port=3306;Database=QLSV;Uid=root;Pwd=MAT_KHAU_CUA_BAN;CharSet=utf8mb4;";

        // Mỗi lệnh dùng một connection riêng, "using" luôn đóng connection kể cả khi lỗi.
        // Không bắt lỗi ở đây: MySqlException tự ném lên tầng trên.
        public int ExecuteNonQuery(string sql, params MySqlParameter[] parameters)
        {
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, con))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(string sql, params MySqlParameter[] parameters)
        {
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, con))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                con.Open();
                return cmd.ExecuteScalar();
            }
        }

        public DataTable GetDataTable(string sql, params MySqlParameter[] parameters)
        {
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, con))
            using (var da = new MySqlDataAdapter(cmd))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}
