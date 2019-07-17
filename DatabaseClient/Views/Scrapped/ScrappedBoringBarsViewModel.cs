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
        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;

            ObservableCollection<BoringBarVM> _boringBars = new ObservableCollection<BoringBarVM>();
            var boringBars = await (from s in db.boring_bar
                                  orderby s.model
                                  where s.scrapping_date != null
                                  select s).ToListAsync();

            foreach (boring_bar boring in boringBars)
            {
                _boringBars.Add(new BoringBarVM { IsNew = false, TheEntity = boring });
            }
            BoringBars = _boringBars;
            RaisePropertyChanged("BoringBars");
            ThrobberVisible = Visibility.Collapsed;
        }
    }
}
