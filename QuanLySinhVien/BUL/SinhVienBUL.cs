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

    public class SinhVienBUL
    {
        private readonly SinhVienDAL svd = new SinhVienDAL();
        private readonly LopDAL lopDAL = new LopDAL();

        public List<SinhVien> GetAllSinhVien() => svd.GetAllSinhVien();

        public List<SinhVien> GetSinhViensByMaLop(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop)) throw new ArgumentException("Mã lớp không được để trống!");
            return svd.GetSinhViensByMaLop(maLop);
        }

        public SinhVien GetSinhVienById(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV)) throw new ArgumentException("Mã sinh viên không được để trống!");
            return svd.GetSinhVienById(maSV)
                   ?? throw new KeyNotFoundException($"Không tìm thấy sinh viên '{maSV}'!");
        }

        private void Validate(SinhVien sv)
        {
            if (sv == null) throw new ArgumentNullException(nameof(sv));

            var errors = sv.IsInValid();
            if (errors.Count > 0)
                throw new ArgumentException(string.Join(Environment.NewLine, errors.Select(x => x.ErrorMessage)));

            if (sv.NgaySinh.Date > DateTime.Today)
                throw new ArgumentException("Ngày sinh không được lớn hơn ngày hiện tại!");

            if (!lopDAL.Exists(sv.MaLop))
                throw new InvalidOperationException($"Lớp '{sv.MaLop}' không tồn tại!");
        }

        public void AddSinhVien(SinhVien sv)
        {
            Validate(sv);
            if (svd.Exists(sv.MaSV))
                throw new InvalidOperationException("Mã sinh viên đã tồn tại!");
            svd.AddSinhVien(sv);
        }

        public void UpdateSinhVien(SinhVien sv)
        {
            Validate(sv);
            svd.UpdateSinhVien(sv);
        }

        public void DeleteSinhVien(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV)) throw new ArgumentException("Mã sinh viên không được để trống!");
            svd.DeleteSinhVien(maSV);
        }
    }
}
