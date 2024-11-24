using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using ChatTailorAI.Shared.Models.Shared;
using ChatTailorAI.Shared.Models.Tools;

namespace ChatTailorAI.Uwp.Behaviors
{
    public class SelectedItemsBehavior<T> : Behavior<ListView>
    {
        public IList<T> SelectedItems
        {
            get { return (IList<T>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems), typeof(IList<T>), typeof(SelectedItemsBehavior<T>), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItems = AssociatedObject.SelectedItems.Cast<T>().ToList();
        }
    }

    public class ToolsListItemSelectedItemsBehavior : SelectedItemsBehavior<Tool>
    {
    }


    public class FunctionListItemSelectedItemsBehavior : SelectedItemsBehavior<FunctionListItem>
    {
    }
}
