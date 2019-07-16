using GalaSoft.MvvmLight.Command;
using System;
using System.Windows;
using System.Windows.Data;

namespace DatabaseClient
{
    public class BindingSourcePropertyConverter : IEventArgsConverter
    {

        public object Convert(object value, object parameter)
        {
            DataTransferEventArgs e = (DataTransferEventArgs)value;
            Type type = e.TargetObject.GetType();
            BindingExpression binding = ((FrameworkElement)e.TargetObject).GetBindingExpression(e.Property);
            return binding.ResolvedSourcePropertyName ?? "";
        }
    }
}
