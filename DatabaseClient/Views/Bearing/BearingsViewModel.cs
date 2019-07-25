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
                    ShowUserMessage("Brak zmian do zapisania");
                }
            }
            else
            {
                ShowUserMessage("Problem z wprowadzonymi danymi");
            }
        }
        private async void UpdateDB()
        {
            try
            {
                await db.SaveChangesAsync();
                ShowUserMessage("Baza danych zaktualizowana");
            }
            catch (Exception)
            {
                ShowUserMessage("Wystąpił problem z aktualizacją bazy danych");
            }
            ReFocusRow();
        }
        protected override void DeleteCurrent()
        {
            int NumDocs = NumberOfAssignedBoringBars();
            if (NumDocs > 0)
            {
                ShowUserMessage(string.Format("Nie można usunąć. Powiązane z {0} wytaczadłami", NumDocs));
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
            ReFocusRow();
        }
        protected void ReFocusRow(bool withReload = true)
        {
            SelectedBearing = null;
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
