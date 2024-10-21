using BC3.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BC3.ViewModel
{
    public class DonHangViewModel
    {
        public string TenNguoiDat { get; set; }
        public string SoDienThoai {  get; set; }
        public string DiaChi { get; set; }
        public string MaDonHang { get; set; }
        public DateTime NgayGiao { get; set; }
        public List<string> TenComboGop { get; set; }
        public decimal ThanhTien {  get; set; }
        //public string TenCuaHang { get; set; }
    }
}