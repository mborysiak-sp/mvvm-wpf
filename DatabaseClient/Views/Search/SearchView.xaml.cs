using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace DatabaseClient
{
    /// <summary>
    /// Logika interakcji dla klasy SearchView.xaml
    /// </summary>
    public partial class SearchView : UserControl
    {
        public SearchView()
        {
            InitializeComponent();
            DataContext = new SearchViewModel();
        }
    }
}

