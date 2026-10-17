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

    public class LopDAL
    {
        private readonly DataHelper db = new DataHelper();

        private static LopHoc MapRow(DataRow r) => new LopHoc
        {
            MaLop = r["MaLop"].ToString(),
            TenLop = r["TenLop"].ToString()
        };

        public List<LopHoc> GetAllLopHoc()
        {
            var list = new List<LopHoc>();
            foreach (DataRow r in db.GetDataTable("SELECT MaLop, TenLop FROM LopHoc ORDER BY MaLop").Rows)
                list.Add(MapRow(r));
            return list;
        }

        public bool Exists(string maLop)
        {
            object o = db.ExecuteScalar("SELECT COUNT(1) FROM LopHoc WHERE MaLop = @MaLop",
                new MySqlParameter("@MaLop", maLop));
            return Convert.ToInt32(o) > 0;
        }

        public void AddLopHoc(LopHoc lop)
        {
            db.ExecuteNonQuery("INSERT INTO LopHoc (MaLop, TenLop) VALUES (@MaLop, @TenLop)",
                new MySqlParameter("@MaLop", lop.MaLop),
                new MySqlParameter("@TenLop", lop.TenLop));
        }

        public void UpdateLopHoc(LopHoc lop)
        {
            int n = db.ExecuteNonQuery("UPDATE LopHoc SET TenLop = @TenLop WHERE MaLop = @MaLop",
                new MySqlParameter("@TenLop", lop.TenLop),
                new MySqlParameter("@MaLop", lop.MaLop));
            if (n == 0) throw new KeyNotFoundException($"Không tìm thấy lớp '{lop.MaLop}' để cập nhật.");
        }

        public void DeleteLopHoc(string maLop)
        {
            int n = db.ExecuteNonQuery("DELETE FROM LopHoc WHERE MaLop = @MaLop",
                new MySqlParameter("@MaLop", maLop));
            if (n == 0) throw new KeyNotFoundException($"Không tìm thấy lớp '{maLop}' để xóa.");
        }
    }
}
