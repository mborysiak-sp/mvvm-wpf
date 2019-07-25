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
            ReFocusRow();
        }

        protected new void ReFocusRow(bool withReload = true)
        {
            SelectedSpindle = null;
            IsInEditMode = false;
        }

        private int NumberOfAssignedDocuments()
        {
            var count = (from row in db.document_spindle
                         where row.id_spindle == SelectedSpindle.TheEntity.id
                         select row).Count();
            return count;
        }
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
