using DatabaseClient.Messages;
using Support;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace DatabaseClient
{
    public class MainWindowViewModel : NotifyUIBase
    {
        public ObservableCollection<CommandVM> Commands { get; set; }
        public ObservableCollection<ViewVM> Views { get; set; }

        public string Message { get; set; }

        public MainWindowViewModel()
        {
            ObservableCollection<ViewVM> views = new ObservableCollection<ViewVM>
            {
                new ViewVM { ViewDisplay="Spindles", ViewType = typeof(SpindlesView), ViewModelType = typeof(SpindlesViewModel)},
                new ViewVM { ViewDisplay="BoringBars", ViewType = typeof(BoringBarsView), ViewModelType = typeof(BoringBarsViewModel)},
                new ViewVM { ViewDisplay="Bearings", ViewType = typeof(BearingsView), ViewModelType = typeof(BearingsViewModel)},
                new ViewVM { ViewDisplay="ScrappedSpindles", ViewType = typeof(ScrappedSpindlesView), ViewModelType = typeof(ScrappedSpindlesViewModel)},
                new ViewVM { ViewDisplay="ScrappedBoringBars", ViewType = typeof(ScrappedBoringBarsView), ViewModelType = typeof(ScrappedBoringBarsViewModel)},
                new ViewVM { ViewDisplay="AllDocuments", ViewType = typeof(AllDocumentsView), ViewModelType = typeof(AllDocumentsViewModel)},
                new ViewVM { ViewDisplay="DocumentBoringBar", ViewType = typeof(DocumentBoringBarView), ViewModelType = typeof(DocumentBoringBarViewModel)},
                new ViewVM { ViewDisplay="DocumentSpindle", ViewType = typeof(DocumentSpindleView), ViewModelType = typeof(DocumentSpindleViewModel)}
            };
            Views = views;
            RaisePropertyChanged("Views");
            views[0].NavigateExecute();

            ObservableCollection<CommandVM> commands = new ObservableCollection<CommandVM>
            {
                new CommandVM{ CommandDisplay="Insert", IconGeometry=Application.Current.Resources["InsertIcon"] as Geometry , Message=new CommandMessage{ Command =CommandType.Insert}},
                new CommandVM{ CommandDisplay="Edit", IconGeometry=Application.Current.Resources["EditIcon"] as Geometry , Message=new CommandMessage{ Command = CommandType.Edit}},
                new CommandVM{ CommandDisplay="Delete", IconGeometry=Application.Current.Resources["DeleteIcon"] as Geometry , Message=new CommandMessage{ Command = CommandType.Delete}},
                new CommandVM{ CommandDisplay="Refresh", IconGeometry=Application.Current.Resources["RefreshIcon"] as Geometry , Message=new CommandMessage{ Command = CommandType.Refresh}}
            };
            Commands = commands;
            RaisePropertyChanged("Commands");

        }
    }
}
