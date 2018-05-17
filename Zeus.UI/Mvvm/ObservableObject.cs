using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Zeus.UI.Mvvm
{
    /// <summary>
    /// Provides a basic <see cref="INotifyPropertyChanged"/> implementation.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged interface

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Raises a <see cref="PropertyChanged"/> events for the property that call this function.
        /// </summary>
        /// <param name="propertyName">The name of the propery that call the method.</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Raises a <see cref="PropertyChanged"/> events for the property that call this function.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="propertyExpression">The expression that allows to retrieve the property.</param>
        protected void NotifyPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            NotifyPropertyChanged(((propertyExpression.Body as MemberExpression)?.Member as PropertyInfo)?.Name);
        }
        /// <summary>
        /// Set a property value and raise the <see cref="PropertyChanged"/> event if needed.
        /// </summary>
        /// <typeparam name="T">The type of the property that has to be set.</typeparam>
        /// <param name="prop">The property store value.</param>
        /// <param name="val">The new property value.</param>
        /// <param name="propertyName">The property name.</param>
        protected void Set<T>(ref T prop, T val, [CallerMemberName] string propertyName = null)
        {
            if (!prop.Equals(val))
            {
                prop = val;
                NotifyPropertyChanged(propertyName);
            }
        }

        #endregion
    }
}
