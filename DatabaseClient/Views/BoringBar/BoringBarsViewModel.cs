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
    public class BoringBarsViewModel : CrudVMBase
    {
        private BoringBarVM selectedBoringBar;
        public BoringBarVM SelectedBoringBar
        {
            get
            {
                return selectedBoringBar;
            }
            set
            {
                selectedBoringBar = value;
                selectedEntity = value;
                RaisePropertyChanged();
            }
        }

        private BoringBarVM editVM;
        public BoringBarVM EditVM
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

        public ObservableCollection<BoringBarVM> BoringBars { get; set; }
        public ObservableCollection<BearingVM> Bearings { get; set; }

        public BoringBarsViewModel()
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
            catch (Exception e)
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    ErrorMessage = e.InnerException.GetBaseException().ToString();
                }
                ShowUserMessage("Wystąpił problem z aktualizacją bazy danych");
            }
            ReFocusRow();
        }
        
        protected override void DeleteCurrent()
        {
            int NumDocs = NumberOfAssignedDocuments();
            if (NumDocs > 0)
            {
                ShowUserMessage(string.Format("Nie można usunąć. Powiązane z {0} świadectwami", NumDocs));
            }
            else
            {
                db.boring_bar.Remove(SelectedBoringBar.TheEntity);
                BoringBars.Remove(SelectedBoringBar);
                RaisePropertyChanged("BoringBars");
                CommitUpdates();
                selectedEntity = null;
            }
        }

        private int NumberOfAssignedDocuments()
        {
            var count = (from row in db.document_boring_bar
                         where row.id_boring_bar == SelectedBoringBar.TheEntity.id
                         select row).Count();
            return count;
        }
        protected override void Quit()
        {
            ReFocusRow();
        }
        protected void ReFocusRow(bool withReload = true)
        {
            SelectedBoringBar = null;
            IsInEditMode = false;
        }

        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;

            ObservableCollection<BoringBarVM> _boringBars = new ObservableCollection<BoringBarVM>();
            ObservableCollection<BearingVM> _bearings = new ObservableCollection<BearingVM>();

            var boringBars = await (from s in db.boring_bar
                                    orderby s.model
                                    where s.scrapping_date == null
                                    select s).ToListAsync();

            var bearings = await (from b in db.bearing
                                  orderby b.id
                                  select b).ToListAsync();

            foreach (boring_bar boring in boringBars)
            {
                _boringBars.Add(new BoringBarVM { IsNew = false, TheEntity = boring });
            }
            foreach (bearing bear in bearings)
            {
                _bearings.Add(new BearingVM { IsNew = false, TheEntity = bear });
            }
            BoringBars = _boringBars;
            RaisePropertyChanged("BoringBars");
            Bearings = _bearings;
            RaisePropertyChanged("Bearings");
            ThrobberVisible = Visibility.Collapsed;
        }
    }
}
