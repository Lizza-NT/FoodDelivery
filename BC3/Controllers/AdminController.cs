using BC3.Classes;
using BC3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BC3.Controllers
{
    public class AdminController : Controller
    {
        private readonly ComboService _comboService;
        private readonly NguoiDungService _nguoiDungService;
        private readonly CuaHangService _cuaHangService;
        private readonly TinTucService _tinTucService;
        private readonly UuDaiService _uuDaiService;
        private readonly DonHangService _donHangService;
        public AdminController()
        {
            //_nhomService = new NhomService(new Neo4jService());
            _comboService = new ComboService(new Neo4jService());
            _nguoiDungService = new NguoiDungService(new Neo4jService());
            _cuaHangService = new CuaHangService(new Neo4jService());
            _tinTucService = new TinTucService(new Neo4jService());
            _uuDaiService = new UuDaiService(new Neo4jService());
            _donHangService = new DonHangService(new Neo4jService());
        }
        // GET: Admin

        //<-------------------------COMBO--------------------------------->
        // Hiển thị tất cả món ăn
        public async Task<ActionResult> Menu()
        {
            var comboLst = await _comboService.GetAllComboAsync();
            return View(comboLst);
        }
        // Hiển thị trang thêm món ăn
        public async Task<ActionResult> CreateCB()
        {
            var nhomComboList = await _comboService.GetNhomComboList();
            ViewBag.NhomCombo = nhomComboList; // Truyền danh sách nhóm vào ViewBag

            // Nếu danh sách rỗng, có thể thông báo hoặc xử lý
            if (!nhomComboList.Any())
            {
                ViewBag.ErrorMessage = "Không tìm thấy nhóm nào.";
            }

            return View();
        }
        // Xử lý thêm món ăn
        [HttpPost]
        public async Task<ActionResult> CreateCB(Combo comBo)
        {
            if (ModelState.IsValid)
            {
                await _comboService.AddCombo(comBo);
                return RedirectToAction("Menu");
            }
            return View(comBo);
        }
        // Hiển thị trang sửa món ăn
        public async Task<ActionResult> EditCB(string id)
        {
            var comBo = await _comboService.GetComboByIDAsync(id);
            if (comBo == null)
            {
                return HttpNotFound();
            }

            var nhomComboList = await _comboService.GetNhomComboList();
            ViewBag.NhomCombo = nhomComboList; // Truyền danh sách nhóm vào ViewBag

            // Nếu danh sách rỗng, có thể thông báo hoặc xử lý
            if (!nhomComboList.Any())
            {
                ViewBag.ErrorMessage = "Không tìm thấy nhóm nào.";
            }
            return View(comBo);
        }
        // Xử lý sửa món ăn
        [HttpPost]
        public async Task<ActionResult> EditCB(Combo comBo)
        {
            if (ModelState.IsValid)
            {
                await _comboService.UpdateCombo(comBo);
                return RedirectToAction("Menu");
            }
            return View(comBo);
        }
        // Xoá món ăn
        [HttpGet]
        public async Task<ActionResult> DeleteCB(string id)
        {
            await _comboService.DeleteCombo(id);
            return RedirectToAction("Menu");
        }

        //<-------------------------NguoiDung--------------------------------->

        public async Task<ActionResult> NguoiDung()
        {
            var nguoiDungLst = await _nguoiDungService.GetAllNguoiDungAsync();
            return View(nguoiDungLst);
        }
        [HttpPost]
        public async Task<ActionResult> CreateND(NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                await _nguoiDungService.AddNguoiDung(nguoiDung);
                return RedirectToAction("Index");
            }
            return View(nguoiDung);
        }
        public async Task<ActionResult> EditND(string name)
        {
            var user = await _nguoiDungService.GetNguoiDungByUsernameAsync(name);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> EditND(NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                await _nguoiDungService.UpdateNguoiDung(nguoiDung);
                return RedirectToAction("NguoiDung");
            }
            return View(nguoiDung);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteND(string username)
        {
            await _nguoiDungService.DeleteNguoiDung(username);
            return RedirectToAction("NguoiDung");
        }

        //<-------------------------CuaHang--------------------------------->

        public async Task<ActionResult> CuaHang()
        {
            var cuaHangLst = await _cuaHangService.GetAllCuaHang();
            return View(cuaHangLst);
        }
        [HttpPost]
        public async Task<ActionResult> CreateCH(CuaHang cuaHang)
        {
            if (ModelState.IsValid)
            {
                await _cuaHangService.AddCuaHang(cuaHang);
                return RedirectToAction("CuaHang");
            }
            return View(cuaHang);
        }
        public async Task<ActionResult> EditCH(string id)
        {
            var user = await _cuaHangService.GetCuaHangByIDAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> EditCH(CuaHang cuaHang)
        {
            if (ModelState.IsValid)
            {
                await _cuaHangService.UpdateCuaHang(cuaHang);
                return RedirectToAction("CuaHang");
            }
            return View(cuaHang);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteCH(string id)
        {
            await _cuaHangService.DeleteCuaHang(id);
            return RedirectToAction("CuaHang");
        }

        //<-------------------------TinTuc--------------------------------->

        public async Task<ActionResult> TinTuc()
        {
            var tinTucLst = await _tinTucService.GetAllNewsAsync();
            return View(tinTucLst);
        }
        [HttpPost]
        public async Task<ActionResult> CreateTT(TinTuc tinTuc)
        {
            if (ModelState.IsValid)
            {
                await _tinTucService.AddTinTuc(tinTuc);
                return RedirectToAction("TinTuc");
            }
            return View(tinTuc);
        }
        public async Task<ActionResult> EditTT(string id)
        {
            var user = await _tinTucService.GetTinTucByIDAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> EditTT(TinTuc tinTuc)
        {
            if (ModelState.IsValid)
            {
                await _tinTucService.UpdateTinTuc(tinTuc);
                return RedirectToAction("TinTuc");
            }
            return View(tinTuc);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteTT(string id)
        {
            await _tinTucService.DeleteTinTuc(id);
            return RedirectToAction("TinTuc");
        }

        //<-------------------------UuDai--------------------------------->

        public async Task<ActionResult> UuDai()
        {
            var uuDaiLst = await _uuDaiService.GetAllVoucherAsync();
            return View(uuDaiLst);
        }
        [HttpPost]
        public async Task<ActionResult> CreateUD(UuDai uuDai)
        {
            if (ModelState.IsValid)
            {
                await _uuDaiService.AddUuDai(uuDai);
                return RedirectToAction("UuDai");
            }
            return View(uuDai);
        }
        public async Task<ActionResult> EditUD(string id)
        {
            var user = await _uuDaiService.GetUuDaiByIDAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> EditUD(UuDai uuDai)
        {
            if (ModelState.IsValid)
            {
                await _uuDaiService.UpdateUuDai(uuDai);
                return RedirectToAction("TinTuc");
            }
            return View(uuDai);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteUD(string id)
        {
            await _uuDaiService.DeleteUuDai(id);
            return RedirectToAction("TinTuc");
        }

        //<-------------------------DonHang--------------------------------->

        public async Task<ActionResult> DonHang()
        {
            var donHangLst = await _donHangService.GetAllOrdersAsync();
            return View(donHangLst);
        }
        [HttpPost]
        public async Task<ActionResult> CreateDH(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                await _donHangService.AddDonHang(donHang);
                return RedirectToAction("DonHang");
            }
            return View(donHang);
        }
        public async Task<ActionResult> EditDH(string id)
        {
            var user = await _donHangService.GetDonHangByIDAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> EditDH(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                await _donHangService.UpdateDonHang(donHang);
                return RedirectToAction("DonHang");
            }
            return View(donHang);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteDH(string id)
        {
            await _donHangService.DeleteDonHang(id);
            return RedirectToAction("DonHang");
        }
    }
}