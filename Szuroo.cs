using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Szuro
{
    internal class Szuroo
    {
        public class adatkezelo
        {
            static string connectionString = "server=localhost;database=auto_adatbazis;uid=root;pwd=root";

            public void LoadData(DataGrid adatterulet, string Marka, string Hasznalat)
            {
                string query = "$SELECT * FROM Markak WHERE (Marka=@Marka OR @Marka IS NULL) and (Hasznalat=@Hasznalat OR @Hasznalat IS NULL)";
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Marka", Marka);
                            cmd.Parameters.AddWithValue("@Hasznalat", Hasznalat);
                            MySqlDataAdapter adapter = new MySqlDataAdapter();
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            adatterulet.Columns.Clear();
                            adatterulet.ItemsSource = dt.DefaultView;
                            adatterulet.Columns.Add(new DataGridTextColumn
                            {
                                Header = "Termék Neve",
                                Binding = new System.Windows.Data.Binding("tnev")
                            });
                            adatterulet.Columns.Add(new DataGridTextColumn
                            {
                                Header = "Termék Márkája",
                                Binding = new System.Windows.Data.Binding("Marka")
                            });
                            adatterulet.Columns.Add(new DataGridTextColumn
                            {
                                Header = "Termék Használata",
                                Binding = new System.Windows.Data.Binding("Hasznalat")
                            });
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
