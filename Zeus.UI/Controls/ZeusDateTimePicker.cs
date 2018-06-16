using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Zeus.UI.Enums;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled <see cref="DatePicker"/>.
    /// </summary>
    [TemplatePart(Name = ElementTextBox, Type = typeof(ZeusTextBox))]
    [TemplatePart(Name = ElementButton, Type = typeof(ZeusButton))]
    [TemplatePart(Name = ElementPopup, Type = typeof(Popup))]
    [TemplatePart(Name = ElementCalendar, Type = typeof(ZeusCalendar))]
    public class ZeusDateTimePicker : Control
    {
        #region Constants

        /// <summary>
        /// Name of the text box element in the control template.
        /// </summary>
        private const string ElementTextBox = "PART_TextBox";
        /// <summary>
        /// Name of the button element in the control template.
        /// </summary>
        private const string ElementButton = "PART_Button";
        /// <summary>
        /// Name of the popup element in the control template.
        /// </summary>
        private const string ElementPopup = "PART_Popup";
        /// <summary>
        /// Name of the calendar element in the control template.
        /// </summary>
        private const string ElementCalendar = "PART_Calendar";

        #endregion

        #region Fields

        /// <summary>
        /// The text box control that will display the selected date or the default text.
        /// </summary>
        private ZeusTextBox m_TextBox;
        /// <summary>
        /// The button control that will open the popup.
        /// </summary>
        private ZeusButton m_DropDownButton;
        /// <summary>
        /// The popup control.
        /// </summary>
        private Popup m_PopUp;
        /// <summary>
        /// The calendar control.
        /// </summary>
        private ZeusCalendar m_Calendar;
        /// <summary>
        /// A flag that enables or disables the textbox text update.
        /// </summary>
        private bool m_DisableTextBoxUpdate;

        #endregion

        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusDateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusDateTimePicker), new FrameworkPropertyMetadata(typeof(ZeusDateTimePicker)));
            EventManager.RegisterClassHandler(typeof(ZeusDateTimePicker), GotFocusEvent, new RoutedEventHandler(OnGotFocus));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusDateTimePicker), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusDateTimePicker"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;
        /// <summary>
        /// Identifies the <see cref="DateFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DateFormatProperty = DependencyProperty.Register("DateFormat", typeof(ZeusDateTimePickerFormats), typeof(ZeusDateTimePicker), new FrameworkPropertyMetadata(ZeusDateTimePickerFormats.Long, OnDateFormatChanged));
        /// <summary>
        /// Identifies the <see cref="TimeFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TimeFormatProperty = DependencyProperty.Register("TimeFormat", typeof(ZeusDateTimePickerFormats), typeof(ZeusDateTimePicker), new FrameworkPropertyMetadata(ZeusDateTimePickerFormats.Long, OnTimeFormatChanged));
        /// <summary>
        /// Identifies the <see cref="IsDropDownOpen"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(ZeusDateTimePicker), new FrameworkPropertyMetadata(false));
        /// <summary>
        /// Identifies the <see cref="IsTodayHighlighted"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTodayHighlightedProperty = DependencyProperty.Register("IsTodayHighlighted", typeof(bool), typeof(ZeusDateTimePicker), new FrameworkPropertyMetadata(true));
        /// <summary>
        /// Identifies the <see cref="FirstDayOfWeek"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FirstDayOfWeekProperty = DependencyProperty.Register("FirstDayOfWeek", typeof(DayOfWeek), typeof(ZeusDateTimePicker), new FrameworkPropertyMetadata(DayOfWeek.Sunday));
        /// <summary>
        /// Identifies the <see cref="SelectedDateTime"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register("SelectedDateTime", typeof(DateTime?), typeof(ZeusDateTimePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateTimeChanged));
        /// <summary>
        /// Identifies the <see cref="IsCalendarVisible"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCalendarVisibleProperty = DependencyProperty.Register("IsCalendarVisible", typeof(bool), typeof(ZeusDateTimePicker), new FrameworkPropertyMetadata(true));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusDateTimePicker"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        /// <summary>
        /// Gets or sets the format that is used to display the date.
        /// </summary>
        public ZeusDateTimePickerFormats DateFormat
        {
            get { return (ZeusDateTimePickerFormats)GetValue(DateFormatProperty); }
            set { SetValue(DateFormatProperty, value); }
        }
        /// <summary>
        /// Gets or sets the format that is used to display the time.
        /// </summary>
        public ZeusDateTimePickerFormats TimeFormat
        {
            get { return (ZeusDateTimePickerFormats)GetValue(TimeFormatProperty); }
            set { SetValue(TimeFormatProperty, value); }
        }
        /// <summary>
        /// Gets or sets a value that indicates whether the drop-down Calendar is open or closed.
        /// </summary>
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }
        /// <summary>
        /// Gets or sets a value that indicates whether today is highlighted or not in the calendar.
        /// </summary>
        public bool IsTodayHighlighted
        {
            get { return (bool)GetValue(IsTodayHighlightedProperty); }
            set { SetValue(IsTodayHighlightedProperty, value); }
        }
        /// <summary>
        /// Gets or sets the first day of the week in the calendar.
        /// </summary>
        public DayOfWeek FirstDayOfWeek
        {
            get { return (DayOfWeek)GetValue(FirstDayOfWeekProperty); }
            set { SetValue(FirstDayOfWeekProperty, value); }
        }
        /// <summary>
        /// Gets the days that are not selectable.
        /// </summary>
        public CalendarBlackoutDatesCollection BlackoutDates
        {
            get { return m_Calendar.BlackoutDates; }
        }
        /// <summary>
        /// Gets or sets the currently selected date and time.
        /// </summary>
        public DateTime? SelectedDateTime
        {
            get { return (DateTime?)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }
        /// <summary>
        /// Gets or sets a flag that indicates if the calendar is visible or not.
        /// </summary>
        public bool IsCalendarVisible
        {
            get { return (bool)GetValue(IsCalendarVisibleProperty); }
            set { SetValue(IsCalendarVisibleProperty, value); }
        }

        #endregion

        #region RoutedEvents

        /// <summary>
        /// Occours when the selected date and time change.
        /// </summary>
        public static readonly RoutedEvent SelectedDateTimeChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateTimeChanged", RoutingStrategy.Direct, typeof(EventHandler<RoutedEventArgs>), typeof(ZeusDateTimePicker));

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the <see cref="SelectedDateTime" /> property is changed.
        /// </summary>
        public event EventHandler<RoutedEventArgs> SelectedDateTimeChanged
        {
            add { AddHandler(SelectedDateTimeChangedEvent, value); }
            remove { RemoveHandler(SelectedDateTimeChangedEvent, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the visual tree for the <see cref="ZeusDateTimePicker"/> control when a new template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //unsubscribe events
            if (m_DropDownButton != null)
            {
                m_DropDownButton.Click -= OnDropDownButtonClicked;
                m_TextBox.LostFocus -= OnTextBoxLostFocus;
                m_TextBox.KeyDown += OnTextBoxKeyDown;
            }
            if (m_TextBox != null)
            {
                m_TextBox.TextChanged -= OnTextBoxTextChanged;
            }
            m_TextBox = GetTemplateChild(ElementTextBox) as ZeusTextBox;
            m_DropDownButton = GetTemplateChild(ElementButton) as ZeusButton;
            m_PopUp = GetTemplateChild(ElementPopup) as Popup;
            m_Calendar = GetTemplateChild(ElementCalendar) as ZeusCalendar;
            //subscribe events and apply bindings
            if (m_DropDownButton != null)
            {
                m_DropDownButton.Click += OnDropDownButtonClicked;
            }
            if (m_TextBox != null)
            {
                m_TextBox.TextChanged += OnTextBoxTextChanged;
                m_TextBox.LostFocus += OnTextBoxLostFocus;
                m_TextBox.KeyDown += OnTextBoxKeyDown;
            }
            if (m_PopUp != null)
            {
                m_PopUp.SetBinding(Popup.IsOpenProperty, new Binding(IsDropDownOpenProperty.Name) { Source = this });
            }
            if (m_Calendar != null)
            {
                m_Calendar.SetBinding(ZeusCalendar.IsTodayHighlightedProperty, new Binding(IsTodayHighlightedProperty.Name) { Source = this });
                m_Calendar.SetBinding(ZeusCalendar.FirstDayOfWeekProperty, new Binding(FirstDayOfWeekProperty.Name) { Source = this });
                m_Calendar.SelectedDatesChanged += OnCalendarSelectedDateChanged;
            }
            WriteValueToTextBox();
        }
        /// <summary>
        /// Updates the selected and display date of the calendar child control.
        /// </summary>
        private void UpdateCalendar()
        {
            if (m_Calendar.SelectedDate != SelectedDateTime.Value.Date)
            {
                if (SelectedDateTime.HasValue)
                {
                    m_Calendar.SelectedDate = SelectedDateTime.Value.Date;
                    m_Calendar.DisplayDate = m_Calendar.SelectedDate.Value;
                }
                else
                {
                    m_Calendar.SelectedDate = SelectedDateTime;
                }
            }
        }
        /// <summary>
        /// Write the current display date and tome to the textbox.
        /// </summary>
        private void WriteValueToTextBox()
        {
            if (m_TextBox != null && !m_DisableTextBoxUpdate)
            {
                if (SelectedDateTime != null)
                {
                    DateTimeFormatInfo formatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
                    string timeFormat = TimeFormat == ZeusDateTimePickerFormats.Long ? formatInfo.LongTimePattern : formatInfo.ShortTimePattern;
                    string dateFormat = DateFormat == ZeusDateTimePickerFormats.Long ? formatInfo.LongDatePattern : formatInfo.ShortDatePattern;
                    DateTime displayed = SelectedDateTime.Value.Date + SelectedDateTime.Value.TimeOfDay;
                    m_TextBox.Text = string.Format("{0} {1}", displayed.ToString(dateFormat), displayed.ToString(timeFormat));
                }
                else
                {
                    m_TextBox.Text = "Select a date";
                }
            }
        }
        /// <summary>
        /// <see cref="DateFormatProperty"/> property changed handler.
        /// </summary>
        /// <param name="d"><see cref="ZeusDateTimePicker"/> that changed its <see cref="DateFormat"/>.</param>
        /// <param name="e"><see cref="DependencyPropertyChangedEventArgs"/>.</param>
        private static void OnDateFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZeusDateTimePicker zdtp = d as ZeusDateTimePicker;
            zdtp?.WriteValueToTextBox();
        }
        /// <summary>
        /// <see cref="TimeFormatProperty"/> property changed handler.
        /// </summary>
        /// <param name="d"><see cref="ZeusDateTimePicker"/> that changed its <see cref="TimeFormat"/>.</param>
        /// <param name="e"><see cref="DependencyPropertyChangedEventArgs"/>.</param>
        private static void OnTimeFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZeusDateTimePicker zdtp = d as ZeusDateTimePicker;
            zdtp?.WriteValueToTextBox();
        }
        /// <summary>
        /// Handles the drop down button click envent.
        /// </summary>
        /// <param name="sender">The object that generates the event.</param>
        /// <param name="e">Information about the occurred event.</param>
        private void OnDropDownButtonClicked(object sender, RoutedEventArgs e)
        {
            IsDropDownOpen = !IsDropDownOpen;
        }
        /// <summary>
        /// <see cref="SelectedDateTimeProperty"/> property changed handler.
        /// </summary>
        /// <param name="d"><see cref="ZeusDateTimePicker"/> that changed its <see cref="SelectedDateTime"/>.</param>
        /// <param name="e"><see cref="DependencyPropertyChangedEventArgs"/>.</param>
        private static void OnSelectedDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZeusDateTimePicker zdtp = d as ZeusDateTimePicker;
            zdtp.RaiseEvent(new RoutedEventArgs(SelectedDateTimeChangedEvent, zdtp));
            //set values on child conrols
            zdtp.UpdateCalendar();
            zdtp.WriteValueToTextBox();
        }
        /// <summary>
        /// Handle the changes in calenda child control SelectedDate property.
        /// </summary>
        /// <param name="sender">The object that raise te evnet.</param>
        /// <param name="e">Information about the event.</param>
        private void OnCalendarSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDateTime = SelectedDateTime == null ? m_Calendar.SelectedDate : m_Calendar.SelectedDate.Value.Date + SelectedDateTime.Value.TimeOfDay;
            WriteValueToTextBox();
            IsDropDownOpen = false;
        }
        /// <summary>
        /// Handle the text changed event of the textbox child control.
        /// </summary>
        /// <param name="sender">The object that trigger the event.</param>
        /// <param name="e">Information about the event.</param>
        private void OnTextBoxTextChanged(object sender, EventArgs e)
        {
            DateTime parsed;
            //try parse the text into a DateTime object
            if (DateTime.TryParse(m_TextBox.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out parsed))
            {
                m_DisableTextBoxUpdate = true;
                SelectedDateTime = parsed;
                m_DisableTextBoxUpdate = false;
            }
        }
        /// <summary>
        /// Handle the got focus event.
        /// </summary>
        private static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            ZeusDateTimePicker zdtp = sender as ZeusDateTimePicker;
            if ((!e.Handled) && (zdtp.m_TextBox != null))
            {
                if (e.OriginalSource == zdtp)
                {
                    zdtp.m_TextBox.Focus();
                    e.Handled = true;
                }
                else if (e.OriginalSource == zdtp.m_TextBox)
                {
                    zdtp.m_TextBox.SelectAll();
                    e.Handled = true;
                }
            }
        }
        /// <summary>
        /// Handle the lost focus event of the textbox child control.
        /// </summary>
        /// <param name="sender">The object that trigger the event.</param>
        /// <param name="e">Information about the event.</param>
        private void OnTextBoxLostFocus(object sender, EventArgs e)
        {
            WriteValueToTextBox();
        }
        /// <summary>
        /// Handle the key down event of the textbox child control.
        /// </summary>
        /// <param name="sender">The object that trigger the event.</param>
        /// <param name="e">Information about the event.</param>
        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                WriteValueToTextBox();
            }
        }

        #endregion
    }
}
