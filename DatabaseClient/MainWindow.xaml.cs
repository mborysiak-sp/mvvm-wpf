using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using DatabaseClient.ViewModels;

namespace DatabaseClient
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
       
        private void MenuTabele_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new TablesViewModel();
        }
        private void MenuFormularzeDodajWytaczadlo_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new DodajWytaczadloViewModel();
        }
        private void MenuFormularzeDodajLozysko_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new DodajLozyskoViewModel();
        }
        private void MenuSearch_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new SearchViewModel();
        }
        private void MenuFormularzeDodajDokument_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new AddDocumentViewModel();
        }
    }
}

