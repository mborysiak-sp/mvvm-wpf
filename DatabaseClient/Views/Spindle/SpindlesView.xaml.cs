using System.Windows.Controls;

namespace DatabaseClient
{
    /// <summary>
    /// Logika interakcji dla klasy SpindlesView.xaml
    /// </summary>
    public partial class SpindlesView : UserControl
    {
        public SpindlesView()
        {
            InitializeComponent();
            this.DataContext = new SpindlesViewModel();
        }

    }
}
