# Quản lý sinh viên (WinForms + ADO.NET + MySQL)

Bài tập về ADO.Net - 17/10/2026.

## Cấu trúc 4 tầng
- `Entity/` : LopHoc, SinhVien (kiểm tra dữ liệu bằng DataAnnotations)
- `DAL/`    : DataHelper (ADO.NET, có tham số) + LopDAL, SinhVienDAL - chỉ truy vấn và ném ngoại lệ
- `BUL/`    : LopBus, SinhVienBUL - kiểm tra nghiệp vụ và ném ngoại lệ
- `GUI/`    : frmSinhVien - nơi duy nhất bắt và hiển thị ngoại lệ

## Cách chạy
1. Cài MySQL Server, chạy script `Database/QLSV_MySQL.sql` (Workbench).
2. Mở `DAL/DataHelper.cs`, thay `MAT_KHAU_CUA_BAN` bằng mật khẩu root của bạn
   (đừng commit mật khẩu thật lên GitHub).
3. `dotnet restore` rồi `dotnet run` (hoặc mở `.csproj` bằng Visual Studio 2022, cần .NET 8 SDK).
