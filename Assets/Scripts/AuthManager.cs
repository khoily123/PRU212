using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Import thư viện UI
using System.Data.SqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

public class AuthManager : MonoBehaviour
{
    private string connectionString = "Server=(local);Database=DefenseTower;User Id=sa;Password=123;Encrypt=false;Trusted_Connection=True";

    public TMPro.TMP_InputField usernameInput;
    public TMPro.TMP_InputField passwordInput;
    public TMPro.TMP_Text messageText;

    public Button loginButton;  // 🔥 Thêm biến Button
    public Button registerButton;

    void Start()
    {
        // 🔥 Gán sự kiện Click cho Button
        loginButton.onClick.AddListener(Login);
        registerButton.onClick.AddListener(Register);
    }

    public void Login()
    {
        string username = usernameInput.text.Trim();
        string password = HashPassword(passwordInput.text.Trim());

        if (AuthenticateUser(username, password))
        {
            int maxLevel = GetMaxLevel(username); // Lấy Level cao nhất mà user chơi đến
            PlayerPrefs.SetString("LoggedInUser", username);
            PlayerPrefs.SetInt("MaxLevel", maxLevel); // Lưu vào bộ nhớ tạm

            messageText.text = "Đăng nhập thành công!";
            SceneManager.LoadScene("LevelSelection"); // Chuyển đến màn chọn level
        }
        else
        {
            messageText.text = "Sai tên đăng nhập hoặc mật khẩu!";
        }
    }


    private int GetMaxLevel(string username)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT MaxLevel FROM Players WHERE Username = @Username";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                object result = cmd.ExecuteScalar();
                return (result != null) ? Convert.ToInt32(result) : 1;
            }
        }
    }

    public void Register()
    {
        string username = usernameInput.text.Trim();
        string password = HashPassword(passwordInput.text.Trim());

        if (RegisterUser(username, password))
        {
            messageText.text = "Đăng ký thành công! Đang chuyển đến game...";
            SceneManager.LoadScene("LevelSelection"); // Load màn game sau khi đăng ký
        }
        else
        {
            messageText.text = "Tên đăng nhập đã tồn tại!";
        }
    }

    private bool AuthenticateUser(string username, string password)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM Players WHERE Username = @Username AND PasswordHash = @Password";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
    }

    private bool RegisterUser(string username, string password)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "INSERT INTO Players (Username, PasswordHash) VALUES (@Username, @Password)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException)
                {
                    return false; // Trường hợp username đã tồn tại
                }
            }
        }
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
