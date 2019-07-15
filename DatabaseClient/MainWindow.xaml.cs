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
using GalaSoft.MvvmLight.Messaging;
using DatabaseClient.Messages;
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
            this.DataContext = new MainWindowViewModel();
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
            EditFrame.Content = nm.View;
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

