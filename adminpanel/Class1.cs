using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;

namespace adminpanel
{
    internal class Class1
    {
        // Felhasználók listája
        private static List<string> users = new List<string>
        {
            "Kiss László", "Nagy Anna", "Szabó Béla", "Tóth Katalin", "Horváth Gábor",
            "Varga Erzsébet", "Kovács Péter", "Molnár Zsófia", "Németh István", "Farkas Anikó",
            "Balogh Ferenc", "Simon Andrea", "Lukács János", "Nagy Zoltán", "Kiss Éva",
            "Tóth Tamás", "Molnár Attila", "Fekete Ildikó", "Varga László", "Németh Gergely",
            "Balogh Norbert", "Farkas Anikó", "Tóth Éva", "Szabó Eszter", "Miklós Gábor",
            "Hegedűs Miklós", "Horváth Lajos", "Nagy Judit", "Tóth Béla", "Kis Anna",
            "Kovács Zsolt", "Varga Gergely", "Nagy Gábor", "Kiss Judit", "Balázs László",
            "Kovács Ilona", "Juhász Andrea", "Kiss Gábor", "Molnár Réka", "Németh Péter"
        };

        private const string ConnectionString = "Server=localhost;Database=auto_adatbazis;Uid=root;Pwd=root;";

        // SHA256 titkosítás
        private static string HashPassword(string password)
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

        // Véletlenszerű jelszó generálása
        private static string GenerateRandomPassword()
        {
            var rand = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[rand.Next(s.Length)]).ToArray());
        }

        // Felhasználók jelszó generálása és adatbázis frissítése
        public static void UpdatePasswords()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                for (int i = 0; i < users.Count; i++)
                {
                    string user = users[i];
                    string randomPassword = GenerateRandomPassword();
                    string hashedPassword = HashPassword(randomPassword);

                    // Frissítjük a jelszót az adatbázisban
                    string updateQuery = "UPDATE regisztracio SET jelszo = @password WHERE email = @email";
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@email", $"{user.ToLower().Replace(" ", ".")}@email.hu"); // Feltételezve, hogy az email címek így épülnek
                    cmd.ExecuteNonQuery();

                    Console.WriteLine($"{i + 1}. {user} - Jelszó: {randomPassword} (Hash: {hashedPassword})");
                }
            }
        }
    }
}
