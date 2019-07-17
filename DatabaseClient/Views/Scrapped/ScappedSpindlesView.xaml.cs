using System.Windows.Controls;

namespace DatabaseClient
{
    /// <summary>
    /// Logika interakcji dla klasy SpindlesView.xaml
    /// </summary>
    public partial class ScrappedSpindlesView : UserControl
    {
        public ScrappedSpindlesView()
        {
            InitializeComponent();
            this.DataContext = new SpindlesViewModel();
        }

    }
}
