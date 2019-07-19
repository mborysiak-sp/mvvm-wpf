using DatabaseClient.EntityData;
using Support;

namespace DatabaseClient
{
    public class DocumentVM : VMBase
    {
        public all_documents TheEntity
        {
            get
            {
                return (all_documents)base.theEntity;
            }
            set
            {
                theEntity = value;
                RaisePropertyChanged();
            }
        }

        public DocumentVM()
        {
            TheEntity = new all_documents();
        }
    }
}
