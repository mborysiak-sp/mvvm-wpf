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
    public class DocumentBoringBarViewModel : CrudVMBase
    {
        private DocumentBoringBarVM selectedDocumentBoringBar;
        public DocumentBoringBarVM SelectedDocumentBoringBar
        {
            get
            {
                return selectedDocumentBoringBar;
            }
            set
            {
                selectedDocumentBoringBar = value;
                selectedEntity = value;
                RaisePropertyChanged();
            }
        }
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
        private DocumentBoringBarVM editVM;
        public DocumentBoringBarVM EditVM
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
        public ObservableCollection<DocumentBoringBarVM> DocumentBoringBars { get; set; }
        public ObservableCollection<BoringBarVM> BoringBars { get; set; }

        public DocumentBoringBarViewModel()
            : base()
        {

        }
        protected override void EditCurrent()
        {
            EditVM = SelectedDocumentBoringBar;
            IsInEditMode = true;
        }
        protected override void InsertNew()
        {
            EditVM = new DocumentBoringBarVM();
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
                    DocumentBoringBars.Add(EditVM);
                    db.document_boring_bar.Add(EditVM.TheEntity);
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
                db.document_boring_bar.Remove(SelectedDocumentBoringBar.TheEntity);
                DocumentBoringBars.Remove(SelectedDocumentBoringBar);
                RaisePropertyChanged("DocumentBoringBars");
                CommitUpdates();
                selectedEntity = null;
        }
        protected override void Quit()
        {
                ReFocusRow();
        }
        protected void ReFocusRow(bool withReload = true)
        {
            SelectedDocumentBoringBar = null;
            IsInEditMode = false;
        }
        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;

            ObservableCollection<DocumentBoringBarVM> _documentBoringBar = new ObservableCollection<DocumentBoringBarVM>();
            ObservableCollection<BoringBarVM> _boringBar = new ObservableCollection<BoringBarVM>();

            var documentBoringBar = await (from s in db.document_boring_bar
                                            orderby s.id
                                            select s).ToListAsync();

            var boringBar = await (from s in db.boring_bar
                                           orderby s.id
                                           select s).ToListAsync();

            foreach (document_boring_bar docbar in documentBoringBar)
            {
                _documentBoringBar.Add(new DocumentBoringBarVM { IsNew = false, TheEntity = docbar });
            }
            foreach (boring_bar bar in boringBar)
            {
                _boringBar.Add(new BoringBarVM { IsNew = false, TheEntity = bar });
            }
            DocumentBoringBars = _documentBoringBar;
            RaisePropertyChanged("DocumentBoringBars");
            BoringBars = _boringBar;
            RaisePropertyChanged("BoringBars");
            ThrobberVisible = Visibility.Collapsed;
        }
    }
}
