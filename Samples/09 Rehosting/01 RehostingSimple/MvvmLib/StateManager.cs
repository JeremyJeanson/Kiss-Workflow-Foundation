using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MvvmLib
{
    public sealed class StateManager : DependencyObject
    {
        private const String StatePropertyName = "State";

        static StateManager()
        {
            StateProperty = DependencyProperty.RegisterAttached(
                StatePropertyName,
                typeof(String),
                typeof(StateManager),
                new PropertyMetadata("StateManager", StateChanged));
        }

        public static String GetState(DependencyObject obj)
        {
            return (String)obj.GetValue(StateProperty);
        }

        public static void SetState(DependencyObject obj, String value)
        {
            obj.SetValue(StateProperty, value);
        }

        // Using a DependencyProperty as the backing store for State.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateProperty;

        private static void StateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            System.Diagnostics.Contracts.Contract.Ensures(!(d is FrameworkElement), "StateManager doit être utilisé avec un FrameworkElement!");
            FrameworkElement control = d as FrameworkElement;
            String state = (String)e.NewValue;
            if (String.IsNullOrEmpty(state))
            {
                VisualStateManager.GoToState(control, "Default", false);
            }
            else
            {
                VisualStateManager.GoToState(control, state, false);
            }
        }        
    }
}
