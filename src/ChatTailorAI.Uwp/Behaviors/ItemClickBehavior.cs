using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace ChatTailorAI.Uwp.Behaviors
{
    public class ItemClickBehavior : Behavior<ListViewBase>
    {
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(ItemClickBehavior),
            new PropertyMetadata(default(ICommand)));

        private void HandleItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(Command is ICommand command) ||
                !command.CanExecute(e.ClickedItem))
            {
                return;
            }

            command.Execute(e.ClickedItem);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
            {
                AssociatedObject.ItemClick += HandleItemClick;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject != null)
            {
                AssociatedObject.ItemClick -= HandleItemClick;
            }
        }
    }
}
