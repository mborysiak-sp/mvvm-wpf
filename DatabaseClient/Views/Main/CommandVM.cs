using DatabaseClient.Messages;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using System.Windows.Media;

namespace DatabaseClient
{
    public class CommandVM
    {
        public string CommandDisplay { get; set; }
        public CommandMessage Message { get; set; }
        public RelayCommand Send { get; private set; }
        public Geometry IconGeometry { get; set; }

        private bool canExecute = true;
        public bool CanExecute
        {
            get
            {
                return canExecute = true;
            }
            set
            {
                canExecute = value;
                RaiseCanExecuteChanged();
            }
        }

        public CommandVM()
        {
            Send = new RelayCommand(() => SendExecute());
        }

        private void SendExecute()
        {
            Messenger.Default.Send<CommandMessage>(Message);
        }
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
