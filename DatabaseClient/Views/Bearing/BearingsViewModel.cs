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
    public class BearingsViewModel : CrudVMBase
    {
        private BearingVM selectedBearing;
        public BearingVM SelectedBearing
        {
            get
            {
                return selectedBearing;
            }
            set
            {
                selectedBearing = value;
                selectedEntity = value;
                RaisePropertyChanged();
            }
        }

        private BearingVM editVM;
        public BearingVM EditVM
        {
            get
            {
                return editVM;
            }
            set
            {
                editVM = value;
                editEntity = editVM.TheEntity;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<BearingVM> Bearings { get; set; }
        public BearingsViewModel()
            : base()
        {

        }
        protected override void EditCurrent()
        {
            EditVM = SelectedBearing;
            IsInEditMode = true;
        }
        protected override void InsertNew()
        {
            EditVM = new BearingVM();
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
                    Bearings.Add(EditVM);
                    db.bearing.Add(EditVM.TheEntity);
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
            catch (Exception)
            {
                ShowUserMessage("There was a problem updating the database");
            }
            ReFocusRow();
        }
        protected override void DeleteCurrent()
        {
            int NumDocs = NumberOfAssignedBoringBars();
            if (NumDocs > 0)
            {
                ShowUserMessage(string.Format("Cannot delete - there are {0} Orders for this Customer", NumDocs));
            }
            else
            {
                db.bearing.Remove(SelectedBearing.TheEntity);
                Bearings.Remove(SelectedBearing);
                RaisePropertyChanged("Bearings");
                CommitUpdates();
                selectedEntity = null;
            }
        }
        protected override void Quit()
        {
            if (!EditVM.IsNew)
            {
                ReFocusRow();
            }
        }
        protected void ReFocusRow(bool withReload = true)
        {
            //int id = EditVM.TheEntity.id;
            //SelectedBoringBar = null;
            //await db.Entry(EditVM.TheEntity).ReloadAsync();
            //await Application.Current.Dispatcher.InvokeAsync(new Action(() =>
            //{
            //    SelectedBoringBar = BoringBars.Where(e => e.TheEntity.id == id).FirstOrDefault();
            //    SelectedBoringBar.TheEntity = SelectedBoringBar.TheEntity;
            //    SelectedBoringBar.TheEntity.ClearErrors();
            //}), DispatcherPriority.ContextIdle);
            IsInEditMode = false;
        }

        private int NumberOfAssignedBoringBars()
        {
            var count = (from row in db.boring_bar
                         where row.id_bearing == SelectedBearing.TheEntity.id
                         select row).Count();
            return count;
        }

        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;

            ObservableCollection<BearingVM> _bearings = new ObservableCollection<BearingVM>();
            var bearings = await (from s in db.bearing
                                  orderby s.model
                                  select s).ToListAsync();

            foreach (bearing bear in bearings)
            {
                _bearings.Add(new BearingVM { IsNew = false, TheEntity = bear });
            }
            Bearings = _bearings;
            RaisePropertyChanged("Bearings");
            ThrobberVisible = Visibility.Collapsed;
        }
    }
}
