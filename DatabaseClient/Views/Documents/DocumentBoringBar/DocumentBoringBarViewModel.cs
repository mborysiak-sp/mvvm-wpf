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
                db.document_boring_bar.Remove(SelectedDocumentBoringBar.TheEntity);
                DocumentBoringBars.Remove(SelectedDocumentBoringBar);
                RaisePropertyChanged("BoringBars");
                CommitUpdates();
                selectedEntity = null;
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
        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;

            ObservableCollection<DocumentBoringBarVM> _documentBoringBar = new ObservableCollection<DocumentBoringBarVM>();
            var documentBoringBar = await (from s in db.document_boring_bar
                                  orderby s.id
                                  select s).ToListAsync();

            foreach (document_boring_bar docbar in documentBoringBar)
            {
                _documentBoringBar.Add(new DocumentBoringBarVM { IsNew = false, TheEntity = docbar });
            }
            DocumentBoringBars = _documentBoringBar;
            RaisePropertyChanged("AllDocuments");
            ThrobberVisible = Visibility.Collapsed;
        }
    }
}
