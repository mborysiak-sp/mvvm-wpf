using DatabaseClient.EntityData;
using Support;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace DatabaseClient
{
    class SearchViewModel : CrudVMBase
    {
        public ObservableCollection<DocumentVM> AllDocuments { get; set; }
        public SearchViewModel()
            : base()
        {

        }
        protected async override void GetData()
        {
            ThrobberVisible = Visibility.Visible;
            string model = string.Empty, number = string.Empty;
            ObservableCollection<DocumentVM> _allDocuments = new ObservableCollection<DocumentVM>();
            var allDocuments = await (from s in db.all_documents
                                      orderby s.model
                                      where s.model == model &&
                                      s.number == number
                                      select s ).ToListAsync();

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
