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
        protected override void EditCurrent()
        {
            EditVM = SelectedBoringBar;
            IsInEditMode = true;
        }
        protected override void InsertNew()
        {
            EditVM = new BoringBarVM();
            IsInEditMode = true;
        }
        protected override void CommitUpdates()
        {
            if (EditVM == null || EditVM.TheEntity == null)
            {
                if (db.ChangeTracker.HasChanges())
                {
                    UpdateDB();
                }
                return;
            }
            if (EditVM.TheEntity.IsValid())
            {
                if (EditVM.IsNew)
                {
                    EditVM.IsNew = false;
                    BoringBars.Add(EditVM);
                    db.boring_bar.Add(EditVM.TheEntity);
                    UpdateDB();
                }
                else if (db.ChangeTracker.HasChanges())
                {
                    UpdateDB();
                }
                else
                {
                    ShowUserMessage("No changes to save");
                }
            }
            else
            {
                ShowUserMessage("There are problems with the data entered");
            }
        }
        private async void UpdateDB()
        {
            try
            {
                await db.SaveChangesAsync();
                ShowUserMessage("Database Updated");
            }
            catch (Exception e)
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    ErrorMessage = e.InnerException.GetBaseException().ToString();
                }
                ShowUserMessage("There was a problem updating the database");
            }
            ReFocusRow();
        }
        protected override void DeleteCurrent()
        {
            db.boring_bar.Remove(SelectedBoringBar.TheEntity);
            BoringBars.Remove(SelectedBoringBar);
            RaisePropertyChanged("BoringBars");
            CommitUpdates();
            selectedEntity = null;
        }
        protected override void Quit()
        {
            ReFocusRow();
        }
        protected new void ReFocusRow(bool withReload = true)
        {
            SelectedBoringBar = null;
            IsInEditMode = false;
        }

        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;

            ObservableCollection<BoringBarVM> _scrappedBoringBars = new ObservableCollection<BoringBarVM>();
            ObservableCollection<BearingVM> _bearings = new ObservableCollection<BearingVM>();

            var scrappedBoringBars = await (from s in db.boring_bar
                                            orderby s.model
                                            where s.scrapping_date != null
                                            select s).ToListAsync();

            var bearings = await (from b in db.bearing
                                  orderby b.id
                                  select b).ToListAsync();

            foreach (boring_bar scrappedBoring in scrappedBoringBars)
            {
                _scrappedBoringBars.Add(new BoringBarVM { IsNew = false, TheEntity = scrappedBoring });
            }

            foreach (bearing bear in bearings)
            {
                _bearings.Add(new BearingVM { IsNew = false, TheEntity = bear });
            }
            ScrappedBoringBars = _scrappedBoringBars;
            RaisePropertyChanged("ScrappedBoringBars");
            Bearings = _bearings;
            RaisePropertyChanged("Bearings");
            ThrobberVisible = Visibility.Collapsed;
        }
    }
}
