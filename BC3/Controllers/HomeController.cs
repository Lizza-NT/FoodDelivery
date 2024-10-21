using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BC3.Services;

namespace BC3.Controllers
{
    public class HomeController : Controller
    {
        private readonly CuaHangService _cuaHangService;
        public HomeController()
        {
            var neo4jService = new Neo4jService();
            _cuaHangService = new CuaHangService(neo4jService);
        }
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public async Task<ActionResult> ChiNhanh(string search)
        {
            var stores = await _cuaHangService.GetStoresByRegionAsync(); // Lấy tất cả cửa hàng có sắp xếp

            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                stores = stores.Where(x =>
                    x.ThanhPho.ToLower().Contains(search) ||
                    x.Quan.ToLower().Contains(search)).ToList();

                ViewBag.SearchMode = true; // Đánh dấu rằng đang trong chế độ tìm kiếm
            }
            else
            {
                ViewBag.SearchMode = false; // Không tìm kiếm, hiển thị toàn bộ cửa hàng
            }

            ViewBag.Search = search;
            ViewBag.HasStores = stores.Any(); // Kiểm tra xem có cửa hàng nào được tìm thấy không
            return View(stores);
        }
    }
}