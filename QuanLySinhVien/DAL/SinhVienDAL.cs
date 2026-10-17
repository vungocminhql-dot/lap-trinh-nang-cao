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

    public class SinhVienDAL
    {
        private readonly DataHelper db = new DataHelper();

        private const string SelectAll =
            "SELECT MaSV, HoTen, NgaySinh, GioiTinh, Email, SoDienThoai, MaLop, Diem FROM SinhVien";

        private static SinhVien MapRow(DataRow r) => new SinhVien
        {
            MaSV = r["MaSV"].ToString(),
            HoTen = r["HoTen"].ToString(),
            NgaySinh = Convert.ToDateTime(r["NgaySinh"]),
            GioiTinh = r["GioiTinh"].ToString(),
            Email = r["Email"].ToString(),
            SoDienThoai = r["SoDienThoai"].ToString(),
            MaLop = r["MaLop"].ToString(),
            Diem = r["Diem"] == DBNull.Value ? 0 : Convert.ToDouble(r["Diem"])
        };

        private static MySqlParameter[] ToParams(SinhVien sv) => new[]
        {
            new MySqlParameter("@MaSV", sv.MaSV),
            new MySqlParameter("@HoTen", sv.HoTen),
            new MySqlParameter("@NgaySinh", sv.NgaySinh.Date),
            new MySqlParameter("@GioiTinh", sv.GioiTinh),
            new MySqlParameter("@Email", sv.Email),
            new MySqlParameter("@SoDienThoai", sv.SoDienThoai),
            new MySqlParameter("@MaLop", sv.MaLop),
            new MySqlParameter("@Diem", sv.Diem)
        };

        public List<SinhVien> GetAllSinhVien()
        {
            var list = new List<SinhVien>();
            foreach (DataRow r in db.GetDataTable(SelectAll + " ORDER BY MaSV").Rows)
                list.Add(MapRow(r));
            return list;
        }

        public SinhVien GetSinhVienById(string maSV)   // trả null nếu không có; tầng BUL sẽ chuyển thành ngoại lệ
        {
            DataTable dt = db.GetDataTable(SelectAll + " WHERE MaSV = @MaSV",
                new MySqlParameter("@MaSV", maSV));
            return dt.Rows.Count == 0 ? null : MapRow(dt.Rows[0]);
        }

        public List<SinhVien> GetSinhViensByMaLop(string maLop)
        {
            var list = new List<SinhVien>();
            DataTable dt = db.GetDataTable(SelectAll + " WHERE MaLop = @MaLop ORDER BY MaSV",
                new MySqlParameter("@MaLop", maLop));
            foreach (DataRow r in dt.Rows) list.Add(MapRow(r));
            return list;
        }

        public bool Exists(string maSV)
        {
            object o = db.ExecuteScalar("SELECT COUNT(1) FROM SinhVien WHERE MaSV = @MaSV",
                new MySqlParameter("@MaSV", maSV));
            return Convert.ToInt32(o) > 0;
        }

        public void AddSinhVien(SinhVien sv)
        {
            db.ExecuteNonQuery(
                @"INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, Email, SoDienThoai, MaLop, Diem)
                  VALUES (@MaSV, @HoTen, @NgaySinh, @GioiTinh, @Email, @SoDienThoai, @MaLop, @Diem)",
                ToParams(sv));
        }

        public void UpdateSinhVien(SinhVien sv)
        {
            int n = db.ExecuteNonQuery(
                @"UPDATE SinhVien
                  SET HoTen = @HoTen, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, Email = @Email,
                      SoDienThoai = @SoDienThoai, MaLop = @MaLop, Diem = @Diem
                  WHERE MaSV = @MaSV",
                ToParams(sv));
            if (n == 0) throw new KeyNotFoundException($"Không tìm thấy sinh viên '{sv.MaSV}' để cập nhật.");
        }

        public void DeleteSinhVien(string maSV)
        {
            int n = db.ExecuteNonQuery("DELETE FROM SinhVien WHERE MaSV = @MaSV",
                new MySqlParameter("@MaSV", maSV));
            if (n == 0) throw new KeyNotFoundException($"Không tìm thấy sinh viên '{maSV}' để xóa.");
        }
    }
}
