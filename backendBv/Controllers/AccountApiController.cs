using backendBv.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backendBv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        // Giả lập dữ liệu người dùng
        private static readonly List<Users> Users = new List<Users>
        {
            new Users { Username = "user1", Password = "password1", Role = "Admin" },
            new Users { Username = "user2", Password = "password2",Role = "User" }
        };
        // POST: api/AccountApi/login
        //api dang nhap
        [HttpPost("login")]
        public IActionResult Login([FromBody] Users loginUser)
        {
            try
            {
                // Kiểm tra thông tin đăng nhập
                var user = Users.FirstOrDefault(u => u.Username == loginUser.Username && u.Password == loginUser.Password);
                if (user == null)
                {
                    return Unauthorized("Invalid credentials.");
                }

                // Lưu thông tin người dùng vào session
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                return Ok(new { Message = "Login successful" });
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý lỗi
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
            // Lưu thông tin người dùng vào session
          
        }
        // POST: api/AccountApi/logout
        //api dang xuat
        [HttpPost("logout")]
        public IActionResult Logout() 
        {
            try
            {
                //xoa session
                HttpContext.Session.Clear();
                return Ok(new { Message = "Logout successful" });
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }
        //POST: api/AccountApi/Register
        //api dang ky tai khoan
        [HttpPost("register")]
        public IActionResult Register([FromBody] Users registerUser) 
        {
            try
            {
                //kiem tra tai khoan da ton tai chua
                var existingUser = Users.FirstOrDefault(u => u.Username == registerUser.Username);
                if(existingUser != null) 
                {
                    return BadRequest("Username already exists.");
                }
                //them tai khoan moi
                Users.Add(registerUser);
                return Ok(new { Message = "Registration successful" });
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }
        //Get : Api/AccountApi/status
        //api kiem tra trang thai dang nhap
        [HttpGet("status")]
        public IActionResult Status() 
        {
            try
            {
                var username = HttpContext.Session.GetString("Username");
                var role = HttpContext.Session.GetString("Role");
                if (string.IsNullOrEmpty(username)) 
                {
                    return Unauthorized(new { IsLoggedIn = false });
                }
                return Ok(new { IsLoggedIn = true, Username = username, Role = role });
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }
    }
}
