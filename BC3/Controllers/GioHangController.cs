using BC3.Classes;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BC3.Controllers
{
    public class GioHangController : Controller
    {
        private readonly IGraphClient _neo4jClient;
        public GioHangController(IGraphClient neo4jClient)
        {
            _neo4jClient = neo4jClient;
        }
        // GET: GioHang
        public ActionResult GioRong()
        {
            return View();
        }

        // Tạo giỏ hàng
        public List<GioHang> LayGioHang()
        {
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if (list == null)
            {
                list = new List<GioHang>();
                Session["GioHang"] = list;
            }
            return list;
        }

        // Thêm sản phẩm vào giỏ hàng
        public async Task<ActionResult> ThemGioHang(string comboID, string strUrl)
        {
            // Lấy giỏ
            List<GioHang> list = LayGioHang();

            // Kiểm tra combo có chưa?
            GioHang sp = list.Find(x => x.ComboID == comboID);
            if (sp == null) // chưa có
            {
                sp = new GioHang(_neo4jClient, comboID); // Tạo sản phẩm mới từ Neo4j
                await sp.LoadComboFromNeo(comboID);  // Tải thông tin combo từ Neo4j
                sp.Soluong = 1;
                list.Add(sp);
            }
            else // đã có trong giỏ hàng
            {
                sp.Soluong++;
            }

            return Redirect(strUrl);
        }

        // Tổng số lượng
        private int tongSoLuong()
        {
            int sum = 0;
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if (list != null)
            {
                sum = list.Sum(x => x.Soluong);
            }
            return sum;
        }

        // Tổng thành tiền
        private double tongThanhTien()
        {
            double ttt = 0;
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if (list != null)
            {
                ttt = list.Sum(x => x.Soluong * x.GiaBan);
            }
            return ttt;
        }

        //View giỏ hàng
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("GioRong", "GioHang");
            }
            List<GioHang> list = LayGioHang();

            ViewBag.tongSoLuong = tongSoLuong();
            ViewBag.tongThanhTien = tongThanhTien();

            return View(list);
        }

        //Nút dẫn tới view giỏ
        public async Task<ActionResult> GioHangPartial()
        {
            ViewBag.tongSoLuong = tongSoLuong();
            return PartialView("_GioHangPartial");
        }

        //xoá thông tin 1 combo
        public ActionResult XoaGioHang(string comboID)
        {
            // Lấy giỏ hàng
            List<GioHang> list = LayGioHang();
            // Kiểm tra combo có chưa?
            GioHang sp = list.Single(x => x.ComboID == comboID);

            if (sp != null) // có
            {
                list.RemoveAll(s => s.ComboID == comboID);
            }
            if (list.Count == 0)
            {
                return RedirectToAction("GioRong", "GioHang");
            }
            return RedirectToAction("GioHang", "GioHang");
        }

        //xoá hết giỏ
        public ActionResult XoaGioHang_ALL()
        {
            List<GioHang> list = LayGioHang();
            list.Clear();
            return RedirectToAction("GioRong", "GioHang");
        }

        //Nếu combo đã có trong giỏ rồi thì cập nhật số lượng
        public ActionResult CapNhatGioHang(string comboID, FormCollection f)
        {
            // Lấy giỏ
            List<GioHang> list = LayGioHang();
            // Kiểm tra có combo không
            GioHang sp = list.Single(s => s.ComboID == comboID);
            //
            if (sp != null)
            {
                sp.Soluong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang", "GioHang");
        }
    }
}
