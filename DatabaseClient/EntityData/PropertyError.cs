using Support;

namespace DatabaseClient
{
    public class PropertyError : NotifyUIBase
    {
        private string propertyName;

        public string PropertyName
        {
            get
            {
                return propertyName;
            }
            set
            {
                propertyName = value;
                RaisePropertyChanged();
            }
        }
        private string error;
        public string Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
                RaisePropertyChanged();
            }
        }
        private bool added;
        public bool Added
        {
            get
            {
                return added;
            }
            set
            {
                added = value;
                RaisePropertyChanged();
            }
        }
    }
}