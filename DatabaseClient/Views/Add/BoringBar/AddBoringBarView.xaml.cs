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
    /// Logika interakcji dla klasy DodajWytaczadloView.xaml
    /// </summary>
    public partial class AddBoringBarView : UserControl
    {
        public AddBoringBarView()
        {
            InitializeComponent();
        }

        public void DodajWytaczadlo(object sender, RoutedEventArgs e)
        {
            string conString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            int wytaczadloID, lozyskoID;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                string sql = "SELECT id_lozysko FROM lozysko WHERE typ = @typ";
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("typ", dodajWytaczadloLozysko.Text);
                    lozyskoID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                con.Close();
            }
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                string sql = "SELECT id_wytaczadlo FROM wytaczadlo ORDER BY id_wytaczadlo DESC";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("nazwa", dodajWytaczadloLozysko.Text);
                    wytaczadloID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                con.Close();
            }
            using (SqlConnection con = new SqlConnection(conString))
            {
                string sql = "INSERT INTO wytaczadlo (typ, nr_rysunku) VALUES (@typ,@nrRysunku)";
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("typ", dodajWytaczadloTyp.Text);
                    cmd.Parameters.AddWithValue("nrRysunku", dodajWytaczadloNumerRysunku.Text);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Wprowadzono wytaczadło");
                con.Close();
            }
            using (SqlConnection con = new SqlConnection(conString))
            {
                string sql = "INSERT INTO wytaczadlo_lozysko (id_wytaczadlo, id_lozysko) VALUES (@wytaczadloID, @lozyskoID)";
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("wytaczadloID", wytaczadloID + 1);
                    cmd.Parameters.AddWithValue("lozyskoID", lozyskoID);
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }
    }
}

