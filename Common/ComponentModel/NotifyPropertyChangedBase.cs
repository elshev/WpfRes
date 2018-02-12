using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Common.Reflection;

namespace Common.ComponentModel
{
    /// <summary>
    /// NotifyPropertyChangedBase: base implementation of INotifyPropertyChanged interface
    /// </summary>
    [Serializable]
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        #region [ INotifyPropertyChanged Members ]

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public static string GetPropertyNameFromExpression<T>(Expression<Func<T>> property)
        {
            return ReflectionHelper.GetPropertyNameFromExpression(property);
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            RaisePropertyChanged(GetPropertyNameFromExpression(property));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.PropertyName))
                VerifyPropertyName(args.PropertyName);

            var handler = PropertyChanged;
            if (handler != null)
                handler(this, args);
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance) == null)
            {
                string msg = string.Format("Error in VerifyPropertyName debug method. Invalid property name: {0}.{1}", ToString(), propertyName);
                throw new Exception(msg);
            }
        }
    }
}