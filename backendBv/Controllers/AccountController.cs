using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace backendBv.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            //kiểm tra nếu người dùng đã đăng nhập thì chuyển hướng đến trang chủ
            if(HttpContext.Session.GetString("User") != null)
            {
                //lấy thông tin người dùng từ session role là admin hay user
                string userRole = HttpContext.Session.GetString("Role");
                ViewBag.User = HttpContext.Session.GetString("User");
                ViewBag.Role = userRole;

                return View();
            } 
            //nếu chưa đăng nhập thì chuyển đến trang login
           return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            //giả sử tài khoản hợp lệ là admin/admin
            if (username == "admin" && password == "123")
            {
                //lưu thông tin người dùng vào session
                HttpContext.Session.SetString("User", username);
                HttpContext.Session.SetString("Role", "Admin"); // set role as Admin
                return RedirectToAction("Index");
            } else if (username == "user" && password == "123")
            {
                HttpContext.Session.SetString("User", username);
                HttpContext.Session.SetString("Role", "User"); // set role as User
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }
              
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            //kiểm tra nếu tài khoản đã tồn tại
            if (HttpContext.Session.GetString(username) != null)
            {
                ViewBag.ErrorMessage = "UserName already exits";
                return View();
            }
            //giả sử đăng ký thành công
            //trong thực tế bạn cần lưu thông tin người dùng vào cơ sở dữ liệu
            //lưu thông tin người dùng vào session (chỉ để minh họa, không nên làm vậy trong thực tế)
            HttpContext.Session.SetString(username, password);
            HttpContext.Session.SetString("User", username);
            ViewBag.ErrorMessage = "Registration successful. Please log in.";
            return RedirectToAction("Index");
        }
        public IActionResult AdminPage()
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                
                return View();
            }
            else
            {
                return RedirectToAction("Index");
           

            }
        }
        public IActionResult UserPage()
        {
            if (HttpContext.Session.GetString("Role") == "User")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public IActionResult Logout()
        {
            //xóa thông tin người dùng khỏi session
            HttpContext.Session.Remove("User");
            return RedirectToAction("Login");
        }
    }
}
