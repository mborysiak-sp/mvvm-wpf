using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

namespace DatabaseClient
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(
                    CultureInfo.CurrentCulture.IetfLanguageTag)));
            base.OnStartup(e);
        }
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            MessageBox.Show("An unexpected exception has occurred. Shutting down the application. Please check the log file for more details.");
            // Prevent default unhandled exception processing
            args.Handled = true;
            Environment.Exit(0);
        }
    }
}
