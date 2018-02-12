using System;
using System.Windows;

namespace Common.Wpf.Res
{
    /// <summary>
    /// Stub for binding target.
    /// </summary>
    public class ValueFreezable : Freezable
    {
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(ValueFreezable), new UIPropertyMetadata(null));

        protected override Freezable CreateInstanceCore()
        {
            return (Freezable)Activator.CreateInstance(GetType());
        }
    }
}