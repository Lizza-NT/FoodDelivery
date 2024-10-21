//using BC3.Classes;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace BC3.Services
//{
//    public class GioHangService
//    {
//        public List<GioHang> Items { get; set; } = new List<GioHang>();
//        public void AddItem(int macb, string tencb, decimal gia, int soluong)
//        {
//            var item = Items.FirstOrDefault(i => i.MaCB == macb);
//            if (item != null)
//            {
//                item.Soluong += soluong;
//   
//            else
//            {
//                Items.Add(new GioHang
//                {
//                    MaCB = macb,
//                    TenCB = tencb,
//                    GiaBan = gia,
//                    Soluong = soluong
//                });
//            }
//        }

//        public void RemoveItem(int macb)
//        {
//            var item = Items.FirstOrDefault(i => i.MaCB == macb);
//            if (item != null)
//            {
//                Items.Remove(item);
//            }
//        }

//        public decimal Total => Items.Sum(i => i.GiaBan * i.Soluong);
//    }
//}