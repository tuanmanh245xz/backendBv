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
        public bool AuthenticateUser(string username, string password)
        {
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM Users Where UserName = @UserName AND Password = @Password";
                //open connection
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Password", password); // In a real application, ensure passwords are hashed and salted
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result > 0;
            }
        }
        public bool RegisterUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //check if user already exists
                string checkQuery = "SELECT COUNT(1) FROM Users WHERE UserName = @UserName";
                SqlCommand cmd = new SqlCommand(checkQuery, connection);
                cmd.Parameters.AddWithValue("@UserName", username);
                int userExists = Convert.ToInt32(cmd.ExecuteScalar());
                if (userExists > 0)
                {
                    return false; // User already exists
                }
                //insert new user
                string insertQuery = "INSERT INTO Users (UserName, Password) VALUES (@UserName, @Password)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, connection);
                insertCmd.Parameters.AddWithValue("@UserName", username);
                insertCmd.Parameters.AddWithValue("@Password", password); // In a real application, ensure passwords are hashed and salted
                int rowsAffected = insertCmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}
