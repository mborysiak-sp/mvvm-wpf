using DatabaseClient.EntityData;
using DatabaseClient.Messages;
using GalaSoft.MvvmLight.Messaging;
using Support;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Threading;

namespace DatabaseClient
{
    public class ScrappedBoringBarsViewModel : BoringBarsViewModel
    {
        public ObservableCollection<BoringBarVM> ScrappedBoringBars { get; set; }
        public ScrappedBoringBarsViewModel()
            : base()
        {

        }
        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;

            ObservableCollection<BoringBarVM> _scrappedBoringBars = new ObservableCollection<BoringBarVM>();
            var scrappedBoringBars = await (from s in db.boring_bar
                                  orderby s.model
                                  where s.scrapping_date != null
                                  select s).ToListAsync();

            foreach (boring_bar scrappedBoring in scrappedBoringBars)
            {
                _scrappedBoringBars.Add(new BoringBarVM { IsNew = false, TheEntity = scrappedBoring });
            }
            ScrappedBoringBars = _scrappedBoringBars;
            RaisePropertyChanged("ScrappedBoringBars");
            ThrobberVisible = Visibility.Collapsed;
        }
    }
}
