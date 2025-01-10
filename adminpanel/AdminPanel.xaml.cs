using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace AuthApp
{
    public partial class AdminPanel : Window
    {
        private readonly string _connectionString = "Server=localhost;Database=auto_adatbazis;Uid=root;Pwd=root;";
        private string _selectedTable;

        public AdminPanel()
        {
            InitializeComponent();
            LoadTables();
        }

        private async void LoadTables()
        {
            using (var db = new MySqlConnection(_connectionString))
            {
                try
                {
                    await db.OpenAsync();
                    string query = "SHOW TABLES";
                    var cmd = new MySqlCommand(query, db);
                    var reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        TableComboBox.Items.Add(reader.GetString(0));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba a tablak betoltesekor: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void LoadData()
        {
            if (string.IsNullOrEmpty(_selectedTable))
            {
                MessageBox.Show("Valassz egy tablat!", "Figyelmeztetes", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new MySqlConnection(_connectionString))
            {
                try
                {
                    await db.OpenAsync();
                    string query = $"SELECT * FROM {_selectedTable}";
                    var adapter = new MySqlDataAdapter(query, db);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    CarDataGrid.ItemsSource = dataTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba az adatok betoltese soran: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string GetPrimaryKeyColumn(string tableName)
        {
            return tableName switch
            {
                "autok" => "Auto_ID",
                "markak" => "Marka_ID",
                "szinek" => "Szin_ID",
                "modellek" => "Modell_ID",
                "hasznalat" => "Hasznalat_ID",
                "dolgozo" => "id",
                "eladok" => "Elado_ID",
                "sebessegvaltok" => "Sebessegvalto_ID",
                "motortipusok" => "Motortipus_ID",
                "motorspecifikaciok" => "Motorspecifikacio_ID",
                "parkolo" => "parkolo",
                "regisztracio" => "id",
                _ => null // Ismeretlen tabla
            };
        }

        private async Task<List<string>> GetColumnNamesAsync(string tableName)
        {
            var columns = new List<string>();
            using (var db = new MySqlConnection(_connectionString))
            {
                try
                {
                    await db.OpenAsync();
                    string query = $"SHOW COLUMNS FROM {tableName}";
                    var cmd = new MySqlCommand(query, db);
                    var reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        columns.Add(reader.GetString(0)); // Az oszlop neve az első mező
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba az oszlopok lekerdezese soran: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return columns;
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedTable))
            {
                MessageBox.Show("Valassz egy tablat!", "Figyelmeztetes", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var columnNames = await GetColumnNamesAsync(_selectedTable);

            if (columnNames.Count == 0)
            {
                MessageBox.Show("Nem sikerult oszlopokat betolteni a tablahoz.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Add meg az adatokat vesszovel elvalasztva a kovetkezo sorrendben:\n{string.Join(", ", columnNames)}",
                "Adat Hozzaadasa", "");

            var values = input.Split(',');

            if (values.Length != columnNames.Count)
            {
                MessageBox.Show("A megadott adatok szama nem egyezik az oszlopok szamaval!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new MySqlConnection(_connectionString))
            {
                try
                {
                    await db.OpenAsync();
                    string query = $"INSERT INTO {_selectedTable} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", values.Select((_, i) => $"@param{i}"))})";
                    var cmd = new MySqlCommand(query, db);

                    for (int i = 0; i < values.Length; i++)
                        cmd.Parameters.AddWithValue($"@param{i}", values[i]);

                    await cmd.ExecuteNonQueryAsync();
                    MessageBox.Show("Uj adat hozzaadva!");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba az adat hozzaadasa soran: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarDataGrid.SelectedItem is not DataRowView rowView)
            {
                MessageBox.Show("Valassz ki egy sort!", "Figyelmeztetes", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string primaryKeyColumn = GetPrimaryKeyColumn(_selectedTable);

            if (string.IsNullOrEmpty(primaryKeyColumn))
            {
                MessageBox.Show("Nem talalhato primer kulcs a kivalasztott tablahoz!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var columnNames = await GetColumnNamesAsync(_selectedTable);

            if (columnNames.Count == 0)
            {
                MessageBox.Show("Nem sikerult oszlopokat betolteni a tablahoz.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Add meg az adatokat vesszovel elvalasztva a kovetkezo sorrendben:\n{string.Join(", ", columnNames)}",
                "Adatok Modositasa", "");

            var values = input.Split(',');

            if (values.Length != columnNames.Count)
            {
                MessageBox.Show("A megadott adatok szama nem egyezik az oszlopok szamaval!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new MySqlConnection(_connectionString))
            {
                try
                {
                    await db.OpenAsync();
                    string query = $"UPDATE {_selectedTable} SET {string.Join(", ", columnNames.Select((col, i) => $"{col} = @param{i}"))} WHERE {primaryKeyColumn} = @id";
                    var cmd = new MySqlCommand(query, db);

                    for (int i = 0; i < values.Length; i++)
                        cmd.Parameters.AddWithValue($"@param{i}", values[i]);

                    cmd.Parameters.AddWithValue("@id", rowView[primaryKeyColumn]);
                    await cmd.ExecuteNonQueryAsync();
                    MessageBox.Show("Adatok frissitve!");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba az adatok modositasa soran: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarDataGrid.SelectedItem is not DataRowView rowView)
            {
                MessageBox.Show("Valassz ki egy sort!", "Figyelmeztetes", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string primaryKeyColumn = GetPrimaryKeyColumn(_selectedTable);

            if (string.IsNullOrEmpty(primaryKeyColumn))
            {
                MessageBox.Show("Nem talalhato primer kulcs a kivalasztott tablahoz!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var db = new MySqlConnection(_connectionString))
            {
                try
                {
                    await db.OpenAsync();
                    string query = $"DELETE FROM {_selectedTable} WHERE {primaryKeyColumn} = @id";
                    var cmd = new MySqlCommand(query, db);
                    cmd.Parameters.AddWithValue("@id", rowView[primaryKeyColumn]);

                    await cmd.ExecuteNonQueryAsync();
                    MessageBox.Show("Adat torolve!");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba az adat torlese soran: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedTable = TableComboBox.SelectedItem?.ToString();
            LoadData();
        }

        private void CarDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CarDataGrid.SelectedItem is DataRowView rowView)
            {
                MessageBox.Show($"Kivalasztott sor: {rowView[0]}");
            }
        }
    }
}