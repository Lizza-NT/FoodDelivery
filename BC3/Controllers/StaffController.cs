using BC3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BC3.Controllers
{
    public class StaffController : Controller
    {
        private readonly DonHangService _donHangService;
        public StaffController()
        {            
            _donHangService = new DonHangService(new Neo4jService());
        }
        // GET: Staff
        //Đơn mới
        public async Task<ActionResult> DonMoi(string tencuahang)
        {
            // Nếu tên cửa hàng chưa có, lấy từ session
            if (string.IsNullOrEmpty(tencuahang))
            {
                tencuahang = Session["TenCuaHang"] as string;
            }

            // Gọi service để lấy danh sách đơn hàng mới theo cửa hàng
            var moi = await _donHangService.Get_DonHang_ByCuaHang_And_TrangThai(tencuahang, "New");
            return View(moi);
        }
        [HttpPost]
        public async Task<ActionResult> DonMoi(string id, string tencuahang)
        {
            // Lấy đơn hàng theo ID
            var donHang = await _donHangService.GetDonHangByIDAsync(id);

            // Cập nhật trạng thái nếu đơn hàng tồn tại
            if (donHang != null)
            {
                donHang.TrangThai = "Processing";
                await _donHangService.UpdateDonHang_TrangThaiAsync(donHang);
            }

            // Chuyển hướng lại trang DonMoi và truyền tên cửa hàng để tránh mất dữ liệu
            return RedirectToAction("DonMoi", new { tencuahang = tencuahang });
        }


        //Đơn mới nhận
        public async Task<ActionResult> XuLy(string tencuahang)
        {
            // Nếu tên cửa hàng chưa có, lấy từ session
            if (string.IsNullOrEmpty(tencuahang))
            {
                tencuahang = Session["TenCuaHang"] as string;
            }

            // Gọi service để lấy danh sách đơn hàng mới theo cửa hàng
            var moi = await _donHangService.Get_DonHang_ByCuaHang_And_TrangThai(tencuahang, "Processing");
            return View(moi);
        }
        [HttpPost]
        public async Task<ActionResult> XuLy(string id, string tencuahang)
        {
            // Lấy đơn hàng theo ID
            var donHang = await _donHangService.GetDonHangByIDAsync(id);

            // Cập nhật trạng thái nếu đơn hàng tồn tại
            if (donHang != null)
            {
                donHang.TrangThai = "Deliveried";
                await _donHangService.UpdateDonHang_TrangThaiAsync(donHang);
            }

            // Chuyển hướng lại trang DonMoi và truyền tên cửa hàng để tránh mất dữ liệu
            return RedirectToAction("XuLy", new { tencuahang = tencuahang });
        }

        public async Task<ActionResult> DangGiao(string tencuahang)
        {
            // Nếu tên cửa hàng chưa có, lấy từ session
            if (string.IsNullOrEmpty(tencuahang))
            {
                tencuahang = Session["TenCuaHang"] as string;
            }

            // Gọi service để lấy danh sách đơn hàng mới theo cửa hàng
            var moi = await _donHangService.Get_DonHang_ByCuaHang_And_TrangThai(tencuahang, "Deliveried");
            return View(moi);
        }
        [HttpPost]
        public async Task<ActionResult> DangGiao(string id, string tencuahang)
        {
            // Lấy đơn hàng theo ID
            var donHang = await _donHangService.GetDonHangByIDAsync(id);

            // Cập nhật trạng thái nếu đơn hàng tồn tại
            if (donHang != null)
            {
                donHang.TrangThai = "Done";
                await _donHangService.UpdateDonHang_TrangThaiAsync(donHang);
            }

            // Chuyển hướng lại trang DonMoi và truyền tên cửa hàng để tránh mất dữ liệu
            return RedirectToAction("DangGiao", new { tencuahang = tencuahang });
        }

        public async Task<ActionResult> HoanThanh(string tencuahang)
        {
            // Nếu tên cửa hàng chưa có, lấy từ session
            if (string.IsNullOrEmpty(tencuahang))
            {
                tencuahang = Session["TenCuaHang"] as string;
            }

            // Gọi service để lấy danh sách đơn hàng mới theo cửa hàng
            var moi = await _donHangService.Get_DonHang_ByCuaHang_And_TrangThai(tencuahang, "Done");
            return View(moi);
        }
    }
}