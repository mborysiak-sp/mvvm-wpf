using DatabaseClient.EntityData;
using Support;

namespace DatabaseClient
{
    public class DocumentBoringBarVM : VMBase
    {
        public document_boring_bar TheEntity
        {
            get
            {
                return (document_boring_bar)base.theEntity;
            }
            set
            {
                theEntity = value;
                RaisePropertyChanged();
            }
        }

        public DocumentBoringBarVM()
        {
            TheEntity = new document_boring_bar();
        }
    }
}
