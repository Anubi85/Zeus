using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Zeus.UI.Enums;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled clock.
    /// </summary>
    [TemplatePart(Name = c_ElementHourIncreaseButton, Type = typeof(ZeusButton))]
    [TemplatePart(Name = c_ElementHourDecreaseButton, Type = typeof(ZeusButton))]
    [TemplatePart(Name = c_ElementHourDisplay, Type = typeof(ZeusTextBlock))]
    [TemplatePart(Name = c_ElementMinuteIncreaseButton, Type = typeof(ZeusButton))]
    [TemplatePart(Name = c_ElementMinuteDecreaseButton, Type = typeof(ZeusButton))]
    [TemplatePart(Name = c_ElementMinuteDisplay, Type = typeof(ZeusTextBlock))]
    [TemplatePart(Name = c_ElementSecondIncreaseButton, Type = typeof(ZeusButton))]
    [TemplatePart(Name = c_ElementSecondDecreaseButton, Type = typeof(ZeusButton))]
    [TemplatePart(Name = c_ElementSecondDisplay, Type = typeof(ZeusTextBlock))]
    [TemplatePart(Name = c_ElementHourView, Type = typeof(Grid))]
    [TemplatePart(Name = c_ElementMinSecView, Type = typeof(Grid))]
    public class ZeusClock : Control
    {
        #region Constants

        /// <summary>
        /// Name of the hour increase button element in the control template.
        /// </summary>
        private const string c_ElementHourIncreaseButton = "PART_HourIncreaseButton";
        /// <summary>
        /// Name of the hour decrease button element in the control template.
        /// </summary>
        private const string c_ElementHourDecreaseButton = "PART_HourDecreaseButton";
        /// <summary>
        /// Name of the minute increase button element in the control template.
        /// </summary>
        private const string c_ElementMinuteIncreaseButton = "PART_MinuteIncreaseButton";
        /// <summary>
        /// Name of the minute decrease button element in the control template.
        /// </summary>
        private const string c_ElementMinuteDecreaseButton = "PART_MinuteDecreaseButton";
        /// <summary>
        /// Name of the second increase button element in the control template.
        /// </summary>
        private const string c_ElementSecondIncreaseButton = "PART_SecondIncreaseButton";
        /// <summary>
        /// Name of the second decrease button element in the control template.
        /// </summary>
        private const string c_ElementSecondDecreaseButton = "PART_SecondDecreaseButton";
        /// <summary>
        /// Name of the hour display text box element in the control template.
        /// </summary>
        private const string c_ElementHourDisplay = "PART_HourDisplay";
        /// <summary>
        /// Name of the minute display text box element in the control template.
        /// </summary>
        private const string c_ElementMinuteDisplay = "PART_MinuteDisplay";
        /// <summary>
        /// Name of the second display text box element in the control template.
        /// </summary>
        private const string c_ElementSecondDisplay = "PART_SecondDisplay";
        /// <summary>
        /// Name of the hour view grid element in the control template.
        /// </summary>
        private const string c_ElementHourView = "PART_HourView";
        /// <summary>
        /// Name of the minutes/seconds view grid element in the control template.
        /// </summary>
        private const string c_ElementMinSecView = "PART_MinSecView";

        #endregion

        #region Fields

        /// <summary>
        /// The text block control that will display selected time hour.
        /// </summary>
        private ZeusTextBlock m_HourDisplay;
        /// <summary>
        /// The button that allows to increase the selected time hours.
        /// </summary>
        private ZeusButton m_IncreaseHourButton;
        /// <summary>
        /// The button that allows to decrease the selected time hours.
        /// </summary>
        private ZeusButton m_DecreaseHourButton;
        /// <summary>
        /// The text block control that will display selected time minutes.
        /// </summary>
        private ZeusTextBlock m_MinuteDisplay;
        /// <summary>
        /// The button that allows to increase the selected time minutes.
        /// </summary>
        private ZeusButton m_IncreaseMinuteButton;
        /// <summary>
        /// The button that allows to decrease the selected time minutes.
        /// </summary>
        private ZeusButton m_DecreaseMinuteButton;
        /// <summary>
        /// The text block control that will display selected time seconds.
        /// </summary>
        private ZeusTextBlock m_SecondDisplay;
        /// <summary>
        /// The button that allows to increase the selected time seconds.
        /// </summary>
        private ZeusButton m_IncreaseSecondButton;
        /// <summary>
        /// The button that allows to decrease the selected time seconds.
        /// </summary>
        private ZeusButton m_DecreaseSecondButton;
        /// <summary>
        /// The grid that will display the hour selection buttons.
        /// </summary>
        private Grid m_HourView;
        /// <summary>
        /// The grid that will display the minute/second selection buttons.
        /// </summary>
        private Grid m_MinSecView;

        #endregion

        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusClock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusClock), new FrameworkPropertyMetadata(typeof(ZeusClock)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusClock), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusClock"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;
        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusClock"/> selected time.
        /// </summary>
        public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register("SelectedTime", typeof(TimeSpan), typeof(ZeusClock), new PropertyMetadata(TimeSpan.Zero));
        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusClock"/> display mode.
        /// </summary>
        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register("DisplayMode", typeof(ZeusClockMode), typeof(ZeusClock), new PropertyMetadata(ZeusClockMode.Clock));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusClock"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        /// <summary>
        /// Gets or sets the selected time.
        /// </summary>
        public TimeSpan SelectedTime
        {
            get { return (TimeSpan)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }
        /// <summary>
        /// Gets or sets the display mode.
        /// </summary>
        public ZeusClockMode DisplayMode
        {
            get { return (ZeusClockMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the visual tree for the <see cref="ZeusClock"/> control when a new template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SetupControls(ref m_HourDisplay, ref m_IncreaseHourButton, ref m_DecreaseHourButton, c_ElementHourDisplay, c_ElementHourIncreaseButton, c_ElementHourDecreaseButton, "hh");
            SetupControls(ref m_MinuteDisplay, ref m_IncreaseMinuteButton, ref m_DecreaseMinuteButton, c_ElementMinuteDisplay, c_ElementMinuteIncreaseButton, c_ElementMinuteDecreaseButton, "mm");
            SetupControls(ref m_SecondDisplay, ref m_IncreaseSecondButton, ref m_DecreaseSecondButton, c_ElementSecondDisplay, c_ElementSecondIncreaseButton, c_ElementSecondDecreaseButton, "ss");
            if (m_HourView != null)
            {
                ClearSelectionView(m_HourView);
            }
            if (m_MinSecView != null)
            {
                ClearSelectionView(m_MinSecView);
            }
            m_HourView = GetTemplateChild(c_ElementHourView) as Grid;
            m_MinSecView = GetTemplateChild(c_ElementMinSecView) as Grid;
            if (m_HourView != null)
            {
                PopulateSelectionView(m_HourView, 1);
            }
            if (m_MinSecView != null)
            {
                PopulateSelectionView(m_MinSecView, 5);
            }
        }
        /// <summary>
        /// Clean up a previously configured hour view.
        /// </summary>
        /// /// <param name="view">The control that has to be cleared.</param>
        private void ClearSelectionView(Grid view)
        {
            foreach (ZeusClockButton btn in view.Children.OfType<ZeusClockButton>())
            {
                btn.Click -= OnClockButtonClick;
            }
        }
        /// <summary>
        /// Populates the hour view with the hour selection buttons.
        /// </summary>
        /// <param name="view">The control that has to be populated.</param>
        /// <param name="factor">The multiplication factor used for content computation.</param>
        private void PopulateSelectionView(Grid view, int factor)
        {
            for (int i = 0; i < view.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < view.ColumnDefinitions.Count; j++)
                {
                    ZeusClockButton btn = new ZeusClockButton();
                    btn.SetValue(Grid.RowProperty, i);
                    btn.SetValue(Grid.ColumnProperty, j);
                    btn.Content = (i * view.ColumnDefinitions.Count + j) * factor;
                    btn.Click += OnClockButtonClick;
                    view.Children.Add(btn);
                }
            }
        }
        /// <summary>
        /// Configures the display, increase and decrease controls for one of the selected time parts.
        /// </summary>
        /// <param name="display">The display control storage field.</param>
        /// <param name="increase">The increase control storage field.</param>
        /// <param name="decrease">The decrease control storage field.</param>
        /// <param name="displayName">The display control template name.</param>
        /// <param name="increaseName">The increase control template name.</param>
        /// <param name="decreaseName">The decrease control template name.</param>
        /// <param name="format">The format string for the displyed data.</param>
        private void SetupControls(ref ZeusTextBlock display, ref ZeusButton increase, ref ZeusButton decrease, string displayName, string increaseName, string decreaseName, string format)
        {
            //unregister event handlers
            if (display != null)
            {
                display.Click -= OnDisplayClick;
            }
            if (increase != null)
            {
                increase.Click -= OnIncreaseButtonClick;
            }
            if (decrease != null)
            {
                decrease.Click -= OnDecreaseButtonClick;
            }
            //get template childs
            display = GetTemplateChild(displayName) as ZeusTextBlock;
            increase = GetTemplateChild(increaseName) as ZeusButton;
            decrease = GetTemplateChild(decreaseName) as ZeusButton;
            //register event handlers and apply bindings
            if (display != null)
            {
                display.SetBinding(TextBlock.TextProperty, new Binding(SelectedTimeProperty.Name) { Source = this, StringFormat = format });
                display.Click += OnDisplayClick;
            }
            if (increase != null)
            {
                increase.Click += OnIncreaseButtonClick;
            }
            if (decrease != null)
            {
                decrease.Click += OnDecreaseButtonClick;
            }
        }
        /// <summary>
        /// Handle the click event on the hour display control.
        /// </summary>
        /// <param name="sender">The object that trigger the event.</param>
        /// <param name="e">Information about the occurred event.</param>
        private void OnDisplayClick(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(m_HourDisplay))
            {
                DisplayMode = ZeusClockMode.Hours;
                return;
            }
            if (sender.Equals(m_MinuteDisplay))
            {
                DisplayMode = ZeusClockMode.Minutes;
                return;
            }
            if (sender.Equals(m_SecondDisplay))
            {
                DisplayMode = ZeusClockMode.Seconds;
                return;
            }
        }
        /// <summary>
        /// Handle the click event on the increase button.
        /// </summary>
        /// <param name="sender">The object that trigger the event.</param>
        /// <param name="e">Information about the occurred event.</param>
        private void OnIncreaseButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(m_IncreaseHourButton))
            {
                SelectedTime = new TimeSpan((SelectedTime.Hours + 1) % 24, SelectedTime.Minutes, SelectedTime.Seconds);
                return;
            }
            if (sender.Equals(m_IncreaseMinuteButton))
            {
                SelectedTime = new TimeSpan(SelectedTime.Hours, (SelectedTime.Minutes + 1) % 60, SelectedTime.Seconds);
                return;
            }
            if (sender.Equals(m_IncreaseSecondButton))
            {
                SelectedTime = new TimeSpan(SelectedTime.Hours, SelectedTime.Minutes, (SelectedTime.Seconds + 1) % 60);
                return;
            }
        }
        /// <summary>
        /// Handle the click event on the decrease button.
        /// </summary>
        /// <param name="sender">The object that trigger the event.</param>
        /// <param name="e">Information about the occurred event.</param>
        private void OnDecreaseButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(m_DecreaseHourButton))
            {
                SelectedTime = new TimeSpan(SelectedTime.Hours == 0 ? 23 : SelectedTime.Hours - 1, SelectedTime.Minutes, SelectedTime.Seconds);
                return;
            }
            if (sender.Equals(m_DecreaseMinuteButton))
            {
                SelectedTime = new TimeSpan(SelectedTime.Hours, SelectedTime.Minutes == 0 ? 59 : SelectedTime.Minutes - 1, SelectedTime.Seconds);
                return;
            }
            if (sender.Equals(m_DecreaseSecondButton))
            {
                SelectedTime = new TimeSpan(SelectedTime.Hours, SelectedTime.Minutes, SelectedTime.Seconds == 0 ? 59 : SelectedTime.Seconds - 1);
                return;
            }
        }
        /// <summary>
        /// Handle the click event on the hour button.
        /// </summary>
        /// <param name="sender">The object that trigger the event.</param>
        /// <param name="e">Information about the occurred event.</param>
        private void OnClockButtonClick(object sender, RoutedEventArgs e)
        {
            ZeusClockButton btn = sender as ZeusClockButton;
            if (btn != null)
            {
                switch (DisplayMode)
                {
                    case ZeusClockMode.Hours:
                        SelectedTime = new TimeSpan((int)btn.Content, SelectedTime.Minutes, SelectedTime.Seconds);
                        break;
                    case ZeusClockMode.Minutes:
                        SelectedTime = new TimeSpan(SelectedTime.Hours, (int)btn.Content, SelectedTime.Seconds);
                        break;
                    case ZeusClockMode.Seconds:
                        SelectedTime = new TimeSpan(SelectedTime.Hours, SelectedTime.Minutes, (int)btn.Content);
                        break;
                }
            }
            DisplayMode = ZeusClockMode.Clock;
        }

        #endregion
    }
}
