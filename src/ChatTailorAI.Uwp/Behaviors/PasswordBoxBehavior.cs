using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace ChatTailorAI.Uwp.Behaviors
{
    public class PasswordBoxBehavior : Trigger<PasswordBox>
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordBoxBehavior),
                new PropertyMetadata(null, OnPasswordChanged));

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
            UpdatePasswordBox();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
        }

        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = AssociatedObject.Password;
        }

        private void UpdatePasswordBox()
        {
            if (AssociatedObject != null && Password != null)
            {
                AssociatedObject.Password = Password;
            }
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PasswordBoxBehavior)?.UpdatePasswordBox();
        }
    }
}