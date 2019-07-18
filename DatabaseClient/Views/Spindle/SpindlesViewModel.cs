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
    public class SpindlesViewModel : CrudVMBase
    {
        private SpindleVM selectedSpindle;
        public SpindleVM SelectedSpindle
        {
            get
            {
                return selectedSpindle;
            }
            set
            {
                selectedSpindle = value;
                selectedEntity = value;
                RaisePropertyChanged();
            }
        }

        private SpindleVM editVM;
        public SpindleVM EditVM
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

        public ObservableCollection<SpindleVM> Spindles { get; set; }
        public SpindlesViewModel()
            : base()
        {

        }
        protected override void EditCurrent()
        {
            EditVM = SelectedSpindle;
            IsInEditMode = true;
        }
        protected override void InsertNew()
        {
            EditVM = new SpindleVM();
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
                    Console.Out.WriteLine(EditVM.TheEntity.id);
                    EditVM.IsNew = false;
                    Spindles.Add(EditVM);
                    db.spindle.Add(EditVM.TheEntity);
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
            int NumDocs = NumberOfAssignedDocuments();
            if (NumDocs > 0)
            {
                ShowUserMessage(string.Format("Cannot delete - there are {0} Orders for this Customer", NumDocs));
            }
            else
            {
                db.spindle.Remove(SelectedSpindle.TheEntity);
                Spindles.Remove(SelectedSpindle);
                RaisePropertyChanged("Spindles");
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
            //SelectedSpindle = null;
            ////await db.Entry(EditVM.TheEntity).ReloadAsync();
            //await Application.Current.Dispatcher.InvokeAsync(new Action(() =>
            //{
            //    SelectedSpindle = Spindles.Where(e => e.TheEntity.id == id).FirstOrDefault();
            //    SelectedSpindle.TheEntity = SelectedSpindle.TheEntity;
            //    SelectedSpindle.TheEntity.ClearErrors();
            //}), DispatcherPriority.ContextIdle);
            IsInEditMode = false;
        }
        private int NumberOfAssignedDocuments()
        {
            var count = (from row in db.document_spindle
                         where row.id_spindle == SelectedSpindle.TheEntity.id
                         select row).Count();
            return count;
        }
        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;

            ObservableCollection<SpindleVM> _spindles = new ObservableCollection<SpindleVM>();
            var spindles = await (from s in db.spindle
                                   orderby s.model
                                   where s.scrapping_date == null
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
