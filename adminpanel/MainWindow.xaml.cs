using System;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using MySql.Data.MySqlClient;

namespace AuthApp
{
    public partial class MainWindow : Window
    {
        private const string ConnectionString = "Server=localhost;Database=auto_adatbazis;Uid=root;Pwd=root;";

        public MainWindow()
        {
            InitializeComponent();
        }

        // SHA256 titkosítás
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string email = RegisterUsername.Text;
            string password = RegisterPassword.Password;
            string name = RegisterName.Text;
            string phone = RegisterPhone.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Minden mezőt ki kell tölteni!");
                return;
            }

            string hashedPassword = HashPassword(password);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();

                    // Ellenőrizd, hogy az email cím már létezik-e
                    string checkQuery = "SELECT COUNT(*) FROM regisztracio WHERE email = @username";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@username", email);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Az email cím már regisztrálva van. Jelentkezz be!");
                        return;
                    }

                    // Ha nincs regisztrálva, hajtsuk végre a regisztrációt
                    string query = "INSERT INTO regisztracio (email, jelszo, nev, telefon) VALUES (@username, @password, @name, @phone)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", email);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Sikeres regisztráció!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt: " + ex.Message);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = LoginUsername.Text;
            string password = LoginPassword.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Minden mezőt ki kell tölteni!");
                return;
            }

            string hashedPassword = HashPassword(password);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT jelszo FROM regisztracio WHERE email = @username";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", email);

                    // Az adatbázisban lévő hashelt jelszó lekérése
                    string storedHashedPassword = cmd.ExecuteScalar()?.ToString();

                    if (storedHashedPassword == null)
                    {
                        MessageBox.Show("Az email cím nem található!");
                        return;
                    }

                    // Kiírjuk a naplóba a jelszavakat a hiba elhárításához
                    Console.WriteLine($"Bejegyzett hash: {storedHashedPassword}");
                    Console.WriteLine($"Bejelentkezés hash: {hashedPassword}");

                    // Az összehasonlítás, hogy megegyeznek-e a hash-ek
                    if (storedHashedPassword == hashedPassword)
                    {
                        MessageBox.Show("Sikeres bejelentkezés!");

                        // AdminPanel megnyitása
                        AdminPanel adminPanel = new AdminPanel();
                        adminPanel.Show();
                        this.Close(); // MainWindow bezárása
                    }
                    else
                    {
                        MessageBox.Show("Hibás email cím vagy jelszó!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt: " + ex.Message);
            }
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
