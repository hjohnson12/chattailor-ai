using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml;

namespace ChatTailorAI.Uwp.Behaviors
{
    public static class ListViewItemBehavior
    {
        public static readonly DependencyProperty IsItemSelectedProperty =
            DependencyProperty.RegisterAttached("IsItemSelected", typeof(bool), typeof(ListViewItemBehavior), new PropertyMetadata(false, OnIsItemSelectedChanged));

        public static bool GetIsItemSelected(DependencyObject obj) => (bool)obj.GetValue(IsItemSelectedProperty);

        public static void SetIsItemSelected(DependencyObject obj, bool value) => obj.SetValue(IsItemSelectedProperty, value);

        private static void OnIsItemSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SelectorItem item)
            {
                item.IsSelected = (bool)e.NewValue;
            }
        }
    }
}
