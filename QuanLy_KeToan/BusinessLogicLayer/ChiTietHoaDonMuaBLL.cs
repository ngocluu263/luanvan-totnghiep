﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuanLy_KeToan.DataAccessLayer;
using System.Windows.Forms;

namespace QuanLy_KeToan.BusinessLogicLayer
{
    class Chi_Tiet_Hoa_Don_Mua
    {
        public string malohdmua { get; set; }
        public string mahdmua { get; set; }
        public string mahang { get; set; }
        public int soluong { get; set; }
        public bool thanhtoan { get; set; }
        public DateTime ngaylap { get; set; }
        public string nguoilap { get; set; }
        public DateTime ngaysua { get; set; }
        public string nguoisua { get; set; }
    }
    class ChiTietHoaDonMuaBLL
    {
        QuanLy_KeToanDataContext QLKT = new QuanLy_KeToanDataContext();
        public IQueryable LayMaHang(string malohdmua)
        {
            var sql = from hang in QLKT.Hangs
                      from lohdmua in QLKT.LoHDMuas
                      where lohdmua.MaLoaiHang==hang.MaLoaiHang && lohdmua.MaLoHDMua==malohdmua
                      select new
                      {
                          hang.MaHang,
                          hang.TenHang,
                      };
            return sql;
        }
        //public int LaySLHangTheoChiTietHDMua(string malohdmua, string mahdmua, string hang)
        //{
        //    int sl = 0;
        //    var sql = from cthdm in QLKT.ChiTietHDMuas
        //              where cthdm.MaLoHDMua == malohdmua && cthdm.MaHDMua == mahdmua && cthdm.MaHang == hang
        //              select cthdm.SoLuong;
        //    foreach (var item in sql)
        //    {
        //        sl = System.Convert.ToInt16(item.ToString());
        //    }
        //    return sl;
        //}
        public List<Chi_Tiet_Hoa_Don_Mua> LayChiTietHoaDonMua(string malohdmua, string mahdmua)
        {
            var sql = (from cthdm in QLKT.ChiTietHDMuas
                       where (cthdm.MaLoHDMua == malohdmua && cthdm.MaHDMua == mahdmua)
                       select new Chi_Tiet_Hoa_Don_Mua
                       {
                           malohdmua = cthdm.MaLoHDMua,
                           mahdmua = cthdm.MaHDMua,
                           mahang = cthdm.MaHang,
                           soluong = System.Convert.ToInt16(cthdm.SoLuong.Value != null ? cthdm.SoLuong : 0),
                           thanhtoan = System.Convert.ToBoolean(cthdm.ThanhToan.Value),
                           ngaylap = Convert.ToDateTime(cthdm.NgayLap == null ? DateTime.Today : cthdm.NgayLap),
                           nguoilap = cthdm.NguoiLap,
                           ngaysua = Convert.ToDateTime(cthdm.NgaySua == null ? DateTime.Today : cthdm.NgaySua),
                           nguoisua = cthdm.NguoiSua,
                       }).ToList<Chi_Tiet_Hoa_Don_Mua>();
            return sql;
        }
        private bool KiemTraDulieu(string malohdmua, string mahdmua, string mahang)
        {
            var sql = from cthdm in QLKT.ChiTietHDMuas
                      where cthdm.MaLoHDMua == malohdmua && cthdm.MaHDMua == mahdmua && cthdm.MaHang == mahang
                      select cthdm;
            if (sql.Count() > 0)
                return true;
            else return false;
        }
        //Code Thêm-Xóa-Sửa Chi Tiết Hóa Đơn Bán

