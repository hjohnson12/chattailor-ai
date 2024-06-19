using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ChatTailorAI.Uwp.Behaviors
{
    public class NumberBoxRoundBehavior : DependencyObject, IBehavior
    {
        public int DecimalPlaces
        {
            get { return (int)GetValue(DecimalPlacesProperty); }
            set { SetValue(DecimalPlacesProperty, value); }
        }

        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register("DecimalPlaces", typeof(int), typeof(NumberBoxRoundBehavior), new PropertyMetadata(2));

        public DependencyObject AssociatedObject { get; private set; }

        public void Attach(DependencyObject associatedObject)
        {
            var control = associatedObject as NumberBox;
            if (control != null)
            {
                AssociatedObject = associatedObject;
                control.ValueChanged += Control_ValueChanged;
            }
        }

        public void Detach()
        {
            var control = AssociatedObject as NumberBox;
            if (control != null)
            {
                control.ValueChanged -= Control_ValueChanged;
                AssociatedObject = null;
            }
        }

        private void Control_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            double roundedValue = Math.Round(sender.Value, DecimalPlaces);
            if (roundedValue != sender.Value)
            {
                sender.Value = roundedValue;
            }
        }
    }
}
