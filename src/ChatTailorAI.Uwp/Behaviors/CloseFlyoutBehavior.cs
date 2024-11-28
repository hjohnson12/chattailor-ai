using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace ChatTailorAI.Uwp.Behaviors
{
    public class CloseFlyoutBehavior : Behavior<Button>
    {
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(CloseFlyoutBehavior), new PropertyMetadata(false, OnIsOpenPropertyChanged));

        private static void OnIsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (CloseFlyoutBehavior)d;
            var button = behavior.AssociatedObject;
            var flyout = FlyoutBase.GetAttachedFlyout(button);

            if (!(bool)e.NewValue)
            {
                flyout?.Hide();
            }
        }
    }

}
