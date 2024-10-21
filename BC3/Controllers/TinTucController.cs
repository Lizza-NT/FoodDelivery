using BC3.Classes;
using BC3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BC3.Controllers
{
    public class TinTucController : Controller
    {
        private readonly TinTucService _tinTucService;

        public TinTucController()
        {
            var neo4jService = new Neo4jService();
            _tinTucService = new TinTucService(neo4jService);
        }

        // GET: TinTuc
        public async Task<ActionResult> TinTucPartial()
        {
            var news = await _tinTucService.GetAllNewsAsync();
            return View(news);
        }

        // Chi tiết tin tức
        public async Task<ActionResult> DetailTin(string id)
        {
            var news = await _tinTucService.GetTinTucByIDAsync(id);
            if (news == null)
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            // Lưu id tin tức vào TempData để sử dụng ở view khác
            TempData["TT"] = news.MaTT;

            var relatedNews = await _tinTucService.GetAllNewsAsync();

            var model = new Tuple<TinTuc, List<TinTuc>>(news, relatedNews);
            return View(model);
        }

        public async Task<ActionResult> TinLQ()
        {
            var news = await _tinTucService.GetAllNewsAsync();
            return PartialView(news);
        }
    }
}
