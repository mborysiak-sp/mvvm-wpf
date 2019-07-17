using DatabaseClient.EntityData;
using Support;

namespace DatabaseClient
{
    public class BoringBarVM : VMBase
    {
        public boring_bar TheEntity
        {
            get
            {
                return (boring_bar)base.theEntity;
            }
            set
            {
                theEntity = value;
                RaisePropertyChanged();
            }
        }

        public BoringBarVM()
        {
            TheEntity = new boring_bar();
        }
    }
}
