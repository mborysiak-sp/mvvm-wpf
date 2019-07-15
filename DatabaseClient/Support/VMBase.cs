using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient.Support
{
    public class VMBase : NotifyUIBase
    {
        private bool isNew = true;
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                RaisePropertyChanged();
            }
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool isDeleted = false;
        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                RaisePropertyChanged();
            }
        }
    }
}
