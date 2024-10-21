using BC3.ViewModel;
using BC3.Services;
using BC3.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BC3.Controllers
{
    public class ComboController : Controller
    {
        private readonly NhomService _nhomService;
        private readonly ComboService _comboService;
        public ComboController()
        {
            _nhomService = new NhomService(new Neo4jService());
            _comboService = new ComboService(new Neo4jService());            
        }

        // GET: Combo
        public async Task<ActionResult> Index()
        {
            var model = new FoodMenuViewModel
            {
                NhomList = await _nhomService.GetAllNhomAsync(),
                ComboList = new List<Combo>() // Khởi tạo trống, sẽ được cập nhật khi người dùng chọn nhóm
            };

            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllCombo()
        {
            var combos = await _comboService.GetAllComboAsync();
            return Json(combos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetCombosByGroupId(string id)
        {
            var combos = await _comboService.GetCombosByGroupIdAsync(id);
            return Json(combos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetComboByID(string id)
        {
            var combo = await _comboService.GetComboByIDAsync(id);
            return Json(combo, JsonRequestBehavior.AllowGet);
        }

        //// Thêm món vào giỏ hàng
        //[HttpPost]
        //public ActionResult AddToCart(int macb, string tencb, decimal gia, int soluong)
        //{
        //    var gioHang = GetCart();
        //    gioHang.AddItem(macb, tencb, gia, soluong);
        //    return RedirectToAction("ViewCart");
        //}

        //// Xem giỏ hàng
        //public ActionResult ViewCart()
        //{
        //    var gioHang = GetCart();
        //    return View(gioHang.Items);
        //}

        //// Xóa món khỏi giỏ hàng
        //public ActionResult RemoveFromCart(int macb)
        //{
        //    var gioHang = GetCart();
        //    gioHang.RemoveItem(macb);
        //    return RedirectToAction("ViewCart");
        //}

        //// Lấy giỏ hàng từ Session
        //private GioHangService GetCart()
        //{
        //    var cart = Session["GioHang"] as GioHangService;
        //    if (cart == null)
        //    {
        //        cart = new GioHangService();
        //        Session["GioHang"] = cart;
        //    }
        //    return cart;
        //}
    }
}