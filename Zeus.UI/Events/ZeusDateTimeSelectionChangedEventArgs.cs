using System;
using System.Windows;

namespace Zeus.UI.Events
{
    /// <summary>
    /// Provides infortmation about a <see cref="Controls.ZeusDateTimePicker.SelectedDateTimeChanged"/> event.
    /// </summary>
    public class ZeusDateTimeSelectionChangedEventArgs : RoutedEventArgs
    {
        #region Constructor

        /// <summary>
        /// Initialize a new instance of <see cref="ZeusDateTimeSelectionChangedEventArgs"/>.
        /// </summary>
        /// <param name="eventId">The <see cref="RoutedEvent"/> associated with the event.</param>
        /// <param name="oldValue">The old property value.</param>
        /// <param name="newValue">The new property value.</param>
        public ZeusDateTimeSelectionChangedEventArgs(RoutedEvent eventId, DateTime? oldValue, DateTime? newValue) : base(eventId)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the old property value.
        /// </summary>
        public DateTime? OldValue { get; private set; }
        /// <summary>
        /// Gets the new property value.
        /// </summary>
        public DateTime? NewValue { get; private set; }

        #endregion
    }
}
