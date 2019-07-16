using DatabaseClient.EntityData;
using Support;
namespace DatabaseClient
{
    public class SpindleVM : VMBase
    {
        public spindle TheSpindle { get; set; }

        public SpindleVM()
        {
            TheSpindle = new spindle();
        }
    }
}
