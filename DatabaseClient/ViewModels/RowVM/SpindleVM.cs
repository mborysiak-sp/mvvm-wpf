using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseClient.Support;
namespace DatabaseClient.ViewModels.RowVM
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
