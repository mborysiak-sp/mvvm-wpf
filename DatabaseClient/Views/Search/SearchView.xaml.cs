using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace DatabaseClient.Views
{
    /// <summary>
    /// Logika interakcji dla klasy SearchView.xaml
    /// </summary>
    public partial class SearchView : UserControl
    {
        public SearchView()
        {
            InitializeComponent();
        }
        private void SearchForItem(object sender, RoutedEventArgs e)
        {
            string conString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            string sql = "SELECT * FROM wszystkie_dokumenty WHERE numer = @number AND typ = @type";
            using (SqlConnection con = new SqlConnection(conString))
            { 
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("type", textboxSearchForItemType.Text);
                    cmd.Parameters.AddWithValue("number", textboxSearchForItemNumber.Text);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable("Documents");
                    sda.Fill(dt);
                    gridSearchResult.ItemsSource = dt.DefaultView;
                }
                con.Close();
            }
        }
    }
}

