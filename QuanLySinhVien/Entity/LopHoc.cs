using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace QuanLySinhVien.Data.Entity
{
    public class LopHoc
    {
        public string MaLop { get; set; } = "";
        public string TenLop { get; set; } = "";
    }
}
