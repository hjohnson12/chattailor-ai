using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace ChatTailorAI.Uwp.Behaviors
{
    public class SelectedItemBehavior : Behavior<GridView>
    {
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(SelectedItemBehavior), new PropertyMetadata(null, OnSelectedItemChanged));

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (SelectedItemBehavior)d;
            var grid = behavior.AssociatedObject;
            if (grid != null)
            {
                grid.SelectedItem = e.NewValue;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItem = this.AssociatedObject.SelectedItem;
        }
    }
}