        public bool ThemCTHDMua(string malohdmua, string mahdmua, string mahang, int soluong,bool thanhtoan, DateTime ngaylap, string nguoilap, DateTime ngaysua, string nguoisua)
        {
            if (KiemTraDulieu(malohdmua, mahdmua, mahang) == true)
            {
                MessageBox.Show("Đã tồn tại dữ liệu này trong CSDL", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            try
            {
                ChiTietHDMua CTHDMua = new ChiTietHDMua();
                CTHDMua.MaLoHDMua = malohdmua;
                CTHDMua.MaHDMua = mahdmua;
                CTHDMua.MaHang = mahang;
                CTHDMua.SoLuong = soluong;
                CTHDMua.ThanhToan = thanhtoan;
                CTHDMua.NgayLap = ngaylap;
                CTHDMua.NguoiLap = nguoilap;
                CTHDMua.NgaySua = ngaysua;
                CTHDMua.NguoiSua = nguoisua;
                QLKT.ChiTietHDMuas.InsertOnSubmit(CTHDMua);
                QLKT.SubmitChanges();
                MessageBox.Show("Lưu thành công 1 record !", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool SuaCTHDMua(string malohdmua, string mahdmua, string mahang, ChiTietHDMua CTHDMua)
        {
            try
            {
                ChiTietHDMua cthdmua = QLKT.ChiTietHDMuas.Single(_cthdmua => _cthdmua.MaLoHDMua == malohdmua && _cthdmua.MaHDMua == mahdmua && _cthdmua.MaHang == mahang);
                cthdmua.SoLuong = CTHDMua.SoLuong;
                cthdmua.ThanhToan = CTHDMua.ThanhToan;
                cthdmua.NgayLap = CTHDMua.NgayLap;
                cthdmua.NguoiLap = CTHDMua.NguoiLap;
                cthdmua.NgaySua = CTHDMua.NgaySua;
                cthdmua.NguoiSua = CTHDMua.NguoiSua;
                QLKT.SubmitChanges();
                MessageBox.Show("Lưu thành công thông tin Chi tiết hóa đơn mua!", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private bool KiemTraChiTietPhieuNhap(string malohdmua, string mahdmua, string mahang)
        {
            var sql = from ctpn in QLKT.ChiTietPhieuNhaps
                      where ctpn.MaLoHDMua == malohdmua && ctpn.MaHDMua == mahdmua && ctpn.MaHang == mahang
                      select ctpn;
            if (sql.Count() > 0)
                return true;
            else
                return false;
        }
        private bool KiemTraChiTietPhieuChi(string malohdmua, string mahdmua, string mahang)
        {
            var sql = from ctpc in QLKT.ChiTietPhieuChis
                      where ctpc.MaLoHDMua == malohdmua && ctpc.MaHDMua == mahdmua && ctpc.MaHang == mahang
                      select ctpc;
            if (sql.Count() > 0)
                return true;
            else
                return false;
        }
        public bool XoaCTHDMua(string malohdmua, string mahdmua, string mahang)
        {
            if (KiemTraChiTietPhieuNhap(malohdmua, mahdmua,mahang) == true)
            {
                MessageBox.Show("Không được xóa dữ liệu này-Liên quan đến dữ liệu Chi Tiết Phiếu Nhập", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (KiemTraChiTietPhieuChi(malohdmua, mahdmua, mahang) == true)
            {
                MessageBox.Show("Không được xóa dữ liệu này-Liên quan đến dữ liệu Chi Tiết Phiếu Chi", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            try
            {
                var delete = from cthdmua in QLKT.ChiTietHDMuas
                             where cthdmua.MaLoHDMua == malohdmua && cthdmua.MaHDMua == mahdmua && cthdmua.MaHang == mahang
                             select cthdmua;
                if (delete.Count() > 0)
                {
                    QLKT.ChiTietHDMuas.DeleteOnSubmit(delete.First());
                    QLKT.SubmitChanges();
                    MessageBox.Show("Xóa thành công 1 record !", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public IQueryable LayNguoiLap()
        {
            var sql = from phanquyen in QLKT.PhanQuyens
                      select new { phanquyen.TenDangNhap };
            return sql;
        }
        //Cập nhật chi tiết phiếu xuất khi thay đổi chi tiết hóa đơn bán
        public bool CapNhatLaiCTPNKhiThayDoiCTHDM(string malohdmua, string mahdmua, string mahang, int sl)
        {
            var ctpn = from _ctpn in QLKT.ChiTietPhieuNhaps
                       where _ctpn.MaLoHDMua == malohdmua && _ctpn.MaHDMua == mahdmua && _ctpn.MaHang == mahang
                       select _ctpn.SoLuong;
            if (ctpn.Count() > 0)
            {
                int _sl = 0;
                foreach (var item in ctpn)
                {
                    _sl = System.Convert.ToInt16(item);
                }
                int sl_khochua = 0;
                var hang = from kc in QLKT.KhoChuas
                           where kc.MaHang == mahang
                           select kc.SL;
                if (hang.Count() > 0)
                {
                    foreach (var item in hang)
                    {
                        sl_khochua = System.Convert.ToInt16(item);
                    }
                }
                int update = sl - _sl;
                sl_khochua += update;
                //Cập nhật chi tiết phiếu xuất
                try
                {
                    ChiTietPhieuNhap chitietpn = QLKT.ChiTietPhieuNhaps.Single(_chitietpn => _chitietpn.MaLoHDMua==malohdmua && _chitietpn.MaHDMua==mahdmua && _chitietpn.MaHang == mahang);
                    chitietpn.SoLuong = sl;
                    QLKT.SubmitChanges();
                    if (update > 0)
                        MessageBox.Show("Số lượng Hàng " + mahang + " nhập thêm vào kho: " + update.ToString() + Environment.NewLine +
                                        "Số lượng Hàng " + mahang + " hiện tại là:" + sl_khochua.ToString(), "Cập nhật lại kho chứa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (update == 0)
                        MessageBox.Show("Số lượng Hàng " + mahang + " giữ nguyên" + Environment.NewLine +
                                        "Số lượng Hàng " + mahang + " hiện tại là:" + sl_khochua.ToString(), "Cập nhật lại kho chứa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Số lượng Hàng " + mahang + " trả ra khỏi kho: " + (-update).ToString() + Environment.NewLine +
                                        "Số lượng Hàng " + mahang + " hiện tại là:" + sl_khochua.ToString(), "Cập nhật lại kho chứa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                //Cập nhật kho chứa
                try
                {
                    KhoChua khochua = QLKT.KhoChuas.Single(_kc => _kc.MaHang == mahang);
                    khochua.SL = sl_khochua;
                    QLKT.SubmitChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
                return false;
        }
        public void CapNhatCTPCKhiThayDoiCTHDM(string malohdmua, string mahdmua, string mahang)
        {
            try
            {
                ChiTietPhieuChiBLL CTPCBLL = new ChiTietPhieuChiBLL();
                ChiTietPhieuChi CTPC = QLKT.ChiTietPhieuChis.Single(_CTPC => _CTPC.MaLoHDMua == malohdmua && _CTPC.MaHDMua == mahdmua && _CTPC.MaHang == mahang);
                CTPC.TienChi = CTPCBLL.TinhGiaTriTheoChiTietHDMua(malohdmua, mahdmua, mahang);
                QLKT.SubmitChanges();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
    
}
