using Microsoft.Win32.SafeHandles;
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

namespace Szuro
{
    public partial class MainWindow : Window
    {
        adatkezelo peldany = new adatkezelo();  // Az adat osztály példánya
        string marka = null;
        string hasznalat = null;

        public MainWindow()
        {
            InitializeComponent();

            peldany.LoadData(MyDataGrid, null, null);  // Adja át a DataGrid-et és a paramétereket
            peldany.LoadCombo(Marka, "Marka", "Markak");
            peldany.LoadCombo(Hasznalat, "Hasznalat", "hasznalat");
        }

        private void Marka_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}