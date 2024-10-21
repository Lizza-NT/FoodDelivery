using BC3.Classes;
using BC3.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BC3.Controllers
{
    public class AccountController : Controller
    { 
        private readonly NguoiDungService _nguoiDungService;

        public AccountController()
        {
            _nguoiDungService = new NguoiDungService(new Neo4jService());
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(NguoiDung user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            if (user.password != user.ConfirmPassword)
            {
                ModelState.AddModelError("", "Mật khẩu và xác nhận mật khẩu không khớp.");
                return View(user);
            }

            if (await _nguoiDungService.RegisterUser(user))
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Đăng ký không thành công.");
            return View(user);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(NguoiDung user)
        {           
            // Gọi phương thức LoginUser để kiểm tra thông tin đăng nhập
            var foundUser = await _nguoiDungService.LoginUser(user.username, user.password);

            if (foundUser != null)
            {
                // Lưu thông tin vào session
                Session["hoTen"] = foundUser.hoTen;

                // Điều hướng dựa trên vai trò
                if (foundUser.role == "Khach")
                {
                    return RedirectToAction("Index", "Combo");
                }
                else if (foundUser.role == "NhanVien" || foundUser.role == "Shipper")
                {
                    if (foundUser.role == "NhanVien")
                    {
                        Session["TenCuaHang"] = await _nguoiDungService.GetCuaHangByNhanVien(user.username);
                    }
                    return RedirectToAction("DonMoi", "Staff");
                }
                else if (foundUser.role == "Admin")
                {
                    return RedirectToAction("Menu", "Admin");
                }
            }

            // Nếu không tìm thấy người dùng, hiển thị thông báo lỗi
            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(user);
        }


        public ActionResult Logout()
        {
            Session.Clear();
            Session.Remove("GioHang");
            return RedirectToAction("Home", "Home");
        }
    }
}
