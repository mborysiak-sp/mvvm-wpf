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

        public ObservableCollection<BoringBarVM> BoringBars { get; set; }
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
            int NumDocs = NumberOfAssignedDocuments();
            if (NumDocs > 0)
            {
                ShowUserMessage(string.Format("Cannot delete - there are {0} Orders for this Customer", NumDocs));
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

        private int NumberOfAssignedDocuments()
        {
            var count = (from row in db.document_boring_bar
                         where row.id_boring_bar == SelectedBoringBar.TheEntity.id
                         select row).Count();
            return count;
        }
        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;

            ObservableCollection<BoringBarVM> _boringBars = new ObservableCollection<BoringBarVM>();
            var boringBars = await (from s in db.boring_bar
                                  orderby s.model
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
