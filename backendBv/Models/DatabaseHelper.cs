using System.Data.SqlClient;

namespace backendBv.Models
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;
        //connection string lấy từ appsettings.json
        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        //xu ly login voi csdl sql server
        // Kiểm tra đăng nhập người dùng
        public bool AuthenticateUser(string username, string password, out string role)
        {
            role = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT Role FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password); // Chú ý cần mã hóa mật khẩu trong thực tế

                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    role = result.ToString();
                    return true;
                }
            }
            return false;
        }
        // Đăng ký người dùng
        public bool RegisterUser(string username, string password, string role)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);

                int userExists = (int)cmd.ExecuteScalar();
                if (userExists > 0)
                {
                    return false; // Tên người dùng đã tồn tại
                }

                string insertQuery = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@Username", username);
                insertCmd.Parameters.AddWithValue("@Password", password); // Mã hóa mật khẩu trong thực tế
                insertCmd.Parameters.AddWithValue("@Role", role);

                int rowsAffected = insertCmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}
