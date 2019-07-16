using DatabaseClient.Messages;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Media.Animation;

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
            Style = (Style)FindResource(typeof(Window));
            Messenger.Default.Register<NavigateMessage>(this, (action) => ShowUserControl(action));
            Messenger.Default.Register<UserMessage>(this, (action) => ReceiveUserMessage(action));
            Messenger.Default.Register<InEdit>(this, (action) => ReceiveInEditMessage(action));
            this.DataContext = new MainWindowViewModel();
        }
        private void ReceiveInEditMessage(InEdit inEdit)
        {
            CommandTab.IsEnabled = !inEdit.Mode;
        }
        private void ReceiveUserMessage(UserMessage msg)
        {
            UIMessage.Opacity = 1;
            UIMessage.Text = msg.Message;
            Storyboard sb = (Storyboard)this.FindResource("FadeUIMessage");
            sb.Begin();
        }
        private void ShowUserControl(NavigateMessage nm)
        {
            CommandTab.SelectedItem = EditTabItem;
            Holder.Content = nm.View;
        }
        //private void MenuTabele_Click(object sender, RoutedEventArgs e)
        //{
        //    DataContext = new SpindlesViewModel();
        //}
        //private void MenuFormularzeDodajWytaczadlo_Click(object sender, RoutedEventArgs e)
        //{
        //    DataContext = new AddBoringBarViewModel();
        //}
        //private void MenuFormularzeDodajLozysko_Click(object sender, RoutedEventArgs e)
        //{
        //    DataContext = new AddBearingViewModel();
        //}
        //private void MenuSearch_Click(object sender, RoutedEventArgs e)
        //{
        //    DataContext = new SearchViewModel();
        //}
        //private void MenuFormularzeDodajDokument_Click(object sender, RoutedEventArgs e)
        //{
        //    DataContext = new AddDocumentViewModel();
        //}
    }
}

