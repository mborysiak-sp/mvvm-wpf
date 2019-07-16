using DatabaseClient.EntityData;
using Support;
namespace DatabaseClient
{
    public class SpindleVM : VMBase
    {
        public spindle TheEntity
        {
            get
            {
                return (spindle)base.theEntity;
            }
            set
            {
                theEntity = value;
                RaisePropertyChanged();
            }
        }

        public SpindleVM()
        {
            TheEntity = new spindle();
        }
    }
}
