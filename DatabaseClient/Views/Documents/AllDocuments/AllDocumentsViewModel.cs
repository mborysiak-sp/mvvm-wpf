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

        public const string TextFilterPropertyName = "TextFilter";

        private string _TextFilter;
        public string TextFilter
        {
            get
            {
                return _TextFilter;
            }
            set
            {
                if (_TextFilter == value)
                    return;
                _TextFilter = value;
                RaisePropertyChanged(TextFilterPropertyName);
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
            foreach(var item in AllDocuments)
            {
                if (item.TheEntity.model.Contains(TextFilter))
                    _filtered.Add(item);
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
