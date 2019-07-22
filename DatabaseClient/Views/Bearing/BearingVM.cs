using DatabaseClient.EntityData;
using Support;

namespace DatabaseClient
{
    public class BearingVM : VMBase
    {
        public bearing TheEntity
        {
            get
            {
                return (bearing)base.theEntity;
            }
            set
            {
                theEntity = value;
                RaisePropertyChanged();
            }
        }

        public BearingVM()
        {
            TheEntity = new bearing();
            TheEntity.MetaSetUp();
        }
    }
}
