using System;
using System.Windows.Controls;

namespace DatabaseClient.Messages
{
    public class NavigateMessage
    {
        public Type ViewType { get; set; }
        public Type ViewModelType { get; set; }
        public UserControl View { get; set; }
    }
}
