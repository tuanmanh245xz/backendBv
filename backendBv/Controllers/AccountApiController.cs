using backendBv.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace backendBv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        //lay danh sach nguoi dung tu DatabaseHelper
        private readonly DatabaseHelper _databaseHelper;
        public AccountApiController(DatabaseHelper databaseHelper) 
        {
            _databaseHelper = databaseHelper;
        }
        // POST: api/AccountApi/login
        //api dang nhap
        [HttpPost("login")]
        public IActionResult Login([FromBody] Users loginUser)
        {
            try
            {
                string role;
                bool isAuthenticated = _databaseHelper.AuthenticateUser(loginUser.Username, loginUser.Password,out role);
                // Kiểm tra thông tin đăng nhập
                //var user = Users.FirstOrDefault(u => u.Username == loginUser.Username && u.Password == loginUser.Password);
                if( !isAuthenticated)
                {
                    return Unauthorized("Invalid credentials.");
                }
                //if (user == null)
                //{
                //    return Unauthorized("Invalid credentials.");
                //}

                // Lưu thông tin người dùng vào session
                HttpContext.Session.SetString("Username", loginUser.Username);
                HttpContext.Session.SetString("Role", role);

                return Ok(new { Message = "Login successful" ,Role = role});
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
            {   bool isRegistered = _databaseHelper.RegisterUser(registerUser.Username, registerUser.Password, registerUser.Role);
                //kiem tra tai khoan da ton tai chua
                //var existingUser = Users.FirstOrDefault(u => u.Username == registerUser.Username,);
                //if(existingUser != null) 
                //{
                //    return BadRequest("Username already exists.");
                //}
                ////them tai khoan moi
                //Users.Add(registerUser);
                //return Ok(new { Message = "Registration successful" });
                if (!isRegistered) 
                {
                    return BadRequest("Username already exists.");
                }
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
