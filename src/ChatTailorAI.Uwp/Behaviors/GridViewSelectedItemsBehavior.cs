using Microsoft.Xaml.Interactivity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ChatTailorAI.Uwp.Behaviors
{
    public class GridViewSelectedItemsBehavior : Behavior<GridView>
    {
        private bool isUpdating; // Add this flag

        public static readonly DependencyProperty SelectedItemsProperty =
    DependencyProperty.Register(nameof(SelectedItems), typeof(IList), typeof(GridViewSelectedItemsBehavior), new PropertyMetadata(null, OnSelectedItemsChanged));

        public IList SelectedItems
        {
            get => (IList)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GridViewSelectedItemsBehavior behavior)
            {
                if (e.OldValue is INotifyCollectionChanged oldCollection)
                {
                    oldCollection.CollectionChanged -= behavior.SelectedItems_CollectionChanged;
                }

                if (e.NewValue is INotifyCollectionChanged newCollection)
                {
                    newCollection.CollectionChanged += behavior.SelectedItems_CollectionChanged;
                }

                behavior.UpdateSelectedItems();
            }
        }

        private async void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (AssociatedObject != null)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    isUpdating = true;

                    if (e.Action == NotifyCollectionChangedAction.Reset)
                    {
                        AssociatedObject.SelectedItems.Clear();
                    }
                    else if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        foreach (var item in e.NewItems)
                        {
                            AssociatedObject.SelectedItems.Add(item);
                        }
                    }
                    // TODO: Get it working with de-select manually, crashes when uncommenting
                    //else if (e.Action == NotifyCollectionChangedAction.Remove)
                    //{
                    //    foreach (var item in e.OldItems)
                    //    {
                    //        AssociatedObject.SelectedItems.Remove(item);
                    //    }
                    //}
                    isUpdating = false;
                });
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
            AssociatedObject.PointerPressed += AssociatedObject_PointerPressed;
            AssociatedObject.PointerReleased += AssociatedObject_PointerReleased;
            UpdateSelectedItems();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
            AssociatedObject.PointerPressed -= AssociatedObject_PointerPressed;
            AssociatedObject.PointerReleased -= AssociatedObject_PointerReleased;
        }
        private void AssociatedObject_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            AssociatedObject.Tag = true;
        }

        private void AssociatedObject_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            AssociatedObject.Tag = false;
        }
        private void UpdateSelectedItems()
        {
            if (AssociatedObject != null && SelectedItems != null)
            {
                // Only update the AssociatedObject if its SelectedItems is empty
                if (AssociatedObject.SelectedItems.Count == 0)
                {
                    foreach (var item in SelectedItems)
                    {
                        AssociatedObject.SelectedItems.Add(item);
                    }
                }
                else
                {
                    // Update the SelectedItems from the AssociatedObject
                    foreach (var item in AssociatedObject.SelectedItems)
                    {
                        //SelectedItems.Add((string)item);
                        SelectedItems.Add(item);
                    }
                }
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedItems != null && !isUpdating)
            {
                isUpdating = true;
                foreach (var item in e.RemovedItems)
                {
                    SelectedItems.Remove(item);
                }

                foreach (var item in e.AddedItems)
                {
                    SelectedItems.Add(item);
                }
                isUpdating = false;
            }
        }
    }
}
