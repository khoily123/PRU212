using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    public Button loginButton;
    public Button registerButton;

    void Start()
    {
        loginButton.onClick.AddListener(Login);
        registerButton.onClick.AddListener(Register);
    }

    public void Login()
    {
        string username = usernameInput.text.Trim();
        string password = HashPassword(passwordInput.text.Trim());

        int playerId = AuthenticateUser(username, password);
        if (playerId > 0)
        {
            int maxLevel = GetMaxLevel(playerId);
            PlayerPrefs.SetInt("PlayerId", playerId);
            PlayerPrefs.SetString("LoggedInUser", username);
            PlayerPrefs.SetInt("MaxLevel", maxLevel);

            messageText.text = "Đăng nhập thành công!";
            SceneManager.LoadScene("LevelSelection");
        }
        else
        {
            messageText.text = "Sai tên đăng nhập hoặc mật khẩu!";
        }
    }

    private int GetMaxLevel(int playerId)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT MaxLevel FROM Players WHERE Id = @PlayerId";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@PlayerId", playerId);
                object result = cmd.ExecuteScalar();
                return (result != null) ? Convert.ToInt32(result) : 1;
            }
        }
    }

    public void Register()
    {
        string username = usernameInput.text.Trim();
        string password = HashPassword(passwordInput.text.Trim());

        int playerId = RegisterUser(username, password);
        if (playerId > 0)
        {
            PlayerPrefs.SetInt("PlayerId", playerId);
            PlayerPrefs.SetString("LoggedInUser", username);
            PlayerPrefs.SetInt("MaxLevel", 1);
            messageText.text = "Đăng ký thành công! Đang chuyển đến game...";
            SceneManager.LoadScene("LevelSelection");
        }
        else
        {
            messageText.text = "Tên đăng nhập đã tồn tại!";
        }
    }

    private int AuthenticateUser(string username, string password)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT Id FROM Players WHERE Username = @Username AND PasswordHash = @Password";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                object result = cmd.ExecuteScalar();
                return (result != null) ? Convert.ToInt32(result) : -1;
            }
        }
    }

    private int RegisterUser(string username, string password)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "INSERT INTO Players (Username, PasswordHash) OUTPUT INSERTED.Id VALUES (@Username, @Password)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                try
                {
                    object result = cmd.ExecuteScalar();
                    return (result != null) ? Convert.ToInt32(result) : -1;
                }
                catch (SqlException)
                {
                    return -1; // Trường hợp username đã tồn tại
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
