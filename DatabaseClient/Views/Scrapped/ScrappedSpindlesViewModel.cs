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
    public class ScrappedSpindlesViewModel : SpindlesViewModel
    {
        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;

            ObservableCollection<SpindleVM> _spindles = new ObservableCollection<SpindleVM>();
            var spindles = await (from s in db.spindle
                                   orderby s.model
                                    where s.scrapping_date != null
                                   select s).ToListAsync();

            foreach (spindle spin in spindles)
            {
                _spindles.Add(new SpindleVM { IsNew = false, TheEntity = spin });
            }
            Spindles = _spindles;
            RaisePropertyChanged("Spindles");
            ThrobberVisible = Visibility.Collapsed;
        }
    }
}
