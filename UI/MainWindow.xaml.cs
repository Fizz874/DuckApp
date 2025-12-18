using System.Collections.ObjectModel;
using System.Configuration;
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
using Strzelecki_Baranowski.DuckApp.INTERFACES;
using Microsoft.Extensions.Configuration;
using Strzelecki_Baranowski.DuckApp.BL;


namespace Strzelecki_Baranowski.DuckApp.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //private readonly BLC _businessLogic;

        ////public ObservableCollection<IDuck> Ducks { get; set; }

        //public DuckListViewModel DuckVM { get; set; }
        //public ProducerListViewModel ProducerVM { get; set; }



        public MainWindow(MainViewModel MainVM)
        {
            InitializeComponent();
            //DataContext = new DuckListViewModel();
            //MainViewModel MainVM = new MainViewModel(businessLogic);
            DataContext = MainVM;


            //Lista.ItemsSource = DuckVM.Ducks;
            //EditView.Visibility= Visibility.Collapsed;

        }

        //private void ShowDucks_Click(object sender, RoutedEventArgs e)
        //{
        //    Lista.ItemsSource = DuckVM.Ducks;
        //}

        //private void ShowProducers_Click(object sender, RoutedEventArgs e)
        //{
        //    Lista.ItemsSource = ProducerVM.Producers;
        //}


        //private void ShowSpecifiedProducer_Click(object sender, RoutedEventArgs e)
        //{
        //    //var producer = (sender as Button)?.Tag;

        //    //Lista.ItemsSource = ProducerVM.Producers;   
        //    //Lista.SelectedItem = ProducerVM.Producers.FirstOrDefault(x => x.Name == producer); 

        //    var link = (Hyperlink)sender;
        //    var vm = link.DataContext as DuckViewModel;
        //    var producerName = vm?.ProducerName;

        //    Lista.ItemsSource = ProducerVM.Producers;
        //    if (!string.IsNullOrEmpty(producerName))
        //    {
        //        Lista.SelectedItem = ProducerVM.Producers
        //            .FirstOrDefault(p => p.Name == producerName);
        //    }


        //}


        //private void EditItem_Click(object sender, RoutedEventArgs e)
        //{

        //    //TODO - rozróżnienie między Duck i Producer
        //    EditView.Visibility = Visibility.Visible;
        //    DetailsView.Visibility = Visibility.Collapsed;
        //    return;
        //    // pobierz zaznaczony element z ListView
        //    var selected = Lista.SelectedItem as DuckViewModel;
        //    if (selected == null) return;

        //    // utwórz i otwórz okno edycji
        //    var dialog = new EditDuckWindow
        //    {
        //        DataContext = selected
        //    };

        //    // ShowDialog blokuje główne okno do czasu zamknięcia
        //    bool? result = dialog.ShowDialog();

        //    if (result == true)
        //    {
        //        // np. odśwież listę po edycji
        //        Lista.Items.Refresh();
        //    }
        //}


        //private void AddItem_Click(object sender, RoutedEventArgs e)
        //{
        //    //TODO
        //}

        //private void RemoveItem_Click(object sender, RoutedEventArgs e)
        //{
        //    //TODO
        //}




    }
}