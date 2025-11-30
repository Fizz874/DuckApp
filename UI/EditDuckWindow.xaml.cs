using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Strzelecki_Baranowski.DuckApp.UI
{
    /// <summary>
    /// Logika interakcji dla klasy EditDuckWindow.xaml
    /// </summary>
    public partial class EditDuckWindow : Window
    {
        public EditDuckWindow()
        {
            InitializeComponent();
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            //TODO

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Anulowanie – zamyka okno i zwraca false
            DialogResult = false;
            Close();
        }


    }
}
