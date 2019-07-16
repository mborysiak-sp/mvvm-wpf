using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Support
{
    public partial class Throbber : UserControl
    {
        public Throbber()
        {
            InitializeComponent();
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(VisibleChanged);
        }

        void VisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                Dispatcher.BeginInvoke(
                DispatcherPriority.ContextIdle,
                new Action(delegate ()
                {
                    this.ArcContainer.Focus();
                }));
            }
        }
    }
}
