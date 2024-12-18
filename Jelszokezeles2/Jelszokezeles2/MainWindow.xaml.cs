using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Diagnostics.Contracts;

namespace Jelszokezeles2
{
    public partial class MainWindow : Window
    {
        public class Data
        {
            string connectionData = "server=localhost;database=jelszokezeles;uid=vasarlo2;pwd=vasarlo2";

            public bool adatlekerdez(string felhasznalo)
            {
                bool van = true;
                try
                {
                    MySqlConnection conn = new MySqlConnection(connectionData);
                    conn.Open();
                    string query = "select nev from felhasznalok where nev=@param";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@param", felhasznalo);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (!dr.Read()) van = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return van;
            }

            public void adatbeir(string nev, string jelszo)
            {
                try
                {
                    if (!adatlekerdez(nev))
                    {
                        MySqlConnection conn = new MySqlConnection(connectionData);
                        conn.Open();
                        string query = "insert into felhasznalok(nev,jelszo) values (@param1,@param2)";
                        MySqlCommand command = new MySqlCommand(query, conn);
                        command.Parameters.AddWithValue("@param1", nev);
                        command.Parameters.AddWithValue("@param2", jelszo);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Sikeres adatfelvétel", "Siker!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else MessageBox.Show("A felhasználóvén már foglalt!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            public string titkosit(string input)
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                    StringBuilder builder = new StringBuilder();
                    foreach (byte b in bytes)
                    {
                        builder.Append(b.ToString("x2"));
                    }
                    return builder.ToString();
                }
            }

        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(felhasznalo.Text) && !string.IsNullOrWhiteSpace(jelszo.Password))
            {
                string felhasznalonevp = felhasznalo.Text;
                string jelszop = jelszo.Password;
                Data peldany = new Data();
                bool eredmeny = peldany.adatlekerdez(felhasznalonevp);
                if (eredmeny) 
                {
                    MessageBox.Show("A felhasználónév már foglalt!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string kodolt = peldany.titkosit(jelszop);
                    peldany.adatbeir(felhasznalonevp, kodolt);
                    felhasznalo.Clear();
                    jelszo.Clear();
                }
            }
            else
            {
                MessageBox.Show("Hiányos adatok!");
            }
        }
    }
}