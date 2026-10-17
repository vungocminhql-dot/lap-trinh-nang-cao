using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace QuanLySinhVien.BUL
{
    using QuanLySinhVien.Data.DAL;
    using QuanLySinhVien.Data.Entity;

    public class LopBus
    {
        private readonly LopDAL lopDAL = new LopDAL();
        private readonly SinhVienDAL svDAL = new SinhVienDAL();

        public List<LopHoc> GetAllLopHoc() => lopDAL.GetAllLopHoc();

        private static void Validate(LopHoc lop)
        {
            if (lop == null) throw new ArgumentNullException(nameof(lop));
            if (string.IsNullOrWhiteSpace(lop.MaLop)) throw new ArgumentException("Mã lớp không được để trống!");
            if (string.IsNullOrWhiteSpace(lop.TenLop)) throw new ArgumentException("Tên lớp không được để trống!");
        }

        public void AddLopHoc(LopHoc lop)
        {
            Validate(lop);
            if (lopDAL.Exists(lop.MaLop))
                throw new InvalidOperationException($"Mã lớp '{lop.MaLop}' đã tồn tại!");
            lopDAL.AddLopHoc(lop);
        }

        public void UpdateLopHoc(LopHoc lop)
        {
            Validate(lop);
            lopDAL.UpdateLopHoc(lop);
        }

        public void DeleteLopHoc(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop)) throw new ArgumentException("Mã lớp không được để trống!");
            if (svDAL.GetSinhViensByMaLop(maLop).Count > 0)
                throw new InvalidOperationException("Lớp còn sinh viên, không thể xóa!");
            lopDAL.DeleteLopHoc(maLop);
        }
    }
}
