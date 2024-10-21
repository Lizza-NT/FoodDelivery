using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BC3.Services;

namespace BC3.Controllers
{
    public class UuDaiController : Controller
    {
        private readonly UuDaiService _uuDaiService;

        public UuDaiController()
        {
            var neo4jService = new Neo4jService();
            _uuDaiService = new UuDaiService(neo4jService);
        }

        // GET: UuDai
        public async Task<ActionResult> UuDaiPartial()
        {
            var vouchers = await _uuDaiService.GetAllVoucherAsync();
            return View(vouchers);
        }

        // Chi tiết ưu đãi
        public async Task<ActionResult> DetailUD(string id)
        {
            var voucher = await _uuDaiService.GetUuDaiByIDAsync(id);
            var allVouchers = await _uuDaiService.GetAllVoucherAsync();
            // Chuyển dữ liệu đến View dưới dạng Tuple
            var model = Tuple.Create(voucher, allVouchers);
            return View(model);
        }
    }
}
