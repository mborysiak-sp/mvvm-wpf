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
        ObservableCollection<DocumentVM> FilteredDocuments = new ObservableCollection<DocumentVM>();
        public ObservableCollection<DocumentVM> AllDocuments { get; set; }
        public AllDocumentsViewModel()
            : base()
        {
        }

        protected override void Filter()
        {

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
            Filtered
            RaisePropertyChanged("AllDocuments");
            ThrobberVisible = Visibility.Collapsed;
        }
    }
}
