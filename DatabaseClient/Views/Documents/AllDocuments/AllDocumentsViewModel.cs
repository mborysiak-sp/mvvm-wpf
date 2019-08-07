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
    public class AllDocumentsViewModel : CrudVMBase
    {
        public ObservableCollection<DocumentVM> AllDocuments { get; set; }
        public AllDocumentsViewModel()
            : base()
        {
        }

        private DocumentVM selectedDocument;
        public DocumentVM SelectedDocument
        {
            get
            {
                return selectedDocument;
            }
            set
            {
                selectedDocument = value;
                selectedEntity = value;
                RaisePropertyChanged("SelectedDocument");
                Filter();
            }
        }

        public const string MyItemListPropertyName = "MyItemList";

        private ObservableCollection<DocumentVM> _filtered = new ObservableCollection<DocumentVM>();
        public ObservableCollection<DocumentVM> FilteredList
        {
            get
            {
                return _filtered;
            }
            set
            {
                if (_filtered == value)
                    return;

                _filtered = value;
                RaisePropertyChanged("FilteredList");
            }
        }

        public void Filter()
        {
            if(!(_filtered is null))
                _filtered.Clear();
            foreach(var doc in AllDocuments)
            {
                if (SelectedDocument.TheEntity.model == null)
                    _filtered.Add(doc);
                else if ((doc.TheEntity.model + doc.TheEntity.number).Equals(SelectedDocument.TheEntity.model + SelectedDocument.TheEntity.number))
                    _filtered.Add(doc);
            }
        }

        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;

            ObservableCollection<DocumentVM> _allDocuments = new ObservableCollection<DocumentVM>();
            var allDocuments = await (from s in db.all_documents
                                  orderby s.model
                                  select s).ToListAsync();

            foreach (all_documents doc in allDocuments)
            {
                _allDocuments.Add(new DocumentVM { IsNew = false, TheEntity = doc });
            }
            
            AllDocuments = _allDocuments;
            
            RaisePropertyChanged("AllDocuments");
            ThrobberVisible = Visibility.Collapsed;
        }
    }
}
