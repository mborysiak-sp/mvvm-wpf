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
    public class DocumentSpindleViewModel : CrudVMBase
    {
        private DocumentSpindleVM selectedDocumentSpindle;
        public DocumentSpindleVM SelectedDocumentSpindle
        {
            get
            {
                return selectedDocumentSpindle;
            }
            set
            {
                selectedDocumentSpindle = value;
                selectedEntity = value;
                RaisePropertyChanged();
            }
        }
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
        private DocumentSpindleVM editVM;
        public DocumentSpindleVM EditVM
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
        public ObservableCollection<DocumentSpindleVM> DocumentSpindles { get; set; }
        public ObservableCollection<SpindleVM> Spindles { get; set; }

        public DocumentSpindleViewModel()
            : base()
        {

        }
        protected override void EditCurrent()
        {
            EditVM = SelectedDocumentSpindle;
            IsInEditMode = true;
        }
        protected override void InsertNew()
        {
            EditVM = new DocumentSpindleVM();
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
                    DocumentSpindles.Add(EditVM);
                    db.document_spindle.Add(EditVM.TheEntity);
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
                db.document_spindle.Remove(SelectedDocumentSpindle.TheEntity);
                DocumentSpindles.Remove(SelectedDocumentSpindle);
                RaisePropertyChanged("DocumentSpindles");
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
            //SelectedSpindle = null;
            //await db.Entry(EditVM.TheEntity).ReloadAsync();
            //await Application.Current.Dispatcher.InvokeAsync(new Action(() =>
            //{
            //    SelectedSpindle = Spindles.Where(e => e.TheEntity.id == id).FirstOrDefault();
            //    SelectedSpindle.TheEntity = SelectedSpindle.TheEntity;
            //    SelectedSpindle.TheEntity.ClearErrors();
            //}), DispatcherPriority.ContextIdle);
            IsInEditMode = false;
        }
        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;

            ObservableCollection<DocumentSpindleVM> _documentSpindle = new ObservableCollection<DocumentSpindleVM>();
            ObservableCollection<SpindleVM> _spindle = new ObservableCollection<SpindleVM>();

            var documentSpindle = await (from s in db.document_spindle
                                            orderby s.id
                                            select s).ToListAsync();

            var Spindle = await (from s in db.spindle
                                           orderby s.id
                                           select s).ToListAsync();

            foreach (document_spindle docspin in documentSpindle)
            {
                _documentSpindle.Add(new DocumentSpindleVM { IsNew = false, TheEntity = docspin });
            }
            foreach (spindle spin in Spindle)
            {
                _spindle.Add(new SpindleVM { IsNew = false, TheEntity = spin });
            }
            DocumentSpindles = _documentSpindle;
            RaisePropertyChanged("DocumentSpindles");
            Spindles = _spindle;
            RaisePropertyChanged("Spindles");
            ThrobberVisible = Visibility.Collapsed;
        }
    }
}
