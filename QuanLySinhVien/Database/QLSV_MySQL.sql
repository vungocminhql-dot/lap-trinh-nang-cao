CREATE DATABASE IF NOT EXISTS QLSV CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE QLSV;

CREATE TABLE IF NOT EXISTS LopHoc (
    MaLop  VARCHAR(20)  NOT NULL PRIMARY KEY,
    TenLop VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS SinhVien (
    MaSV        VARCHAR(20)  NOT NULL PRIMARY KEY,
    HoTen       VARCHAR(100) NOT NULL,
    NgaySinh    DATE         NOT NULL,
    GioiTinh    VARCHAR(10)  NOT NULL,
    Email       VARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(20)  NOT NULL,
    MaLop       VARCHAR(20)  NOT NULL,
    Diem        DOUBLE       NOT NULL CHECK (Diem BETWEEN 0 AND 10),
    FOREIGN KEY (MaLop) REFERENCES LopHoc(MaLop)
);

INSERT IGNORE INTO LopHoc VALUES
('CSE0001', 'Khoa học máy tính 01'),
('CSE0002', 'Khoa học máy tính 02'),
('CSE0003', 'Khoa học máy tính 03');
