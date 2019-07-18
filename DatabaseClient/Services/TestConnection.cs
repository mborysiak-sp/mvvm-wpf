using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace DatabaseClient
{
    class TestConnection
    {
        public TestConnection()
        {
            Connect();
        }

        private void Connect()
        {
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}



