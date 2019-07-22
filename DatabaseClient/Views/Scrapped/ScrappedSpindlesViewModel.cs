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
        public ObservableCollection<SpindleVM> ScrappedSpindles { get; set; }
        public ScrappedSpindlesViewModel()
            : base()
        {

        }
        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;

            ObservableCollection<SpindleVM> _scrappedSpindles = new ObservableCollection<SpindleVM>();
            var spindles = await (from s in db.spindle
                                   orderby s.model
                                    where s.scrapping_date != null
                                   select s).ToListAsync();

            foreach (spindle spin in spindles)
            {
                _scrappedSpindles.Add(new SpindleVM { IsNew = false, TheEntity = spin });
            }
            ScrappedSpindles = _scrappedSpindles;
            RaisePropertyChanged("ScrappedSpindles");
            ThrobberVisible = Visibility.Collapsed;
        }
    }
}
