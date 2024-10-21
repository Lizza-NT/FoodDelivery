using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BC3.Classes
{
    public class NguoiDung
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
        public string username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        public string hoTen { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại phải có 10 hoặc 11 chữ số.")]
        public string sdt { get; set; }
        public string email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string password { get; set; }
        public string quan { get; set; }
        public string duong{ get; set; }
        public string thanhpho { get; set; }
        public string role { get; set; }
        public string cuaHang { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu.")]
        [Compare("password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }
    }
}