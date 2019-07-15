using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Logika interakcji dla klasy DodajLozyskoView.xaml
    /// </summary>
    public partial class AddBearingView : UserControl
    {
        public AddBearingView()
        {
            InitializeComponent();
        }
        public void DodajLozysko(object sender, RoutedEventArgs e)
        {
            string conString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                string sql = "INSERT INTO lozysko (typ) VALUES (@typ)";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("typ", dodajLozyskoTyp.Text);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Wprowadzono łożysko");
                con.Close();
            }
        }
    }
}
