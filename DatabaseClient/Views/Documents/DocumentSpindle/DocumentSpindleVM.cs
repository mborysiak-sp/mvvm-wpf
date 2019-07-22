using DatabaseClient.EntityData;
using Support;

namespace DatabaseClient
{
    public class DocumentSpindleVM : VMBase
    {
        public document_spindle TheEntity
        {
            get
            {
                return (document_spindle)base.theEntity;
            }
            set
            {
                theEntity = value;
                RaisePropertyChanged();
            }
        }

        public DocumentSpindleVM()
        {
            TheEntity = new document_spindle();
        }
    }
}
