using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace DatabaseClient
{
    public class DataGridRowBehavior : Behavior<DataGridRow>
    {
        public static bool GetIsDataGridRowFocusedWhenSelected(DataGridRow dataGridRow)
        {
            return (bool)dataGridRow.GetValue(IsDataGridRowFocusedWhenSelectedProperty);
        }

        public static void SetIsDataGridRowFocusedWhenSelected(
            DataGridRow dataGridRow, bool value)
        {
            dataGridRow.SetValue(IsDataGridRowFocusedWhenSelectedProperty, value);
        }

        public static readonly DependencyProperty IsDataGridRowFocusedWhenSelectedProperty =
            DependencyProperty.RegisterAttached(
                "IsDataGridRowFocusedWhenSelected",
                typeof(bool),
                typeof(DataGridRowBehavior),
                new UIPropertyMetadata(false, OnIsDataGridRowFocusedWhenSelectedChanged));

        static void OnIsDataGridRowFocusedWhenSelectedChanged(
            DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            DataGridRow item = depObj as DataGridRow;
            if (item == null)
                return;
            if (e.NewValue is bool == false)
                return;
            if ((bool)e.NewValue)
                item.Selected += OnDataGridRowSelected;
            else
                item.Selected -= OnDataGridRowSelected;
        }
        static void OnDataGridRowSelected(object sender, RoutedEventArgs e)
        {
            DataGridRow row = e.OriginalSource as DataGridRow;
            if (!(Keyboard.FocusedElement is DataGridCell) && row != null)
            {
                row.Focusable = true;
                Keyboard.Focus(row);
            }
        }
    }
}
