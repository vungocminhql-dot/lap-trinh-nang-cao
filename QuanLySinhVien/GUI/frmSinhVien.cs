using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace QuanLySinhVien.GUI
{
    using QuanLySinhVien.BUL;
    using QuanLySinhVien.Data.Entity;

    public class frmSinhVien : Form
    {
        private readonly LopBus lopBus = new LopBus();
        private readonly SinhVienBUL svb = new SinhVienBUL();

        private readonly ComboBox cboLop = new ComboBox();
        private readonly TextBox txtMaSV = new TextBox();
        private readonly TextBox txtHoTen = new TextBox();
        private readonly TextBox txtEmail = new TextBox();
        private readonly TextBox txtDienThoai = new TextBox();
        private readonly DateTimePicker dtpNgaySinh = new DateTimePicker();
        private readonly NumericUpDown numDiem = new NumericUpDown();
        private readonly RadioButton rdoNam = new RadioButton();
        private readonly RadioButton rdoNu = new RadioButton();
        private readonly CheckBox chkLocTheoLop = new CheckBox();
        private readonly DataGridView dgvSinhVien = new DataGridView();

        private readonly Button butThem = new Button();
        private readonly Button butSua = new Button();
        private readonly Button butTim = new Button();
        private readonly Button butXoa = new Button();
        private readonly Button butLamMoi = new Button();

        public frmSinhVien()
        {
            Text = "QUẢN LÝ SINH VIÊN";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1000;
            Height = 700;
            MinimumSize = new System.Drawing.Size(850, 600);

            TaoGiaoDien();

            Load += frmSinhVien_Load;
            butThem.Click += butThem_Click;
            butSua.Click += butSua_Click;
            butTim.Click += butTim_Click;
            butXoa.Click += butXoa_Click;
            butLamMoi.Click += (s, e) => LamMoi();
            cboLop.SelectedIndexChanged += (s, e) => { if (chkLocTheoLop.Checked) HienThiDanhSach(); };
            chkLocTheoLop.CheckedChanged += (s, e) => HienThiDanhSach();
            dgvSinhVien.CellClick += dgvSinhVien_CellClick;
        }

        // ---------- Xử lý ngoại lệ tập trung ----------
        private void HienLoi(Exception ex)
        {
            switch (ex)
            {
                case ArgumentException _:      // gồm cả ArgumentNullException
                    MessageBox.Show(ex.Message, "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case KeyNotFoundException _:
                    MessageBox.Show(ex.Message, "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case InvalidOperationException _:
                    MessageBox.Show(ex.Message, "Không thể thực hiện", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case MySqlException _:
                    MessageBox.Show("Lỗi cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    MessageBox.Show("Lỗi không xác định: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        // ---------- Dựng giao diện ----------
        private void TaoGiaoDien()
        {
            var lblTieuDe = new Label
            {
                Text = "QUẢN LÝ SINH VIÊN",
                Left = 20, Top = 15, Width = 920, Height = 35,
                Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            Controls.Add(lblTieuDe);

            ThemNhan("Mã sinh viên:", 25, 75);
            ThemControl(txtMaSV, 145, 72, 250);

            ThemNhan("Họ và tên:", 25, 115);
            ThemControl(txtHoTen, 145, 112, 250);

            ThemNhan("Ngày sinh:", 25, 155);
            ThemControl(dtpNgaySinh, 145, 152, 250);
            dtpNgaySinh.Format = DateTimePickerFormat.Short;

            ThemNhan("Giới tính:", 25, 195);
            rdoNam.Text = "Nam";
            rdoNu.Text = "Nữ";
            ThemControl(rdoNam, 145, 192, 70);
            ThemControl(rdoNu, 220, 192, 70);
            rdoNam.Checked = true;

            ThemNhan("Email:", 460, 75);
            ThemControl(txtEmail, 555, 72, 300);

            ThemNhan("Điện thoại:", 460, 115);
            ThemControl(txtDienThoai, 555, 112, 300);

            ThemNhan("Lớp:", 460, 155);
            ThemControl(cboLop, 555, 152, 300);
            cboLop.DropDownStyle = ComboBoxStyle.DropDownList;

            ThemNhan("Điểm:", 460, 195);
            numDiem.Minimum = 0;
            numDiem.Maximum = 10;
            numDiem.DecimalPlaces = 2;
            numDiem.Increment = 0.25M;
            ThemControl(numDiem, 555, 192, 100);

            chkLocTheoLop.Text = "Lọc DS theo lớp đang chọn";
            ThemControl(chkLocTheoLop, 670, 192, 250);

            DatNut(butThem, "Thêm sinh viên", 25, 245, 140);
            DatNut(butSua, "Cập nhật", 175, 245, 110);
            DatNut(butTim, "Tìm theo mã", 295, 245, 120);
            DatNut(butXoa, "Xóa sinh viên", 425, 245, 130);
            DatNut(butLamMoi, "Làm mới", 565, 245, 100);

            dgvSinhVien.Left = 25;
            dgvSinhVien.Top = 295;
            dgvSinhVien.Width = 920;
            dgvSinhVien.Height = 330;
            dgvSinhVien.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvSinhVien.ReadOnly = true;
            dgvSinhVien.AllowUserToAddRows = false;
            dgvSinhVien.AllowUserToDeleteRows = false;
            dgvSinhVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSinhVien.MultiSelect = false;
            dgvSinhVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Controls.Add(dgvSinhVien);
        }

        private void ThemNhan(string text, int x, int y)
        {
            Controls.Add(new Label { Text = text, Left = x, Top = y + 4, Width = 115, Height = 25 });
        }

        private void ThemControl(Control control, int x, int y, int width)
        {
            control.Left = x;
            control.Top = y;
            control.Width = width;
            control.Height = 25;
            Controls.Add(control);
        }

        private void DatNut(Button button, string text, int x, int y, int width)
        {
            button.Text = text;
            button.SetBounds(x, y, width, 32);
            Controls.Add(button);
        }

        // ---------- Sự kiện ----------
        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            try
            {
                cboLop.DataSource = lopBus.GetAllLopHoc();
                cboLop.DisplayMember = "TenLop";
                cboLop.ValueMember = "MaLop";
                HienThiDanhSach();
            }
            catch (Exception ex) { HienLoi(ex); }
        }

        private void HienThiDanhSach()
        {
            try
            {
                string maLop = cboLop.SelectedValue?.ToString();

                if (chkLocTheoLop.Checked && !string.IsNullOrEmpty(maLop))
                    dgvSinhVien.DataSource = svb.GetSinhViensByMaLop(maLop);
                else
                    dgvSinhVien.DataSource = svb.GetAllSinhVien();
            }
            catch (Exception ex) { HienLoi(ex); }
        }

        private SinhVien LayDuLieuTuForm() => new SinhVien
        {
            MaSV = txtMaSV.Text.Trim(),
            HoTen = txtHoTen.Text.Trim(),
            NgaySinh = dtpNgaySinh.Value.Date,
            GioiTinh = rdoNam.Checked ? "Nam" : "Nữ",
            Email = txtEmail.Text.Trim(),
            SoDienThoai = txtDienThoai.Text.Trim(),
            MaLop = cboLop.SelectedValue?.ToString() ?? "",
            Diem = (double)numDiem.Value
        };

        private void DoDuLieuLenForm(SinhVien sv)
        {
            txtMaSV.Text = sv.MaSV;
            txtHoTen.Text = sv.HoTen;
            txtEmail.Text = sv.Email;
            txtDienThoai.Text = sv.SoDienThoai;

            DateTime ns = sv.NgaySinh;
            if (ns < dtpNgaySinh.MinDate) ns = dtpNgaySinh.MinDate;
            if (ns > dtpNgaySinh.MaxDate) ns = dtpNgaySinh.MaxDate;
            dtpNgaySinh.Value = ns;

            rdoNam.Checked = sv.GioiTinh == "Nam";
            rdoNu.Checked = sv.GioiTinh == "Nữ";
            numDiem.Value = (decimal)Math.Min(10, Math.Max(0, sv.Diem));
            cboLop.SelectedValue = sv.MaLop;
        }

        private void butThem_Click(object sender, EventArgs e)
        {
            try
            {
                svb.AddSinhVien(LayDuLieuTuForm());
                MessageBox.Show("Thêm sinh viên thành công!");
                HienThiDanhSach();
                LamMoi();
            }
            catch (Exception ex) { HienLoi(ex); }
        }

        private void butSua_Click(object sender, EventArgs e)
        {
            try
            {
                svb.UpdateSinhVien(LayDuLieuTuForm());
                MessageBox.Show("Cập nhật sinh viên thành công!");
                HienThiDanhSach();
            }
            catch (Exception ex) { HienLoi(ex); }
        }

        private void butTim_Click(object sender, EventArgs e)
        {
            try
            {
                SinhVien sv = svb.GetSinhVienById(txtMaSV.Text.Trim());
                DoDuLieuLenForm(sv);
                dgvSinhVien.DataSource = new List<SinhVien> { sv };
            }
            catch (Exception ex) { HienLoi(ex); }
        }

        private void butXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maSV = txtMaSV.Text.Trim();
                if (string.IsNullOrWhiteSpace(maSV))
                    throw new ArgumentException("Hãy nhập mã sinh viên cần xóa!");

                if (MessageBox.Show("Bạn có chắc muốn xóa sinh viên này?", "Xác nhận",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                svb.DeleteSinhVien(maSV);
                MessageBox.Show("Xóa sinh viên thành công!");
                LamMoi();
                HienThiDanhSach();
            }
            catch (Exception ex) { HienLoi(ex); }
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                if (dgvSinhVien.Rows[e.RowIndex].DataBoundItem is SinhVien sv)
                    DoDuLieuLenForm(sv);
            }
            catch (Exception ex) { HienLoi(ex); }
        }

        private void LamMoi()
        {
            txtMaSV.Clear();
            txtHoTen.Clear();
            txtEmail.Clear();
            txtDienThoai.Clear();
            dtpNgaySinh.Value = DateTime.Today;
            rdoNam.Checked = true;
            numDiem.Value = 0;
            if (cboLop.Items.Count > 0) cboLop.SelectedIndex = 0;
            txtMaSV.Focus();
        }
    }
}
