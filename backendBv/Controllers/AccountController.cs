using backendBv.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace backendBv.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseHelper _databaseHelper;
        public AccountController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }
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
            //if (username == "admin" && password == "123")
            //{
            //    //lưu thông tin người dùng vào session
            //    HttpContext.Session.SetString("User", username);
            //    HttpContext.Session.SetString("Role", "Admin"); // set role as Admin
            //    return RedirectToAction("Index");
            //} else if (username == "user" && password == "123")
            //{
            //    HttpContext.Session.SetString("User", username);
            //    HttpContext.Session.SetString("Role", "User"); // set role as User
            //    return RedirectToAction("Index");
            //}
            //else
            //{
            //    ViewBag.Error = "Invalid username or password";
            //    return View();
            //}
            //sử dụng databaseHelper để kiểm tra đăng nhập
            string role;
           bool isAuthenticated = _databaseHelper.AuthenticateUser(username, password, out role);
            if (isAuthenticated)
            {
                HttpContext.Session.SetString("User", username);
                HttpContext.Session.SetString("Role", role); // set role from database
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
        public IActionResult Register(string username, string password, string role)
        {
            ////kiểm tra nếu tài khoản đã tồn tại
            //if (HttpContext.Session.GetString(username) != null)
            //{
            //    ViewBag.ErrorMessage = "UserName already exits";
            //    return View();
            //}
            ////giả sử đăng ký thành công
            ////trong thực tế bạn cần lưu thông tin người dùng vào cơ sở dữ liệu
            ////lưu thông tin người dùng vào session (chỉ để minh họa, không nên làm vậy trong thực tế)
            //HttpContext.Session.SetString(username, password);
            //HttpContext.Session.SetString("User", username);
            //ViewBag.ErrorMessage = "Registration successful. Please log in.";
            //return RedirectToAction("Index");
            bool isRegistered = _databaseHelper.RegisterUser(username, password, role);
            if (isRegistered)
            {
                //đăng ký thành công, chuyển hướng đến trang đăng nhập
                HttpContext.Session.SetString(username, password);
                HttpContext.Session.SetString("User", username);
                ViewBag.ErrorMessage = "Registration successful. Please log in.";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.ErrorMessage = "Username already exists. Please choose a different username.";
                return View();
            }
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
