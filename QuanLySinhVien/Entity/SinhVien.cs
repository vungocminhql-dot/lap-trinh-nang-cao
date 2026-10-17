using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace QuanLySinhVien.Data.Entity
{
    public class SinhVien
    {
        [Required(ErrorMessage = "Mã sinh viên không được để trống!")]
        public string MaSV { get; set; } = "";

        [Required(ErrorMessage = "Họ tên không được để trống!")]
        public string HoTen { get; set; } = "";

        public DateTime NgaySinh { get; set; } = DateTime.Today;

        public string GioiTinh { get; set; } = "Nam";

        [Required(ErrorMessage = "Email không được để trống!")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng!")]
        public string SoDienThoai { get; set; } = "";

        [Required(ErrorMessage = "Chưa chọn lớp!")]
        public string MaLop { get; set; } = "";

        [Range(0, 10, ErrorMessage = "Điểm phải từ 0 đến 10!")]
        public double Diem { get; set; }

        public List<ValidationResult> IsInValid()
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this), results, validateAllProperties: true);
            return results;
        }
    }
}
